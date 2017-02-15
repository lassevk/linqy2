using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.Partition{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class PartitionEnumerable<T> : WrappingEnumerable<T, List<T>>
    {
        private readonly int _PartitionSize;

        /// <summary>
        /// Constructs a new instance of the <see cref="PartitionEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to partition.
        /// </param>
        /// <param name="partitionSize">
        /// The size of each partition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="partitionSize"/> is less than 1.</para>
        /// </exception>
        /// <remarks>
        /// Note that the last partition may be short a few elements if the <paramref name="collection"/> does not have a number of elements that is
        /// divisible by the <paramref name="partitionSize"/>.
        /// </remarks>
        public PartitionEnumerable([NotNull] IEnumerable<T> collection, int partitionSize)
            : base(collection)
        {
            if (partitionSize < 1)
                throw new ArgumentOutOfRangeException("");

            _PartitionSize = partitionSize;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<List<T>> GetEnumerator()
        {
            var partition = new List<T>();
            foreach (var element in Collection)
            {
                partition.Add(element);
                if (partition.Count == _PartitionSize)
                {
                    yield return partition;
                    partition = new List<T>();
                }
            }
            if (partition.Count > 0)
                yield return partition;
        }
    }
}