using System;
using System.Collections;
using System.Collections.Generic;

// ReSharper disable PossibleNullReferenceException

namespace Linqy.Tests
{
    public class AnnotatedElementComparer<T> : IComparer<AnnotatedElement<T>>, IComparer
    {
        private readonly IComparer<T> _ElementComparer;

        public AnnotatedElementComparer(IComparer<T> elementComparer = null)
        {
            _ElementComparer = elementComparer ?? Comparer<T>.Default;
        }

        public int Compare(AnnotatedElement<T> x, AnnotatedElement<T> y)
        {
            int rc = x.Index.CompareTo(y.Index);
            if (rc != 0)
                return rc;

            rc = _ElementComparer.Compare(x.Element, y.Element);
            if (rc != 0)
                return rc;

            rc = x.IsFirst.CompareTo(y.IsFirst);
            if (rc != 0)
                return rc;

            return x.IsLast.CompareTo(y.IsLast);
        }

        public int Compare(object x, object y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (ReferenceEquals(x, null))
                return -1;
            if (ReferenceEquals(y, null))
                return +1;

            if (x is AnnotatedElement<T> a && y is AnnotatedElement<T> b)
                return Compare(a, b);

            throw new NotSupportedException();
        }
    }
}
