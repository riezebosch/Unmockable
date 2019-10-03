using System;
using Unmockable.Setup;

namespace Unmockable.Result
{
    internal class ActionResult : IResult<Nothing>
    {
        public Nothing Result
        {
            get
            {
                IsDone = true;
                return Nothing.Empty;
            }
        }

        public bool IsDone { get; private set; }
        public IResult<Nothing> Add(IResult<Nothing> next) => throw new NotImplementedException();
    }
}