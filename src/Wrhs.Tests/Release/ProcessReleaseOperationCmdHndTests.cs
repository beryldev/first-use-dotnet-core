using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Release;
using Xunit;

namespace Wrhs.Tests.Release
{
    public class ProcessReleaseOperationCmdHndTests
        : ProcessOperationCmdHndTestsBase<ProcessReleaseOperationCommand, ProcessReleaseOperationEvent>
    {
        protected override ProcessReleaseOperationCommand CreateCommand()
        {
            return new ProcessReleaseOperationCommand();
        }

        protected override ICommandHandler<ProcessReleaseOperationCommand> CreateHandler(IValidator<ProcessReleaseOperationCommand> validator, 
            IEventBus eventBus, IStockService stockService, IOperationService operationSrv)
        {
            return new ProcessReleaseOperationCommandHandler(validator, eventBus, stockService, operationSrv);
        }

        [Fact]
        public void ShouldRegisterDecrementingShift()
        {
            var quantity = 0M;
            command.ProductId = 1;
            command.Quantity = 10;
            command.SrcLocation = "SrcLocation";
            stockServiceMock.Setup(m=>m.Save(It.IsNotNull<Shift>()))
                .Callback((Shift shift)=>{
                    quantity = shift.Quantity;
                });

            handler.Handle(command);
            
            quantity.Should().Be(-10);
        }

        [Fact]
        public void ShouldRegisterShiftWithSrcLocation()
        {
            var location = "";
            command.ProductId = 1;
            command.Quantity = 10;
            command.SrcLocation = "SrcLocation";
            command.DstLocation = "DstLocation";
            stockServiceMock.Setup(m=>m.Save(It.IsNotNull<Shift>()))
                .Callback((Shift shift)=>{
                    location = shift.Location;
                });

            handler.Handle(command);
            
            location.Should().Be(command.SrcLocation);
        }

        [Fact]
        public void ShouldPublishEventWithRegisteredShift()
        {
             var shift = new Shift();
            command.ProductId = 1;
            command.Quantity = 10;
            command.SrcLocation = "SrcLocation";
            eventBusMock.Setup(m=>m.Publish(It.IsNotNull<ProcessReleaseOperationEvent>()))
                .Callback((ProcessReleaseOperationEvent @event)=>{
                    shift = @event.Shifts.First();
                });

            handler.Handle(command);
            
            shift.ProductId.Should().Be(1);
            shift.Location.Should().Be("SrcLocation");
            shift.OperationId.Should().Be(1);
            shift.Quantity.Should().Be(-10);
        }

        protected override int GetExpectedNumOfShifts()
        {
            return 1;
        }
    }
}