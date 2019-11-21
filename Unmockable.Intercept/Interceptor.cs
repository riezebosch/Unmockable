namespace Unmockable
{
    public static class Interceptor
    {
        public static IIntercept<T> For<T>() => new Intercept<T>();
    }
}