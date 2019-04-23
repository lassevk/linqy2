using System;
using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This is the base class for <see cref="IEnumerable{T}"/> implementations in Linqy.
    /// </summary>
    /// <typeparam name="TInput">
    /// The type of elements in the wrapped collection.
    /// </typeparam>
    /// <typeparam name="TOutput">
    /// The type of elements the wrapped collection will produce.
    /// </typeparam>
    public abstract class WrappingEnumerable<TInput, TOutput> : IEnumerable<TOutput>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="WrappingEnumerable{TInput,TOutput}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to wrap.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        protected WrappingEnumerable([NotNull] IEnumerable<TInput> collection)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        /// <summary>
        /// Gets the collection that is being wrapped.
        /// </summary>
        [NotNull]
        protected IEnumerable<TInput> Collection
        {
            get;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public abstract IEnumerator<TOutput> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}