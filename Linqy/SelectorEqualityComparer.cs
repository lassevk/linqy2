using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="IEqualityComparer{T}"/> for a type, given that it
    /// has to select out from the whole type the properties it should use for the comparison.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the elements to compare.
    /// </typeparam>
    /// <typeparam name="TSelector">
    /// The type to derive from <typeparamref name="T"/> to use for comparison.
    /// </typeparam>
    public sealed class SelectorEqualityComparer<T, TSelector> : IEqualityComparer<T>
    {
        [NotNull]
        private readonly Func<T, TSelector> _SelectorFunc;

        [NotNull]
        private readonly IEqualityComparer<TSelector> _EqualityComparer;

        /// <summary>
        /// Constructs a new instance of the <see cref="SelectorEqualityComparer{T,TSelector}"/> class.
        /// </summary>
        /// <param name="selectorFunc">
        /// The delegate that extracts the selector object from the underlying element.
        /// </param>
        /// <param name="equalityComparer">
        /// The equality comparer that can compare two selector objects, or <c>null</c> if the default <see cref="IEqualityComparer{T}"/> for
        /// <typeparamref name="TSelector"/> should be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="selectorFunc"/> is <c>null</c>.</para>
        /// </exception>
        public SelectorEqualityComparer([NotNull] Func<T, TSelector> selectorFunc, [CanBeNull] IEqualityComparer<TSelector> equalityComparer = null)
        {
            _SelectorFunc = selectorFunc ?? throw new ArgumentNullException(nameof(selectorFunc));
            _EqualityComparer = equalityComparer ?? EqualityComparer<TSelector>.Default;
        }

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
        /// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
        public bool Equals(T x, T y)
        {
            return _EqualityComparer.Equals(_SelectorFunc(x), _SelectorFunc(y));
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(T obj)
        {
            var selector = _SelectorFunc(obj);
            if (selector == null)
                return 0;
            return _EqualityComparer.GetHashCode(selector);
        }
    }
}
