namespace Unmockable.Setup
{
    /// <summary>
    /// Only used to handle Actions like Func.
    /// </summary>
    internal class Nothing
    {
        public static readonly Nothing Empty = new Nothing();

        private Nothing()
        {
        }
    }
}