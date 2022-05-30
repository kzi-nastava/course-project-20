using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public static class UserService
    {
        public static int maxID = -1;

        public static void CalculateMaxID()
        {
            maxID = -1;
            foreach (User user in UserRepository.Users)
            {
                if (user.ID > maxID) {
                    maxID = user.ID;
                }
            }
        }

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

        public static Doctor GetDoctor(int ID)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == ID)
                {
                    return doctor;
                }
            }

            return null;
        }

    }
}
