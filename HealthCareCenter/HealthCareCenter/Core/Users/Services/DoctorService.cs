using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCareCenter.Core.Users.Services
{
    public static class DoctorService
    {
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

        public static Doctor Get(int id)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == id)
                {
                    return doctor;
                }
            }

            return null;
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

        public static List<Doctor> SearchByKeyword(string searchKeyword, string searchCriteria)
        {
            List<Doctor> doctorsByKeyword;
            switch (searchCriteria)
            {
                case "First name":
                    doctorsByKeyword = SearchByFirstName(searchKeyword);
                    break;

                case "Last name":
                    doctorsByKeyword = SearchByLastName(searchKeyword);
                    break;

                case "Professional area":
                    doctorsByKeyword = SearchByProfessionalArea(searchKeyword);
                    break;

                default:
                    doctorsByKeyword = new List<Doctor>();
                    break;
            }

            return doctorsByKeyword;
        }

        private static List<Doctor> SearchByFirstName(string firstName)
        {
            List<Doctor> doctorsByKeyword = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.FirstName.ToLower().Contains(firstName))
                {
                    doctorsByKeyword.Add(doctor);
                }
            }

            return doctorsByKeyword;
        }

        private static List<Doctor> SearchByLastName(string lastName)
        {
            List<Doctor> doctorsByKeyword = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.LastName.ToLower().Contains(lastName))
                {
                    doctorsByKeyword.Add(doctor);
                }
            }

            return doctorsByKeyword;
        }

        private static List<Doctor> SearchByProfessionalArea(string professionalArea)
        {
            List<Doctor> doctorsByKeyword = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type.ToString().ToLower().Contains(professionalArea))
                {
                    doctorsByKeyword.Add(doctor);
                }
            }

            return doctorsByKeyword;
        }

        public static List<Doctor> GetSortedByCriteria(List<Doctor> doctors, string sortCriteria, string searchCriteria)
        {
            switch (sortCriteria)
            {
                case "Search criteria":
                    switch (searchCriteria)
                    {
                        case "First name":
                            doctors.Sort(new DoctorFirstNameCompare());
                            break;

                        case "Last name":
                            doctors.Sort(new DoctorLastNameCompare());
                            break;

                        case "Professional area":
                            doctors.Sort(new DoctorProfessionalAreaCompare());
                            break;
                    }
                    break;

                case "Rating":
                    doctors.Sort(new DoctorRatingCompare(new DoctorSurveyRatingService()));
                    break;
            }

            return doctors;
        }
    }
}