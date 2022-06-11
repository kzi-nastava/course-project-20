using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.Secretary.Controllers
{
    public class ScheduleAppointmentReferralController
    {
        private IVacationRequestService _vacationRequestService;
        private ITermsService _termsService;
        private IReferralsService _referralsService;

        public ScheduleAppointmentReferralController()
        {
            AppointmentRepository.Load();
        }

        public ScheduleAppointmentReferralController(IVacationRequestService vacationRequestService, ITermsService termsService, IReferralsService referralsService)
        {
            _vacationRequestService = vacationRequestService;
            _termsService = termsService;
            _referralsService = referralsService;
            AppointmentRepository.Load();
        }

        public List<string> GetAvailableTerms(int doctorID, DateTime when)
        {
            if (_vacationRequestService.OnVacation(doctorID, when))
            {
                throw new Exception("The doctor is on vacation at this time.");
            }
            return _termsService.GetAvailableTerms(doctorID, when);
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

            _referralsService.Schedule(referral, appointment);
        }
    }
}
