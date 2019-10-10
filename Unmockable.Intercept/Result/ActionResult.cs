using System.Linq.Expressions;
using Unmockable.Setup;

namespace Unmockable.Result
{
    internal class ActionResult : FuncResult<Nothing>
    {
        public ActionResult(LambdaExpression expression) : base(Nothing.Empty, expression)
        {
        }
        public override IResult<Nothing> Add(IResult<Nothing> next) => next;
        public override string ToString() => "void";
    }
}