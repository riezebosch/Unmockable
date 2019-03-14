using System;
using Unmockable.Exceptions;

namespace Unmockable
{
    public static class Arg
    {
        public static T Ignore<T>() => throw new PlaceholderException();

        public static T Equals<T>(Func<T, bool> pred) => throw new PlaceholderException();
    }
}