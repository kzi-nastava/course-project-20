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

        public static void UpdateDoctor(int doctorID, Appointment appointment)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == doctorID)
                {
                    doctor.AppointmentIDs.Add(appointment.ID);
                    break;
                }
            }
        }
    }
}
