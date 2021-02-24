using Unmockable.Matchers;

namespace Unmockable.Result
{
    internal class ActionResult : FuncResult<Void>
    {
        public ActionResult() : 
            base(new Void())
        {
        }
        
        public override IResult<Void> Add(IResult<Void> result, IMemberMatcher matcher) => result;
    }
}