using HealthCareCenter.Model;
using HealthCareCenter.SecretaryGUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public class AppointmentService
    {
        public static Appointment Find(AppointmentDisplay appointmentDisplay)
        {
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID == appointmentDisplay.ID)
                {
                    return appointment;
                }
            }
            return null;
        }
    }
}
