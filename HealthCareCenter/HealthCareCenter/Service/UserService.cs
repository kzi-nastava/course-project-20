using HealthCareCenter.Model;

namespace HealthCareCenter.Service
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
