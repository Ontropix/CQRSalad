﻿using System;

namespace CQRSalad.Domain
{
    public interface ICommand
    {
    }

    public interface IQuery<TResult>
    {
    }
    
    public class DomainMessage<TBody>
    {
        public string Id { get; set; }
        public TBody Body { get; set; }
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; }
    }

    //public class DomainQuery<TResult>
    //{
    //    public IQuery<TResult> Body { get; set; } 
    //    public MessageMetadata Meta { get; set; }
    //}
}
