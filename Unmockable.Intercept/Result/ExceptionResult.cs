using System;
using Unmockable.Matchers;

namespace Unmockable.Result
{
    internal class ExceptionResult<T, TException> : 
        IResult<T> where TException: Exception, new()
    {
        public T Value
        {
            get
            {
                IsDone = true;
                throw new TException();
            }
        }

        public bool IsDone { get; private set; }
        public IResult<T> Add(IResult<T> result, IMemberMatcher matcher) => 
            new MultipleResults<T>(matcher)
                .Add(this, matcher)
                .Add(result, matcher);
        public override string ToString() => 
            typeof(TException).Name;
    }
}