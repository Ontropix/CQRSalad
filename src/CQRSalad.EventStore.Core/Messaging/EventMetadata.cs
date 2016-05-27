﻿using System;

namespace CQRSalad.EventStore.Core
{
    public class EventMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        public string AggregateId { get; set; }

        /// <summary>
        /// Aggregate that produced this event
        /// </summary>
        public string AggregateRoot { get; set; }

        /// <summary>
        /// Time when command was created
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}