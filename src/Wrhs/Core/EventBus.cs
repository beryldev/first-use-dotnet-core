using System;
using System.Collections.Generic;
using System.Linq;

namespace Wrhs.Core
{
    public class EventBus : IEventBus
    {
        private readonly Func<Type, IEnumerable<IEventHandler>> handlersFactory;

        public EventBus(Func<Type, IEnumerable<IEventHandler>> handlersFactory)
        {
            this.handlersFactory = handlersFactory;
        }
        
        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = handlersFactory(typeof(TEvent))
                .Cast<IEventHandler<TEvent>>();

            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }
        }
    }
}