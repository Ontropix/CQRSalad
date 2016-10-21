using CQRSalad.Domain;

namespace CQRSalad.EventSourcing
{
    public static class CommandExtensions
    {
        public static TEvent MapToEvent<TEvent>(this ICommand command) where TEvent : class, IEvent, new()
        {
            TEvent evnt = new TEvent();
            evnt.InjectFromCommand(command);
            return evnt;
        }
    }
}