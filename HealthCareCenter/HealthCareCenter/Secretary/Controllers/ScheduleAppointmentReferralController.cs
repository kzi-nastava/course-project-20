using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.Secretary.Controllers
{
    public class ScheduleAppointmentReferralController
    {
        public ScheduleAppointmentReferralController()
        {
            AppointmentRepository.Load();
        }

        public List<string> GetAvailableTerms(int doctorID, DateTime when)
        {
            if (VacationRequestService.OnVacation(doctorID, when))
            {
                throw new Exception("The doctor is on vacation at this time.");
            }
            return AppointmentService.GetAvailableTerms(doctorID, when);
        }

        public List<HospitalRoomDisplay> GetRooms(bool checkup)
        {
            return HospitalRoomService.GetRooms(checkup);
        }

        public void Schedule(Referral referral, Appointment appointment)
        {
            if (appointment.ScheduledDate <= DateTime.Now)
            {
                AppointmentRepository.LargestID--;
                throw new Exception("Your term date has to be in the future.");
            }
            if (HospitalRoomService.IsOccupied(appointment.HospitalRoomID, appointment.ScheduledDate))
            {
                AppointmentRepository.LargestID--;
                throw new Exception("You must select a different room that is not occupied at the term date and time.");
            }

            ReferralsService.Schedule(referral, appointment);
        }
    }
}
