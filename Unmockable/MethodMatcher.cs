using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Unmockable
{
    public class MethodMatcher
    {
        private readonly MethodCallExpression _call;
        private readonly ArgumentsMatcher _arguments;

        public MethodMatcher(MethodCallExpression call)
        {
            _call = call;
            _arguments = new ArgumentsMatcher(_call.Arguments);
        }

        public override int GetHashCode() => 
            _call.Method.Name.GetHashCode() ^ _arguments.GetHashCode();

        public override bool Equals(object obj) => 
            obj is MethodMatcher m &&
            m._call.Method.Name.Equals(_call.Method.Name) &&
            m._arguments.Equals(_arguments);

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
                obj is ArgumentsMatcher x && x._arguments.SequenceEqual(_arguments);
            
            private static IArgumentMatcher ToMatcher(Expression arg)
            {
                var value = Expression.Lambda(arg).Compile().DynamicInvoke();
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
                obj is ValueArgument x && x._value.Equals(_value);
        }

        private class NullArgument : IArgumentMatcher
        {
            public override int GetHashCode() => 0;

            public override string ToString() => "null";

            public override bool Equals(object obj) => obj is NullArgument;
        }
        
        private class CollectionArgument : IArgumentMatcher
        {
            private readonly IEnumerable<object> _collection;

            public CollectionArgument(IEnumerable<object> collection) => 
                _collection = collection;

            public override int GetHashCode() => 
                _collection.Aggregate(0, (hash, item) => hash ^ item.GetHashCode());

            public override string ToString() => 
                $"[{string.Join(", ", _collection)}]";

            public override bool Equals(object obj) =>
                obj is CollectionArgument x && x._collection.SequenceEqual(_collection);
        }
    }
}