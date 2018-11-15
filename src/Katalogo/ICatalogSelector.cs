using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Katalogo
{
    public interface ICatalogSelector
    {
        void RegisterCatalogs();
    }

    public class CatalogSelector : ICatalogSelector
    {
        private readonly IEnumerable<Type> _catalogs;
        private readonly IServiceCollection _services;

        public CatalogSelector(IEnumerable<Type> catalogs, IServiceCollection services)
        {
            _catalogs = catalogs;
            _services = services;
        }

        public void RegisterCatalogs()
        {
            foreach (Type catalog in _catalogs)
            {
                Activator.CreateInstance(catalog, _services);
            }
        }
    }
}
