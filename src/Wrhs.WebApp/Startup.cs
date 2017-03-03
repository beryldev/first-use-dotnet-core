using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Data;
using Wrhs.Data.ContextFactory;
using Wrhs.Data.Service;
using Wrhs.Delivery;
using Wrhs.Products;
using Wrhs.Release;
using Wrhs.Relocation;
using Wrhs.Services;
using Wrhs.WebApp.Utils;
using System.Linq;
using Microsoft.AspNetCore.Identity;

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
            
            services.AddDbContext<IdentityDbContext>(options => 
                options.UseSqlite("Data Source=users.sqlite", 
            optionsBuilder => optionsBuilder.MigrationsAssembly("Wrhs.WebApp")));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            // Add framework services.
            services.AddMvc().AddJsonOptions(options => {
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddMemoryCache();
            
            ConfigureDI(services);

            services.Configure<IdentityOptions>(options =>
            {
                options.Cookies.ApplicationCookie.LoginPath="/Account/Login";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ILoggerFactory loggerFactory, IdentityDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dbContext.Database.Migrate(); //this will generate the db if it does not exist
            }

           
              

            app.UseIdentity();
            app.UseStaticFiles();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if(!dbContext.Users.Any(u=>u.UserName=="admin"))
            {
                var result = userManager.CreateAsync(new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@local.pl"
                }, "Admin123!@#");

            }
                
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}");
            });

            using(var context = SqliteContextFactory.Create("Filename=./wrhs.db"))
            {
                context.Database.EnsureCreated();
            }
        }

        void ConfigureDI(IServiceCollection services)
        {
            services.AddTransient(typeof(WrhsContext),
                (IServiceProvider provider) => 
                { 
                    var context = SqliteContextFactory.Create("Filename=./wrhs.db"); 
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

            services.AddTransient(typeof(IDocumentService), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                var numerator = new DocumentNumerator(new Dictionary<DocumentType, string>
                {
                    {DocumentType.Delivery, "DLV"},
                    {DocumentType.Relocation, "RLC"},
                    {DocumentType.Release, "RLS"}
                });
                return new DocumentService(context, numerator);
            });

            services.AddTransient(typeof(IOperationService), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new OperationService(context);
            });

            services.AddTransient(typeof(IStockService), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new StockService(context);
            });

            services.AddTransient(typeof(IEventBus), (IServiceProvider provider)=>
            {
                var context = provider.GetService(typeof(WrhsContext)) as WrhsContext;
                return new EventBus((Type type)=>{return new List<IEventHandler>();});
            });

            services.AddTransient(typeof(ICommandBus), (IServiceProvider provider)=>
            {
                var operationSrv = provider.GetService(typeof(IOperationService)) as IOperationService;
                var productSrv = provider.GetService(typeof(IProductService)) as IProductService;
                var stockSrv = provider.GetService(typeof(IStockService)) as IStockService;
                var docSrv = provider.GetService(typeof(IDocumentService)) as IDocumentService;
                var eventBus = provider.GetService(typeof(IEventBus)) as IEventBus;
                var commands = new Dictionary<Type, Func<ICommandHandler>>
                {
                    { typeof(CreateDeliveryDocumentCommand), ()=>{
                        var validator = new CreateDeliveryDocumentCommandValidator(productSrv);
                        return new CreateDeliveryDocumentCommandHandler(validator, eventBus, docSrv);
                    }},
                    { typeof(CreateRelocationDocumentCommand), ()=>{
                        var validator = new CreateRelocationDocumentCommandValidator(productSrv, stockSrv);
                        return new CreateRelocationDocumentCommandHandler(validator, eventBus, docSrv);
                    }},
                    { typeof(CreateReleaseDocumentCommand), ()=>{
                        var validator = new CreateReleaseDocumentCommandValidator(productSrv, stockSrv);
                        return new CreateReleaseDocumentCommandHandler(validator, eventBus, docSrv);
                    }},
                    { typeof(RemoveDocumentCommand), ()=>{
                        var validator = new RemoveDocumentCommandValidator(docSrv);
                        return new RemoveDocumentCommandHandler(validator, eventBus, docSrv);
                    }},
                    { typeof(ChangeDocStateCommand), ()=>{
                        var validator = new ChangeDocStateCommandValidator(docSrv);
                        return new ChangeDocStateCommandHandler(validator, eventBus, docSrv);
                    }},
                    { typeof(UpdateDeliveryDocumentCommand), ()=>{
                        var innerValid = new CreateDeliveryDocumentCommandValidator(productSrv);
                        var validator = new UpdateDeliveryDocumentCommandValidator(innerValid);
                        return new UpdateDeliveryDocumentCommandHandler(validator, eventBus, docSrv);
                    }},
                    { typeof(UpdateRelocationDocumentCommand), ()=>{
                        var innerValid = new CreateRelocationDocumentCommandValidator(productSrv, stockSrv);
                        var validator = new UpdateRelocationDocumentCommandValidator(innerValid);
                        return new UpdateRelocationDocumentCommandHandler(validator, eventBus, docSrv);
                    }},
                    { typeof(UpdateReleaseDocumentCommand), ()=>{
                        var innerValid = new CreateReleaseDocumentCommandValidator(productSrv, stockSrv);
                        var validator = new UpdateReleaseDocumentCommandValidator(innerValid);
                        return new UpdateReleaseDocumentCommandHandler(validator, eventBus, docSrv);
                    }},
                    { typeof(CreateProductCommand), ()=>{
                        var validator = new CreateProductCommandValidator(productSrv);
                        return new CreateProductCommandHandler(validator, eventBus, productSrv);
                    }},
                    { typeof(UpdateProductCommand), ()=>{
                        var validator = new UpdateProductCommandValidator(productSrv);
                        return new UpdateProductCommandHandler(validator, eventBus, productSrv);
                    }},
                    { typeof(DeleteProductCommand), ()=>{
                        var validator = new DeleteProductCommandValidator();
                        return new DeleteProductCommandHandler(validator, eventBus, productSrv);
                    }},
                    { typeof(BeginOperationCommand), ()=>{
                        var validator = new BeginOperationCommandValidator(docSrv, operationSrv);
                        return new BeginOperationCommandHandler(validator, eventBus, operationSrv);
                    }},
                    { typeof(ProcessDeliveryOperationCommand), ()=>{
                        var validator = new ProcessDeliveryOperationCommandValidator(operationSrv, productSrv);
                        return new ProcessDeliveryOperationCommandHandler(validator, eventBus, stockSrv, operationSrv);
                    }},
                    { typeof(ProcessRelocationOperationCommand), ()=>{
                        var validator = new ProcessRelocationOperationCommandValidator(operationSrv, productSrv);
                        return new ProcessRelocationOperationCommandHandler(validator, eventBus, stockSrv, operationSrv);
                    }},
                    { typeof(ProcessReleaseOperationCommand), ()=>{
                        var validator = new ProcessReleaseOperationCommandValidator(operationSrv, productSrv);
                        return new ProcessReleaseOperationCommandHandler(validator, eventBus, stockSrv, operationSrv);
                    }},
                    { typeof(ExecuteOperationCommand), ()=>{
                        var validator = new ExecuteOperationCommandValidator(operationSrv);
                        var parameters = new HandlerParameters
                        {
                            OperationService = operationSrv,
                            DocumentService = docSrv,
                            StockService = stockSrv
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
