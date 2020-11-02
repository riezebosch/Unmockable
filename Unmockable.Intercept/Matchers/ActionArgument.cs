using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Unmockable.Matchers
{
    internal class ActionArgument : IArgumentMatcher, IEquatable<ValueArgument>
    {
        private readonly Expression _pred;
        
        public ActionArgument(Expression pred) => _pred = pred;

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => throw new InvalidOperationException();

        public bool Equals(ValueArgument other)
        {
            try
            {
                ((Delegate) Expression.Lambda(_pred).Compile().DynamicInvoke()).DynamicInvoke(other.Value);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException!;
            }
            
            return true;
        }

        public override bool Equals(object obj) => Equals(obj as ValueArgument ?? throw new ArgumentException($"only supports {nameof(ValueArgument)}", nameof(obj))); 

        public override string ToString() => _pred.ToString();
    }
}