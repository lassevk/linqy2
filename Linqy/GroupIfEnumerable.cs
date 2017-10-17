using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.GroupIf{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class GroupIfEnumerable<T> : WrappingEnumerable<T, List<T>> 
    {
        [NotNull]
        private readonly Func<T, T, bool> _GroupPredicate;

        /// <summary>
        /// Constructs a new instance of the <see cref="GroupIfEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to group.
        /// </param>
        /// <param name="groupPredicate">
        /// The predicate to use to determine if the current element belongs in the same group
        /// as the previous element.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="groupPredicate"/> is <c>null</c>.</para>
        /// </exception>
        public GroupIfEnumerable([NotNull] IEnumerable<T> collection, [NotNull] Func<T, T, bool> groupPredicate)
            : base(collection)
        {
            _GroupPredicate = groupPredicate ?? throw new ArgumentNullException(nameof(groupPredicate));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override IEnumerator<List<T>> GetEnumerator()
        {
            var group = new List<T>();
            foreach (var pair in Collection.Lag())
            {
                if (group.Count == 0)
                    group.Add(pair.Element);
                else if (_GroupPredicate(pair.LaggingElement, pair.Element))
                    group.Add(pair.Element);
                else
                {
                    yield return group;
                    group = new List<T>
                    {
                        pair.Element
                    };
                }
            }

            if (group.Count > 0)
                yield return group;
        }
    }
}
