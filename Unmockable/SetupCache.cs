using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable
{
    internal class SetupCache
    {
        private readonly IDictionary<MethodMatcher, InterceptSetupBase<Intercept>> _setups = new Dictionary<MethodMatcher, InterceptSetupBase<Intercept>>();

        public TItem ToCache<TItem>(TItem setup) where TItem: InterceptSetupBase<Intercept>
        {
            _setups[setup.Expression.ToMatcher()] = setup;
            return setup;
        }

        public TItem FromCache<TItem>(LambdaExpression m) where TItem: InterceptSetupBase<Intercept>
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