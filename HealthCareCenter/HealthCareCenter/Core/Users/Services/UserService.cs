using HealthCareCenter.Core.Users.Models;

namespace HealthCareCenter.Core.Users.Services
{
    public class UserService : IUserService
    {
        private readonly BaseUserRepository _userRepository;

        public UserService(BaseUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string GetFullName(int id)
        {
            foreach (User user in _userRepository.Users)
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
