using System;
using System.Collections.Generic;
using System.Linq;

namespace Unmockable.Exceptions
{
    public class NotExecutedException<T> : Exception
    {
        public NotExecutedException(IEnumerable<InterceptSetup<T>> not) : base(string.Join(Environment.NewLine, not.Select(x => x.Expression.ToString())))
        {
        }
    }
}