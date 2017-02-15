using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.Lead{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class LeadEnumerable<T> : WrappingEnumerable<T, LeadItem<T>>
    {
        /// <summary>
        /// The default amount of lead.
        /// </summary>
        public const int DefaultAmount = 1;

        private readonly int _Amount;

        /// <summary>
        /// Constructs a new instance of the <see cref="LeadEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to lead.
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
        public LeadEnumerable([NotNull] IEnumerable<T> collection, int amount = DefaultAmount)
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
        public override IEnumerator<LeadItem<T>> GetEnumerator()
        {
            if (_Amount == 0)
                return GetNoLeadEnumerator();

            return GetLeadEnumerator();
        }

        [NotNull]
        private IEnumerator<LeadItem<T>> GetLeadEnumerator()
        {
            var queue = new Queue<T>();
            using (var enumerator = Collection.GetEnumerator())
            {
                for (int index = 0; index < _Amount; index++)
                {
                    if (!enumerator.MoveNext())
                        break;
                    queue.Enqueue(enumerator.Current);
                }

                while (enumerator.MoveNext())
                {
                    yield return new LeadItem<T>(queue.Dequeue(), enumerator.Current);
                    queue.Enqueue(enumerator.Current);
                }

                while (queue.Count > 0)
                    yield return new LeadItem<T>(queue.Dequeue(), default(T));
            }
        }

        [NotNull]
        private IEnumerator<LeadItem<T>> GetNoLeadEnumerator()
        {
            return Collection.Select(element => new LeadItem<T>(element, element)).GetEnumerator();
        }
    }
}