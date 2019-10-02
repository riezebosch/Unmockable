using Unmockable.Setup;

namespace Unmockable.Result
{
    internal class NothingResult : INextResult<Nothing>
    {
        public Nothing Result => Nothing.Empty;
        public INextResult<Nothing>? Next { get; set; }

        public NothingResult()
        {
            Next = this;
        }
    }
}