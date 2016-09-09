using System;

namespace CQRSalad.Domain
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