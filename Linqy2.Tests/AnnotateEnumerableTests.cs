using System;
using System.Linq;

using NUnit.Framework;

// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace Linqy2.Tests
{
    [TestFixture]
    public class AnnotateEnumerableTests
    {
        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AnnotateEnumerable<int>(null, 0));
        }

        [Test]
        public void GetEnumerator_CollectionWithOneStringElement_ReturnsAnnotatedElementWithNullPreviousElement()
        {
            var collection = new[] { "ELEMENT" };
            var annotated = new AnnotateEnumerable<string>(collection).ToList();

            Assert.That(annotated.Count, Is.EqualTo(1));
            Assert.That(annotated[0].PreviousElement, Is.Null);
        }

        [Test]
        public void GetEnumerator_CollectionWithOneStringElementAndNonZeroStartIndex_ReturnsAnnotatedElementWithCorrectIndex()
        {
            var collection = new[] { "ELEMENT" };
            var annotated = new AnnotateEnumerable<string>(collection, 42).ToList();

            Assert.That(annotated.Count, Is.EqualTo(1));
            Assert.That(annotated[0].Index, Is.EqualTo(42));
        }

        [Test]
        public void GetEnumerator_CollectionWithOneStringElement_ReturnsAnnotatedElementWithNullNextElement()
        {
            var collection = new[] { "ELEMENT" };
            var annotated = new AnnotateEnumerable<string>(collection).ToList();

            Assert.That(annotated.Count, Is.EqualTo(1));
            Assert.That(annotated[0].NextElement, Is.Null);
        }

        [Test]
        public void GetEnumerator_CollectionWithOneStringElement_ReturnsAnnotatedElementWithIsFirstTrue()
        {
            var collection = new[] { "ELEMENT" };
            var annotated = new AnnotateEnumerable<string>(collection).ToList();

            Assert.That(annotated.Count, Is.EqualTo(1));
            Assert.That(annotated[0].IsFirst, Is.True);
        }

        [Test]
        public void GetEnumerator_CollectionWithOneStringElement_ReturnsAnnotatedElementWithIsLastTrue()
        {
            var collection = new[] { "ELEMENT" };
            var annotated = new AnnotateEnumerable<string>(collection).ToList();

            Assert.That(annotated.Count, Is.EqualTo(1));
            Assert.That(annotated[0].IsLast, Is.True);
        }

        [Test]
        public void GetEnumerator_BasicTestCaseWithTwoStrings()
        {
            var collection = new[] { "A", "B" };
            var annotated = new AnnotateEnumerable<string>(collection, 42).ToList();

            CollectionAssert.AreEqual(new[]
                                      {
                                          new AnnotatedElement<string>(42, "A", true, null, false, "B"),
                                          new AnnotatedElement<string>(43, "B", false, "A", true, null)
                                      }, annotated, new AnnotatedElementComparer<string>());
        }

        [Test]
        public void GetEnumerator_BasicTestCaseWithThreeStrings()
        {
            var collection = new[] { "A", "B", "C" };
            var annotated = new AnnotateEnumerable<string>(collection, 42).ToList();

            CollectionAssert.AreEqual(new[]
                                      {
                                          new AnnotatedElement<string>(42, "A", true, null, false, "B"),
                                          new AnnotatedElement<string>(43, "B", false, "A", false, "C"),
                                          new AnnotatedElement<string>(44, "C", false, "B", true, null)
                                      }, annotated, new AnnotatedElementComparer<string>());
        }
    }
}