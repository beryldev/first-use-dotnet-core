using System.Linq;
using FluentAssertions;
using Wrhs.Common;
using Wrhs.Data.Service;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Service
{
    public class StockServiceTests : TestsBase
    {
        private readonly StockService stockSrv;

        public StockServiceTests() : base()
        {
            this.stockSrv = new StockService(context);
            context.Documents.Add(new Document());
            context.Operations.Add(new Operation{DocumentId=1});
            context.SaveChanges();
        }

        [Fact]
        public void ShouldReturnStockWithProductOnGetStockAtLoc()
        {
            context.Products.Add(new Product { Name="Prod1", Code = "P1" });
            context.Products.Add(new Product { Name="Prod2", Code = "P2" });
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=1, Location="L1", Quantity = 10});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=1, Location="L1", Quantity = 20});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = false, ProductId=1, Location="L1", Quantity = 20});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=1, Location="L2", Quantity = 50});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=2, Location="L1", Quantity = 50});
            context.SaveChanges();

            var stock = stockSrv.GetStockAtLocation(1, "L1");

            stock.Quantity.Should().Be(30);
            stock.Product.Should().NotBeNull();
            stock.Product.Name.Should().Be("Prod1");
        }

        [Fact]
        public void ShouldReturnProductStockOnGetProductStock()
        {
            context.Products.Add(new Product { Name="Prod1", Code = "P1" });
            context.Products.Add(new Product { Name="Prod2", Code = "P2" });
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=1, Location="L1", Quantity = 10});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=1, Location="L1", Quantity = 20});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = false, ProductId=1, Location="L1", Quantity = 20});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=1, Location="L2", Quantity = 50});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=2, Location="L1", Quantity = 50});
            context.SaveChanges();

            var stock = stockSrv.GetProductStock(1);

            stock.Should().NotBeNullOrEmpty();
            stock.Should().HaveCount(2);
            stock.Sum(s=>s.Quantity).Should().Be(80);
        }
    }
}