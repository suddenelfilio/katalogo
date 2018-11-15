using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace Katalogo
{
    internal class TypeSourceSelector : ITypeSourceSelector
    {
        public IServiceCollection Services { get; }

        public TypeSourceSelector(IServiceCollection services)
        {
            Services = services;
        }

        /// <inheritdoc />
        public ICatalogSelector FromAssemblyOf<T>()
        {
            return InternalFromAssembliesOf(new[] { typeof(T).GetTypeInfo() });
        }

        public ICatalogSelector FromCallingAssembly()
        {
            return FromAssemblies(Assembly.GetCallingAssembly());
        }

        public ICatalogSelector FromExecutingAssembly()
        {
            return FromAssemblies(Assembly.GetExecutingAssembly());
        }

        public ICatalogSelector FromEntryAssembly()
        {
            return FromAssemblies(Assembly.GetEntryAssembly());
        }

        public ICatalogSelector FromApplicationDependencies()
        {
            return FromApplicationDependencies(_ => true);
        }

        public ICatalogSelector FromApplicationDependencies(Func<Assembly, bool> predicate)
        {
            try
            {
                return FromDependencyContext(DependencyContext.Default, predicate);
            }
            catch
            {
                // Something went wrong when loading the DependencyContext, fall
                // back to loading all referenced assemblies of the entry assembly...
                return FromAssemblyDependencies(Assembly.GetEntryAssembly());
            }
        }

        public ICatalogSelector FromDependencyContext(DependencyContext context)
        {
            return FromDependencyContext(context, _ => true);
        }

        public ICatalogSelector FromDependencyContext(DependencyContext context, Func<Assembly, bool> predicate)
        {
            Preconditions.NotNull(context, nameof(context));
            Preconditions.NotNull(predicate, nameof(predicate));

            Assembly[] assemblies = context.RuntimeLibraries
                .SelectMany(library => library.GetDefaultAssemblyNames(context))
                .Select(Assembly.Load)
                .Where(predicate)
                .ToArray();

            return InternalFromAssemblies(assemblies);
        }

        public ICatalogSelector FromAssemblyDependencies(Assembly assembly)
        {
            Preconditions.NotNull(assembly, nameof(assembly));

            List<Assembly> assemblies = new List<Assembly> { assembly };

            try
            {
                AssemblyName[] dependencyNames = assembly.GetReferencedAssemblies();

                foreach (AssemblyName dependencyName in dependencyNames)
                {
                    try
                    {
                        // Try to load the referenced assembly...
                        assemblies.Add(Assembly.Load(dependencyName));
                    }
                    catch
                    {
                        // Failed to load assembly. Skip it.
                    }
                }

                return InternalFromAssemblies(assemblies);
            }
            catch
            {
                return InternalFromAssemblies(assemblies);
            }
        }

        public ICatalogSelector FromAssembliesOf(params Type[] types)
        {
            Preconditions.NotNull(types, nameof(types));

            return InternalFromAssembliesOf(types.Select(x => x.GetTypeInfo()));
        }

        public ICatalogSelector FromAssembliesOf(IEnumerable<Type> types)
        {
            Preconditions.NotNull(types, nameof(types));

            return InternalFromAssembliesOf(types.Select(t => t.GetTypeInfo()));
        }

        public ICatalogSelector FromAssemblies(params Assembly[] assemblies)
        {
            Preconditions.NotNull(assemblies, nameof(assemblies));

            return InternalFromAssemblies(assemblies);
        }

        public ICatalogSelector FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            Preconditions.NotNull(assemblies, nameof(assemblies));

            return InternalFromAssemblies(assemblies);
        }



        private ICatalogSelector InternalFromAssembliesOf(IEnumerable<TypeInfo> typeInfos)
        {
            return InternalFromAssemblies(typeInfos.Select(t => t.Assembly));
        }

        private ICatalogSelector InternalFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            return new CatalogSelector(assemblies.SelectMany(asm => asm.DefinedTypes).Where(x => x.IsAssignableTo(typeof(Catalog))).Select(x => x.AsType()), Services);
        }


    }
}
