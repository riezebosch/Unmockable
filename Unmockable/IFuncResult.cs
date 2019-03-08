using System;
using System.Threading.Tasks;

namespace Unmockable
{
    public interface IFuncResult<T, TResult>
    {
        Intercept<T> Returns(TResult result);
        
        Intercept<T> Throws<TException>() 
            where TException: Exception, new();
    }
}