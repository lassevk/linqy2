using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.Lag{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class LagEnumerable<T> : WrappingEnumerable<T, LagItem<T>> 
    {
        /// <summary>
        /// The default amount of lag.
        /// </summary>
        public const int DefaultAmount = 1;

        private readonly int _Amount;

        /// <summary>
        /// Constructs a new instance of the <see cref="LagEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to lag.
        /// </param>
        /// <param name="amount">
        /// The amount of lag to introduce.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="amount"/> is less than 0.</para>
        /// </exception>
        public LagEnumerable([NotNull] IEnumerable<T> collection, int amount = DefaultAmount)
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
        public override IEnumerator<LagItem<T>> GetEnumerator()
        {
            if (_Amount == 0)
                return GetNoLagEnumerator();

            return GetLagEnumerator();
        }

        [NotNull]
        private IEnumerator<LagItem<T>> GetLagEnumerator()
        {
            var queue = new Queue<T>();
            using (var enumerator = Collection.GetEnumerator())
            {
                for (int index = 0; index < _Amount; index++)
                {
                    if (!enumerator.MoveNext())
                        yield break;

                    yield return new LagItem<T>(enumerator.Current, default(T));
                    queue.Enqueue(enumerator.Current);
                }

                while (enumerator.MoveNext())
                {
                    yield return new LagItem<T>(enumerator.Current, queue.Dequeue());
                    queue.Enqueue(enumerator.Current);
                }
            }
        }

        [NotNull]
        private IEnumerator<LagItem<T>> GetNoLagEnumerator()
        {
            return Collection.Select(element => new LagItem<T>(element, element)).GetEnumerator();
        }
    }
}