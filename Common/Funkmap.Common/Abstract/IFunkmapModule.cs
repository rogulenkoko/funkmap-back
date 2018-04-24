using Autofac;

namespace Funkmap.Common.Abstract
{
    public interface IFunkmapModule
    {
        void Register(ContainerBuilder builder);
    }
}
