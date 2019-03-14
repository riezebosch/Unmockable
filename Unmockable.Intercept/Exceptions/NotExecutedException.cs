using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Unmockable.Exceptions
{
    public class NotExecutedException : Exception
    {
        public NotExecutedException(IEnumerable<Expression> not) : base(string.Join(Environment.NewLine, not.Select(x => x.ToString())))
        {
        }
    }
}