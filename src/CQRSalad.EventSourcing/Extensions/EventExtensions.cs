using System.Collections.Generic;
using System.Linq;
using CQRSalad.EventSourcing.ValueInjection;
using CQRSalad.EventSourcing.ValueInjection.Convensions;
using CQRSalad.EventStore.Core;

namespace CQRSalad.EventSourcing
{
    public static class EventExtensions
    {
        public static IEvent InjectFromCommand<TCommand>(this IEvent evnt, TCommand command)
        {
            ValueInjecter.Inject<CommandToEventConvention>(target: evnt, source: command);
            return evnt;
        }

        //public static void Validate(this IEvent evnt)
        //{
        //    Argument.StringNotEmpty(evnt.AggregateId, "AggregateId");
        //    Argument.StringNotEmpty(evnt.Meta.EventId, "EventId");
        //    Argument.StringNotEmpty(evnt.Meta.CommandId, "CommandId");
        //    Argument.StringNotEmpty(evnt.Meta.SenderId, "SenderId");
        //    Argument.StringNotEmpty(evnt.Meta.AggregateRoot, "AggregateRoot");
        //}
    }

    public static class EventActivator
    {
        public static TEvent CreateInstance<TEvent, TCommand>(TCommand injector)
            where TEvent : IEvent, new()
        {
            TEvent evnt = new TEvent();
            evnt.InjectFromCommand(injector);
            return evnt;
        }
    }

    public static class EventStreamExtensions
    {
        public static List<IEvent> GetBodies(this List<DomainEvent> stream)
        {
            return stream.Select(x => x.Body).ToList();
        }
    }
}
