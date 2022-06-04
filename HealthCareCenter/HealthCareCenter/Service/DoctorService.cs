using System.Collections.Generic;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    public static class DoctorService
    {
        public static List<Doctor> GetDoctorsByType(string chosenType)
        {
            List<Doctor> doctors = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type.Equals(chosenType))
                    doctors.Add(doctor);
            }
            return doctors;
        }
        public static List<Doctor> GetDoctorsBySpecialization(string chosenType)
        {
            List<Doctor>doctors = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type.Equals(chosenType))
                    doctors.Add(doctor);
            }
            return doctors;
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
                    doctors.Sort(new DoctorRatingCompare());
                    break;
            }

            return doctors;
        }
    }
}
