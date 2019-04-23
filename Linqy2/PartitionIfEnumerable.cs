using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.PartitionIf{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class PartitionIfEnumerable<T> : WrappingEnumerable<T, List<T>> 
    {
        [NotNull]
        private readonly Func<T, T, bool> _PartitionPredicate;

        /// <summary>
        /// Constructs a new instance of the <see cref="PartitionIfEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to partition.
        /// </param>
        /// <param name="partitionPredicate">
        /// The predicate to use to determine if the current element belongs in the same partition
        /// as the previous element.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="partitionPredicate"/> is <c>null</c>.</para>
        /// </exception>
        public PartitionIfEnumerable([NotNull] IEnumerable<T> collection, [NotNull] Func<T, T, bool> partitionPredicate)
            : base(collection)
        {
            _PartitionPredicate = partitionPredicate ?? throw new ArgumentNullException(nameof(partitionPredicate));
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
            foreach (var pair in Collection.Lag())
            {
                if (partition.Count == 0)
                    partition.Add(pair.Element);
                else if (_PartitionPredicate(pair.LaggingElement, pair.Element))
                    partition.Add(pair.Element);
                else
                {
                    yield return partition;
                    partition = new List<T>
                    {
                        pair.Element
                    };
                }
            }

            if (partition.Count > 0)
                yield return partition;
        }
    }
}
