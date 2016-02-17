using System;

namespace CQRSalad.EventSourcing
{
    public class CommandProducedNoEventsException : ApplicationException
    {
        public object Command { get; set; }

        public CommandProducedNoEventsException(object command) 
            : base ($"Command '{command.GetType().AssemblyQualifiedName}' hasn't produced any events")
        {
            Command = command;
        }
    }
}