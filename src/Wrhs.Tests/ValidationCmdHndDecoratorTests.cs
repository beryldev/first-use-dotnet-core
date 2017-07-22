using Moq;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.CommandHandlers;
using Wrhs.Core.Exceptions;
using Xunit;

namespace Wrhs.Tests
{
    public class ValidationCmdHndDecoratorTests
    {
        private FakeCommand command;
        private Mock<ICommandHandler<FakeCommand>> fakeInnerHandler;
        private Mock<IValidator<FakeCommand>> fakeValidator;
        private ValidationCmdHndDecorator<FakeCommand> handler;

        public ValidationCmdHndDecoratorTests()
        {
            command = new FakeCommand();
            fakeInnerHandler = new Mock<ICommandHandler<FakeCommand>>();
            fakeValidator = new Mock<IValidator<FakeCommand>>();
            handler = new ValidationCmdHndDecorator<FakeCommand>(fakeInnerHandler.Object, fakeValidator.Object);

        }

        [Fact]
        public void ShouldOnlyThrowCommandValidationExceptionWhenValidationFails()
        {
            fakeValidator.Setup(m => m.Validate(It.IsAny<FakeCommand>()))
                .Returns(new List<ValidationResult>() { new ValidationResult("", "") });

            Assert.Throws<CommandValidationException>(() => { handler.Handle(command); });
            fakeInnerHandler.Verify(h => h.Handle(It.IsAny<FakeCommand>()), Times.Never());
        }

        [Fact]
        public void ShouldHanldeByInnerHandlerWhenValidationSuccess()
        {
            fakeValidator.Setup(m => m.Validate(It.IsAny<FakeCommand>()))
                .Returns(new List<ValidationResult>());

            handler.Handle(command);

            fakeInnerHandler.Verify(h => h.Handle(command), Times.Once());
        }


        public class FakeCommand : ICommand
        {

        }
    }
}
