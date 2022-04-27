using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public static class UserService
    {
        public static int maxUserID = -1;

        public static void CalculateMaxUserID()
        {
            maxUserID = -1;
            foreach (User user in UserRepository.Users)
            {
                if (user.ID > maxUserID) {
                    maxUserID = user.ID;
                }
            }
        }
    }
}
