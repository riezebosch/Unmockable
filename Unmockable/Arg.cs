using System;

namespace Unmockable
{
    public static class Arg
    {
        public static T Ignore<T>()
        {
            throw new NotImplementedException();
        }
    }
}