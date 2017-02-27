using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Delivery;
using Xunit;

namespace Wrhs.Tests.Delivery
{
    public class ProcessDeliveryOperationCmdHndTests
        : ProcessOperationCmdHndTestsBase<ProcessDeliveryOperationCommand, ProcessDeliveryOperationEvent>
    {
        
        [Fact]
        public void ShouldRegisterUnconfirmedShiftOnHandle()
        {
            var validOperationId = false;
            var validProductId = false;
            var validQuantity = false;
            var unconfirmed = false;
            var operation = new Operation { Id = 1 };
            command.ProductId = 1;
            command.Quantity = 5;
            command.OperationGuid = "guid";
            operationSrvMock.Setup(m=>m.GetOperationByGuid(command.OperationGuid))
                .Returns(operation);
            stockServiceMock.Setup(m=>m.Save(It.IsAny<Shift>()))
                .Callback((Shift shift)=>{
                    validOperationId = shift.OperationId == operation.Id;
                    validProductId = shift.ProductId == command.ProductId;
                    validQuantity = shift.Quantity == command.Quantity;
                    unconfirmed = !shift.Confirmed;
                });

            handler.Handle(command);

            stockServiceMock.Verify(m=>m.Save(It.IsAny<Shift>()), Times.Once());
            validOperationId.Should().BeTrue();
            validProductId.Should().BeTrue();
            validQuantity.Should().BeTrue();
            unconfirmed.Should().BeTrue();
        }

        [Fact]
        public void ShouldPublishEventAfterRegisterShift()
        {
            var validOperationId = false;
            var validProductId = false;
            var validQuantity = false;
            var validLocation = false;
            var unconfirmed = false;
            var operation = new Operation { Id = 1 };
            command.ProductId = 1;
            command.Quantity = 5;
            command.OperationGuid = "guid";
            command.DstLocation = "loc01";
            operationSrvMock.Setup(m=>m.GetOperationByGuid(command.OperationGuid))
                .Returns(operation);
            eventBusMock.Setup(m=>m.Publish(It.IsAny<ProcessOperationEvent>()))
                .Callback((ProcessOperationEvent @event)=>{
                    var shift = @event.Shifts.First();
                    validOperationId = shift.OperationId == operation.Id;
                    validProductId = shift.ProductId == command.ProductId;
                    validQuantity = shift.Quantity == command.Quantity;
                    validLocation = shift.Location == "loc01";
                    unconfirmed = !shift.Confirmed;
                });

            handler.Handle(command);

            eventBusMock.Verify(m=>m.Publish(It.IsAny<ProcessOperationEvent>()), Times.Once());
            validOperationId.Should().BeTrue();
            validProductId.Should().BeTrue();
            validQuantity.Should().BeTrue();
            validLocation.Should().BeTrue();
            unconfirmed.Should().BeTrue();
        }

        protected override ICommandHandler<ProcessDeliveryOperationCommand> CreateHandler(IValidator<ProcessDeliveryOperationCommand> validator, 
            IEventBus eventBus, IStockService stockService, IOperationService operationSrv)
        {
            return new ProcessDeliveryOperationCommandHandler(validator, eventBus, stockService, operationSrv);
        }

        protected override ProcessDeliveryOperationCommand CreateCommand()
        {
            return new ProcessDeliveryOperationCommand();
        }

        protected override int GetExpectedNumOfShifts()
        {
           return 1;
        }
    }
}