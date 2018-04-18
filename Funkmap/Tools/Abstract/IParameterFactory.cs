using Funkmap.Domain.Abstract;
using Funkmap.Models.Requests;

namespace Funkmap.Tools.Abstract
{
    public interface IParameterFactory
    {
        IFilterParameter CreateParameter(FilteredRequest request);
    }
}
