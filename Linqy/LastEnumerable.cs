using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using static Linqy.ReSharperEnsurances;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.Last{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class LastEnumerable<T> : WrappingEnumerable<T, T>
    {
        private readonly int _Amount;

        /// <summary>
        /// Constructs a new instance of the <see cref="LastEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to aggregate.
        /// </param>
        /// <param name="amount"></param>
        public LastEnumerable([NotNull] IEnumerable<T> collection, int amount)
            : base(collection)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "amount must be zero or greater");

            _Amount = amount;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override IEnumerator<T> GetEnumerator()
        {
            if (_Amount == 0)
            {
                IEnumerable<T> emptyEnumerable = Enumerable.Empty<T>();
                assume(emptyEnumerable != null);
                return emptyEnumerable.GetEnumerator();
            }

            if (Collection is IList<T> list)
                return GetListEnumerator(list, _Amount);

            return GetEnumerableEnumerator();
        }

        [NotNull]
        private static IEnumerator<T> GetListEnumerator([NotNull] IList<T> list, int amount)
        {
            var startIndex = Math.Max(0, list.Count - amount);
            var endIndex = list.Count;

            for (int index = startIndex; index < endIndex; index++)
                yield return list[index];
        }

        [NotNull]
        private IEnumerator<T> GetEnumerableEnumerator()
        {
            var buffer = new Queue<T>();

            foreach (var element in Collection)
            {
                buffer.Enqueue(element);
                if (buffer.Count > _Amount)
                    buffer.Dequeue();
            }

            while (buffer.Count > 0)
                yield return buffer.Dequeue();
        }
    }
}