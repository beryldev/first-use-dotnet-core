using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Xunit;

namespace Wrhs.Tests
{
    public class ExecuteOperationCmdHndTests
    {
        protected readonly ExecuteOperationCommand command;
        protected readonly ICommandHandler<ExecuteOperationCommand> handler;
        protected readonly Mock<IValidator<ExecuteOperationCommand>> validatorMock;
        protected readonly Mock<IEventBus> eventBusMock;
        protected readonly Mock<IOperationService> operationSrvMock;
        protected readonly Mock<IDocumentPersist> docPersistMock;
        protected readonly Mock<IShiftPersist> shiftPersistMock;
        protected readonly Mock<IOperationPersist> operationPersistMock;

        public ExecuteOperationCmdHndTests()
        {
            eventBusMock = new Mock<IEventBus>();
            
            validatorMock = new Mock<IValidator<ExecuteOperationCommand>>();
            validatorMock.Setup(m=>m.Validate(It.IsAny<ExecuteOperationCommand>()))
                .Returns(new List<ValidationResult>());

            operationSrvMock = new Mock<IOperationService>();
            docPersistMock = new Mock<IDocumentPersist>();
            shiftPersistMock = new Mock<IShiftPersist>();
            operationPersistMock = new Mock<IOperationPersist>();

            command = CreateCommand();
            var parameters = new HandlerParameters
            {
                OperationService = operationSrvMock.Object,
                OperationPersist = operationPersistMock.Object,
                DocumentPersist = docPersistMock.Object,
                ShiftPersist = shiftPersistMock.Object
            };
            handler = CreateHandler(validatorMock.Object, eventBusMock.Object, parameters);
        }

        protected ExecuteOperationCommand CreateCommand()
        {
            return new ExecuteOperationCommand { OperationGuid = "some-guid"};
        }

        protected ICommandHandler<ExecuteOperationCommand> CreateHandler(IValidator<ExecuteOperationCommand> validator,
            IEventBus eventBus, HandlerParameters parameters)
        {
            return new ExecuteOperationCommandHandler(validator, eventBus, parameters);
        }

        protected OperationType GetExpectedOperationType()
        {
            return OperationType.Delivery;
        }

        protected DocumentType GetExpectedDocumentType()
        {
            return DocumentType.Delivery;
        }

        [Fact]
        public void ShouldChangeDocumentStateToExecuted()
        {
            var operation = CreateOperation();
            operationSrvMock.Setup(m=>m.GetOperationByGuid(command.OperationGuid))
                .Returns(operation);

            handler.Handle(command);

            operation.Document.State.Should().Be(DocumentState.Executed);
            docPersistMock.Verify(m=>m.Update(It.IsNotNull<Document>()), Times.Once());
        }

        [Fact]
        public void ShouldConfirmAllOperationShifts()
        {
            var operation = CreateOperation();
            operationSrvMock.Setup(m=>m.GetOperationByGuid(command.OperationGuid))
                .Returns(operation);

            handler.Handle(command);

            operation.Shifts.Where(s=>!s.Confirmed).Should().BeEmpty();
            shiftPersistMock.Verify(m=>m.Update(It.IsNotNull<Shift>()), Times.Exactly(2));
        }

        [Fact]
        public void ShouldChangeOperationStatusToDone()
        {
           var operation = CreateOperation();
           operationSrvMock.Setup(m=>m.GetOperationByGuid(command.OperationGuid))
                .Returns(operation);

            handler.Handle(command);

            operation.Status.Should().Be(OperationStatus.Done);
            operationPersistMock.Verify(m=>m.Update(It.IsNotNull<Operation>()), Times.Once());
        }

        [Fact]
        public void ShouldAfterSuccesHandlePublishEvent()
        {
            Operation publishedOperation = null;
            var operation = CreateOperation();
            operationSrvMock.Setup(m=>m.GetOperationByGuid(command.OperationGuid))
                .Returns(operation);
            eventBusMock.Setup(m=>m.Publish(It.IsNotNull<ExecuteOperationEvent>()))
                .Callback((ExecuteOperationEvent @event)=>{ publishedOperation = @event.Operation; });

            handler.Handle(command);

            publishedOperation.Should().Be(operation);
        }

        [Fact]
        public void ShouldOnlyThrowValidationExceptionWhenValidationFail()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<ExecuteOperationCommand>()))
                .Returns(new List<ValidationResult>{new ValidationResult("Field", "Message")});

            Assert.Throws<CommandValidationException>(()=>{
                handler.Handle(command);
            });

            docPersistMock.Verify(m=>m.Update(It.IsNotNull<Document>()), Times.Never());
            shiftPersistMock.Verify(m=>m.Update(It.IsNotNull<Shift>()), Times.Never());
            operationPersistMock.Verify(m=>m.Update(It.IsNotNull<Operation>()), Times.Never());
        }

        protected Operation CreateOperation()
        {
            var document = new Document { Type = GetExpectedDocumentType(),  State = DocumentState.Confirmed };
            var shifts = new List<Shift> { new Shift{Confirmed=false}, new Shift{Confirmed=false}};
            var operation = new Operation
            {
                Type = GetExpectedOperationType(),
                Document = document,
                Shifts = shifts
            };

            return operation;
        }
    }
}