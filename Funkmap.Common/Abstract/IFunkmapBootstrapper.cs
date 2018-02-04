using Autofac;

namespace Funkmap.Common.Abstract
{
    public interface IFunkmapBootstrapper
    {
        void Configure(IContainer container);
    }
}
