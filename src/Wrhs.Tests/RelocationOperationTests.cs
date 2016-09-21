using System;
using Moq;
using NUnit.Framework;
using Wrhs.Operations;
using Wrhs.Operations.Relocation;

namespace Wrhs.Tests
{
    [TestFixture]
    public class RelocationOperationTests
    {
        [Test]
        public void OperationCanBasedOnRelocationDocument()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanAccessDirectlyToBaseRelocationDocument()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void ThrowsExceptionWhenPerformOperationWithoutBaseDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new RelocationOperation();

            Assert.Throws<InvalidOperationException>(()=>
            {
               operation.Perform(mock.Object); 
            });
        }
    }
}