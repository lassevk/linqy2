using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy
{
    /// <summary>
    /// This class is the base class for criteria-implementations, classes that answer a simple yes/no (true/false) question
    /// about a given collection.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elements in the collection.
    /// </typeparam>
    public abstract class CollectionCriteria<T>
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="CollectionCriteria{T}"/> class.
        /// </summary>
        /// <param name="collection">
        /// The collection that a question will be answered for.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> is <c>null</c>.
        /// </exception>
        protected CollectionCriteria([NotNull] IEnumerable<T> collection)
        {
            Collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        /// <summary>
        /// Gets the answer value of the criteria.
        /// </summary>
        /// <returns>
        /// <c>true</c> or <c>false</c>.
        /// </returns>
        // ReSharper disable once UnusedMemberInSuper.Global
        public abstract bool GetValue();

        /// <summary>
        /// Gives descendants access to the collection specified in the constructor call.
        /// </summary>
        [NotNull]
        protected IEnumerable<T> Collection { get; }
    }
}