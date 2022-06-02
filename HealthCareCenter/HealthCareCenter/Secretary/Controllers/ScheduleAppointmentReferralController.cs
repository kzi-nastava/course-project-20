using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.Controllers
{
    public class ScheduleAppointmentReferralController
    {
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
            return HospitalRoomService.GetRoomsForDisplay(checkup);
        }

        public void Schedule(Referral referral, Appointment appointment)
        {
            try
            {
                CheckIfTimePassed(appointment.ScheduledDate);
                CheckIfRoomOccupied(appointment.ScheduledDate, appointment.HospitalRoomID);
            }
            catch (Exception)
            {
                AppointmentRepository.LargestID--;
                throw;
            }

            ReferralsService.ScheduleAppointment(referral, appointment);
        }

        private void CheckIfRoomOccupied(DateTime time, int roomID)
        {
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.HospitalRoomID != roomID)
                {
                    continue;
                }
                if (appointment.ScheduledDate.CompareTo(time) == 0)
                {
                    throw new Exception("You must select a different room that is not occupied at the term date and time.");
                }
            }
        }

        private void CheckIfTimePassed(DateTime scheduledDate)
        {
            if (scheduledDate <= DateTime.Now)
            {
                throw new Exception("Your term date has to be in the future.");
            }
        }
    }
}
