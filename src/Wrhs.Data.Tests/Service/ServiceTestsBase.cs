using FluentAssertions;
using Wrhs.Data.Service;
using Xunit;

namespace Wrhs.Data.Tests.Service
{
    public abstract class ServiceTestsBase<T> : TestsBase where T : class
    {
        protected readonly BaseService<T> service;

        protected ServiceTestsBase() : base()
        {
            for(var i=0; i<4; i++)
                context.Add(CreateEntity(i));
            context.SaveChanges();

            service = CreateService(context);
        }

        protected abstract BaseService<T> CreateService(WrhsContext context);

        protected abstract T CreateEntity(int i);

         [Fact]
        public void ShouldReturnPageWithProductsOnGet()
        {
            var result = service.Get();

            result.Should().NotBeNull();
            result.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void ShouldReturnDefaultFirstPageOnGetWithoutParameters()
        {
            var result = service.Get();

            result.Page.Should().Be(1);
            result.PageSize.Should().Be(20);
        }

        [Fact]
        public void ShouldReturnRequestedPageWithDefaultPageSizeOnGet()
        {
            var result = service.Get(2);

            result.Page.Should().Be(2);
            result.PageSize.Should().Be(20);
        }

        [Fact]
        public void ShouldReturnRequestedPageAndPageSizeOnGet()
        {
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
            var result = service.Get(page, pageSize);

            result.Items.Should().HaveCount(expected);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 0)]
        [InlineData(0, 4)]
        public void ShouldReturnRequestedDataWithDefaultPageSize(int page, int expected)
        {
            var result = service.Get(page);

            result.Items.Should().HaveCount(expected);
        }

        [Fact]
        public void ShouldReturnRequestedDataWithDefaultPageAndPageSize()
        {
            var result = service.Get();

            result.Items.Should().HaveCount(4);
        }
    }
}