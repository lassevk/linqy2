using System;
using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace Linqy.Tests
{
    [TestFixture]
    public class AddIndexEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AddIndexEnumerable<int>(null));
        }

        [Test]
        public void GetEnumerator_CollectionAndNoStartIndex_ReturnsCorrectlyIndexedElements()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new AddIndexEnumerable<int>(collection);

            CollectionAssert.AreEqual(new[]
            {
                new IndexedElement<int>(0, 1),
                new IndexedElement<int>(1, 2),
                new IndexedElement<int>(2, 3),
            }, output);
        }

        [Test]
        public void GetEnumerator_CollectionAndStartIndex_ReturnsCorrectlyIndexedElements()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new AddIndexEnumerable<int>(collection, 10);

            CollectionAssert.AreEqual(new[]
            {
                new IndexedElement<int>(10, 1),
                new IndexedElement<int>(11, 2),
                new IndexedElement<int>(12, 3),
            }, output);
        }
    }
}
