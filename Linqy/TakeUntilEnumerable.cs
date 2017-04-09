using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.TakeUntil{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class TakeUntilEnumerable<T> : IEnumerable<T>
    {
        [NotNull]
        private readonly IEnumerable<T> _Collection;

        [NotNull]
        private readonly Predicate<T> _Predicate;

        /// <summary>
        /// Constructs a new instance of the <see cref="TakeUntilEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// A sequence to return elements from.
        /// </param>
        /// <param name="predicate">
        /// A function to test each element for a condition.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="predicate"/> is <c>null</c>.</para>
        /// </exception>
        public TakeUntilEnumerable([NotNull] IEnumerable<T> collection, [NotNull] Predicate<T> predicate)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            _Collection = collection;
            _Predicate = predicate;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var element in _Collection)
            {
                yield return element;
                if (_Predicate(element))
                    break;
            }
        }
    }
}