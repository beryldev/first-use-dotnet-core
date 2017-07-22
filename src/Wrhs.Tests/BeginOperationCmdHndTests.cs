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
        private readonly Mock<IOperationService> fakeOperationService;
        private readonly Mock<IEventBus> fakeEventBus;
        private readonly BeginOperationCommandHandler handler;
        private readonly BeginOperationCommand command;

        public BeginOperationCmdHndTests()
        {
            fakeOperationService = new Mock<IOperationService>();
            fakeEventBus = new Mock<IEventBus>();
            
            command = CreateCommand();

            handler = new BeginOperationCommandHandler(fakeEventBus.Object, fakeOperationService.Object);
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

        protected OperationType GetValidOperationType()
        {
            return OperationType.Delivery;
        }
        
        [Fact]
        public void ShouldRegisterNewOperationWhenCommandValid()
        {   
            var wasValidOperation = false;
            fakeOperationService.Setup(m=>m.Save(It.IsAny<Operation>()))
                .Callback((Operation oper) =>
                { 
                    wasValidOperation = oper.Type == GetValidOperationType();
                });
            

            handler.Handle(command); 

            fakeOperationService.Verify(m=>m.Save(It.IsAny<Operation>()), Times.Once());
            wasValidOperation.Should().BeTrue();
        }
    }
}