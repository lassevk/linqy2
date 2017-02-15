using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable AssignNullToNotNullAttribute

namespace Linqy.Tests
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void Partition_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.Partition(collection, 5));
        }

        [Test]
        public void Partition_ZeroPartitionSize_ThrowsArgumentOutOfRangeException()
        {
            IEnumerable<int> collection = Enumerable.Range(0, 10);

            Assert.Throws<ArgumentOutOfRangeException>(() => CollectionExtensions.Partition(collection, 0));
        }

        [TestCase(1, "(1), (2), (3), (4), (5), (6), (7), (8), (9), (10)")]
        [TestCase(2, "(1,2), (3,4), (5,6), (7,8), (9,10)")]
        [TestCase(3, "(1,2,3), (4,5,6), (7,8,9), (10)")]
        [TestCase(4, "(1,2,3,4), (5,6,7,8), (9,10)")]
        [TestCase(5, "(1,2,3,4,5), (6,7,8,9,10)")]
        [TestCase(6, "(1,2,3,4,5,6), (7,8,9,10)")]
        [TestCase(7, "(1,2,3,4,5,6,7), (8,9,10)")]
        [TestCase(8, "(1,2,3,4,5,6,7,8), (9,10)")]
        [TestCase(9, "(1,2,3,4,5,6,7,8,9), (10)")]
        [TestCase(10, "(1,2,3,4,5,6,7,8,9,10)")]
        [TestCase(11, "(1,2,3,4,5,6,7,8,9,10)")]
        [TestCase(100, "(1,2,3,4,5,6,7,8,9,10)")]
        public void Partition_WithTestCases_ProducesExpectedResults(int partitionSize, string expected)
        {
            IEnumerable<int> collection = Enumerable.Range(1, 10);

            var partitions = CollectionExtensions.Partition(collection, partitionSize);
            var output = string.Join(", ", partitions.Select(partition => $"({string.Join(",", partition)})"));

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void Except_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.Except(collection, 5));
        }

        [TestCase(-1, "0,1,2,3,4,5,6,7,8,9")]
        [TestCase(0, "1,2,3,4,5,6,7,8,9")]
        [TestCase(1, "0,2,3,4,5,6,7,8,9")]
        [TestCase(2, "0,1,3,4,5,6,7,8,9")]
        [TestCase(3, "0,1,2,4,5,6,7,8,9")]
        [TestCase(11, "0,1,2,3,4,5,6,7,8,9")]
        public void Except_WithTestCases_ProducesExpectedResults(int valueToOmmit, string expected)
        {
            IEnumerable<int> collection = Enumerable.Range(0, 10);

            var values = CollectionExtensions.Except(collection, valueToOmmit);
            var output = string.Join(",", values);

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        public void Append_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.Append(collection, 5));
        }

        [Test]
        public void Append_NulladditionalElements_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = Enumerable.Range(0, 10);
            int[] additionalElements = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.Append(collection, additionalElements));
        }

        [TestCase("0,1,2", "0,1,2", "0,1,2,0,1,2")]
        [TestCase("0,1,2", "", "0,1,2")]
        [TestCase("", "0,1,2", "0,1,2")]
        [TestCase("0,1,2", "10", "0,1,2,10")]
        public void Append_WithTestCases_ProducesExpectedResults(string collectionString, string additionalElementsString, string expectedString)
        {
            List<int> collection = collectionString.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToList();
            int[] additionalElements = additionalElementsString.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();
            int[] expected = expectedString.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();

            var output = CollectionExtensions.Append(collection, additionalElements);

            CollectionAssert.AreEqual(expected, output);
        }

        [Test]
        public void NonNull_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.NonNull(collection));
        }

        [Test]
        public void NonNull_CollectionWithValueTypes_ReturnsWholeCollection()
        {
            IEnumerable<int> collection = Enumerable.Range(0, 10).ToList();

            var output = CollectionExtensions.NonNull(collection);

            CollectionAssert.AreEqual(collection, output);
        }

        [Test]
        public void NonNull_CollectionWithReferenceTypesButNoNullElements_ReturnsWholeCollection()
        {
            IEnumerable<string> collection = Enumerable.Range(0, 10).Select(idx => idx.ToString()).ToList();

            var output = CollectionExtensions.NonNull(collection);

            CollectionAssert.AreEqual(collection, output);
        }

        [Test]
        public void NonNull_CollectionWithReferenceTypesAndNullElements_ReturnsCollectionWithoutNullelements()
        {
            IEnumerable<string> collection = new[]
            {
                "A", "B", null, "C", "D", null, "E"
            };

            var output = CollectionExtensions.NonNull(collection);

            CollectionAssert.AreEqual(new[]
            {
                "A", "B", "C", "D", "E"
            }, output);
        }

        [Test]
        public void NonEmpty_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<string> collection = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.NonEmpty(collection));
        }

        [Test]
        public void NonEmpty_CollectionEmptyElements_ReturnsCollectionWithoutEmptyElements()
        {
            IEnumerable<string> collection = new[]
            {
                "A", "B", null, "C", "D", "", "E", " ", "F"
            };

            var output = CollectionExtensions.NonEmpty(collection);

            CollectionAssert.AreEqual(new[]
            {
                "A", "B", "C", "D", "E", " ", "F"
            }, output);
        }

        [Test]
        public void NonEmptyOrWhiteSpace_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<string> collection = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.NonEmptyOrWhiteSpace(collection));
        }

        [Test]
        public void NonEmptyOrWhiteSpace_CollectionEmptyElements_ReturnsCollectionWithoutEmptyAndWhiteSpaceElements()
        {
            IEnumerable<string> collection = new[]
            {
                "A", "B", null, "C", "D", "", "E", " ", "F"
            };

            var output = CollectionExtensions.NonEmptyOrWhiteSpace(collection);

            CollectionAssert.AreEqual(new[]
            {
                "A", "B", "C", "D", "E", "F"
            }, output);
        }

        [Test]
        public void Lead_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            IEnumerable<int> collection = Enumerable.Range(0, 10);

            Assert.Throws<ArgumentOutOfRangeException>(() => CollectionExtensions.Lead(collection, -1));
        }

        [Test]
        public void Lead_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.Lead(collection));
        }

        [TestCase("0,1,2,3,4", 1, "(0,1),(1,2),(2,3),(3,4),(4,0)")]
        [TestCase("0,1,2,3,4", 2, "(0,2),(1,3),(2,4),(3,0),(4,0)")]
        [TestCase("0,1,2,3,4", 0, "(0,0),(1,1),(2,2),(3,3),(4,4)")]
        [TestCase("0,1,2,3,4", 10, "(0,0),(1,0),(2,0),(3,0),(4,0)")]
        public void Lead_WithTestCases_ProducesCorrectResults(string collectionString, int amount, string expectedString)
        {
            int[] collection = collectionString.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();

            var output = CollectionExtensions.Lead(collection, amount);
            var outputString = string.Join(",", output.Select(item => $"({item.Element},{item.LeadingElement})"));

            Assert.That(outputString, Is.EqualTo(expectedString));
        }

        [Test]
        public void Lag_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            IEnumerable<int> collection = Enumerable.Range(0, 10);

            Assert.Throws<ArgumentOutOfRangeException>(() => CollectionExtensions.Lag(collection, -1));
        }

        [Test]
        public void Lag_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.Lag(collection));
        }

        [TestCase("0,1,2,3,4", 1, "(0,0),(1,0),(2,1),(3,2),(4,3)")]
        [TestCase("0,1,2,3,4", 2, "(0,0),(1,0),(2,0),(3,1),(4,2)")]
        [TestCase("0,1,2,3,4", 0, "(0,0),(1,1),(2,2),(3,3),(4,4)")]
        [TestCase("0,1,2,3,4", 10, "(0,0),(1,0),(2,0),(3,0),(4,0)")]
        public void Lag_WithTestCases_ProducesCorrectResults(string collectionString, int amount, string expectedString)
        {
            int[] collection = collectionString.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();

            var output = CollectionExtensions.Lag(collection, amount);
            var outputString = string.Join(",", output.Select(item => $"({item.Element},{item.LaggingElement})"));

            Assert.That(outputString, Is.EqualTo(expectedString));
        }

        [Test]
        public void AddIndex_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.AddIndex(collection));
        }

        [Test]
        public void AddIndex_NoStartIndexSpecified_ProducesCorrectResults()
        {
            IEnumerable<string> collection = new[]
            {
                "A", "B", "C"
            };

            var output = CollectionExtensions.AddIndex(collection);

            CollectionAssert.AreEqual(new[]
            {
                new IndexedElement<string>(0, "A"),
                new IndexedElement<string>(1, "B"),
                new IndexedElement<string>(2, "C")
            }, output);
        }

        [Test]
        public void AddIndex_WithStartIndexSpecified_ProducesCorrectResults()
        {
            IEnumerable<string> collection = new[]
            {
                "A", "B", "C"
            };

            var output = CollectionExtensions.AddIndex(collection, 42);

            CollectionAssert.AreEqual(new[]
            {
                new IndexedElement<string>(42, "A"),
                new IndexedElement<string>(43, "B"),
                new IndexedElement<string>(44, "C")
            }, output);
        }

        [Test]
        public void GroupIf_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;
            Func<int, int, bool> groupPredicate = (i1, i2) => i2 == i1 + 1;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.GroupIf(collection, groupPredicate));
        }

        [Test]
        public void GroupIf_NullGroupPredicate_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = Enumerable.Range(0, 10);
            Func<int, int, bool> groupPredicate = null;

            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.GroupIf(collection, groupPredicate));
        }

        [Test]
        public void GroupIf_BasicRangeGrouping_ReturnsCorrectGrouping()
        {
            var input = new[]
            {
                1, 2, 3, 5, 6, 7, 9
            };

            var output = CollectionExtensions.GroupIf(input, (prev, current) => current == prev + 1).Select(group => $"{group.First()}-{group.Last()}").ToList();

            CollectionAssert.AreEqual(new[]
            {
                "1-3",
                "5-7",
                "9-9"
            }, output);
        }

        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = null;
            Func<int, int, bool> aggregatePredicate = (i1, i2) => true;
            Func<int> getSeed = () => 0;
            Func<int, int, int> aggregateFunc = (i1, i2) => i1 + i2;
            Func<int, int> resultSelector = i => i;
            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.AggregateIf(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
        }

        [Test]
        public void Constructor_NullAggregatePredicate_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = new int[0];
            Func<int, int, bool> aggregatePredicate = null;
            Func<int> getSeed = () => 0;
            Func<int, int, int> aggregateFunc = (i1, i2) => i1 + i2;
            Func<int, int> resultSelector = i => i;
            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.AggregateIf(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
        }

        [Test]
        public void Constructor_NullGetSeed_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = new int[0];
            Func<int, int, bool> aggregatePredicate = (i1, i2) => true;
            Func<int> getSeed = null;
            Func<int, int, int> aggregateFunc = (i1, i2) => i1 + i2;
            Func<int, int> resultSelector = i => i;
            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.AggregateIf(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
        }

        [Test]
        public void Constructor_NullAggregateFunc_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = new int[0];
            Func<int, int, bool> aggregatePredicate = (i1, i2) => true;
            Func<int> getSeed = () => 0;
            Func<int, int, int> aggregateFunc = null;
            Func<int, int> resultSelector = i => i;
            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.AggregateIf(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
        }

        [Test]
        public void Constructor_NullResultSelector_ThrowsArgumentNullException()
        {
            IEnumerable<int> collection = new int[0];
            Func<int, int, bool> aggregatePredicate = (i1, i2) => true;
            Func<int> getSeed = () => 0;
            Func<int, int, int> aggregateFunc = (i1, i2) => i1 + i2;
            Func<int, int> resultSelector = null;
            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.AggregateIf(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector));
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

            var output = CollectionExtensions.AggregateIf(collection,
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