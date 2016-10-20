using CQRSalad.Domain;

namespace CQRSalad.EventSourcing
{
    public static class EventActivator
    {
        public static TEvent CreateInstance<TEvent, TCommand>(TCommand injector)
            where TEvent : IEvent, new() 
            where TCommand : ICommand
        {
            TEvent evnt = new TEvent();
            evnt.InjectFromCommand(injector);
            return evnt;
        }
    }
}