using System;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Katalogo
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds registrations to the <paramref name="services"/> collection using
        /// conventions specified using the <paramref name="action"/>.
        /// </summary>
        /// <param name="services">The services to add to.</param>
        /// <param name="action">The configuration action.</param>
        /// <exception cref="ArgumentNullException">If either the <paramref name="services"/>
        /// or <paramref name="action"/> arguments are <c>null</c>.</exception>
        public static IServiceCollection Scan(this IServiceCollection services, Action<ITypeSourceSelector> action)
        {
            Preconditions.NotNull(services, nameof(services));
            Preconditions.NotNull(action, nameof(action));

            TypeSourceSelector selector = new TypeSourceSelector(services);

            action(selector);

            return services;
        }

        public static IServiceCollection RegisterCatalog<T>(this IServiceCollection services) where T : Catalog
        {
            Activator.CreateInstance(typeof(T), services);
            return services;
        }


    }
}
