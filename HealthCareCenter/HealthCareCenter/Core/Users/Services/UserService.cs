using HealthCareCenter.Core.Users.Models;

namespace HealthCareCenter.Core.Users.Services
{
    public static class UserService
    {
        public static string GetFullName(int id)
        {
            foreach (User user in UserRepository.Users)
            {
                if (user.ID == id)
                {
                    return user.FirstName + " " + user.LastName;
                }
            }

            return "";
        }
    }
}
