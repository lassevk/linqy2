﻿using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This <see cref="CollectionCriteria{T}"/> descendant answers the question "does the collection have at least N elements".
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class HasAtLeastCollectionCriteria<T> : CollectionCriteria<T>
    {
        private readonly int _Count;

        /// <summary>
        /// Constructs a new instance of <see cref="HasAtLeastCollectionCriteria{T}"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection to check.
        /// </param>
        /// <param name="count">
        /// The number of elements the collection is required to have in order to answer <c>true</c>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is <c>null</c>.
        /// </exception>
        public HasAtLeastCollectionCriteria([NotNull] IEnumerable<T> collection, int count)
            : base(collection)
        {
            _Count = count;
        }

        /// <summary>
        /// Determines if the collection has at least N elements.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the collection has at least N elements;
        /// otherwise, <c>false</c>.
        /// </returns>
        public override bool GetValue()
        {
            if (_Count <= 0)
                return true;

            int count = _Count;
            using (var enumerator = Collection.GetEnumerator())
            {
                while (count-- > 0)
                    if (!enumerator.MoveNext())
                        return false;

                return true;
            }
        }
    }
}