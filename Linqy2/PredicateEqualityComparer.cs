using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Linqy2
{
    internal class PredicateEqualityComparer<T> : IEqualityComparer<T>
    {
        [NotNull]
        private readonly Func<T, T, bool> _PartitionPredicate;

        public PredicateEqualityComparer([NotNull] Func<T,T,bool> partitionPredicate)
        {
            _PartitionPredicate = partitionPredicate ?? throw new ArgumentNullException(nameof(partitionPredicate));
        }

        public bool Equals(T x, T y) =>
            _PartitionPredicate(x, y);

        public int GetHashCode(T obj) =>
            throw new NotSupportedException();
    }
}