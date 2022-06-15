using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Referrals.Models;
using HealthCareCenter.Core.Referrals.Services;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.VacationRequests.Services;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Referrals.Controllers
{
    public class ScheduleAppointmentReferralController
    {
        private readonly IVacationRequestService _vacationRequestService;
        private readonly ITermsService _termsService;
        private readonly IReferralsService _referralsService;

        public ScheduleAppointmentReferralController()
        {
        }

        public ScheduleAppointmentReferralController(IVacationRequestService vacationRequestService, ITermsService termsService, IReferralsService referralsService)
        {
            _vacationRequestService = vacationRequestService;
            _termsService = termsService;
            _referralsService = referralsService;
        }

        public List<string> GetAvailableTerms(int doctorID, DateTime when)
        {
            if (_vacationRequestService.OnVacation(doctorID, when))
            {
                throw new Exception("The doctor is on vacation at this time.");
            }
            return _termsService.GetAvailableTerms(doctorID, when);
        }

        public List<HospitalRoomForDisplay> GetRooms(bool checkup)
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
