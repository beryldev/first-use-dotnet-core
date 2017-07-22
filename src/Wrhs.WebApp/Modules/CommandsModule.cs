using Autofac;
using System;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Core.CommandHandlers;

namespace Wrhs.WebApp.Modules
{
    public class CommandsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var wrhsAsm = System.Reflection.Assembly
                .Load(new System.Reflection.AssemblyName("Wrhs"));

            builder.RegisterAssemblyTypes(ThisAssembly, wrhsAsm)
                .Where(x => x.IsAssignableTo<ICommandHandler>())
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly, wrhsAsm)
                .Where(x => x.IsAssignableTo<IValidator>())
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(ValidationCmdHndDecorator<>)).AsSelf();

            builder.Register((c) => new HandlerParameters
            {
                DocumentService = c.Resolve<IDocumentService>(),
                OperationService = c.Resolve<IOperationService>(),
                StockService = c.Resolve<IStockService>()
            });

            builder.Register<Func<Type, ICommandHandler>>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();

                return t =>
                {
                    var handlerType = typeof(ICommandHandler<>).MakeGenericType(t);
                    var handler = (ICommandHandler)ctx.Resolve(handlerType);

                    var validatorType = typeof(IValidator<>).MakeGenericType(t);
                    var validator = (IValidator)ctx.Resolve(validatorType);

                    var decorType = typeof(ValidationCmdHndDecorator<>).MakeGenericType(t);
                    var decor = (ICommandHandler)ctx.Resolve(decorType,
                        new TypedParameter(typeof(ICommandHandler), handler),
                        new TypedParameter(typeof(IValidator), validator));

                    return decor;
                };
            });

            builder.RegisterType<CommandBus>()
                .AsImplementedInterfaces();
        }
    }
}
