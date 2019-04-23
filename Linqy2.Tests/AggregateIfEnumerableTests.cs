using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

// ReSharper disable ConvertToLocalFunction
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace Linqy2.Tests
{
    [TestFixture]
    public class AggregateIfEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;
            Func<int, int, bool> aggregatePredicate = (i1, i2) => true;
            Func<int> getSeed = () => 0;
            Func<int, int, int> aggregateFunc = (i1, i2) => i1 + i2;
            Func<int, int> resultSelector = i => i;
            Assert.Throws<ArgumentNullException>(() => new AggregateIfEnumerable<int, int, int>(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
        }

        [Test]
        public void Constructor_NullAggregatePredicate_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = new int[0];
            Func<int, int, bool> aggregatePredicate = null;
            Func<int> getSeed = () => 0;
            Func<int, int, int> aggregateFunc = (i1, i2) => i1 + i2;
            Func<int, int> resultSelector = i => i;
            Assert.Throws<ArgumentNullException>(() => new AggregateIfEnumerable<int, int, int>(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
        }

        [Test]
        public void Constructor_NullGetSeed_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = new int[0];
            Func<int, int, bool> aggregatePredicate = (i1, i2) => true;
            Func<int> getSeed = null;
            Func<int, int, int> aggregateFunc = (i1, i2) => i1 + i2;
            Func<int, int> resultSelector = i => i;
            Assert.Throws<ArgumentNullException>(() => new AggregateIfEnumerable<int, int, int>(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
        }

        [Test]
        public void Constructor_NullAggregateFunc_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = new int[0];
            Func<int, int, bool> aggregatePredicate = (i1, i2) => true;
            Func<int> getSeed = () => 0;
            Func<int, int, int> aggregateFunc = null;
            Func<int, int> resultSelector = i => i;
            Assert.Throws<ArgumentNullException>(() => new AggregateIfEnumerable<int, int, int>(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
        }

        [Test]
        public void Constructor_NullResultSelector_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = new int[0];
            Func<int, int, bool> aggregatePredicate = (i1, i2) => true;
            Func<int> getSeed = () => 0;
            Func<int, int, int> aggregateFunc = (i1, i2) => i1 + i2;
            Func<int, int> resultSelector = null;
            Assert.Throws<ArgumentNullException>(() => new AggregateIfEnumerable<int, int, int>(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
        }

        [Test]
        public void GetEnumerator_CollectionWithGroups_ReturnsCorrectResults()
        {
            var collection = new[]
            {
                1, 2, 3, 4, 5,
                7,
                10, 11,
                13,
                15,
                17, 18, 19
            };

            var output = new AggregateIfEnumerable<int, string, string>(collection,
                (prev, current) => current == prev + 1,
                () => string.Empty,
                (aggregator, element) => (aggregator + ", " + element).TrimStart(',', ' '),
                aggregator => aggregator).ToList();

            CollectionAssert.AreEqual(new[]
            {
                "1, 2, 3, 4, 5",
                "7",
                "10, 11",
                "13",
                "15",
                "17, 18, 19"
            }, output);
        }
    }
}
