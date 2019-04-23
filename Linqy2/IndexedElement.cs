using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This type is returned from <see cref="CollectionExtensions.AddIndex{T}"/>
    /// and <see cref="AddIndexEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements that are indexed.
    /// </typeparam>
    public struct IndexedElement<T> : IEquatable<IndexedElement<T>>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="IndexedElement{T}"/> struct.
        /// </summary>
        /// <param name="index">
        /// The index of the element.
        /// </param>
        /// <param name="element">
        /// The element from the collection.
        /// </param>
        public IndexedElement(int index, [CanBeNull] T element)
        {
            Index = index;
            Element = element;
        }

        /// <summary>
        /// The index of the <see cref="Element"/>.
        /// </summary>
        public int Index
        {
            get;
        }
        
        /// <summary>
        /// The element from the collection.
        /// </summary>
        [CanBeNull]
        public T Element
        {
            get;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IndexedElement<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Element, other.Element) && Index == other.Index;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false. 
        /// </returns>
        /// <param name="obj">The object to compare with the current instance. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is IndexedElement<T> element && Equals(element);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(Element) * 397) ^ Index;
            }
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return $"#{Index}: {Element}";
        }
    }
}