using System;
using System.Linq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Core.Search;
using Wrhs.Documents;
using Wrhs.Operations.Delivery;

namespace Wrhs.Tests
{
    public abstract class DocumentSearchTests<T> where T : ISearchableDocument, IEntity
    {
        [Test]
        [TestCase(5, 2)]
        [TestCase(10, 5)]
        [TestCase(20, 7)]
        [TestCase(321, 30)]
        public void ShouldReturnAllDocumentsWhenSearchWithEmptyCriteria(int itemsCount, int pageSize)
        {
            var repo = CreateRepository(itemsCount);
            var search = CreateSearch(repo);
            var criteria = search.MakeCriteria();
            criteria.PerPage = pageSize;

            var result = search.Exec(criteria);

            Assert.AreEqual(itemsCount, result.Total);
            Assert.LessOrEqual(pageSize, result.Items.Count());
        }

        [Test]
        [TestCase("D/001/2016")]
        [TestCase("D/0010/2016")]
        [TestCase("D/007/2016")]
        public void ShouldReturnDocumentWhenSearchByFullNumber(string fullNumber)
        {
            var repo = CreateRepository(20);
            var search = CreateSearch(repo);
            var criteria = (DocumentSearchCriteria<T>)search.MakeCriteria();

            criteria.WhereFullNumber(Condition.Equal, fullNumber);
            var result = search.Exec(criteria);

            Assert.AreEqual(1, result.Items.Count());
            Assert.AreEqual(1, result.Total);
            Assert.AreEqual(fullNumber, result.Items.First().FullNumber);
        }

        [Test]
        [TestCase("2016", 20)]
        [TestCase("D/006/2016", 1)]
        [TestCase("D/001", 11)]
        public void ShouldReturnDocumentsWhenSearchByContainsFullNumber(string fullNumber, int count)
        {
            var repo = CreateRepository(20);
            var search = CreateSearch(repo);
            var criteria = (DocumentSearchCriteria<T>)search.MakeCriteria();

            criteria.WhereFullNumber(Condition.Contains, fullNumber);
            var result = search.Exec(criteria);

            Assert.AreEqual(count, result.Total);
        }

        [Test]
        [TestCase(20, 10)]
        [TestCase(30, 20)]
        [TestCase(5, 0)]
        public void ShouldReturnDocumentsWhenSearchByIssueDate(int itemsCount, int shouldReturn)
        {
            var repo = CreateRepository(itemsCount);
            var search = CreateSearch(repo);
            var criteria = (DocumentSearchCriteria<T>)search.MakeCriteria();

            criteria.WhereIssueDate(Condition.Equal, new DateTime(2016, 10, 24));
            var result = search.Exec(criteria);

            Assert.AreEqual(shouldReturn, result.Total);
        }

        ResourceSearch<T> CreateSearch(IRepository<T> repo)
        {
            var criteriaFactory = new DocumentSearchCriteriaFactory<T>();
            return new ResourceSearch<T>(repo, new Paginator<T>(), criteriaFactory);
        }

        IRepository<T> CreateRepository(int itemsCount)
        {

            var repo = RepositoryFactory<T>.Make();
            for(var i=0; i<itemsCount; i++)
            {
                var fullNumber = $"D/00{i+1}/2016";
                var issueDate = i < 10 ? new DateTime(2016, 10, 23) : new DateTime(2016, 10, 24);
                var doc = CreateDocument(fullNumber, issueDate);
  
                repo.Save(doc);
            }

            return repo;
        }

        protected abstract T CreateDocument(string fullNumber, DateTime issueDate);
    }
}