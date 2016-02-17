namespace CQRSalad.Dispatching.Async
{
    /// <summary>
    /// Domain Message
    /// </summary>
    public sealed class DomainMessage<TBody>
    {
        /// <summary>
        /// Body of a message
        /// </summary>
        public TBody Body { get; set; }

        /// <summary>
        /// Message metadata
        /// </summary>
        public MessageMetadata Meta { get; set; }
    }
}