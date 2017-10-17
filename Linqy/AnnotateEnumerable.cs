using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using static Linqy.ReSharperEnsurances;

namespace Linqy
{
    /// <summary>
    /// This class implements <see cref="CollectionExtensions.AddIndex{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public class AnnotateEnumerable<T> : WrappingEnumerable<T, AnnotatedElement<T>>
    {
        /// <summary>
        /// This is the default starting index to use if nothing is specified.
        /// </summary>
        public const int DefaultStartIndex = 0;

        private readonly int _StartIndex;

        /// <summary>
        /// Constructs a new instance of the <see cref="AnnotateEnumerable{T}"/> class.
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
        public AnnotateEnumerable([NotNull] IEnumerable<T> collection, int startIndex = DefaultStartIndex)
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
        public override IEnumerator<AnnotatedElement<T>> GetEnumerator()
        {
            using (var enumerator = Collection.GetEnumerator())
            {
                assume(enumerator != null);
                if (!enumerator.MoveNext())
                    yield break;

                bool isFirst = true;
                int index = _StartIndex;
                T previousElement = default(T);
                T currentElement = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    yield return new AnnotatedElement<T>(index, currentElement, isFirst, previousElement, false, enumerator.Current);

                    previousElement = currentElement;
                    currentElement = enumerator.Current;
                    index++;
                    isFirst = false;
                }

                yield return new AnnotatedElement<T>(index, currentElement, isFirst, previousElement, true, default(T));
            }
        }
    }
}