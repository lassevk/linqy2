using System;
using System.Collections.Generic;

using NUnit.Framework;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace Linqy.Tests
{
    [TestFixture]
    public class CollectionCriteriaTests
    {
        [Test]
        public void HasAtLeastCollectionCriteria_Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HasAtLeastCollectionCriteria<int>(null, 0));
        }

        [Test]
        public void HasAtMostCollectionCriteria_Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HasAtMostCollectionCriteria<int>(null, 0));
        }

        public static IEnumerable<TestCaseData> HasAtLeastCollectionCriteria_GetValue_TestCases()
        {
            yield return new TestCaseData(new int[0], 0, true).SetName("Empty collection, count = 0, returns true");
            yield return new TestCaseData(new int[1], 0, true).SetName("Non-empty collection, count = 0, returns true");
            yield return new TestCaseData(new int[2], 2, true).SetName("Non-empty collection, count = length of collection, returns true");
            yield return new TestCaseData(new int[2], 1, true).SetName("Non-empty collection, count < length of collection, returns true");
            yield return new TestCaseData(new int[2], 3, false).SetName("Non-empty collection, count > length of collection, returns false");
        }

        [Test]
        [TestCaseSource(nameof(HasAtLeastCollectionCriteria_GetValue_TestCases))]
        public void HasAtLeastCollectionCriteria_GetValue_WithTestCases(IEnumerable<int> collection, int count, bool expected)
        {
            var output = new HasAtLeastCollectionCriteria<int>(collection, count).GetValue();

            Assert.That(output, Is.EqualTo(expected));
        }

        public static IEnumerable<TestCaseData> HasAtMostCollectionCriteria_GetValue_TestCases()
        {
            yield return new TestCaseData(new int[0], 0, true).SetName("Empty collection, count = 0, returns true");
            yield return new TestCaseData(new int[1], 0, false).SetName("Non-empty collection, count = 0, returns false");
            yield return new TestCaseData(new int[2], 2, true).SetName("Non-empty collection, count = length of collection, returns true");
            yield return new TestCaseData(new int[2], 1, false).SetName("Non-empty collection, count < length of collection, returns false");
            yield return new TestCaseData(new int[2], 3, true).SetName("Non-empty collection, count > length of collection, returns true");
        }

        [Test]
        [TestCaseSource(nameof(HasAtMostCollectionCriteria_GetValue_TestCases))]
        public void HasAtMostCollectionCriteria_GetValue_WithTestCases(IEnumerable<int> collection, int count, bool expected)
        {
            var output = new HasAtMostCollectionCriteria<int>(collection, count).GetValue();

            Assert.That(output, Is.EqualTo(expected));
        }
    }
}