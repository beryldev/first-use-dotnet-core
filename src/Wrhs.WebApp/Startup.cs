using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wrhs.Core;
using Wrhs.Data;
using Wrhs.Data.ContextFactory;
using Wrhs.Data.Repository;
using Wrhs.Operations;
using Wrhs.Products;

namespace Wrhs.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            
            ConfigureDI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }

        void ConfigureDI(IServiceCollection services)
        {
            services.AddTransient(typeof(WrhsContext),
                (IServiceProvider provider) => { return SqliteContextFactory.Create("Filename=./wrhs.db"); });

             services.AddTransient(typeof(IRepository<Product>), (IServiceProvider provider)=>
            { 
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new ProductRepository(context);
            });

            services.AddTransient(typeof(IRepository<Allocation>), (IServiceProvider provider)=>
            { 
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new AllocationRepository(context);
            });

            services.AddTransient(typeof(IStockCache), (IServiceProvider provider)=>
            { 
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new DbStockCache(context);
            });

            services.AddTransient(typeof(IAllocationService), (IServiceProvider provider)=>
            { 
                var allocationRepository = provider.GetService(typeof(IRepository<Allocation>)) as IRepository<Allocation>;
                return new AllocationService(allocationRepository);
            });

            services.AddTransient(typeof(IWarehouse), (IServiceProvider provider)=>
            { 
                var allocationService = provider.GetService(typeof(IAllocationService)) as IAllocationService;
                var stockCache = provider.GetService(typeof(IStockCache)) as IStockCache;
                return new Warehouse(allocationService, stockCache);
            });
        }
    }
}
