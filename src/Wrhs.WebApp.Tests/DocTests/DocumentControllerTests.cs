using System;
using System.Collections.Generic;
using Moq;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Documents;
using Wrhs.WebApp.Controllers.Documents;
using Xunit;

namespace Wrhs.WebApp.Tests
{
    public abstract class DocumentControllerTests<TDoc, TLine> : IDisposable
        where TDoc : class, IEntity, INumerableDocument, IDocument<TLine>, ISearchableDocument
        where TLine : IEntity, IDocumentLine
    {
        protected Mock<IRepository<TDoc>> repository;

        protected DocumentController<TDoc, TLine> controller;

        public DocumentControllerTests()
        {
            repository = CreateDocRepository();
            controller = CreateDocController();
        }

        public void Dispose()
        {
            controller.Dispose();
        }

        protected virtual Mock<IRepository<TDoc>> CreateDocRepository()
        {
            return new Mock<IRepository<TDoc>>();
        }

        protected abstract DocumentController<TDoc, TLine> CreateDocController();

        [Fact]
        public void ShouldReturnDocumentsOnGetWithoutParameters()
        {
            repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

            var result = controller.Get();

            Assert.IsType<PaginateResult<TDoc>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithFullNumberParameter()
        {
            repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

            var result = controller.Get(fullNumber: "D/001/2016");

            Assert.IsType<PaginateResult<TDoc>>(result);
        }

        [Fact]
        public void ShouldReturnDocumentsOnGetWithIssueDateParameter()
        {
            repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

            var result = controller.Get(issueDate: DateTime.Today);

            Assert.IsType<PaginateResult<TDoc>>(result);
        }

        [Fact]
        public void ShouldReturnRequestedPageOnGet()
        {
            repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

            var result = controller.Get(page: 2);

            Assert.Equal(2, result.Page);
        }

        [Fact]
        public void ShouldReturnRequestedPageSize()
        {
            repository.Setup(m=>m.Get()).Returns(new List<TDoc>());

            var result = controller.Get(perPage: 30);

            Assert.Equal(30, result.PerPage);
        }
    }
}