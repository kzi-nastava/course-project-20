using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    class AppointmentDateCompare : IComparer<Appointment>
    {
        public int Compare(Appointment a1, Appointment a2)
        {
            return a1.ScheduledDate.CompareTo(a2.ScheduledDate);
        }
    }

    class AppointmentDoctorCompare : IComparer<Appointment>
    {
        public int Compare(Appointment a1, Appointment a2)
        {
            string a1DoctorName = "", a2DoctorName = "";
            foreach (Doctor doctor in UserRepository.Doctors)
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

    class AppointmentProfessionalAreaCompare : IComparer<Appointment>
    {
        public int Compare(Appointment a1, Appointment a2)
        {
            string a1ProfessionalArea = "", a2ProfessionalArea = "";
            foreach (Doctor doctor in UserRepository.Doctors)
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
}
