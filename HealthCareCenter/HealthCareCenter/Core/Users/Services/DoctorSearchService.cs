using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Users.Services
{
    public class DoctorSearchService : IDoctorSearchService
    {
        private readonly BaseUserRepository _userRepository;

        public DoctorSearchService(BaseUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<Doctor> SearchByFirstName(string firstName)
        {
            List<Doctor> doctorsByKeyword = new List<Doctor>();
            foreach (Doctor doctor in _userRepository.Doctors)
            {
                if (doctor.FirstName.ToLower().Contains(firstName))
                {
                    doctorsByKeyword.Add(doctor);
                }
            }

            return doctorsByKeyword;
        }

        public List<Doctor> SearchByLastName(string lastName)
        {
            List<Doctor> doctorsByKeyword = new List<Doctor>();
            foreach (Doctor doctor in _userRepository.Doctors)
            {
                if (doctor.LastName.ToLower().Contains(lastName))
                {
                    doctorsByKeyword.Add(doctor);
                }
            }

            return doctorsByKeyword;
        }

        public List<Doctor> SearchByProfessionalArea(string professionalArea)
        {
            List<Doctor> doctorsByKeyword = new List<Doctor>();
            foreach (Doctor doctor in _userRepository.Doctors)
            {
                if (doctor.Type.ToString().ToLower().Contains(professionalArea))
                {
                    doctorsByKeyword.Add(doctor);
                }
            }

            return doctorsByKeyword;
        }
    }
}
