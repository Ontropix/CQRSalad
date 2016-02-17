using System;

namespace CQRSalad.Dispatching.Async
{
    public class MessageMetadata
    {
        /// <summary>
        /// Id of user or process who sent this message
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Time when command was created
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}