using System;

namespace CQRSalad.Dispatching
{
    internal class Subscription
    {
        public Type MessageType { get; set; }
        public Type HandlerType { get; set; }
        public MessageInvoker Invoker { get; set; }
        public Priority Priority { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Subscription;
            if (other == null)
            {
                return false;
            }

            return MessageType == other.MessageType && HandlerType == other.HandlerType;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 69;
                hash = (hash * 7799) ^ (HandlerType?.GetHashCode() ?? 0);
                hash = (hash * 7799) ^ (MessageType?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}