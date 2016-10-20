using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wrhs.Core.Search;

namespace Wrhs.Tests
{
    [TestFixture]
    public class PaginatorTests
    {
        [Test]
        public void OnDefaultPaginateReturnFirstPage()
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items);

            Assert.AreEqual(1, result.Page);
        }

        [Test]
        public void OnDefaultPageSizeIsTenItems()
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items);

            Assert.AreEqual(10, result.PerPage);
        }

        [Test]
        public void OnPaginateReturnTotalCount()
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items);

            Assert.AreEqual(items.Count, result.Total);
        }

        [Test]
        public void OnPaginateDivideItemsToPages()
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items);

            Assert.AreEqual(10, result.Items.Count());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void OnPaginateReturnDesiredPage(int page)
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items, page);

            Assert.AreEqual(page, result.Page);
            Assert.AreEqual(10, result.Items.Count());
        }

        [Test]
        [TestCase(20)]
        [TestCase(17)]
        [TestCase(10)]
        [TestCase(3)]
        public void OnPaginateReturnDesiredPageSize(int size)
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            var result = paginator.Paginate(items, 1, size);
            
            Assert.AreEqual(size, result.Items.Count());
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-99)]
        [TestCase(-1000)]
        public void OnPassNegativeOrZeroPageThrowArgumentException(int page)
        {
            var paginator = new Paginator<object>();
            var items = MakeItems(20);

            Assert.Throws<ArgumentException>(()=>
            {
                paginator.Paginate(items, page);
            }); 
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-99)]
        [TestCase(-1000)]
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