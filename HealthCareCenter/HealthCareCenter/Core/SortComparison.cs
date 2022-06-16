using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core
{
    internal class AppointmentDateCompare : IComparer<Appointment>
    {
        public int Compare(Appointment a1, Appointment a2)
        {
            return a1.ScheduledDate.CompareTo(a2.ScheduledDate);
        }
    }

    internal class AppointmentDoctorCompare : IComparer<Appointment>
    {
        private readonly BaseUserRepository _userRepository;

        public AppointmentDoctorCompare(BaseUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int Compare(Appointment a1, Appointment a2)
        {
            string a1DoctorName = "", a2DoctorName = "";
            foreach (Doctor doctor in _userRepository.Doctors)
            {
                if (doctor.ID == a1.DoctorID)
                {
                    a1DoctorName = doctor.FirstName + " " + doctor.LastName;
                }

                if (doctor.ID == a2.DoctorID)
                {
                    a2DoctorName = doctor.FirstName + " " + doctor.LastName;
                }

                if (a1DoctorName != "" && a2DoctorName != "")
                {
                    break;
                }
            }

            return a1DoctorName.CompareTo(a2DoctorName);
        }
    }

    internal class AppointmentProfessionalAreaCompare : IComparer<Appointment>
    {
        private readonly BaseUserRepository _userRepository;

        public AppointmentProfessionalAreaCompare(BaseUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public int Compare(Appointment a1, Appointment a2)
        {
            string a1ProfessionalArea = "", a2ProfessionalArea = "";
            foreach (Doctor doctor in _userRepository.Doctors)
            {
                if (doctor.ID == a1.DoctorID)
                {
                    a1ProfessionalArea = doctor.Type;
                }

                if (doctor.ID == a2.DoctorID)
                {
                    a2ProfessionalArea = doctor.Type;
                }

                if (a1ProfessionalArea != "" && a2ProfessionalArea != "")
                {
                    break;
                }
            }

            return a1ProfessionalArea.CompareTo(a2ProfessionalArea);
        }
    }

    internal class DoctorFirstNameCompare : IComparer<Doctor>
    {
        public int Compare(Doctor d1, Doctor d2)
        {
            return d1.FirstName.CompareTo(d2.FirstName);
        }
    }

    internal class DoctorLastNameCompare : IComparer<Doctor>
    {
        public int Compare(Doctor d1, Doctor d2)
        {
            return d1.LastName.CompareTo(d2.LastName);
        }
    }

    internal class DoctorProfessionalAreaCompare : IComparer<Doctor>
    {
        public int Compare(Doctor d1, Doctor d2)
        {
            return d1.Type.CompareTo(d2.Type);
        }
    }

    internal class DoctorRatingCompare : IComparer<Doctor>
    {
        private readonly IDoctorSurveyRatingService _doctorSurveyRatingService;

        public DoctorRatingCompare(IDoctorSurveyRatingService doctorSurveyRatingService)
        {
            _doctorSurveyRatingService = doctorSurveyRatingService;
        }

        public int Compare(Doctor d1, Doctor d2)
        {
            return _doctorSurveyRatingService.GetAverageRating(d1.ID).CompareTo(_doctorSurveyRatingService.GetAverageRating(d2.ID));
        }
    }
}