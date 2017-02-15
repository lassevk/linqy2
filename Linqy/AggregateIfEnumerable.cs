using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.AddIndex{T}"/>.
    /// </summary>
    /// <typeparam name="TElement">
    /// The type of elements in the collection.
    /// </typeparam>
    /// <typeparam name="TAggregate">
    /// The type of value to aggregate into.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of value to produce from the aggregate operation.
    /// </typeparam>
    public class AggregateIfEnumerable<TElement, TAggregate, TResult> : WrappingEnumerable<TElement, TResult>
    {
        [NotNull]
        private readonly Func<TElement, TElement, bool> _AggregatePredicate;

        [NotNull]
        private readonly Func<TAggregate> _GetSeed;

        [NotNull]
        private readonly Func<TAggregate, TElement, TAggregate> _AggregateFunc;

        [NotNull]
        private readonly Func<TAggregate, TResult> _ResultSelector;

        /// <summary>
        /// Constructs a new instance of the <see cref="AggregateIfEnumerable{TElement,TAggregate,TResult}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to aggregate.
        /// </param>
        /// <param name="aggregatePredicate">
        /// The predicate that determines if the current element should be aggregated into the same aggregated value as
        /// the previous element.</param>
        /// <param name="getSeed">
        /// A delegate that will be used to provide the initial aggregated value for a new group of
        /// elements.</param>
        /// <param name="aggregateFunc">
        /// The delegate that aggregates values together.
        /// </param>
        /// <param name="resultSelector">
        /// The delegate that takes the aggregated values and produce the final output
        /// from this collection.
        /// </param>
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
        public AggregateIfEnumerable([NotNull] IEnumerable<TElement> collection, [NotNull] Func<TElement, TElement, bool> aggregatePredicate, [NotNull] Func<TAggregate> getSeed, [NotNull] Func<TAggregate, TElement, TAggregate> aggregateFunc, [NotNull] Func<TAggregate, TResult> resultSelector)
            : base(collection)
        {
            if (aggregatePredicate == null)
                throw new ArgumentNullException(nameof(aggregatePredicate));
            if (getSeed == null)
                throw new ArgumentNullException(nameof(getSeed));
            if (aggregateFunc == null)
                throw new ArgumentNullException(nameof(aggregateFunc));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            _AggregatePredicate = aggregatePredicate;
            _GetSeed = getSeed;
            _AggregateFunc = aggregateFunc;
            _ResultSelector = resultSelector;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override IEnumerator<TResult> GetEnumerator()
        {
            var aggregator = _GetSeed();
            bool first = true;
            foreach (var element in Collection.Lag())
            {
                if (first)
                {
                    aggregator = _AggregateFunc(aggregator, element.Element);
                    first = false;
                }
                else if (_AggregatePredicate(element.LaggingElement, element.Element))
                {
                    aggregator = _AggregateFunc(aggregator, element.Element);
                }
                else
                {
                    yield return _ResultSelector(aggregator);
                    aggregator = _AggregateFunc(_GetSeed(), element.Element);
                }
            }
            if (!first)
                yield return _ResultSelector(aggregator);
        }
    }
}