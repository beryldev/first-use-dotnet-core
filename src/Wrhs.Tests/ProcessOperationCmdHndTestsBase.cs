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
    public abstract class ProcessOperationCmdHndTestsBase<TCommand, TEvent>
        where TCommand : ProcessOperationCommand
        where TEvent : ProcessOperationEvent
    {
        protected readonly Mock<IValidator<TCommand>> validatorMock;
        protected readonly Mock<IEventBus> eventBusMock;
        protected readonly Mock<IStockService> stockServiceMock;
        protected readonly Mock<IOperationService> operationSrvMock;
        protected readonly ICommandHandler<TCommand> handler;
        protected readonly TCommand command;

        protected ProcessOperationCmdHndTestsBase()
        {
            validatorMock = new Mock<IValidator<TCommand>>();
            validatorMock.Setup(m=>m.Validate(It.IsAny<TCommand>())).Returns(new List<ValidationResult>());
            eventBusMock = new Mock<IEventBus>();
            stockServiceMock = new Mock<IStockService>();
            operationSrvMock = new Mock<IOperationService>();
            operationSrvMock.Setup(m=>m.GetOperationByGuid("some-guid"))
                .Returns(new Operation{Id = 1});

            handler = CreateHandler(validatorMock.Object, eventBusMock.Object,
                stockServiceMock.Object, operationSrvMock.Object);
            command = CreateCommand();
            command.OperationGuid = "some-guid";
        }

        protected abstract ICommandHandler<TCommand> CreateHandler(IValidator<TCommand> validator,
            IEventBus eventBus, IStockService stockService, IOperationService operationSrv);

        protected abstract TCommand CreateCommand();

        protected abstract int GetExpectedNumOfShifts();

        [Fact]
        public void ShouldOnlyThrowValidationExceptionWhenValidationFails()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<TCommand>()))
                .Returns(new List<ValidationResult>{new ValidationResult("Field", "Message")});

            Assert.Throws<CommandValidationException>(()=>{
                handler.Handle(command);
            });

            stockServiceMock.Verify(m=>m.Save(It.IsAny<Shift>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsAny<ProcessOperationEvent>()), Times.Never());
        }

        [Fact]
        public void EachRegisteredShiftShouldHaveValidOperationId()
        {
            var shifts = new List<Shift>();
            stockServiceMock.Setup(m=>m.Save(It.IsNotNull<Shift>()))
                .Callback((Shift shift)=>{ shifts.Add(shift); });

            handler.Handle(command);

            shifts.Count(x=>x.OperationId==1).Should().Be(GetExpectedNumOfShifts());
        }

    }
}