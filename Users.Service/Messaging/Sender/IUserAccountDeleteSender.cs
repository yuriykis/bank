using Users.Service.Models;

namespace Users.Service.Messaging.Sender
{
    public interface IUserAccountDeleteSender
    {
        void SendDeleteUserMessage(UserMessageModel user);
    }
}