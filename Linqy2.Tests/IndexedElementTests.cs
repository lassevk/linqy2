using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException

namespace Linqy2.Tests
{
    [TestFixture]
    public class IndexedElementTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void Index_AfterGivingValueInConstructor_RetainsGivenValue(int testcase)
        {
            var value = new IndexedElement<string>(testcase, "test");

            Assert.That(value.Index, Is.EqualTo(testcase));
        }

        [Test]
        [TestCase("test")]
        [TestCase("other test")]
        public void Element_AfterGivingValueInConstructor_RetainsGivenValue(string testcase)
        {
            var value = new IndexedElement<string>(0, testcase);

            Assert.That(value.Element, Is.EqualTo(testcase));
        }

        [Test]
        public void Equals_NullReference_ReturnsFalse()
        {
            var value = new IndexedElement<string>(42, "Meaning of life");

            bool result = value.Equals(null);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_ItselfBoxed_ReturnsTrue()
        {
            var value = new IndexedElement<string>(42, "Meaning of life");

            bool result = value.Equals((object)value);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_OtherIndexedElementWithDifferentIndex_ReturnsFalse()
        {
            var value1 = new IndexedElement<string>(42, "Meaning of life");
            var value2 = new IndexedElement<string>(41, "Meaning of life");

            bool result = value1.Equals(value2);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_OtherIndexedElementWithDifferentElements_ReturnsFalse()
        {
            var value1 = new IndexedElement<string>(42, "Meaning of life");
            var value2 = new IndexedElement<string>(42, "Meaning of liff");

            bool result = value1.Equals(value2);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_OtherIndexedElementWithSameValues_ReturnsTrue()
        {
            var value1 = new IndexedElement<string>(42, "Meaning of life");
            var value2 = new IndexedElement<string>(42, "Meaning of life");

            bool result = value1.Equals(value2);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GetHashCode_OtherIndexedElementWithSameValues_ReturnsSameValue()
        {
            var value1 = new IndexedElement<string>(42, "Meaning of life");
            var value2 = new IndexedElement<string>(42, "Meaning of life");

            Assert.That(value2.GetHashCode(), Is.EqualTo(value1.GetHashCode()));
        }

        [Test]
        public void GetHashCode_OtherIndexedElementWithDifferentIndex_ReturnsDifferentValue()
        {
            var value1 = new IndexedElement<string>(42, "Meaning of life");
            var value2 = new IndexedElement<string>(41, "Meaning of life");

            Assert.That(value2.GetHashCode(), Is.Not.EqualTo(value1.GetHashCode()));
        }

        [Test]
        public void GetHashCode_OtherIndexedElementWithDifferentElement_ReturnsDifferentValue()
        {
            var value1 = new IndexedElement<string>(42, "Meaning of life");
            var value2 = new IndexedElement<string>(42, "Meaning of liff");

            Assert.That(value2.GetHashCode(), Is.Not.EqualTo(value1.GetHashCode()));
        }
    }
}