using System;
using System.Collections.Generic;
using JetBrains.Annotations;

using static Linqy.ReSharperEnsurances;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.DistinctBy{T,TSelector}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    /// <typeparam name="TSelector">
    /// The type of selector object to be derived from the elements of the collection.
    /// </typeparam>
    public sealed class DistinctByEnumerable<T, TSelector> : WrappingEnumerable<T, T>
    {
        [NotNull]
        private readonly Func<T, TSelector> _SelectorFunc;

        [NotNull]
        private readonly IEqualityComparer<TSelector> _EqualityComparer;

        /// <summary>
        /// Constructs a new instance of the <see cref="DistinctByEnumerable{T,TSelector}"/> class.
        /// </summary>
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
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="selectorFunc"/> is <c>null</c>.</para>
        /// </exception>
        public DistinctByEnumerable([NotNull] IEnumerable<T> collection, [NotNull] Func<T, TSelector> selectorFunc, [CanBeNull] IEqualityComparer<TSelector> equalityComparer = null)
            : base(collection)
        {
            _SelectorFunc = selectorFunc ?? throw new ArgumentNullException(nameof(selectorFunc));
            assume(EqualityComparer<TSelector>.Default != null);
            _EqualityComparer = equalityComparer ?? EqualityComparer<TSelector>.Default;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override IEnumerator<T> GetEnumerator()
        {
            var set = new HashSet<TSelector>(_EqualityComparer);
            foreach (var element in Collection)
            {
                var selector = _SelectorFunc(element);
                if (set.Contains(selector))
                    continue;

                set.Add(selector);
                yield return element;
            }
        }
    }
}
