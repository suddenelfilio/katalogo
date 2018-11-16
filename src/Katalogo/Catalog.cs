using Microsoft.Extensions.DependencyInjection;

namespace Katalogo
{
    public abstract class Catalog
    {
        public IServiceCollection Services { get; }

        protected Catalog(IServiceCollection services)
        {
            Services = services;
        }
    }
}
