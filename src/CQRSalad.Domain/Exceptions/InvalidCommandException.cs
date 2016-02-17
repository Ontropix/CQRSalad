using System;

namespace CQRSalad.Domain
{
    public class InvalidCommandException<TCommand> : ApplicationException
    {
        public TCommand Command { get; set; }

        public InvalidCommandException(string message, TCommand command)
            : base(message)
        {
            Command = command;
        }
    }
}