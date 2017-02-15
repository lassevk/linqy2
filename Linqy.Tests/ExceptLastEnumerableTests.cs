using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace Linqy.Tests
{
    [TestFixture]
    public class ExceptLastEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => new ExceptLastEnumerable<int>(collection, 0));
        }

        [Test]
        public void Constructor_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ExceptLastEnumerable<int>(new List<int>(), -1));
        }

        [Test]
        public void GetEnumerator_WithAmountGreaterThanNumberOfElements_ReturnsCorrectResults()
        {
            var collection = Enumerable.Range(0, 10);

            var output = new ExceptLastEnumerable<int>(collection, 20);

            CollectionAssert.AreEqual(new int[0], output);
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
            list.Count.Returns(2);
            list[0].Returns(42);
            list[1].Returns(17);

            new ExceptLastEnumerable<int>(list, 1).ToList();

            Assert.That(callCount, Is.EqualTo(0));
        }

        [Test]
        public void GetEnumerator_ForIListCollectionWhereAmountIsZero_ReturnsExpectedResult()
        {
            var list = Enumerable.Range(0, 10).ToList();

            var output = new ExceptLastEnumerable<int>(list, 0).ToList();

            CollectionAssert.AreEqual(new[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9
            }, output);
        }

        [Test]
        public void GetEnumerator_ForIListCollectionWhereAmountIsLessThanWholeList_ReturnsExpectedResult()
        {
            var list = Enumerable.Range(0, 10).ToList();

            var output = new ExceptLastEnumerable<int>(list, 5).ToList();

            CollectionAssert.AreEqual(new[]
            {
                0, 1, 2, 3, 4
            }, output);
        }

        [Test]
        public void GetEnumerator_ForIListCollectionWhereAmountIsGreaterThanWholeList_ReturnsExpectedResult()
        {
            var list = Enumerable.Range(0, 10).ToList();

            var output = new ExceptLastEnumerable<int>(list, 25).ToList();

            CollectionAssert.AreEqual(new int[0], output);
        }

        [Test]
        public void GetEnumerator_ForIEnumerableCollectionWhereAmountZero_ReturnsExpectedResult()
        {
            var list = Substitute.For<IEnumerable<int>>();
            list.GetEnumerator().Returns(Enumerable.Range(0, 10).GetEnumerator());

            var output = new ExceptLastEnumerable<int>(list, 0).ToList();

            CollectionAssert.AreEqual(new[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8, 9
            }, output);
        }

        [Test]
        public void GetEnumerator_ForIEnumerableCollectionWhereAmountIsLessThanWholeCollection_ReturnsExpectedResult()
        {
            var list = Substitute.For<IEnumerable<int>>();
            list.GetEnumerator().Returns(Enumerable.Range(0, 10).GetEnumerator());

            var output = new ExceptLastEnumerable<int>(list, 2).ToList();

            CollectionAssert.AreEqual(new[]
            {
                0, 1, 2, 3, 4, 5, 6, 7
            }, output);
        }

        [Test]
        public void GetEnumerator_ForIEnumerableCollectionWhereAmountIsGreaterThanWholeCollection_ReturnsExpectedResult()
        {
            var list = Substitute.For<IEnumerable<int>>();
            list.GetEnumerator().Returns(Enumerable.Range(0, 10).GetEnumerator());

            var output = new ExceptLastEnumerable<int>(list, 20).ToList();

            CollectionAssert.AreEqual(new int[0], output);
        }
    }
}