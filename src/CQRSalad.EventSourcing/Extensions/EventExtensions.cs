using CQRSalad.Domain;
using CQRSalad.EventSourcing.ValueInjection;
using CQRSalad.EventSourcing.ValueInjection.Convensions;

namespace CQRSalad.EventSourcing
{
    internal static class EventExtensions
    {
        public static TEvent InjectFromCommand<TEvent, TCommand>(this TEvent evnt, TCommand command)
            where TEvent : IEvent
            where TCommand : ICommand
        {
            ValueInjecter.Inject<CommandToEventConvention>(target: evnt, source: command);
            return evnt;
        }
    }
}
