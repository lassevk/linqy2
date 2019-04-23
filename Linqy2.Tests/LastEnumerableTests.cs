using System;
using System.Collections.Generic;
using System.Linq;

using NSubstitute;

using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace Linqy2.Tests
{
    [TestFixture]
    public class LastEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => new LastEnumerable<int>(collection, 0));
        }

        [Test]
        public void Constructor_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LastEnumerable<int>(new List<int>(), -1));
        }

        [Test]
        public void GetEnumerator_WithAmountGreaterThanNumberOfElements_ReturnsCorrectResults()
        {
            var collection = Enumerable.Range(0, 10);

            var output = new LastEnumerable<int>(collection, 20);

            CollectionAssert.AreEqual(new[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9
            }, output);
        }

        [Test]
        public void GetEnumerator_WithZeroAmount_DoesNotCallGetEnumeratorOnCollection()
        {
            var collection = Substitute.For<IEnumerable<int>>();
            int callCount = 0;
            collection.GetEnumerator().Returns(ci =>
            {
                callCount++;
                return Enumerable.Range(0, 10).GetEnumerator();
            });

            new LastEnumerable<int>(collection, 0).ToList();

            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void GetEnumerator_ForIListCollection_DoesNotCallGetEnumeratorOnCollection()
        {
            var list = Substitute.For<IList<int>>();
            int callCount = 0;
            list.GetEnumerator().Returns(ci =>
            {
                callCount++;
                return Enumerable.Range(0, 10).GetEnumerator();
            });
            list.Count.Returns(1);
            list[0].Returns(42);

            new LastEnumerable<int>(list, 1).ToList();

            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void GetEnumerator_ForIEnumerableCollectionWhereAmountIsLessThanWholeCollection_ReturnsExpectedResult()
        {
            var list = Substitute.For<IEnumerable<int>>();
            list.GetEnumerator().Returns(Enumerable.Range(0, 10).GetEnumerator());

            var output = new LastEnumerable<int>(list, 2).ToList();

            CollectionAssert.AreEqual(new[]
            {
                8, 9
            }, output);
        }

        [Test]
        public void GetEnumerator_ForIEnumerableCollectionWhereAmountIsGreaterThanWholeCollection_ReturnsExpectedResult()
        {
            var list = Substitute.For<IEnumerable<int>>();
            list.GetEnumerator().Returns(Enumerable.Range(0, 10).GetEnumerator());

            var output = new LastEnumerable<int>(list, 20).ToList();

            CollectionAssert.AreEqual(new[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9
            }, output);
        }
    }
}
