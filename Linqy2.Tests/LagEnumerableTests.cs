using System;
using System.Collections.Generic;
using System.Linq;

using NSubstitute;

using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable PossibleNullReferenceException
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace Linqy2.Tests
{
    [TestFixture]
    public class LagEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LagEnumerable<int>(null));
        }

        [Test]
        public void Constructor_NegativeAmount_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LagEnumerable<int>(new int[0], -1));
        }

        [Test]
        public void GetEnumerator_WithCollectionAndAmountOf0_ReturnsCorrectLagItems()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new LagEnumerable<int>(collection, 0);

            CollectionAssert.AreEqual(new[]
            {
                new LagItem<int>(1, 1),
                new LagItem<int>(2, 2),
                new LagItem<int>(3, 3),
            }, output);
        }

        [Test]
        public void GetEnumerator_WithMoreLagThanElementsInCollection_ReturnsCorrectLagItems()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new LagEnumerable<int>(collection, 10);

            CollectionAssert.AreEqual(new[]
            {
                new LagItem<int>(1, 0),
                new LagItem<int>(2, 0),
                new LagItem<int>(3, 0),
            }, output);
        }

        [Test]
        public void GetEnumerator_WithCollectionAndAmountOf1_ReturnsCorrectLagItems()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new LagEnumerable<int>(collection);

            CollectionAssert.AreEqual(new[]
            {
                new LagItem<int>(1, 0),
                new LagItem<int>(2, 1),
                new LagItem<int>(3, 2),
            }, output);
        }

        [Test]
        public void GetEnumerator_WithCollectionAndAmountOf2_ReturnsCorrectLagItems()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new LagEnumerable<int>(collection, 2);

            CollectionAssert.AreEqual(new[]
            {
                new LagItem<int>(1, 0),
                new LagItem<int>(2, 0),
                new LagItem<int>(3, 1),
            }, output);
        }

        [Test]
        public void GetEnumerator_WithCollectionAndAmountOf2_OnlyEnumeratesSourceCollectionOnce()
        {
            var collection = Substitute.For<IEnumerable<int>>();
            int callCount = 0;
            collection.GetEnumerator().Returns(ci =>
            {
                callCount++;
                return new List<int>
                {
                    1, 2, 3
                }.GetEnumerator();
            });

            new LagEnumerable<int>(collection, 2).ToList();

            Assert.That(callCount, Is.EqualTo(1));
        }
    }
}