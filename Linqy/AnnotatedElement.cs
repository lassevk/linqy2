using System;

using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This type is returned from <see cref="CollectionExtensions.Annotate{T}"/>
    /// and <see cref="AnnotateEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements that are indexed.
    /// </typeparam>
    public struct AnnotatedElement<T>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="AnnotatedElement{T}"/> struct.
        /// </summary>
        /// <param name="index">
        /// The index of the element.
        /// </param>
        /// <param name="element">
        /// The element from the collection.
        /// </param>
        /// <param name="isFirst">
        /// A value indicating whether this is the first element obtained from the collection.
        /// </param>
        /// <param name="previousElement">
        /// The previous element from the collection, or <code>default(T)</code> if this is the first element (<paramref name="isFirst"/> is <c>true</c>).
        /// </param>
        /// <param name="isLast">
        /// A value indicating whether this is the last element obtained from the collection.
        /// </param>
        /// <param name="nextElement">
        /// The next element from the collection, or <code>default(T)</code> if this is the last element (<paramref name="isLast"/> is <c>true</c>).
        /// </param>
        public AnnotatedElement(int index, [CanBeNull] T element, bool isFirst, T previousElement, bool isLast, T nextElement)
        {
            Index = index;
            Element = element;
            IsFirst = isFirst;
            PreviousElement = previousElement;
            IsLast = isLast;
            NextElement = nextElement;
        }

        /// <summary>
        /// The index of the <see cref="Element"/>.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The element from the collection.
        /// </summary>
        [CanBeNull]
        public T Element { get; }

        /// <summary>
        /// Gets a value indicating whether this is the first element obtained from the collection.
        /// </summary>
        public bool IsFirst { get; }

        /// <summary>
        /// The Previous element from the collection, or <code>default(T)</code> if this is the first element (<see name="IsFirst"/> is <c>true</c>).
        /// </summary>
        public T PreviousElement { get; }

        /// <summary>
        /// Gets a value indicating whether this is the last element obtained from the collection.
        /// </summary>
        public bool IsLast { get; }

        /// <summary>
        /// The next element from the collection, or <code>default(T)</code> if this is the last element (<see name="IsLast"/> is <c>true</c>).
        /// </summary>
        public T NextElement { get; }
    }
}