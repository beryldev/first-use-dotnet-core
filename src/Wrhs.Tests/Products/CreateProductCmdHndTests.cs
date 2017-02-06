using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class CreateProductCmdHndTests : BaseProductCmdHndTests
    {
        private readonly CreateProductCommand command;

        private readonly Mock<IValidator<CreateProductCommand>> validatorMock;

        private readonly CreateProductCommandHandler handler;

        public CreateProductCmdHndTests() : base()
        {
            command = new CreateProductCommand();
            validatorMock = new Mock<IValidator<CreateProductCommand>>();
            handler = new CreateProductCommandHandler(validatorMock.Object,
                eventBusMock.Object, productSrvMock.Object);  
        }

        [Fact]
        public void ShouldSaveNewProductWhenCmdValid()
        {
            var passedEan = false;
            var passedSku = false;

            command.Ean = "some-ean";
            command.Sku = "some-sku";
            validatorMock.Setup(m=>m.Validate(It.IsAny<CreateProductCommand>()))
                .Returns(new List<ValidationResult>());
            productSrvMock.Setup(m=>m.Save(It.IsNotNull<Product>()))
                .Callback((Product p)=>{
                    passedEan = p.Ean == "some-ean";
                    passedSku = p.Sku == "some-sku";
                });

            handler.Handle(command);

            productSrvMock.Verify(m=>m.Save(It.IsAny<Product>()), Times.Once());
            passedEan.Should().BeTrue();
            passedSku.Should().BeTrue();
        }

        [Fact]
        public void ShouldPublishEventAfterSaveProduct()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<CreateProductCommand>()))
                .Returns(new List<ValidationResult>());

            handler.Handle(command);

            eventBusMock.Verify(m=>m.Publish(It.IsAny<CreateProductEvent>()), Times.Once());
        }

        [Fact]
        public void ShouldOnlyThrowValidationExceptionWhenInvalidCmd()
        {
            validatorMock.Setup(m=>m.Validate(It.IsAny<CreateProductCommand>()))
                .Returns(new List<ValidationResult>(){new ValidationResult("Field", "Message")});
            
            Assert.Throws<CommandValidationException>(()=>
            {
                handler.Handle(command);
            });

            productSrvMock.Verify(m=>m.Save(It.IsAny<Product>()), Times.Never());
            eventBusMock.Verify(m=>m.Publish(It.IsAny<CreateProductEvent>()), Times.Never());
        }

        [Theory]
        [InlineData("prod1")]
        [InlineData("PROD1")]
        [InlineData("pRoD1")]
        public void ShouldSaveProductWithUpperCaseCode(string code)
        {
            var validCase = false;
            productSrvMock.Setup(m=>m.Save(It.IsNotNull<Product>()))
                .Callback((Product p) => {
                    validCase = p.Code.Equals(code.ToUpper(), StringComparison.CurrentCulture);
                });
            command.Code = code;

            handler.Handle(command);

            validCase.Should().BeTrue();
        }

    }
}