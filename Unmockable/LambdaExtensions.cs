using System.Linq.Expressions;
using Unmockable.Exceptions;

namespace Unmockable
{
    public static class LambdaExtensions
    {
        public static int ToKey(this LambdaExpression m)
        {
            if (!(m.Body is MethodCallExpression))
            {
                throw new NotInstanceMethodCallException(m.ToString());
            }
            
            return m.ToString().GetHashCode();
        }
        
        public static MethodMatcher ToMatcher(this LambdaExpression m)
        {
            return new MethodMatcher(m); 
        }
    }
}