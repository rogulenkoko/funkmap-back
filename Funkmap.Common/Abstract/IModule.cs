using Autofac;

namespace Funkmap.Common.Abstract
{
    public interface IModule
    {
        void Register(ContainerBuilder builder);
    }
}
