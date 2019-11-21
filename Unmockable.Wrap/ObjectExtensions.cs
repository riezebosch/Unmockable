namespace Unmockable
{
    public static class ObjectExtensions
    {
        public static IUnmockable<T> Wrap<T>(this T item) => new Wrap<T>(item);
    }
}