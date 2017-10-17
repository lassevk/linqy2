using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.AddIndex{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class ExceptLastEnumerable<T> : WrappingEnumerable<T, T>
    {
        private readonly int _Amount;

        /// <summary>
        /// Constructs a new instance of the <see cref="ExceptLastEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="amount"></param>
        public ExceptLastEnumerable([NotNull] IEnumerable<T> collection, int amount)
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
        public override IEnumerator<T> GetEnumerator()
        {
            if (_Amount == 0)
                return Collection.GetEnumerator();

            if (Collection is IList<T> list)
                return GetListEnumerator(list, _Amount);

            return GetEnumerableEnumerator();
        }

        [NotNull]
        private static IEnumerator<T> GetListEnumerator([NotNull] IList<T> list, int amount)
        {
            for (int index = 0; index < list.Count - amount; index++)
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
                    yield return buffer.Dequeue();
            }
        }
    }
}