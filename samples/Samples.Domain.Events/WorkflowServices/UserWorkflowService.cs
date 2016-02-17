using System.Threading.Tasks;
using Platform.Domain;
using Platform.EventSourcing;
using Samples.Domain.Events.User.Events;
using Samples.Domain.User.Commands;

namespace Samples.Domain.Events.WorkflowServices
{
    public class UserWorkflowService : WorkflowService
    {
        public UserWorkflowService(IDomainBus domainBus) : base(domainBus)
        {
        }

        public async Task On(UserFollowedEvent evnt)
        {
            await ProduceCommandAsync(new AddFollowerCommand
            {
                UserId = evnt.AggregateId
            }, evnt.AggregateId); //todo
        }
    }
}
