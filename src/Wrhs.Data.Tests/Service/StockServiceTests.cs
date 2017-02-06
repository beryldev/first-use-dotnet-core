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

        [Fact]
        public void ShouldReturnEmptyCollectionOnGetProductStockWhenNoStocks()
        {
            var stock = stockSrv.GetProductStock(10);

            stock.Should().NotBeNull();
            stock.Should().BeEmpty();
        }

        [Theory]
        [InlineDataAttribute(1, 1, 1, 30)]
        [InlineDataAttribute(1, 2, 2, 80)]
        [InlineDataAttribute(1, 10, 3, 130)]
        [InlineDataAttribute(2, 10, 0, 0)]
        [InlineDataAttribute(2, 2, 1, 50)]
        public void ShouldReturnRequestedPageWithSizeOnGetStocks(int page, int pageSize, int expCount, decimal expTotalQuant)
        {
            context.Products.Add(new Product { Name="Prod1", Code = "P1" });
            context.Products.Add(new Product { Name="Prod2", Code = "P2" });
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=1, Location="L1", Quantity = 10});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=1, Location="L1", Quantity = 20});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = false, ProductId=1, Location="L1", Quantity = 20});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=1, Location="L2", Quantity = 50});
            context.Shifts.Add(new Shift{ OperationId=1, Confirmed = true, ProductId=2, Location="L1", Quantity = 50});
            context.SaveChanges();

            var result = stockSrv.GetStocks(page, pageSize);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(expCount);
            result.Page.Should().Be(page);
            result.PageSize.Should().Be(pageSize);
            result.Items.Sum(i => i.Quantity).Should().Be(expTotalQuant);
        }

        [Fact]
        public void ShouldStoreShiftInContextOnSave()
        {
            var shift = new Shift
            {
                OperationId = 1,
                ProductId = 1,
                Quantity = 10,
                Location = "loc"
            };

            stockSrv.Save(shift);

            context.Shifts.Should().HaveCount(1);
            context.Shifts.First().OperationId.Should().Be(1);
            context.Shifts.First().ProductId.Should().Be(1);
            context.Shifts.First().Quantity.Should().Be(10);
            context.Shifts.First().Location.Should().Be("loc");
        }

        [Fact]
        public void ShouldUpdateDataInContextOnUpdate()
        {
            var shift = new Shift
            {
                OperationId = 1,
                ProductId = 1,
                Quantity = 10,
                Location = "loc",
                Confirmed = false
            };
            context.Shifts.Add(shift);
            context.SaveChanges();

            shift.Confirmed = true;
            stockSrv.Update(shift);

            context.Shifts.Should().HaveCount(1);
            context.Shifts.First().Confirmed.Should().BeTrue();
            context.Shifts.First().Location.Should().Be("loc");
            context.Shifts.First().Quantity.Should().Be(10);
        }
    }
}