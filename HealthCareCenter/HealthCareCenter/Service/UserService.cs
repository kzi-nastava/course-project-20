using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public static class UserService
    {
        public static string GetUserFullName(int ID)
        {
            foreach (User user in UserRepository.Users)
            {
                if (user.ID == ID)
                {
                    return user.FirstName + " " + user.LastName;
                }
            }

            return "";
        }

    }
}
