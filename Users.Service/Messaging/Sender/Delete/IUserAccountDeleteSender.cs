using Users.Service.Models;

namespace Users.Service.Messaging.Sender.Delete
{
    public interface IUserAccountDeleteSender
    {
        void SendDeleteUserMessage(UserMessageModel user);
    }
}