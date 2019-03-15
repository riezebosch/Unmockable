using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Unmockable.Exceptions;

namespace Unmockable.Setup
{
    internal class ResultMachine<TResult>
    {
        private readonly IList<Func<TResult>> _results = new List<Func<TResult>>();
        
        private int _invocation;

        public bool IsExecuted => _invocation >= _results.Count;

        public Func<TResult> NextResult(LambdaExpression expression)
        {
            return _results.ElementAtOrDefault(_invocation++) ?? DefaultResult(expression);
        }

        private static Func<TResult> DefaultResult(LambdaExpression expression)
        {
            if (typeof(TResult) == typeof(Task))
                return () => (TResult)(object)Task.CompletedTask;

            if (typeof(TResult) == typeof(Nothing))
                return () => (TResult)(object)default(Nothing);

            return () => throw new NoMoreResultsSetupException(expression.ToString());
        }

        public void Add<TException>() where TException : Exception, new()
        {
            _results.Add(() => throw new TException());
        }

        public void Add(TResult result)
        {
            _results.Add(() => result);
        }
    }
}