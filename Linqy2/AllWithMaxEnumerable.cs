using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.AllWithMax{T,TSelector}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    /// <typeparam name="TSelector">
    /// The type of selector object to be derived from the elements of the collection, used to determine what the maximum value is.
    /// </typeparam>
    public class AllWithMaxEnumerable<T, TSelector> : WrappingEnumerable<T, T>
    {
        [NotNull]
        private readonly Func<T, TSelector> _SelectorFunc;

        [NotNull]
        private readonly IComparer<TSelector> _Comparer;

        /// <summary>
        /// Constructs a new instance of the <see cref="AllWithMaxEnumerable{T,TSelector}"/> class.
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
        public AllWithMaxEnumerable([NotNull] IEnumerable<T> collection, [NotNull] Func<T, TSelector> selectorFunc, [CanBeNull] IComparer<TSelector> comparer = null)
            : base(collection)
        {
            _SelectorFunc = selectorFunc ?? throw new ArgumentNullException(nameof(selectorFunc));
            _Comparer = comparer ?? Comparer<TSelector>.Default;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public override IEnumerator<T> GetEnumerator()
        {
            var allWithMax = new List<T>();
            TSelector currentMax = default(TSelector);
            foreach (var element in Collection)
            {
                var selector = _SelectorFunc(element);
                if (allWithMax.Count == 0)
                {
                    allWithMax.Add(element);
                    currentMax = selector;
                }
                else
                {
                    var comparison = _Comparer.Compare(selector, currentMax);
                    if (comparison == 0)
                        allWithMax.Add(element);
                    else if (comparison > 0)
                    {
                        allWithMax.Clear();
                        allWithMax.Add(element);
                        currentMax = selector;
                    }
                }
            }

            return allWithMax.GetEnumerator();
        }
    }
}
