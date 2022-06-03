using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static List<Doctor> GetDoctorsOfType(string type)
        {
            List<Doctor> doctors = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type == type)
                {
                    doctors.Add(doctor);
                }
            }
            return doctors;
        }

        public static List<string> GetTypesOfDoctors()
        {
            List<string> types = new List<string>();
            types.AddRange(UserRepository.Doctors.Where(doctor => !types.Contains(doctor.Type)).Select(doctor => doctor.Type));
            return types;
        }

        public static void RemoveUnavailableDoctors(List<Doctor> availableDoctors, Appointment appointment)
        {
            foreach (Doctor doctor in availableDoctors)
            {
                if (doctor.ID == appointment.DoctorID)
                {
                    availableDoctors.Remove(doctor);
                    return;
                }
            }
        }
    }
}
