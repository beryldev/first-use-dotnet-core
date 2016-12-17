using FluentAssertions;
using Wrhs.Data.Service;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Service
{
    public abstract class ServiceTestsBase<T> : TestsBase where T : class
    {
        protected ServiceTestsBase() : base()
        {
            for(var i=0; i<4; i++)
                context.Add(CreateEntity(i));
            context.SaveChanges();
        }

        protected abstract T CreateEntity(int i);

        protected abstract BaseService<T> GetService();

         [Fact]
        public void ShouldReturnPageWithProductsOnGet()
        {
            var service = GetService();
            var result = service.Get();

            result.Should().NotBeNull();
            result.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnDefaultFirstPageOnGetWithoutParameters()
        {
            var service = GetService();
            var result = service.Get();

            result.Page.Should().Be(1);
            result.PageSize.Should().Be(20);
        }

        [Fact]
        public void ShouldReturnRequestedPageWithDefaultPageSizeOnGet()
        {
            var service = GetService();
            var result = service.Get(2);

            result.Page.Should().Be(2);
            result.PageSize.Should().Be(20);
        }

        [Fact]
        public void ShouldReturnRequestedPageAndPageSizeOnGet()
        {
            var service = GetService();
            var result = service.Get(4, 10);

            result.Page.Should().Be(4);
            result.PageSize.Should().Be(10);
        }

        [Theory]
        [InlineData(1, 2, 2)]
        [InlineData(2, 2, 2)]
        [InlineData(1, 4, 4)]
        [InlineData(2, 3, 1)]        
        public void ShouldReturnRequestedAndLimitedData(int page, int pageSize, int expected)
        {
            var service = GetService();
            var result = service.Get(page, pageSize);

            result.Items.Should().HaveCount(expected);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 0)]
        [InlineData(0, 4)]
        public void ShouldReturnRequestedDataWithDefaultPageSize(int page, int expected)
        {
            var service = GetService();
            var result = service.Get(page);

            result.Items.Should().HaveCount(expected);
        }

        [Fact]
        public void ShouldReturnRequestedDataWithDefaultPageAndPageSize()
        {
            var service = GetService();
            var result = service.Get();

            result.Items.Should().HaveCount(4);
        }
    }
}