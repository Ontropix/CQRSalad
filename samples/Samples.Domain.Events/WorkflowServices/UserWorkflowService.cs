using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.Dispatching.Priority;
using CQRSalad.Domain;
using CQRSalad.EventSourcing;

namespace Samples.Domain.Model.WorkflowServices
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
