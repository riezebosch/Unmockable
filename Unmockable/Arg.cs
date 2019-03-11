using System;

namespace Unmockable
{
    public class Arg
    {
        public static T Ignore<T>()
        {
            throw new NotImplementedException();
        }
    }
}