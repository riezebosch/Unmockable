namespace Unmockable
{
    public static class ObjectExtensions
    {
        public static IUnmockable<T> Wrap<T>(this T item)
        {
            return new Wrap<T>(item);
        }
    }
}