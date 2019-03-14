using System.Linq.Expressions;

namespace Unmockable
{
    internal static class LambdaExtensions
    {
        public static int ToKey(this LambdaExpression m)
        {
            return m.ToString().GetHashCode();
        }
    }
}