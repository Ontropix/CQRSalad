using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace Samples.API.ServiceStack.ServiceModel
{
    [Route("/hello/{Name}")]
    public class CreateTodoListRequest : IReturn<CreateTodoListResponse>
    {
        public string Title { get; set; }
    }

    public class CreateTodoListResponse
    {
        public string Id { get; set; }
    }
}