using System;
using System.Linq.Expressions;

namespace Unmockable.Result
{
    public class ResultContext<TResult>
    {
        private readonly LambdaExpression _expression;
        private INextResult<TResult> _current ;
        private INextResult<TResult> _last;

        public ResultContext(LambdaExpression expression = null)
        {
            _expression = expression ?? LambdaExpression.Lambda(LambdaExpression.Empty());
            _last = _current = new First<TResult>(_expression);
        }

        public bool IsDone => _current == _last || _current == _current.Next;

        public TResult Next()
        {
            return (_current = _current.Next ?? new Done<TResult>(_expression)).Result;
        }

        public void Add(TResult result)
        {
            Add(new ValueResult<TResult>(result));
        }

        private void Add(INextResult<TResult> next)
        {
            _last = _last.Next = next;
        }

        public void Add<TException>() where TException : Exception, new()
        {
            Add(new ExceptionResult<TResult, TException>());
        }
    }
}