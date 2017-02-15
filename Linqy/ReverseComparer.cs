using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class wraps a <see cref="IComparer{T}"/> and returns "the opposite ordering", basically by just
    /// returning the negative value from calling <see cref="IComparer{T}.Compare"/>. That is, if the
    /// underlying comparer returns a positive value, <see cref="ReverseComparer{T}"/> returns a negative
    /// value, and vice versa.
    /// </summary>
    /// <typeparam name="T">
    /// The type of values to compare.
    /// </typeparam>
    public sealed class ReverseComparer<T> : IComparer<T>
    {
        [NotNull]
        private readonly IComparer<T> _Comparer;

        /// <summary>
        /// Constructs a new instance of the <see cref="ReverseComparer{T}"/> class.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> to reverse the comparison of.
        /// </param>
        public ReverseComparer([NotNull] IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            _Comparer = comparer;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
        /// See <see cref="IComparer{T}.Compare"/> for more information.
        /// </returns>
        /// <param name="x">
        /// The first object to compare.
        /// </param>
        /// <param name="y">
        /// The second object to compare.
        /// </param>
        public int Compare(T x, T y)
        {
            return -_Comparer.Compare(x, y);
        }
    }
}