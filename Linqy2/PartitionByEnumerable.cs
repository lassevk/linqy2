using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.PartitionBy{T,TKey}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    /// <typeparam name="TKey">
    /// The type of keys to extract from the elements.
    /// </typeparam>
    public class PartitionByEnumerable<T, TKey> : WrappingEnumerable<T, List<T>> 
    {
        [NotNull]
        private readonly Func<T, TKey> _KeySelector;

        [NotNull]
        private readonly IEqualityComparer<TKey> _KeyComparer;

        /// <summary>
        /// Constructs a new instance of the <see cref="PartitionByEnumerable{T,TKey}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to partition.
        /// </param>
        /// <param name="keySelector">
        /// A delegate used to extract a key from the elements. The keys are compared and if two sequential keys from
        /// the collection returns different keys, a new partition is started from the second of the two.
        /// </param>
        /// <param name="keyComparer">
        /// A <see cref="IEqualityComparer{T}"/> used to compare keys.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="keySelector"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="keyComparer"/> is <c>null</c>.</para>
        /// </exception>
        public PartitionByEnumerable([NotNull] IEnumerable<T> collection, [NotNull] Func<T, TKey> keySelector, [NotNull] IEqualityComparer<TKey> keyComparer)
            : base(collection)
        {
            _KeySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
            _KeyComparer = keyComparer ?? throw new ArgumentNullException(nameof(keyComparer));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override IEnumerator<List<T>> GetEnumerator()
        {
            var partition = new List<T>();
            TKey previousKey = default;
            foreach (var element in Collection)
            {
                if (partition.Count == 0)
                {
                    partition.Add(element);
                    continue;
                }

                TKey key = _KeySelector(element);
                if (_KeyComparer.Equals(previousKey, key))
                    partition.Add(element);
                else
                {
                    yield return partition;
                    partition = new List<T>
                                {
                                    element
                                };
                }

                previousKey = key;
            }

            if (partition.Count > 0)
                yield return partition;
        }
    }
}