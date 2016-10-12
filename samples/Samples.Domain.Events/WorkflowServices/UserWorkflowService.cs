using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.Dispatching.Priority;
using CQRSalad.Domain;
using CQRSalad.EventSourcing;
using Samples.Domain.Events.User;
using Samples.Domain.Interface.User;

namespace Samples.Domain.Events.WorkflowServices
{
    [DispatcherHandler]
    public class UserWorkflowService : WorkflowService
    {
        public UserWorkflowService(IDomainBus domainBus) : base(domainBus)
        {
        }

        [DispatchingPriority(DispatchingPriority.Low)]
        public async Task On(UserFollowedEvent evnt)
        {
            await ProduceCommandAsync(
                command: new AddFollowerCommand
                {
                    UserId = evnt.FollowingUserId,
                    FollowerUserId = evnt.UserId
                },
                sender: evnt.UserId);
        }
    }
}
