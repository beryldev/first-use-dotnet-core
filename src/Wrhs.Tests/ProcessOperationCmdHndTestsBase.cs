using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Xunit;

namespace Wrhs.Tests
{
    public abstract class ProcessOperationCmdHndTestsBase<TCommand, TEvent>
        where TCommand : ProcessOperationCommand
        where TEvent : ProcessOperationEvent
    {
        protected readonly Mock<IEventBus> eventBusMock;
        protected readonly Mock<IStockService> stockServiceMock;
        protected readonly Mock<IOperationService> operationSrvMock;
        protected readonly ICommandHandler<TCommand> handler;
        protected readonly TCommand command;

        protected ProcessOperationCmdHndTestsBase()
        {
            eventBusMock = new Mock<IEventBus>();
            stockServiceMock = new Mock<IStockService>();
            operationSrvMock = new Mock<IOperationService>();
            operationSrvMock.Setup(m=>m.GetOperationByGuid("some-guid"))
                .Returns(new Operation{Id = 1});

            handler = CreateHandler(eventBusMock.Object, stockServiceMock.Object, operationSrvMock.Object);
            command = CreateCommand();
            command.OperationGuid = "some-guid";
        }

        protected abstract ICommandHandler<TCommand> CreateHandler(IEventBus eventBus, 
            IStockService stockService, IOperationService operationSrv);

        protected abstract TCommand CreateCommand();

        protected abstract int GetExpectedNumOfShifts();

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