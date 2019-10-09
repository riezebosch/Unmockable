using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unmockable.Setup;

namespace Unmockable.Exceptions
{
    public class NotExecutedException : Exception
    {
        internal NotExecutedException(IEnumerable<ISetup> not) : base(string.Join(Environment.NewLine, not.Select(x => x.ToString())))
        {
        }
    }
}