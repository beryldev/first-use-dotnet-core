using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Wrhs.Core;
using Wrhs.Data;
using Wrhs.Data.ContextFactory;
using Wrhs.Data.Repository;
using Wrhs.Documents;
using Wrhs.Operations;
using Wrhs.Operations.Delivery;
using Wrhs.Products;
using Wrhs.Products.Commands;
using Wrhs.WebApp.Utils;

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

            services.AddMemoryCache();
            
            ConfigureDI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            

            // app.UseStaticFiles(new StaticFileOptions()
            // {
            //     FileProvider = new PhysicalFileProvider(
            //         Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
            //     RequestPath = new PathString("/Static")
            // });

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}");
            });

            
        }

        void ConfigureDI(IServiceCollection services)
        {
            services.AddTransient(typeof(WrhsContext),
                (IServiceProvider provider) => { return SqliteContextFactory.Create("Filename=./wrhs.db"); });

            services.AddTransient(typeof(ICache), (IServiceProvider provider)=>
            {
                return new Cache(provider.GetService(typeof(IMemoryCache)) as IMemoryCache);
            });
            
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

            services.AddTransient(typeof(IRepository<DeliveryDocument>), (IServiceProvider provider)=>
            { 
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new DeliveryDocumentRepository(context);
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

            services.AddTransient(typeof(IValidator<CreateProductCommand>), (IServiceProvider provider) => 
            {
                var productRepository = provider.GetService(typeof(IRepository<Product>)) as IRepository<Product>;
                return new CreateProductCommandValidator(productRepository);
            });

            services.AddTransient(typeof(ICommandHandler<CreateProductCommand>), (IServiceProvider provider) => 
            {
                var productRepository = provider.GetService(typeof(IRepository<Product>)) as IRepository<Product>;
                return new CreateProductCommandHandler(productRepository);
            });

            services.AddTransient(typeof(IValidator<IDocAddLineCmd>), (IServiceProvider provider) => 
            {
                var productRepository = provider.GetService(typeof(IRepository<Product>)) as IRepository<Product>;
                return (new DocAddLineCmdValidator(productRepository));
            });
        }
    }
}
