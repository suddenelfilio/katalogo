using Katalogo;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Katalogo.Tests
{

    public class ScanningTests : TestBase
    {
        private IServiceCollection Collection { get; } = new ServiceCollection();

        [Fact]
        public void Scan_Catalogs()
        {
            ServiceDescriptor test = Collection.GetDescriptor<ITest>();
            Assert.Null(test);

            Collection.Scan(scan => scan.FromCallingAssembly().RegisterCatalogs());

            test = Collection.GetDescriptor<ITest>();
            Assert.NotNull(test);
            Assert.Equal(ServiceLifetime.Transient, test.Lifetime);
            Assert.Equal(typeof(Test), test.ImplementationType);
        }

        [Fact]
        public void AddCatalog()
        {
            ServiceDescriptor test = Collection.GetDescriptor<ITest>();
            Assert.Null(test);

            Collection.RegisterCatalog<TestCatalog>();

            test = Collection.GetDescriptor<ITest>();
            Assert.NotNull(test);
            Assert.Equal(ServiceLifetime.Transient, test.Lifetime);
            Assert.Equal(typeof(Test), test.ImplementationType);
        }

    }

    public interface ITest
    {
        void Hello();
    }

    public class Test : ITest
    {
        public void Hello()
        {

        }
    }

    public class TestCatalog : Catalog
    {
        public TestCatalog(IServiceCollection services) : base(services)
        {
            services.AddTransient<ITest, Test>();
        }
    }
}
