using System;
using System.Collections.Generic;
using FluentAssertions;
using Wrhs.Common;
using Xunit;

namespace Wrhs.Data.Tests
{
    public class DocumentNumeratorTests : TestsBase
    {
        private readonly DocumentNumerator numerator;

        public DocumentNumeratorTests() : base()
        {
            var prefixMapping = new Dictionary<DocumentType, string>
            {
                { DocumentType.Delivery, "DLV" },
                { DocumentType.Relocation, "RLC" },
                { DocumentType.Release, "RLS" }
            };

            numerator = new DocumentNumerator(prefixMapping);
            numerator.SetContext(context);
        }

        [Theory]
        [InlineData(DocumentType.Delivery)]
        [InlineData(DocumentType.Relocation)]
        [InlineData(DocumentType.Release)]
        public void ShouldAssignFirstNumberWhenNoDocsInContext(DocumentType type)
        {
            var date = new DateTime(2016, 1, 1);
            var document = new Document { Type = type, IssueDate=date};

            var result = numerator.AssignNumber(document);

            result.FullNumber.Should().NotBeNullOrWhiteSpace();
            result.Number.Should().Be(1);
            result.Month.Should().Be(date.Month);
            result.Year.Should().Be(date.Year);
        }

        [Theory]
        [InlineData(DocumentType.Delivery)]
        [InlineData(DocumentType.Relocation)]
        [InlineData(DocumentType.Release)]
        public void ShouldAssignNextNumberWhenDocsInContext(DocumentType type)
        {
            context.Documents.Add(new Document{ Type = type, Number = 1, Month = 1, Year=2016});
            context.Documents.Add(new Document{ Type = type, Number = 2, Month = 1, Year=2016});
            context.SaveChanges();
            var document = new Document { Type = type, IssueDate=new DateTime(2016, 1, 1)};

            var result = numerator.AssignNumber(document);

            result.Number.Should().Be(3);
        }

        [Theory]
        [InlineData(DocumentType.Delivery, DocumentType.Release)]
        [InlineData(DocumentType.Relocation, DocumentType.Delivery)]
        [InlineData(DocumentType.Release, DocumentType.Relocation)]
        public void ShouldAssignNextNumberInDocumentTypeAndDateScope(DocumentType type, DocumentType other)
        {
            context.Documents.Add(new Document{ Type = type, Number = 1, Month = 1, Year=2016});
            context.Documents.Add(new Document{ Type = type, Number = 2, Month = 1, Year=2016});
            context.Documents.Add(new Document{ Type = type, Number = 1, Month = 2, Year=2016});
            context.Documents.Add(new Document{ Type = type, Number = 2, Month = 2, Year=2016});
            context.Documents.Add(new Document{ Type = type, Number = 3, Month = 2, Year=2016});
            context.Documents.Add(new Document{ Type = other, Number = 1, Month=1, Year=2016});
            context.Documents.Add(new Document{ Type = other, Number = 2, Month=1, Year=2016});
            context.Documents.Add(new Document{ Type = other, Number = 3, Month=1, Year=2016});
            context.SaveChanges();
            var document = new Document { Type = type, IssueDate=new DateTime(2016, 1, 1)};

            var result = numerator.AssignNumber(document);

            result.Number.Should().Be(3);
        }
    }
}