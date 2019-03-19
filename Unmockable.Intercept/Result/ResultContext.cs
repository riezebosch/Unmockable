using System;

namespace Unmockable.Result
{
    internal class ResultContext<TResult>
    {
        private INextResult<TResult> _current ;
        private INextResult<TResult> _last;

        public ResultContext() => _last = _current = new First<TResult>();

        public bool IsDone => _current == _last || _current == _current.Next;
        
        public bool HasNext => _current.Next != null;

        public TResult Next() => (_current = _current.Next).Result;

        public void Add(TResult result) => Add(new ValueResult<TResult>(result));

        public void Add<TException>() where TException : Exception, new() => Add(new ExceptionResult<TResult, TException>());

        private void Add(INextResult<TResult> next) => _last = _last.Next = next;
    }
}