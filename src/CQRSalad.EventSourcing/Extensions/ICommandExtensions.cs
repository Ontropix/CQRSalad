using System;
using CQRSalad.Domain;

namespace CQRSalad.EventSourcing
{
    public static class ICommandExtensions
    {
        public static TEvent MapToEvent<TEvent>(this ICommand command) where TEvent : class, IEvent, new()
        {
            TEvent evnt = new TEvent();
            evnt.InjectFromCommand(command);
            return evnt;
        }

        internal static string GetAggregateId(this ICommand command)
        {
            var handler = CommandsPropertyCache.GetPropertyHandler(command.GetType());
            return handler(command);
        }
    }
}