using CQRSalad.Infrastructure.ValueInjection;

// ReSharper disable once CheckNamespace
namespace CQRSalad.EventSourcing
{
    public static class ICommandExtensions
    {
        public static TEvent MapToEvent<TEvent>(this ICommand command) where TEvent : class, IEvent, new()
        {
            TEvent evnt = new TEvent();
            ValueInjecter.Inject(command, evnt);
            return evnt;
        }
    }
}