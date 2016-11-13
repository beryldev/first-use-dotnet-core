using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core.Search;
using Xunit;

namespace Wrhs.Tests
{
    public class PaginatorTests
    {
        [Fact]
        public void OnDefaultPaginateReturnFirstPage()
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items);

            Assert.Equal(1, result.Page);
        }

        [Fact]
        public void OnDefaultPageSizeIsTenItems()
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items);

            Assert.Equal(10, result.PerPage);
        }

        [Fact]
        public void OnPaginateReturnTotalCount()
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items);

            Assert.Equal(items.Count, result.Total);
        }

        [Fact]
        public void OnPaginateDivideItemsToPages()
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items);

            Assert.Equal(10, result.Items.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void OnPaginateReturnDesiredPage(int page)
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items, page);

            Assert.Equal(page, result.Page);
            Assert.Equal(10, result.Items.Count());
        }

        [Theory]
        [InlineData(20)]
        [InlineData(17)]
        [InlineData(10)]
        [InlineData(3)]
        public void OnPaginateReturnDesiredPageSize(int size)
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items, 1, size);
            
            Assert.Equal(size, result.Items.Count());
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-99)]
        [InlineData(-1000)]
        public void OnPassNegativeOrZeroPageThrowArgumentException(int page)
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            Assert.Throws<ArgumentException>(()=>
            {
                paginator.Paginate(items, page);
            }); 
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-99)]
        [InlineData(-1000)]
        public void OnPassNegativePageSizeThrowArgumentException(int size)
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            Assert.Throws<ArgumentException>(()=>
            {
                paginator.Paginate(items, 1, size);
            }); 
        }

        protected List<object> MakeItems(int count)
        {
            var items = new List<object>();
            for(var i=0; i<count; i++)
                items.Add(new object());

            return items;
        }
    }
}