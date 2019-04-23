using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.Batch{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class BatchEnumerable<T> : WrappingEnumerable<T, List<T>>
    {
        private readonly int _BatchSize;

        /// <summary>
        /// Constructs a new instance of the <see cref="BatchEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to create batches from.
        /// </param>
        /// <param name="batchSize">
        /// The size of each batch.
        /// </param>
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
        public BatchEnumerable([NotNull] IEnumerable<T> collection, int batchSize)
            : base(collection)
        {
            if (batchSize < 1)
                throw new ArgumentOutOfRangeException("");

            _BatchSize = batchSize;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<List<T>> GetEnumerator()
        {
            var batch = new List<T>();
            foreach (var element in Collection)
            {
                batch.Add(element);
                if (batch.Count == _BatchSize)
                {
                    yield return batch;
                    batch = new List<T>();
                }
            }
            if (batch.Count > 0)
                yield return batch;
        }
    }
}