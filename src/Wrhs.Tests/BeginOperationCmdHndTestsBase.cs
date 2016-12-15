using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Xunit;

namespace Wrhs.Tests
{
    public abstract class BeginOperationCmdHndTestsBase<TCommand, TEvent>
        where TCommand : BeginOperationCommand
        where TEvent : BeginOperationEvent
    {
        protected readonly Mock<IValidator<TCommand>> validatorMock;
        protected readonly Mock<IEventBus> eventBusMock;
        protected readonly Mock<IOperationPersist> operationPersistMock;
        protected readonly BeginOperationCommandHandler<TCommand, TEvent> handler;
        protected readonly TCommand command;

        protected BeginOperationCmdHndTestsBase()
        {
            validatorMock = new Mock<IValidator<TCommand>>();
            validatorMock.Setup(m=>m.Validate(It.IsAny<TCommand>()))
                .Returns(new List<ValidationResult>());

            eventBusMock = new Mock<IEventBus>();

            operationPersistMock = new Mock<IOperationPersist>();
            
            command = CreateCommand();

            handler = CreateCommandHandler(validatorMock.Object, eventBusMock.Object,
                operationPersistMock.Object);
        }

        protected abstract TCommand CreateCommand();

        protected abstract BeginOperationCommandHandler<TCommand, TEvent> CreateCommandHandler(IValidator<TCommand> validator,
            IEventBus eventBus, IOperationPersist operationPersist);

        protected abstract OperationType GetValidOperationType();
        
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
            
            eventBusMock.Setup(m=>m.Publish(It.IsAny<TEvent>()))
                .Callback((TEvent @event)=>{
                    validDocumentId = @event.Operation.DocumentId == command.DocumentId;
                    validOperationGuid = @event.Operation.OperationGuid == command.OperationGuid;
                    validOperationType = @event.Operation.Type == GetValidOperationType();  
                });

            handler.Handle(command);

            eventBusMock.Verify(m=>m.Publish(It.IsAny<TEvent>()), Times.Once());
            validDocumentId.Should().BeTrue();
            validOperationGuid.Should().BeTrue();
            validOperationType.Should().BeTrue();
        }

        [Fact]
        public void ShouldOnlyThrowValidationExceptionWhenValidationFails()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<TCommand>()))
                .Returns(new List<ValidationResult>{new ValidationResult("Field", "Message")});

            Assert.Throws<CommandValidationException>(()=>{
                handler.Handle(command);
            });

            operationPersistMock.Verify(m=>m.Save(It.IsAny<Operation>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsAny<TEvent>()), Times.Never());
        }  
    }
}