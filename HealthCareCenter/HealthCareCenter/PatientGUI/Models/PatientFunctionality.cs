using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.ViewModels;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Models
{
    class PatientFunctionality
    {
        private static PatientFunctionality instance;

        private PatientFunctionality() { }

        public static PatientFunctionality GetInstance()
        {
            return instance is null ? new PatientFunctionality() : instance;
        }

        public List<AppointmentTerm> GetAllPossibleTermsForCreateAppointment(int chosenDoctorID, DateTime chosenScheduleDate)
        {
            List<AppointmentTerm> allPossibleTerms = AppointmentTermService.GetDailyTermsFromRange(Constants.StartWorkTime, 0, Constants.EndWorkTime, 0);
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.DoctorID == chosenDoctorID &&
                    appointment.ScheduledDate.Date.CompareTo(chosenScheduleDate.Date) == 0)
                {
                    AppointmentTerm unavailableSchedule = new AppointmentTerm(appointment.ScheduledDate.Hour, appointment.ScheduledDate.Minute);
                    allPossibleTerms.Remove(unavailableSchedule);
                }
            }

            return allPossibleTerms;
        }

        public void ScheduleAppointment(DateTime scheduleDate, int doctorID, int healthRecordID, int hospitalRoomID)
        {
            if (CheckCreationTroll())
            {
                return;
            }

            Appointment newAppointment = new Appointment
            {
                ID = ++AppointmentRepository.LargestID,
                Type = Enums.AppointmentType.Checkup,
                CreatedDate = DateTime.Now,
                ScheduledDate = scheduleDate,
                Emergency = false,
                DoctorID = doctorID,
                HealthRecordID = healthRecordID,
                HospitalRoomID = hospitalRoomID,
                PatientAnamnesis = null
            };
            HospitalRoomService.AddAppointmentToRoom(hospitalRoomID, newAppointment.ID);
            AppointmentRepository.Appointments.Add(newAppointment);

            AppointmentRepository.Save();
        }

        public bool CheckCreationTroll()
        {
            return false;
        }

        public List<Doctor> SearchDoctorByKeyword(string searchKeyword, string searchCriteria)
        {
            List<Doctor> doctorsByKeyword;
            switch (searchCriteria)
            {
                case "First name":
                    doctorsByKeyword = SearchDoctorByFirstName(searchKeyword);
                    break;
                case "Last name":
                    doctorsByKeyword = SearchDoctorByLastName(searchKeyword);
                    break;
                case "Professional area":
                    doctorsByKeyword = SearchDoctorByProfessionalArea(searchKeyword);
                    break;
                default:
                    doctorsByKeyword = new List<Doctor>();
                    break;
            }

            return doctorsByKeyword;

        }

        private List<Doctor> SearchDoctorByFirstName(string firstName)
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

        private List<Doctor> SearchDoctorByLastName(string lastName)
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

        private List<Doctor> SearchDoctorByProfessionalArea(string professionalArea)
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

        public List<Doctor> GetSortedDoctorsByCriteria(List<Doctor> doctors, string sortCriteria, string searchCriteria)
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
