using Users.Service.Models;

namespace Users.Service.Messaging.Sender.Create
{
    public interface IUserAccountCreateSender
    {
        void SendCreateUserMessage(UserMessageModel user);
    }
}