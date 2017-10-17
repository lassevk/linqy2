using System;
using System.Collections.Generic;
using JetBrains.Annotations;

using static Linqy.ReSharperEnsurances;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.AllWithMin{T,TSelector}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    /// <typeparam name="TSelector">
    /// The type of selector object to be derived from the elements of the collection, used to determine what the maximum value is.
    /// </typeparam>
    public class AllWithMinEnumerable<T, TSelector> : AllWithMaxEnumerable<T, TSelector>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="AllWithMinEnumerable{T,TSelector}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to return distinct elements from.
        /// </param>
        /// <param name="selectorFunc">
        /// A selector delegate that for each element returns the data used to determine which elements have the biggest selected value.
        /// </param>
        /// <param name="comparer">
        /// The comparer, or <c>null</c> if default comparer should be used, that determines
        /// which selector values are greater than others.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="selectorFunc"/> is <c>null</c>.</para>
        /// </exception>
        public AllWithMinEnumerable([NotNull] IEnumerable<T> collection, [NotNull] Func<T, TSelector> selectorFunc, [CanBeNull] IComparer<TSelector> comparer = null)
            : base(collection, selectorFunc, CreateNegativeComparer(comparer))
        {
        }

        [NotNull]
        private static IComparer<TSelector> CreateNegativeComparer([CanBeNull] IComparer<TSelector> comparer)
        {
            assume(Comparer<TSelector>.Default != null);
            return new ReverseComparer<TSelector>(comparer ?? Comparer<TSelector>.Default);
        }
    }
}