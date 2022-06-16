using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Surveys.Repositories;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCareCenter.Core.Users.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorSearchService _doctorSearchService;
        private readonly BaseUserRepository _userRepository;

        public DoctorService(
            IDoctorSearchService doctorSearchService,
            BaseUserRepository userRepository)
        {
            _doctorSearchService = doctorSearchService;
            _userRepository = userRepository;
        }

        public List<Doctor> GetDoctorsOfType(string type)
        {
            List<Doctor> doctors = new List<Doctor>();
            foreach (Doctor doctor in _userRepository.Doctors)
            {
                if (doctor.Type == type)
                {
                    doctors.Add(doctor);
                }
            }
            return doctors;
        }

        public List<string> GetTypesOfDoctors()
        {
            List<string> types = new List<string>();
            types.AddRange(_userRepository.Doctors.Where(doctor => !types.Contains(doctor.Type)).Select(doctor => doctor.Type));
            return types;
        }

        public Doctor Get(int id)
        {
            foreach (Doctor doctor in _userRepository.Doctors)
            {
                if (doctor.ID == id)
                {
                    return doctor;
                }
            }

            return null;
        }

        public void RemoveUnavailableDoctors(List<Doctor> availableDoctors, Appointment appointment)
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

        public List<Doctor> SearchByKeyword(string searchKeyword, string searchCriteria)
        {
            List<Doctor> doctorsByKeyword;
            switch (searchCriteria)
            {
                case "First name":
                    doctorsByKeyword = _doctorSearchService.SearchByFirstName(searchKeyword);
                    break;

                case "Last name":
                    doctorsByKeyword = _doctorSearchService.SearchByLastName(searchKeyword);
                    break;

                case "Professional area":
                    doctorsByKeyword = _doctorSearchService.SearchByProfessionalArea(searchKeyword);
                    break;

                default:
                    doctorsByKeyword = new List<Doctor>();
                    break;
            }

            return doctorsByKeyword;
        }

        public List<Doctor> GetSortedByCriteria(List<Doctor> doctors, string sortCriteria, string searchCriteria)
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
                    doctors.Sort(new DoctorRatingCompare(new DoctorSurveyRatingService(new DoctorSurveyRatingRepository(), new UserRepository())));
                    break;
            }

            return doctors;
        }
    }
}