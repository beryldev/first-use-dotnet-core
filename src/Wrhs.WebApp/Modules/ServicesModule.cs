using Autofac;
using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Data;
using Wrhs.Data.Service;
using Wrhs.Services;

namespace Wrhs.WebApp.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var mapping = new Dictionary<DocumentType, string>
            {
                { DocumentType.Delivery, "DLV" },
                { DocumentType.Relocation, "RLC" },
                { DocumentType.Release, "RLS" }
            };

            builder.RegisterType<WrhsContext>().AsSelf();
            builder.RegisterType<FakeEventBus>().As<IEventBus>();
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<StockService>().As<IStockService>();
            builder.RegisterType<OperationService>().As<IOperationService>();
            builder.RegisterType<DocumentService>().As<IDocumentService>();
            builder.Register<IDocumentNumerator>((c) => new DocumentNumerator(mapping));
        }
    }

    public class FakeEventBus : IEventBus
    {
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            return;
        }
    }
}
