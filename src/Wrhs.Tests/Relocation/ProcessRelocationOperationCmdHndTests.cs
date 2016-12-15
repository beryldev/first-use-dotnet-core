using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Relocation;
using Xunit;

namespace Wrhs.Tests.Relocation
{
    public class ProcessRelocationOperationCmdHndTests
        : ProcessOperationCmdHndTestsBase<ProcessRelocationOperationCommand, ProcessRelocationOperationEvent>
    {
        protected override ProcessRelocationOperationCommand CreateCommand()
        {
            return new ProcessRelocationOperationCommand();
        }

        protected override ICommandHandler<ProcessRelocationOperationCommand> CreateHandler(IValidator<ProcessRelocationOperationCommand> validator,
            IEventBus eventBus, IShiftPersist shiftPersist, IOperationService operationSrv)
        {
            return new ProcessRelocationOperationCommandHandler(validator,
                eventBus, shiftPersist, operationSrv);
        }

        [Fact]
        public void ShouldRegisterTwoShiftsWhenValidCommand()
        {
            handler.Handle(command);

            shiftPersistMock.Verify(m=>m.Save(It.IsNotNull<Shift>()), Times.Exactly(2));
        }

        [Fact]
        public void ShouldDecrementQuantityAtSrcLocation()
        {
            var shiftOut = 0M;
            command.Quantity = 10;
            command.SrcLocation = "srcLocation";
            shiftPersistMock.Setup(m=>m.Save(It.IsNotNull<Shift>()))
                .Callback((Shift shift)=>{
                    if(shift.Location==command.SrcLocation)
                        shiftOut = shift.Quantity;
                });

            handler.Handle(command);

            shiftOut.Should().Be(-10);
        }

        [Fact]
        public void ShouldIncrementQuantityAtDstLocation()
        {
            var shiftIn = 0M;
            command.Quantity = 10;
            command.DstLocation = "dstLocation";
            shiftPersistMock.Setup(m=>m.Save(It.IsNotNull<Shift>()))
                .Callback((Shift shift)=>{
                    if(shift.Location==command.DstLocation)
                        shiftIn = shift.Quantity;
                });

            handler.Handle(command);

            shiftIn.Should().Be(10);
        }

        [Fact]
        public void ShouldInEventFirstBeOutShiftSecondInShift()
        {
            var firstOut = false;
            var secondIn = false;
            command.Quantity = 10;
            command.SrcLocation = "srcLocation";
            command.DstLocation = "dstLocation";
            eventBusMock.Setup(m=>m.Publish(It.IsNotNull<ProcessRelocationOperationEvent>()))
                .Callback((ProcessRelocationOperationEvent @event)=>{
                    firstOut = @event.Shifts.ToArray()[0].Location == "srcLocation";
                    secondIn = @event.Shifts.ToArray()[1].Location == "dstLocation";
                });


            handler.Handle(command);

            firstOut.Should().BeTrue();
            secondIn.Should().BeTrue();
        }

        protected override int GetExpectedNumOfShifts()
        {
            return 2;
        }
    }
}