using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable.Setup
{
    internal class SetupCache
    {
        private readonly IDictionary<IUnmockableMatcher, ISetup> _setups = new Dictionary<IUnmockableMatcher, ISetup>();

        public TItem Store<TItem>(TItem setup) where TItem: ISetup
        {
            _setups[setup.Expression.ToMatcher()] = setup;
            return setup;
        }
        
        public ISetup<TResult> Load<TResult>(LambdaExpression m)
        {
            var key = m.ToMatcher();
            if (!_setups.TryGetValue(key, out var setup))
            {
                throw new NoSetupException(key.ToString());
            }

            return (ISetup<TResult>)setup;
        }

        public void Verify()
        {
            var not = _setups
                .Values
                .Where(x => !x.IsExecuted)
                .Select(x => x.Expression)
                .ToList();
            if (not.Any())
            {
                throw new NotExecutedException(not);
            }
        }
    }
}