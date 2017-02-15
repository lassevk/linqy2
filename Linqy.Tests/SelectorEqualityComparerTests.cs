using System;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace Linqy.Tests
{
    [TestFixture]
    public class SelectorEqualityComparerTests
    {
        [Test]
        public static void Constructor()
        {
            Assert.Throws<ArgumentNullException>(() => new SelectorEqualityComparer<string, int>(null));
        }

        [Test]
        public static void Equals_TwoDifferentStringsOfEqualLength_ReturnsTrue()
        {
            var comparer = new SelectorEqualityComparer<string, int>(s => s?.Length ?? 0);

            var output = comparer.Equals("AAA", "BBB");

            Assert.That(output, Is.True);
        }

        [Test]
        public static void GetHashCode_OfStringWithLength3_Returns3()
        {
            // This exploits the fact that the hash code of an Int32 is the Int32 itself.
            var comparer = new SelectorEqualityComparer<string, int>(s => s?.Length ?? 0);

            var output = comparer.GetHashCode("AAA");

            Assert.That(output, Is.EqualTo(3));
        }
    }
}
