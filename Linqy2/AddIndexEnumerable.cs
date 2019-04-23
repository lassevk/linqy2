using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.AddIndex{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class AddIndexEnumerable<T> : WrappingEnumerable<T, IndexedElement<T>>
    {
        /// <summary>
        /// This is the default starting index to use if nothing is specified.
        /// </summary>
        public const int DefaultStartIndex = 0;

        private readonly int _StartIndex;

        /// <summary>
        /// Constructs a new instance of the <see cref="AddIndexEnumerable{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection to add an index to.
        /// </param>
        /// <param name="startIndex">
        /// The starting index for the first element.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="collection"/> is <c>null</c>.</para>
        /// </exception>
        public AddIndexEnumerable([NotNull] IEnumerable<T> collection, int startIndex = DefaultStartIndex)
            : base(collection)
        {
            _StartIndex = startIndex;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override IEnumerator<IndexedElement<T>> GetEnumerator()
        {
            return Collection.Select((element, index) => new IndexedElement<T>(_StartIndex + index, element)).GetEnumerator();
        }
    }
}
