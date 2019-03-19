using System;
using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable.Result
{
    internal class ResultContext<TResult>
    {
        private readonly LambdaExpression _expression;
        private INextResult<TResult> _current ;
        private INextResult<TResult> _last;

        public ResultContext() : this(LambdaExpression.Lambda(LambdaExpression.Empty()))
        {
        }

        public ResultContext(LambdaExpression expression)
        {
            _expression = expression;
            _last = _current = new First<TResult>();
        }

        public bool IsDone => _current == _last || _current == _current.Next;

        public TResult Next() => (_current = _current.Next ?? throw new NoResultsSetupException(_expression.ToString())).Result;

        public void Add(TResult result) => Add(new ValueResult<TResult>(result));

        private void Add(INextResult<TResult> next) => _last = _last.Next = next;

        public void Add<TException>() where TException : Exception, new() => Add(new ExceptionResult<TResult, TException>());
    }
}