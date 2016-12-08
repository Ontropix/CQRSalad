using CQRSalad.Dispatching;
using CQRSalad.Infrastructure;

namespace Samples.Domain.Workflow.WorkflowServices
{
    [DispatcherHandler]
    public class UserWorkflowService : WorkflowService
    {
        public UserWorkflowService(IDomainBus domainBus) : base(domainBus)
        {
        }

        //[DispatchingPriority(DispatchingPriority.Low)]
        //public async Task On(UserFollowed evnt)
        //{
        //    await ProduceCommandAsync(
        //        command: new AddFollowerCommand
        //        {
        //            UserId = evnt.FollowingUserId,
        //            FollowerUserId = evnt.UserId
        //        },
        //        sender: evnt.UserId);
        //}
    }
}
