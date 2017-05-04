using System.Threading.Tasks;
using CQRSalad.Dispatching;
using Samples.Domain.User;

namespace Samples.ViewModel.SingleUseHandlers
{
    //[DispatcherHandler]
    public sealed class UserEmailSingleUseHandler
    {
        private readonly IEmailSender _emailSender;
        private const string EMAIL_GREETING = "Thanks for registering!";

        public UserEmailSingleUseHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task Apply(UserCreated evnt)
        {
            await _emailSender.SendEmail(evnt.Email, EMAIL_GREETING);
        }
    }

    public interface IEmailSender
    {
        Task SendEmail(string email, string text);
    }

    public class MockEmailSender : IEmailSender
    {
        public async Task SendEmail(string email, string text)
        {
            await Task.Delay(4000);
        }
    }
}
