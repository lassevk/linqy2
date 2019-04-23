using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace Linqy2.Tests
{
    [TestFixture]
    public class DistinctByEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DistinctByEnumerable<int, int>(null, i => i, EqualityComparer<int>.Default));
        }

        [Test]
        public void Constructor_NullSelectorFunc_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DistinctByEnumerable<int, int>(new int[0], null, EqualityComparer<int>.Default));
        }

        [Test]
        public void GetEnumerator_CollectionWithStringsWithTheSameLength_OnlyReturnsStringWithUniqueLengths()
        {
            var collection = new[]
            {
                "A",
                "B",
                "CC",
                "DDD",
                "EEEE",
                "FFF",
                "GG",
                "HHHH"
            };

            var enumerable = new DistinctByEnumerable<string, int>(collection, s => s.Length);
            var output = enumerable.ToList();

            CollectionAssert.AreEqual(new[]
            {
                "A",
                "CC",
                "DDD",
                "EEEE"
            }, output);
        }
    }
}
