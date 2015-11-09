using StepMap.DAL;
using System.ServiceModel;

namespace StepMap.ServiceImpl
{
    public interface IOperationContextProvider
    {
        User CurrentUser { get; }
    }
}