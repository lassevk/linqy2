using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy2
{
    /// <summary>
    /// This type is returned from <see cref="CollectionExtensions.Lag{T}"/>
    /// and <see cref="LagEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public struct LagItem<T> : IEquatable<LagItem<T>>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="LagItem{T}"/> struct.
        /// </summary>
        /// <param name="element">
        /// The current element from the collection.
        /// </param>
        /// <param name="laggingElement">
        /// The lagging element from the collection.
        /// </param>
        public LagItem([CanBeNull] T element, [CanBeNull] T laggingElement)
        {
            Element = element;
            LaggingElement = laggingElement;
        }

        /// <summary>
        /// The current element from the collection.
        /// </summary>
        [CanBeNull]
        public T Element
        {
            get;
        }

        /// <summary>
        /// The lagging element from the collection.
        /// </summary>
        [CanBeNull]
        public T LaggingElement
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
        public bool Equals(LagItem<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Element, other.Element) && EqualityComparer<T>.Default.Equals(LaggingElement, other.LaggingElement);
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
            return obj is LagItem<T> item && Equals(item);
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
                return (EqualityComparer<T>.Default.GetHashCode(Element) * 397) ^ EqualityComparer<T>.Default.GetHashCode(LaggingElement);
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
            return $"Element: {Element}, LaggingElement: {LaggingElement}";
        }
    }
}