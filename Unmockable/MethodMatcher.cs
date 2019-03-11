using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable
{
    public class MethodMatcher
    {
        private readonly MethodCallExpression _call;
        private readonly ArgumentsMatcher _arguments;

        public MethodMatcher(LambdaExpression m)
        {
            _call = m.Body as MethodCallExpression ?? throw new NotInstanceMethodCallException(m.ToString());
            _arguments = new ArgumentsMatcher(_call.Arguments);
        }

        public override int GetHashCode() =>
            _call.Method.Name.GetHashCode();

        public override bool Equals(object obj) => 
            obj is MethodMatcher m &&
            _call.Method.Name.Equals(m._call.Method.Name) &&
            _arguments.Equals(m._arguments);

        public override string ToString() => 
            $"{_call.Method.Name}({_arguments})";

        private class ArgumentsMatcher
        {
            private readonly IEnumerable<IArgumentMatcher> _arguments;
            
            public ArgumentsMatcher(IEnumerable<Expression> arguments) => 
                _arguments = arguments.Select(ToMatcher);
            
            public override int GetHashCode() => 
                _arguments.Aggregate(0, (hash, x) => hash ^ x.GetHashCode());
            
            public override string ToString() => 
                string.Join(", ", _arguments);
            
            public override bool Equals(object obj) => 
                obj is ArgumentsMatcher x && _arguments.SequenceEqual(x._arguments);
        }
        
        private interface IArgumentMatcher
        {
        }       
        
        private class ValueArgument : IArgumentMatcher
        {
            private readonly object _value;
            public ValueArgument(object value) => _value = value;

            public override int GetHashCode() => _value.GetHashCode();

            public override string ToString() => _value.ToString();

            public override bool Equals(object obj) =>
                obj is ValueArgument x && _value.Equals(x._value);
        }

        private class NullArgument : IArgumentMatcher
        {
            public override int GetHashCode() => 0;

            public override string ToString() => "null";

            public override bool Equals(object obj) => obj is NullArgument;
        }
        
        private class CollectionArgument : IArgumentMatcher
        {
            private readonly IEnumerable<IArgumentMatcher> _collection;

            public CollectionArgument(IEnumerable<object> collection) => 
                _collection = collection.Select(ToMatcher);

            public override int GetHashCode() => 
                _collection.Aggregate(0, (hash, item) => hash ^ item.GetHashCode());

            public override string ToString() => 
                $"[{string.Join(", ", _collection)}]";

            public override bool Equals(object obj) =>
                obj is CollectionArgument x && _collection.SequenceEqual(x._collection);
        }
        
        private static IArgumentMatcher ToMatcher(Expression arg)
        {
            if (arg is MethodCallExpression call 
                && call.Method.DeclaringType == typeof(Arg) 
                && call.Method.Name == "Ignore")
            {
                return new IgnoreArgument();
            }
            return ToMatcher(Expression.Lambda(arg).Compile().DynamicInvoke());
        }

        private class IgnoreArgument : IArgumentMatcher
        {
            public override int GetHashCode() => 0;
            public override bool Equals(object obj) => true;
        }

        private static IArgumentMatcher ToMatcher(object value)
        {
            switch (value)
            {
                case null:
                    return new NullArgument();
                case IEnumerable collection:
                    return new CollectionArgument(collection.Cast<object>());
                default:
                    return new ValueArgument(value);
            }
        }
    }
}