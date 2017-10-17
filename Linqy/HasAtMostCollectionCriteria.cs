using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using static Linqy.ReSharperEnsurances;

namespace Linqy
{
    /// <summary>
    /// This <see cref="CollectionCriteria{T}"/> descendant answers the question "does the collection have at most N elements".
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class HasAtMostCollectionCriteria<T> : CollectionCriteria<T>
    {
        private readonly int _Count;

        /// <summary>
        /// Constructs a new instance of <see cref="HasAtMostCollectionCriteria{T}"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection to check.
        /// </param>
        /// <param name="count">
        /// The number of elements the collection is required to have at most in order to answer <c>true</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is <c>null</c>.
        /// </exception>
        public HasAtMostCollectionCriteria([NotNull] IEnumerable<T> collection, int count)
            : base(collection)
        {
            _Count = count;
        }

        /// <summary>
        /// Determines if the collection has at most N elements.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the collection has at most N elements;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool GetValue()
        {
            if (_Count < 0)
                return false;

            int count = _Count;
            using (var enumerator = Collection.GetEnumerator())
            {
                assume(enumerator != null);

                while (count-- > 0)
                    if (!enumerator.MoveNext())
                        return true;

                return !enumerator.MoveNext();
            }
        }
    }
}