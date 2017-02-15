using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable CollectionNeverUpdated.Local

namespace Linqy.Tests
{
    [TestFixture]
    public class AllWithMaxEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AllWithMaxEnumerable<int, int>(null, i => i, Comparer<int>.Default));
        }

        [Test]
        public void Constructor_NullSelectorFunc_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AllWithMaxEnumerable<int, int>(new int[0], null, Comparer<int>.Default));
        }

        [Test]
        public void GetEnumerator_EmptyCollection_ReturnsEmptyResult()
        {
            var collection = new List<string>();

            var output = new AllWithMaxEnumerable<string, int>(collection, s => s?.Length ?? 0).ToList();

            Assert.That(output, Is.Empty);
        }

        [Test]
        public void GetEnumerator_CollectionWithOneElement_ReturnsCollectionWithThatElement()
        {
            var collection = new List<string>
            {
                "AAA"
            };

            var output = new AllWithMaxEnumerable<string, int>(collection, s => s?.Length ?? 0).ToList();

            CollectionAssert.AreEqual(collection, output);
        }

        [Test]
        public void GetEnumerator_CollectionWithTwoElementsOfDifferentLengths_ReturnsCollectionWithTheLongestElement()
        {
            var collection = new List<string>
            {
                "AAA",
                "BBBB",
            };

            var output = new AllWithMaxEnumerable<string, int>(collection, s => s?.Length ?? 0).ToList();

            CollectionAssert.AreEqual(new[]
            {
                "BBBB"
            }, output);
        }

        [Test]
        public void GetEnumerator_CollectionWithMultipleElements_ReturnsCollectionWithAllTheElementsThatHasTheSameLengthAsTheLongestElement()
        {
            var collection = new List<string>
            {
                "AAA",
                "BBBB",
                "CCC",
                "DDDD",
                "EEE",
                "FFFF",
                "GG",
                "H",
                "IIII",
            };

            var output = new AllWithMaxEnumerable<string, int>(collection, s => s?.Length ?? 0).ToList();

            CollectionAssert.AreEqual(new[]
            {
                "BBBB",
                "DDDD",
                "FFFF",
                "IIII"
            }, output);
        }
    }
}
