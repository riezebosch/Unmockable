using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Unmockable.Matchers
{
    internal class WithArgument: 
        IArgumentMatcher, 
        IEquatable<ValueArgument>, 
        IEquatable<NullArgument>
    {
        private readonly LambdaExpression _pred;
        
        public WithArgument(LambdaExpression pred) => 
            _pred = pred;

        [ExcludeFromCodeCoverage]
        public override int GetHashCode() => 
            throw new InvalidOperationException();

        public bool Equals(ValueArgument other)
        {
            InvokeAndUnwrap(other.Value);
            return true;
        }

        public bool Equals(NullArgument other)
        {
            InvokeAndUnwrap(null);
            return true;
        }

        public override bool Equals(object obj) => obj switch
        {
            NullArgument arg => Equals(arg),
            ValueArgument arg => Equals(arg),
            _ => throw new ArgumentException($"{nameof(Arg)}.{nameof(Arg.With)} only supports null and value arguments.", nameof(obj))
        };
            

        public override string ToString() =>
            _pred.ToString();

        private void InvokeAndUnwrap(object value)
        {
            try
            {
                _pred.Compile().DynamicInvoke(value);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException!;
            }
        }
    }
}