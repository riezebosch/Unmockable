using System.Linq.Expressions;

namespace Unmockable.Result
{
    internal class ActionResult : FuncResult<Void>
    {
        public ActionResult(LambdaExpression expression) : 
            base(new Void(), expression)
        {
        }
        
        public override IResult<Void> Next(IResult<Void> next) => next;
    }
}