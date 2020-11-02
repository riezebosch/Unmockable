using System;
using Unmockable.Exceptions;

namespace Unmockable
{
    public static class Arg
    {
        /// <summary>
        /// Ignore the exact value of this argument when matching the invocation with the available setups.
        /// </summary>
        public static T Ignore<T>() => throw new PlaceholderException();

        /// <summary>
        /// Match the exact value of this argument with the custom predicate on the available setups.
        /// </summary>
        public static T Where<T>(Func<T, bool> pred) => throw new PlaceholderException();

        /// <summary>
        /// Do a custom action with the value of this argument, for example something with FluentAssertions.
        /// </summary>
        public static T With<T>(Action<T> action) => throw new PlaceholderException();
    }
}