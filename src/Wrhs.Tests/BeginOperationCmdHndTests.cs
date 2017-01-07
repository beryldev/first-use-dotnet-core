using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Xunit;

namespace Wrhs.Tests
{
    public class BeginOperationCmdHndTests
    {
        protected readonly Mock<IValidator<BeginOperationCommand>> validatorMock;
        protected readonly Mock<IEventBus> eventBusMock;
        protected readonly Mock<IOperationPersist> operationPersistMock;
        protected readonly BeginOperationCommandHandler handler;
        protected readonly BeginOperationCommand command;

        public BeginOperationCmdHndTests()
        {
            validatorMock = new Mock<IValidator<BeginOperationCommand>>();
            validatorMock.Setup(m=>m.Validate(It.IsAny<BeginOperationCommand>()))
                .Returns(new List<ValidationResult>());

            eventBusMock = new Mock<IEventBus>();

            operationPersistMock = new Mock<IOperationPersist>();
            
            command = CreateCommand();

            handler = CreateCommandHandler(validatorMock.Object, eventBusMock.Object,
                operationPersistMock.Object);
        }

        protected BeginOperationCommand CreateCommand()
        {
            return new BeginOperationCommand
            {
                DocumentId = 1,
                OperationGuid = "some-guid",
                OperationType = OperationType.Delivery
            };
        }

        protected BeginOperationCommandHandler CreateCommandHandler(IValidator<BeginOperationCommand> validator,
            IEventBus eventBus, IOperationPersist operationPersist)
        {
            return new BeginOperationCommandHandler(validator, eventBus, operationPersist);
        }

        protected OperationType GetValidOperationType()
        {
            return OperationType.Delivery;
        }
        
        [Fact]
        public void ShouldRegisterNewOperationWhenCommandValid()
        {   
            var wasValidOperation = false;
            operationPersistMock.Setup(m=>m.Save(It.IsAny<Operation>()))
                .Callback((Operation oper) =>
                { 
                    wasValidOperation = oper.Type == GetValidOperationType();
                });
            

            handler.Handle(command); 

            operationPersistMock.Verify(m=>m.Save(It.IsAny<Operation>()), Times.Once());
            wasValidOperation.Should().BeTrue();
        }

        [Fact]
        public void ShouldPublishEventAfterRegister()
        {
            var validDocumentId = false;
            var validOperationGuid = false;
            var validOperationType = false;
            
            eventBusMock.Setup(m=>m.Publish(It.IsAny<BeginOperationEvent>()))
                .Callback((BeginOperationEvent @event)=>{
                    validDocumentId = @event.Operation.DocumentId == command.DocumentId;
                    validOperationGuid = @event.Operation.OperationGuid == command.OperationGuid;
                    validOperationType = @event.Operation.Type == GetValidOperationType();  
                });

            handler.Handle(command);

            eventBusMock.Verify(m=>m.Publish(It.IsAny<BeginOperationEvent>()), Times.Once());
            validDocumentId.Should().BeTrue();
            validOperationGuid.Should().BeTrue();
            validOperationType.Should().BeTrue();
        }

        [Fact]
        public void ShouldOnlyThrowValidationExceptionWhenValidationFails()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<BeginOperationCommand>()))
                .Returns(new List<ValidationResult>{new ValidationResult("Field", "Message")});

            Assert.Throws<CommandValidationException>(()=>{
                handler.Handle(command);
            });

            operationPersistMock.Verify(m=>m.Save(It.IsAny<Operation>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsAny<BeginOperationEvent>()), Times.Never());
        }  
    }
}