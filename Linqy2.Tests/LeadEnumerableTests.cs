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
    public class LeadEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LeadEnumerable<int>(null));
        }

        [Test]
        public void Constructor_NegativeAmount_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LeadEnumerable<int>(new int[0], -1));
        }

        [Test]
        public void GetEnumerator_WithMoreLeadThanNumberOfElementsInCollection_ReturnsCorrectLagItems()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new LeadEnumerable<int>(collection, 10);

            CollectionAssert.AreEqual(new[]
            {
                new LeadItem<int>(1, 0),
                new LeadItem<int>(2, 0),
                new LeadItem<int>(3, 0),
            }, output);
        }

        [Test]
        public void GetEnumerator_WithCollectionAndAmountOf0_ReturnsCorrectLagItems()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new LeadEnumerable<int>(collection, 0);

            CollectionAssert.AreEqual(new[]
            {
                new LeadItem<int>(1, 1),
                new LeadItem<int>(2, 2),
                new LeadItem<int>(3, 3),
            }, output);
        }

        [Test]
        public void GetEnumerator_WithCollectionAndAmountOf1_ReturnsCorrectLagItems()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new LeadEnumerable<int>(collection);

            CollectionAssert.AreEqual(new[]
            {
                new LeadItem<int>(1, 2),
                new LeadItem<int>(2, 3),
                new LeadItem<int>(3, 0),
            }, output);
        }

        [Test]
        public void GetEnumerator_WithCollectionAndAmountOf2_ReturnsCorrectLagItems()
        {
            var collection = new[]
            {
                1, 2, 3
            };

            var output = new LeadEnumerable<int>(collection, 2);

            CollectionAssert.AreEqual(new[]
            {
                new LeadItem<int>(1, 3),
                new LeadItem<int>(2, 0),
                new LeadItem<int>(3, 0),
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

            new LeadEnumerable<int>(collection, 2).ToList();

            Assert.That(callCount, Is.EqualTo(1));
        }
    }
}