using System.Collections.Generic;
using System.Linq;
using Unmockable.Exceptions;
using Unmockable.Matchers;

namespace Unmockable.Setup
{
    internal class SetupCache
    {
        private readonly IDictionary<IMemberMatcher, ISetup> _setups = new Dictionary<IMemberMatcher, ISetup>();

        public TItem Store<TItem>(TItem setup) where TItem: ISetup
        {
            _setups[setup.Expression] = setup;
            return setup;
        }
        
        public ISetup<TResult> Load<TResult>(IMemberMatcher m)
        {
            return _setups.TryGetValue(m, out var setup)
                ? (ISetup<TResult>) setup
                : throw new SetupNotFoundException(m);
        }

        public void Verify()
        {
            var not = _setups
                .Values
                .Where(x => !x.IsExecuted)
                .ToList();
            if (not.Any())
            {
                throw new NotExecutedException(not);
            }
        }
    }
}