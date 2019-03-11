using System;

namespace Unmockable
{
    public static class Arg
    {
        public static T Ignore<T>() => throw new NotImplementedException();

        public static T Equals<T>(Func<T, bool> pred) => throw new NotImplementedException();
    }
}