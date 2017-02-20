using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace Linqy.Tests
{
    [TestFixture]
    public class TakeUntilEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TakeUntilEnumerable<string>(null, s => true));
        }

        [Test]
        public void Constructor_NullPredicate_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TakeUntilEnumerable<string>(new List<string>(), null));
        }

        [Test]
        public void GetEnumerator_WithEmptyCollection_ReturnsEmptyCollection()
        {
            var input = new List<string>();
            var predicate = new Predicate<string>(s => true);

            var output = new TakeUntilEnumerable<string>(input, predicate).ToList();

            CollectionAssert.IsEmpty(output);
        }

        [Test]
        public void GetEnumerator_WithPredicateThatReturnsFalseForAllItems_ReturnsWholeCollection()
        {
            var input = Enumerable.Range(1, 100);
            var predicate = new Predicate<int>(s => false);

            var output = new TakeUntilEnumerable<int>(input, predicate).ToList();

            CollectionAssert.AreEqual(input, output);
        }

        [Test]
        public void GetEnumerator_WithPredicateThatReturnsTrueForOneItem_ReturnsAllItemsUpToAndIncludingThatItem()
        {
            var input = Enumerable.Range(1, 100);
            var predicate = new Predicate<int>(i => i == 42);

            var output = new TakeUntilEnumerable<int>(input, predicate).ToList();

            CollectionAssert.AreEqual(Enumerable.Range(1, 42), output);
        }

        [Test]
        public void GetEnumerator_WithPredicateThatReturnsTrueForOneItemThatIsRepeatedInInputCollection_ReturnsAllItemsUpToAndIncludingThatFirstItem()
        {
            var input = new[] { 1, 2, 3, 3, 3, 3, 3, 4, 5, 6 };
            var predicate = new Predicate<int>(i => i == 3);

            var output = new TakeUntilEnumerable<int>(input, predicate).ToList();

            CollectionAssert.AreEqual(Enumerable.Range(1, 3), output);
        }
    }
}
