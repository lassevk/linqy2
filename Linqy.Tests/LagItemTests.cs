using System;
using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace Linqy.Tests
{
    [TestFixture]
    public class LagItemTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void Element_AfterGivingValueInConstructor_RetainsGivenValue(int testcase)
        {
            var value = new LagItem<int>(testcase, 42);

            Assert.That(value.Element, Is.EqualTo(testcase));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void LaggingElement_AfterGivingValueInConstructor_RetainsGivenValue(int testcase)
        {
            var value = new LagItem<int>(0, testcase);

            Assert.That(value.LaggingElement, Is.EqualTo(testcase));
        }

        [Test]
        public void Equals_NullReference_ReturnsFalse()
        {
            var value = new LagItem<int>(42, 17);

            bool result = value.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_ItselfBoxed_ReturnsTrue()
        {
            var value = new LagItem<int>(42, 17);

            bool result = value.Equals((object)value);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_OtherLagItemWithDifferentElement_ReturnsFalse()
        {
            var value1 = new LagItem<int>(42, 17);
            var value2 = new LagItem<int>(41, 17);

            bool result = value1.Equals(value2);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_OtherLagItemWithDifferentLaggingElement_ReturnsFalse()
        {
            var value1 = new LagItem<int>(42, 17);
            var value2 = new LagItem<int>(42, 18);

            bool result = value1.Equals(value2);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_OtherLagItemWithSameValues_ReturnsTrue()
        {
            var value1 = new LagItem<int>(42, 17);
            var value2 = new LagItem<int>(42, 17);

            bool result = value1.Equals(value2);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetHashCode_OtherLagItemWithSameValues_ReturnsSameValue()
        {
            var value1 = new LagItem<int>(42, 17);
            var value2 = new LagItem<int>(42, 17);

            Assert.That(value2.GetHashCode(), Is.EqualTo(value1.GetHashCode()));
        }

        [Test]
        public void GetHashCode_OtherLagItemWithDifferentElement_ReturnsDifferentValue()
        {
            var value1 = new LagItem<int>(42, 17);
            var value2 = new LagItem<int>(41, 17);

            Assert.That(value2.GetHashCode(), Is.Not.EqualTo(value1.GetHashCode()));
        }

        [Test]
        public void GetHashCode_OtherLagItemWithDifferentLaggingElement_ReturnsDifferentValue()
        {
            var value1 = new LagItem<int>(42, 17);
            var value2 = new LagItem<int>(42, 16);

            Assert.That(value2.GetHashCode(), Is.Not.EqualTo(value1.GetHashCode()));
        }
    }
}