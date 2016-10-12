using System;

namespace CQRSalad.Domain
{
    public class DomainMessage<TBody>
    {
        public string Id { get; set; }
        public TBody Body { get; set; }
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; }
    }
}