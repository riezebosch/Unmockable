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

        public TItem ToCache<TItem>(TItem setup) where TItem: ISetup
        {
            _setups[setup.Expression.ToMatcher()] = setup;
            return setup;
        }

        public TItem FromCache<TItem>(LambdaExpression m) where TItem: ISetup
        {
            var key = m.ToMatcher();
            if (!_setups.TryGetValue(key, out var setup))
            {
                throw new NoSetupException(key.ToString());
            }

            return (TItem)setup;
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