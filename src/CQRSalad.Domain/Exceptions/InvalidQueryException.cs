using System;

namespace CQRSalad.Domain
{
    public class InvalidQueryException<TResult> : ApplicationException
    {
        public IQuery<TResult> Query { get; set; }

        public InvalidQueryException(string message, IQuery<TResult> query)
            : base(message)
        {
            Query = query;
        }
    }
}