using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Data;
using Wrhs.Data.ContextFactory;
using Wrhs.Data.Persist;
using Wrhs.Data.Service;
using Wrhs.Delivery;
using Wrhs.Products;
using Wrhs.Release;
using Wrhs.Relocation;
using Wrhs.Services;
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
            services.AddMvc().AddJsonOptions(options => {
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddMemoryCache();
            
            ConfigureDI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}");
            });
        }

        void ConfigureDI(IServiceCollection services)
        {
            services.AddTransient(typeof(WrhsContext),
                (IServiceProvider provider) => 
                { 
                    var context = SqliteContextFactory.Create("Filename=./wrhs.db");
                    //var context = InMemoryContextFactory.Create();
                    context.Database.EnsureCreated();
                    return context; 
                });

            services.AddTransient(typeof(ICache), (IServiceProvider provider)=>
            {
                var memoryCache = provider.GetService(typeof(IMemoryCache)) as IMemoryCache;
                return new Cache(memoryCache);
            });

            services.AddTransient(typeof(IProductService), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new ProductService(context);
            });

            services.AddTransient(typeof(IProductPersist), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new ProductPersist(context);
            });

            services.AddTransient(typeof(IDocumentService), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new DocumentService(context);
            });

            services.AddTransient(typeof(IDocumentPersist), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                var numerator = new DocumentNumerator(new Dictionary<DocumentType, string>
                {
                    {DocumentType.Delivery, "DLV"},
                    {DocumentType.Relocation, "RLC"},
                    {DocumentType.Release, "RLS"}
                });
                return new DocumentPersist(context, numerator);
            });

            services.AddTransient(typeof(IOperationService), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new OperationService(context);
            });

            services.AddTransient(typeof(IOperationPersist), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new OperationPersist(context);
            });

            services.AddTransient(typeof(IStockService), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new StockService(context);
            });

            services.AddTransient(typeof(IShiftPersist), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new ShiftPersist(context);
            });

            services.AddTransient(typeof(IEventBus), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new EventBus((Type type)=>{return new List<IEventHandler>();});
            });

            services.AddTransient(typeof(ICommandBus), (IServiceProvider provider)=>
            {
                var operationSrv = provider.GetService(typeof(IOperationService)) as IOperationService;
                var operationPersist = provider.GetService(typeof(IOperationPersist)) as IOperationPersist;
                var productSrv = provider.GetService(typeof(IProductService)) as IProductService;
                var productPersist = provider.GetService(typeof(IProductPersist)) as IProductPersist;
                var stockSrv = provider.GetService(typeof(IStockService)) as IStockService;
                var docPersist = provider.GetService(typeof(IDocumentPersist)) as IDocumentPersist;
                var docSrv = provider.GetService(typeof(IDocumentService)) as IDocumentService;
                var eventBus = provider.GetService(typeof(IEventBus)) as IEventBus;
                var shiftPersist = provider.GetService(typeof(IShiftPersist)) as IShiftPersist;
                var commands = new Dictionary<Type, Func<ICommandHandler>>
                {
                    { typeof(CreateDeliveryDocumentCommand), ()=>{
                        var validator = new CreateDeliveryDocumentCommandValidator(productSrv);
                        return new CreateDeliveryDocumentCommandHandler(validator, eventBus, docPersist);
                    }},
                    { typeof(CreateRelocationDocumentCommand), ()=>{
                        var validator = new CreateRelocationDocumentCommandValidator(productSrv, stockSrv);
                        return new CreateRelocationDocumentCommandHandler(validator, eventBus, docPersist);
                    }},
                    { typeof(CreateReleaseDocumentCommand), ()=>{
                        var validator = new CreateReleaseDocumentCommandValidator(productSrv, stockSrv);
                        return new CreateReleaseDocumentCommandHandler(validator, eventBus, docPersist);
                    }},
                    { typeof(CreateProductCommand), ()=>{
                        var validator = new CreateProductCommandValidator(productSrv);
                        return new CreateProductCommandHandler(validator, eventBus, productPersist);
                    }},
                    { typeof(UpdateProductCommand), ()=>{
                        var validator = new UpdateProductCommandValidator(productSrv);
                        return new UpdateProductCommandHandler(validator, eventBus, productPersist, productSrv);
                    }},
                    { typeof(DeleteProductCommand), ()=>{
                        var validator = new DeleteProductCommandValidator();
                        return new DeleteProductCommandHandler(validator, eventBus, productSrv, productPersist);
                    }},
                    { typeof(BeginOperationCommand), ()=>{
                        var validator = new BeginOperationCommandValidator(docSrv, operationSrv);
                        return new BeginOperationCommandHandler(validator, eventBus, operationPersist);
                    }},
                    { typeof(ProcessDeliveryOperationCommand), ()=>{
                        var validator = new ProcessDeliveryOperationCommandValidator(operationSrv, productSrv);
                        return new ProcessDeliveryOperationCommandHandler(validator, eventBus, shiftPersist, operationSrv);
                    }},
                     { typeof(ProcessRelocationOperationCommand), ()=>{
                        var validator = new ProcessRelocationOperationCommandValidator(operationSrv, productSrv);
                        return new ProcessRelocationOperationCommandHandler(validator, eventBus, shiftPersist, operationSrv);
                    }},
                    { typeof(ExecuteOperationCommand), ()=>{
                        var validator = new ExecuteOperationCommandValidator(operationSrv);
                        var parameters = new HandlerParameters
                        {
                            OperationService = operationSrv,
                            OperationPersist = operationPersist,
                            DocumentPersist = docPersist,
                            ShiftPersist = shiftPersist
                        };
                        return new ExecuteOperationCommandHandler(validator, eventBus, parameters);
                    }}
                };

                var commandBus = new CommandBus((Type type)=>{
                    return commands[type].Invoke();
                });

                return commandBus;
            });
        }
    }
}
