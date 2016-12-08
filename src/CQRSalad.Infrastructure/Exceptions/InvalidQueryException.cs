using System;
using CQRSalad.EventSourcing;

namespace CQRSalad.Infrastructure.Exceptions
{
    public class InvalidQueryException<TResult> : ApplicationException
    {
        public IQueryFor<TResult> Query { get; set; }

        public InvalidQueryException(string message, IQueryFor<TResult> query)
            : base(message)
        {
            Query = query;
        }
    }
}