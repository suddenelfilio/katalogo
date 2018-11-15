using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace Katalogo
{
    public interface IAssemblySelector : IFluentInterface
    {
        /// <summary>
        /// Will scan for types from the calling assembly.
        /// </summary>
        ICatalogSelector FromCallingAssembly();

        /// <summary>
        /// Will scan for types from the currently executing assembly.
        /// </summary>
        ICatalogSelector FromExecutingAssembly();

        /// <summary>
        /// Will scan for types from the entry assembly.
        /// </summary>
        ICatalogSelector FromEntryAssembly();

        /// <summary>
        /// Will load and scan all runtime libraries referenced by the currently executing application.
        /// Calling this method is equivalent to calling <see cref="FromDependencyContext(DependencyContext)"/> and passing in <see cref="DependencyContext.Default"/>.
        /// </summary>
        /// <remarks>
        /// If loading <see cref="DependencyContext.Default"/> fails, this method will fall back to calling <see cref="FromAssemblyDependencies(Assembly)"/>,
        /// using the entry assembly.
        /// </remarks>
        ICatalogSelector FromApplicationDependencies();

        /// <summary>
        /// Will load and scan all runtime libraries referenced by the currently executing application.
        /// Calling this method is equivalent to calling <see cref="FromDependencyContext(DependencyContext, Func{Assembly, bool})"/> and passing in <see cref="DependencyContext.Default"/>.
        /// </summary>
        /// <remarks>
        /// If loading <see cref="DependencyContext.Default"/> fails, this method will fall back to calling <see cref="FromAssemblyDependencies(Assembly)"/>,
        /// using the entry assembly.
        /// </remarks>
        /// <param name="predicate">The predicate to match assemblies.</param>
        ICatalogSelector FromApplicationDependencies(Func<Assembly, bool> predicate);

        /// <summary>
        /// Will load and scan all runtime libraries referenced by the currently specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The assembly whose dependencies should be scanned.</param>
        ICatalogSelector FromAssemblyDependencies(Assembly assembly);

        /// <summary>
        /// Will load and scan all runtime libraries in the given <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The dependency context.</param>
        ICatalogSelector FromDependencyContext(DependencyContext context);

        /// <summary>
        /// Will load and scan all runtime libraries in the given <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The dependency context.</param>
        /// <param name="predicate">The predicate to match assemblies.</param>
        ICatalogSelector FromDependencyContext(DependencyContext context, Func<Assembly, bool> predicate);

        /// <summary>
        /// Will scan for types from the assembly of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type in which assembly that should be scanned.</typeparam>
        ICatalogSelector FromAssemblyOf<T>();

        /// <summary>
        /// Will scan for types from the assemblies of each <see cref="Type"/> in <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The types in which assemblies that should be scanned.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="types"/> argument is <c>null</c>.</exception>
        ICatalogSelector FromAssembliesOf(params Type[] types);

        /// <summary>
        /// Will scan for types from the assemblies of each <see cref="Type"/> in <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The types in which assemblies that should be scanned.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="types"/> argument is <c>null</c>.</exception>
        ICatalogSelector FromAssembliesOf(IEnumerable<Type> types);

        /// <summary>
        /// Will scan for types in each <see cref="Assembly"/> in <paramref name="assemblies"/>.
        /// </summary>
        /// <param name="assemblies">The assemblies to should be scanned.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="assemblies"/> argument is <c>null</c>.</exception>
        ICatalogSelector FromAssemblies(params Assembly[] assemblies);

        /// <summary>
        /// Will scan for types in each <see cref="Assembly"/> in <paramref name="assemblies"/>.
        /// </summary>
        /// <param name="assemblies">The assemblies to should be scanned.</param>
        /// <exception cref="ArgumentNullException">If the <paramref name="assemblies"/> argument is <c>null</c>.</exception>
        ICatalogSelector FromAssemblies(IEnumerable<Assembly> assemblies);
    }
}
