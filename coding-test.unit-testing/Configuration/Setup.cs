using coding_test.Data;
using coding_test.Repositories;
using coding_test.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace coding_test.unit_testing.Configuration
{
    public static class Setup
    {
        public static readonly IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        public static ServiceProvider SetupServiceProvider(Action<IServiceCollection> configure = null)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddTransient<IConfiguration>(provider => config)
                .AddDbContext<BankDBContext>(options => options.UseInMemoryDatabase(databaseName: "MyBankTEST"))
                .AddTransient<AccountService>()
                .AddTransient<TransactionService>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddScoped(typeof(IRepository<>), typeof(Repository<>));

            if (configure != null)
            {
                configure(serviceCollection);
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
