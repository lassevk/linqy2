using System;
using System.Linq;

using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace Linqy2.Tests
{
    [TestFixture]
    public class PartitionIfEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PartitionIfEnumerable<int>(null, (i1, i2) => true));
        }

        [Test]
        public void Constructor_NullGroupPredicate_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PartitionIfEnumerable<int>(new int[0], null));
        }

        [Test]
        public void GetEnumerator_BasicRangeGrouping_ReturnsCorrectGrouping()
        {
            var input = new[]
            {
                1, 2, 3, 5, 6, 7, 9
            };

            var output = new PartitionIfEnumerable<int>(input, (prev, current) => current == prev + 1).Select(group => $"{group.First()}-{group.Last()}").ToList();

            CollectionAssert.AreEqual(new[]
            {
                "1-3",
                "5-7",
                "9-9"
            }, output);
        }
    }
}
