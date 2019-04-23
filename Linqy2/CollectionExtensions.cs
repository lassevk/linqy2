using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This class implements the additional LINQ extension methods provided by Linqy.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Combines a lagging element with the current element.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to lag.
        /// </param>
        /// <param name="amount">
        /// The amount of lag to introduce.
        /// </param>
        /// <returns>
        /// A collection of <see cref="LagItem{T}"/> containing the current element from the collection
        /// and the lagging element from a previous position in the same collection.
        /// </returns>
        /// <remarks>
        /// A "Lagging element" is an element from a previous position in the collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="amount"/> is less than 0.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<LagItem<T>> Lag<T>([NotNull] this IEnumerable<T> collection, int amount = LagEnumerable<T>.DefaultAmount)
        {
            return new LagEnumerable<T>(collection, amount);
        }

        /// <summary>
        /// Combines a leading element with the current element.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to lead.
        /// </param>
        /// <param name="amount">
        /// The amount of lead to introduce.
        /// </param>
        /// <returns>
        /// A collection of <see cref="LeadItem{T}"/> containing the current element from the collection
        /// and the leading element from an upcoming position in the same collection.
        /// </returns>
        /// <remarks>
        /// A "Leading element" is an element from an upcoming position in the collection.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="amount"/> is less than 0.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<LeadItem<T>> Lead<T>([NotNull] this IEnumerable<T> collection, int amount = LeadEnumerable<T>.DefaultAmount)
        {
            return new LeadEnumerable<T>(collection, amount);
        }

        /// <summary>
        /// Adds an index to each element in the collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to add an index to.
        /// </param>
        /// <param name="startIndex">
        /// The starting index for the first element.
        /// </param>
        /// <returns>
        /// A collection of <see cref="IndexedElement{T}"/> containing the current element and its index.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<IndexedElement<T>> AddIndex<T>([NotNull] this IEnumerable<T> collection, int startIndex = AddIndexEnumerable<T>.DefaultStartIndex)
        {
            return new AddIndexEnumerable<T>(collection, startIndex);
        }

        /// <summary>
        /// Groups elements into partitions using a delegate to determine if the current element belongs to the same partition
        /// as the previous element.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to partition.
        /// </param>
        /// <param name="partitionPredicate">
        /// The predicate to use to determine if the current element belongs in the same partition
        /// as the previous element.
        /// </param>
        /// <returns>
        /// A collection of <see cref="List{T}"/>, each containing one partition.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="partitionPredicate"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<List<T>> PartitionIf<T>([NotNull] this IEnumerable<T> collection, [NotNull] Func<T, T, bool> partitionPredicate)
        {
            return new PartitionIfEnumerable<T>(collection, partitionPredicate);
        }

        /// <summary>
        /// Groups elements into partitions using a delegate to extract a key used to determine if the element should start
        /// a new partition or be added to the current one.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <typeparam name="TKey">
        /// The type of keys to extract from the elements.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to partition.
        /// </param>
        /// <param name="keySelector">
        /// A delegate used to extract a key from the elements. The keys are compared and if two sequential keys from
        /// the collection returns different keys, a new partition is started from the second of the two.
        /// </param>
        /// <param name="keyComparer">
        /// A <see cref="IEqualityComparer{T}"/> used to compare keys, or <c>null</c> if the default comparer for
        /// <typeparamref name="TKey"/> should be used.
        /// </param>
        /// <returns>
        /// A collection of <see cref="List{T}"/>, each containing one partition.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="keySelector"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// Note that this method is different from `Enumerable.GroupBy` in the sense that it only detects boundary conditions
        /// between partitions, if a new partition is started because the keys different, and later elements could be said
        /// to belong to the previous partition, GroupBy would collect them all, whereas PartitionBy would start new
        /// partitions. PartitionBy is more a streaming interface and does not collect elements into groups from the entire
        /// collection.
        /// </remarks>
        [NotNull]
        public static IEnumerable<List<T>> PartitionBy<T, TKey>([NotNull] this IEnumerable<T> collection, [NotNull] Func<T, TKey> keySelector, IEqualityComparer<TKey> keyComparer = null)
        {
            return new PartitionByEnumerable<T, TKey>(collection, keySelector, keyComparer ?? EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Aggregates elements using a delegate to determine if the current element should be
        /// aggregated into the current aggregated value, and a delegate to do the aggregration.
        /// </summary>
        /// <typeparam name="TElement">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <typeparam name="TAggregate">
        /// The type of aggregated values to produce.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to aggregate.
        /// </param>
        /// <param name="aggregatePredicate">
        /// The predicate that determines if the current element should be aggregated into the same
        /// aggregated value as the previous element.
        /// </param>
        /// <param name="aggregateFunc">
        /// The delegate that will aggregate the current element into an aggregated value.
        /// </param>
        /// <returns>
        /// A collection of the aggregated values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="aggregatePredicate"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="aggregateFunc"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<TAggregate> AggregateIf<TElement, TAggregate>([NotNull] this IEnumerable<TElement> collection, [NotNull] Func<TElement, TElement, bool> aggregatePredicate, [NotNull] Func<TAggregate, TElement, TAggregate> aggregateFunc)
        {
            return new AggregateIfEnumerable<TElement, TAggregate, TAggregate>(collection, aggregatePredicate, () => default, aggregateFunc, aggregate => aggregate);
        }

        /// <summary>
        /// Aggregates elements using a delegate to determine if the current element should be
        /// aggregated into the current aggregated value, and a delegate to do the aggregration.
        /// </summary>
        /// <typeparam name="TElement">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <typeparam name="TAggregate">
        /// The type of aggregated values to produce.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to aggregate.
        /// </param>
        /// <param name="aggregatePredicate">
        /// The predicate that determines if the current element should be aggregated into the same
        /// aggregated value as the previous element.
        /// </param>
        /// <param name="getSeed">
        /// A delegate that will be used to provide the initial aggregated value for a new group of
        /// elements.</param>
        /// <param name="aggregateFunc">
        /// The delegate that will aggregate the current element into an aggregated value.
        /// </param>
        /// <returns>
        /// A collection of the aggregated values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="aggregatePredicate"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="getSeed"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="aggregateFunc"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<TAggregate> AggregateIf<TElement, TAggregate>([NotNull] this IEnumerable<TElement> collection, [NotNull] Func<TElement, TElement, bool> aggregatePredicate, [NotNull] Func<TAggregate> getSeed, [NotNull] Func<TAggregate, TElement, TAggregate> aggregateFunc)
        {
            return new AggregateIfEnumerable<TElement, TAggregate, TAggregate>(collection, aggregatePredicate, getSeed, aggregateFunc, aggregate => aggregate);
        }

        /// <summary>
        /// Aggregates elements using a delegate to determine if the current element should be
        /// aggregated into the current aggregated value, and a delegate to do the aggregration.
        /// </summary>
        /// <typeparam name="TElement">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <typeparam name="TAggregate">
        /// The type of aggregated values to produce.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of result to produce in the final collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to aggregate.
        /// </param>
        /// <param name="aggregatePredicate">
        /// The predicate that determines if the current element should be aggregated into the same
        /// aggregated value as the previous element.
        /// </param>
        /// <param name="aggregateFunc">
        /// The delegate that will aggregate the current element into an aggregated value.
        /// </param>
        /// <param name="resultSelector">
        /// The delegate that takes the aggregated values and produce the final output
        /// from this collection.
        /// </param>
        /// <returns>
        /// A collection of the aggregated values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="aggregatePredicate"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="aggregateFunc"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="resultSelector"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<TResult> AggregateIf<TElement, TAggregate, TResult>([NotNull] this IEnumerable<TElement> collection, [NotNull] Func<TElement, TElement, bool> aggregatePredicate, [NotNull] Func<TAggregate, TElement, TAggregate> aggregateFunc, [NotNull] Func<TAggregate, TResult> resultSelector)
        {
            return new AggregateIfEnumerable<TElement, TAggregate, TResult>(collection, aggregatePredicate, () => default, aggregateFunc, resultSelector);
        }

        /// <summary>
        /// Aggregates elements using a delegate to determine if the current element should be
        /// aggregated into the current aggregated value, and a delegate to do the aggregration.
        /// </summary>
        /// <typeparam name="TElement">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <typeparam name="TAggregate">
        /// The type of aggregated values to produce.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of result to produce in the final collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to aggregate.
        /// </param>
        /// <param name="aggregatePredicate">
        /// The predicate that determines if the current element should be aggregated into the same
        /// aggregated value as the previous element.
        /// </param>
        /// <param name="getSeed">
        /// A delegate that will be used to provide the initial aggregated value for a new group of
        /// elements.
        /// </param>
        /// <param name="aggregateFunc">
        /// The delegate that will aggregate the current element into an aggregated value.
        /// </param>
        /// <param name="resultSelector">
        /// The delegate that takes the aggregated values and produce the final output
        /// from this collection.
        /// </param>
        /// <returns>
        /// A collection of the aggregated values.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="aggregatePredicate"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="getSeed"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="aggregateFunc"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="resultSelector"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<TResult> AggregateIf<TElement, TAggregate, TResult>([NotNull] this IEnumerable<TElement> collection, [NotNull] Func<TElement, TElement, bool> aggregatePredicate, [NotNull] Func<TAggregate> getSeed, [NotNull] Func<TAggregate, TElement, TAggregate> aggregateFunc, [NotNull] Func<TAggregate, TResult> resultSelector)
        {
            return new AggregateIfEnumerable<TElement, TAggregate, TResult>(collection, aggregatePredicate, getSeed, aggregateFunc, resultSelector);
        }

        /// <summary>
        /// Returns the first <paramref name="amount"/> elements from the collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to return the first <paramref name="amount"/> elements from.
        /// </param>
        /// <param name="amount">
        /// The number of elements to return from <paramref name="collection"/>.
        /// </param>
        /// <returns>
        /// A collection containing the first <paramref name="amount"/> elements from the
        /// <paramref name="collection"/>, or less if the collection doesn't have that many
        /// elements.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// This is really just a call to <see cref="Enumerable.Take{TSource}"/> but is included
        /// for parity with <see cref="ExceptFirst{T}"/> and <see cref="Last{T}"/>.
        /// </remarks>
        [NotNull]
        public static IEnumerable<T> First<T>([NotNull] this IEnumerable<T> collection, int amount)
        {
            IEnumerable<T> result = collection.Take(amount);
            return result;
        }

        /// <summary>
        /// Returns all the elements that comes after the first <paramref name="amount"/> elements from the collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to skip the first <paramref name="amount"/> elements in.
        /// </param>
        /// <param name="amount">
        /// The number of elements to skip at the start of <paramref name="collection"/>.
        /// </param>
        /// <returns>
        /// A collection containing the last <paramref name="amount"/> elements from the
        /// <paramref name="collection"/>, or less if the collection doesn't have that many
        /// elements.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// This is really just a call to <see cref="Enumerable.Skip{TSource}"/> but is included
        /// for parity with <see cref="First{T}"/> and <see cref="ExceptLast{T}"/>.
        /// </remarks>
        [NotNull]
        public static IEnumerable<T> ExceptFirst<T>([NotNull] this IEnumerable<T> collection, int amount)
        {
            IEnumerable<T> result = collection.Skip(amount);
            return result;
        }

        /// <summary>
        /// Returns all the elements that comes before the last <paramref name="amount"/> elements from the collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to return the elements from.
        /// </param>
        /// <param name="amount">
        /// The number of elements to ignore at the end of <paramref name="collection"/>.
        /// </param>
        /// <returns>
        /// A collection containing the last <paramref name="amount"/> elements from the
        /// <paramref name="collection"/>, or less if the collection doesn't have that many
        /// elements.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// This method will use an optimized implementation to return the elements if the
        /// underlying <paramref name="collection"/> implements <see cref="IList{T}"/>, otherwise
        /// it will fully materialize the <paramref name="collection"/> and ignore the
        /// last <paramref name="amount"/> elements and return the rest.
        /// </remarks>
        [NotNull]
        public static IEnumerable<T> ExceptLast<T>([NotNull] this IEnumerable<T> collection, int amount)
        {
            return new ExceptLastEnumerable<T>(collection, amount);
        }

        /// <summary>
        /// Returns the last <paramref name="amount"/> elements from the collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to return the last <paramref name="amount"/> elements from.
        /// </param>
        /// <param name="amount">
        /// The number of elements to return from <paramref name="collection"/>.
        /// </param>
        /// <returns>
        /// A collection containing the last <paramref name="amount"/> elements from the
        /// <paramref name="collection"/>, or less if the collection doesn't have that many
        /// elements.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// This method will use an optimized implementation to return the elements if the
        /// underlying <paramref name="collection"/> implements <see cref="IList{T}"/>, otherwise
        /// it will fully materialize the <paramref name="collection"/> and only gather the
        /// last <paramref name="amount"/> elements and return those.
        /// </remarks>
        [NotNull]
        public static IEnumerable<T> Last<T>([NotNull] this IEnumerable<T> collection, int amount)
        {
            return new LastEnumerable<T>(collection, amount);
        }

        /// <summary>
        /// Returns the unique elements as given by the specified selector and comparer.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <typeparam name="TSelector">
        /// The type of the selector for each element that is used to determine equality and uniqueness.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to return distinct elements from.
        /// </param>
        /// <param name="selectorFunc">
        /// A selector delegate that for each element returns the data used to determine uniqueness.
        /// </param>
        /// <param name="equalityComparer">
        /// The comparer, or <c>null</c> if default comparer should be used, that determines
        /// uniqueness.
        /// </param>
        /// <returns>
        /// A collection of all the elements that are distinct according to comparing the
        /// selected information from each element.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="selectorFunc"/> is <c>null</c>.</para>
        /// </exception>
        /// <remarks>
        /// Note that if you think of this method as grouping by the selected properties,
        /// this method will return the first element of each such group, according to the
        /// ordering in the original collection.
        /// </remarks>
        [NotNull]
        public static IEnumerable<T> DistinctBy<T, TSelector>([NotNull] this IEnumerable<T> collection, [NotNull] Func<T, TSelector> selectorFunc, [CanBeNull] IEqualityComparer<TSelector> equalityComparer = null)
        {
            return new DistinctByEnumerable<T, TSelector>(collection, selectorFunc, equalityComparer);
        }

        /// <summary>
        /// Creates batches from the underlying collection with a fixed number of elements.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to partition.
        /// </param>
        /// <param name="batchSize">
        /// The size of each partition.
        /// </param>
        /// <returns>
        /// A collection of <see cref="List{T}"/>, one for each batch.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="batchSize"/> is less than 1.</para>
        /// </exception>
        /// <remarks>
        /// Note that the last batch may be short a few elements if the <paramref name="collection"/> does not have a number of elements that is
        /// divisible by the <paramref name="batchSize"/>.
        /// </remarks>
        [NotNull]
        public static IEnumerable<List<T>> Batch<T>([NotNull] this IEnumerable<T> collection, int batchSize)
        {
            return new BatchEnumerable<T>(collection, batchSize);
        }

        /// <summary>
        /// Returns a concatenated collection consisting of the <paramref name="collection"/>
        /// followed by the <paramref name="additionalElements"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to append elements to.
        /// </param>
        /// <param name="additionalElements">
        /// The elements to append to the <paramref name="collection"/>.
        /// </param>
        /// <returns>
        /// A concatenated collection consisting of the <paramref name="collection"/> followed by the <paramref name="additionalElements"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="additionalElements"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<T> Append<T>([NotNull] this IEnumerable<T> collection, [NotNull] params T[] additionalElements)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (additionalElements == null)
                throw new ArgumentNullException(nameof(additionalElements));

            IEnumerable<T> result = collection.Concat(additionalElements);
            return result;
        }

        /// <summary>
        /// Filters the collection to ommit the single specified element.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to filter.
        /// </param>
        /// <param name="exceptThis">
        /// The element to ommit.
        /// </param>
        /// <returns>
        /// A collection that will ommit <paramref name="exceptThis"/> from the <paramref name="collection"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<T> Except<T>([NotNull] this IEnumerable<T> collection, T exceptThis)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            IEnumerable<T> result = collection.Except(new[]
                                                       {
                                                           exceptThis
                                                       });
            return result;
        }

        /// <summary>
        /// Filters the collection to only return non-null elements.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to filter.
        /// </param>
        /// <returns>
        /// A collection containing only non-null elements from the underlying <paramref name="collection"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull, ItemNotNull]
        public static IEnumerable<T> NonNull<T>([NotNull] this IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return collection.Where(element => element != null);
        }

        /// <summary>
        /// Filters the collection to only return non-null and non-empty strings.
        /// </summary>
        /// <param name="collection">
        /// The collection of strings to filter.
        /// </param>
        /// <returns>
        /// A collection containing only strings from the underlying <paramref name="collection"/> that are both non-null and non-empty strings.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull, ItemNotNull]
        public static IEnumerable<string> NonEmpty([NotNull] this IEnumerable<string> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return collection.Where(element => !string.IsNullOrEmpty(element));
        }

        /// <summary>
        /// Filters the collection to only return non-null, non-empty and non-whitespace-only strings.
        /// </summary>
        /// <param name="collection">
        /// The collection of strings to filter.
        /// </param>
        /// <returns>
        /// A collection containing only strings from the underlying <paramref name="collection"/> that are both non-null, non-empty and does not only contain whitespace.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull, ItemNotNull]
        public static IEnumerable<string> NonEmptyOrWhiteSpace([NotNull] this IEnumerable<string> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            return collection.Where(element => !string.IsNullOrEmpty(element) && !OnlyConsistsOfWhiteSpace(element));
        }

        private static bool OnlyConsistsOfWhiteSpace([NotNull] string value)
        {
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (int index = 0; index < value.Length; index++)
                if (!char.IsWhiteSpace(value, index))
                    return false;
            return true;
        }

        /// <summary>
        /// Filters the collection to only return all the values that have the same selector as the biggest selector. This is similar to
        /// a LINQ query along the lines of <c>Collection.Where(item => selector(item) == Collection.Max(item => selector(item)))</c>
        /// except that it only evaluates the collection once.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <typeparam name="TSelector">
        /// The type of values to consider when looking for "the biggest value".
        /// </typeparam>
        /// <param name="collection">
        /// The collection to filter.
        /// </param>
        /// <param name="selectorFunc">
        /// A delegate used to extract the value to find the maximum of.
        /// </param>
        /// <param name="comparer">
        /// A <see cref="IComparer{T}"/> object that will be used to determine which of the selected values are the biggest,
        /// or <c>null</c> if the default comparer for <typeparamref name="TSelector"/> should be used.
        /// </param>
        /// <returns>
        /// A collection of all the elements from the collection that has the same selected value, that is the biggest selected value of all
        /// the provided elements.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="selectorFunc"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull, ItemNotNull]
        public static IEnumerable<T> AllWithMax<T, TSelector>([NotNull] this IEnumerable<T> collection, [NotNull] Func<T, TSelector> selectorFunc, [CanBeNull] IComparer<TSelector> comparer = null)
        {
            return new AllWithMaxEnumerable<T, TSelector>(collection, selectorFunc, comparer);
        }

        /// <summary>
        /// Filters the collection to only return all the values that have the same selector as the smallest selector. This is similar to
        /// a LINQ query along the lines of <c>Collection.Where(item => selector(item) == Collection.Min(item => selector(item)))</c>
        /// except that it only evaluates the collection once.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <typeparam name="TSelector">
        /// The type of values to consider when looking for "the smallest value".
        /// </typeparam>
        /// <param name="collection">
        /// The collection to filter.
        /// </param>
        /// <param name="selectorFunc">
        /// A delegate used to extract the value to find the minimum of.
        /// </param>
        /// <param name="comparer">
        /// A <see cref="IComparer{T}"/> object that will be used to determine which of the selected values are the smallest,
        /// or <c>null</c> if the default comparer for <typeparamref name="TSelector"/> should be used.
        /// </param>
        /// <returns>
        /// A collection of all the elements from the collection that has the same selected value, that is the smallest selected value of all
        /// the provided elements.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="selectorFunc"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull, ItemNotNull]
        public static IEnumerable<T> AllWithMin<T, TSelector>([NotNull] this IEnumerable<T> collection, [NotNull] Func<T, TSelector> selectorFunc, [CanBeNull] IComparer<TSelector> comparer = null)
        {
            return new AllWithMinEnumerable<T, TSelector>(collection, selectorFunc, comparer);
        }

        /// <summary>
        /// Returns elements from a sequence until a specified condition is <c>true</c>, up to and including the item for
        /// which the condition was <c>true</c>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// A sequence to return elements from.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> that contains the elements from the input <paramref name="collection"/>
        /// that occur up to and including the element for which the test passes.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="predicate"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull, ItemNotNull]
        public static IEnumerable<T> TakeUntil<T>([NotNull] this IEnumerable<T> collection, [NotNull] Predicate<T> predicate)
        {
            return new TakeUntilEnumerable<T>(collection, predicate);
        }

        /// <summary>
        /// Annotates the given collection by encapsulating each element in a small struct with extra information,
        /// such as the index of the element, whether the element is the first and/or last element of the collection, and so on.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to add an index to.
        /// </param>
        /// <param name="startIndex">
        /// The starting index for the first element.
        /// </param>
        /// <returns>
        /// A collection of <see cref="AnnotatedElement{T}"/> containing the current element and its index.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        [NotNull]
        public static IEnumerable<AnnotatedElement<T>> Annotate<T>([NotNull] this IEnumerable<T> collection, int startIndex = AnnotateEnumerable<T>.DefaultStartIndex)
        {
            return new AnnotateEnumerable<T>(collection, startIndex);
        }

        /// <summary>
        /// Evaluates whether the collection has at least the specified number of elements. This is done using early
        /// exit so as soon as the answer is found during enumeration over the collection it will be returned.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to verify that has at least <paramref name="count"/> elements.
        /// </param>
        /// <param name="count">
        /// The number of elements to verify that <paramref name="collection"/> has.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="collection"/> has at least <paramref name="count"/> elements;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Note that if <paramref name="count"/> is equal to or less than 0, the collection is not consumed or enumerated
        /// at all, the result will simply be <c>true</c>.
        /// </remarks>
        public static bool HasAtLeast<T>([NotNull] this IEnumerable<T> collection, int count)
        {
            return new HasAtLeastCollectionCriteria<T>(collection, count).GetValue();
        }

        /// <summary>
        /// Evaluates whether the collection has at most the specified number of elements. This is done using early
        /// exit so as soon as the answer is found during enumeration over the collection it will be returned.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the collection.
        /// </typeparam>
        /// <param name="collection">
        /// The collection to verify that has at most <paramref name="count"/> elements.
        /// </param>
        /// <param name="count">
        /// The number of elements to verify that <paramref name="collection"/> has.
        /// </param>
        /// <returns>
        /// <c>true</c> if <paramref name="collection"/> has at most <paramref name="count"/> elements;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Note that if <paramref name="count"/> is less than 0, the collection is not consumed or enumerated
        /// at all, the result will simply be <c>false</c>.
        /// </remarks>
        public static bool HasAtMost<T>([NotNull] this IEnumerable<T> collection, int count)
        {
            return new HasAtMostCollectionCriteria<T>(collection, count).GetValue();
        }
    }
}