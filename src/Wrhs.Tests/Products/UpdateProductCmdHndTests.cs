using System;
using FluentAssertions;
using Moq;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class UpdateProductCmdHndTests : BaseProductCmdHndTests
    {
        private readonly UpdateProductCommand command;
        private readonly UpdateProductCommandHandler handler;

        public UpdateProductCmdHndTests() : base()
        {
            command = new UpdateProductCommand();
            handler = new UpdateProductCommandHandler(eventBusMock.Object, productSrvMock.Object);
            productSrvMock.Setup(m=>m.GetProductById(It.IsAny<int>())).Returns(new Product());
        }

        [Fact]
        public void ShouldUpdateProductOnHandle()
        {
            var updateEan = false;
            var updateSku = false;
            command.Ean = "some-ean";
            command.Sku = "some-sku";
            productSrvMock.Setup(m=>m.Update(It.IsNotNull<Product>()))
                .Callback((Product p)=>{
                    updateEan = p.Ean == "some-ean";
                    updateSku = p.Sku == "some-sku";
                }); 

            handler.Handle(command);

            productSrvMock.Verify(m=>m.Update(It.IsAny<Product>()), Times.Once());
            updateEan.Should().BeTrue();
            updateSku.Should().BeTrue();
        }

        [Fact]
        public void ShouldPublishEventAfterUpdate()
        {
            handler.Handle(command);

            eventBusMock.Verify(m=>m.Publish(It.IsAny<UpdateProductEvent>()), Times.Once());
        }

        [Theory]
        [InlineData("prod1")]
        [InlineData("PROD1")]
        [InlineData("pRoD1")]
        public void ShouldUpdateProductWithUpperCaseCode(string code)
        {
            var validCase = false;
            productSrvMock.Setup(m=>m.Update(It.IsNotNull<Product>()))
                .Callback((Product p) => {
                    validCase = p.Code.Equals(code.ToUpper(), StringComparison.CurrentCulture);
                });
            command.Code = code;

            handler.Handle(command);

            validCase.Should().BeTrue();
        }
    }
}