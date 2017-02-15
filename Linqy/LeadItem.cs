using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This type is returned from <see cref="CollectionExtensions.Lead{T}"/>
    /// and <see cref="LeadEnumerable{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public struct LeadItem<T> : IEquatable<LeadItem<T>>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="LeadItem{T}"/> struct.
        /// </summary>
        /// <param name="element">
        /// The current element from the collection.
        /// </param>
        /// <param name="leadingElement">
        /// The lagging element from the collection.
        /// </param>
        public LeadItem([CanBeNull] T element, [CanBeNull] T leadingElement)
        {
            Element = element;
            LeadingElement = leadingElement;
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
        /// The leading element from the collection.
        /// </summary>
        [CanBeNull]
        public T LeadingElement
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
        public bool Equals(LeadItem<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Element, other.Element) && EqualityComparer<T>.Default.Equals(LeadingElement, other.LeadingElement);
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
            return obj is LeadItem<T> && Equals((LeadItem<T>)obj);
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
                return (EqualityComparer<T>.Default.GetHashCode(Element) * 397) ^ EqualityComparer<T>.Default.GetHashCode(LeadingElement);
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
            return $"Element: {Element}, LeadingElement: {LeadingElement}";
        }
    }
}