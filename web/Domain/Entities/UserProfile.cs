using System.ComponentModel.DataAnnotations;

namespace web.Domain.Entities
{
    public class UserProfile
    {
        public UserProfile(UserData userData)
        {
            UserData = userData;
        }

        public UserProfile()
        {
            UserData = new UserData();
            
        }

        public UserData UserData { get; set; }
    }
}