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
        private readonly IReferralService _referralsService;
        private readonly IHospitalRoomService _hospitalRoomService;

        public ScheduleAppointmentReferralController(
            IVacationRequestService vacationRequestService,
            ITermsService termsService, 
            IReferralService referralsService,
            IHospitalRoomService hospitalRoomService)
        {
            _vacationRequestService = vacationRequestService;
            _termsService = termsService;
            _referralsService = referralsService;
            _hospitalRoomService = hospitalRoomService;
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
            return _hospitalRoomService.GetRooms(checkup);
        }

        public void Schedule(Referral referral, Appointment appointment)
        {
            if (appointment.ScheduledDate <= DateTime.Now)
            {
                AppointmentRepository.LargestID--;
                throw new Exception("Your term date has to be in the future.");
            }
            if (_hospitalRoomService.IsOccupied(appointment.HospitalRoomID, appointment.ScheduledDate))
            {
                AppointmentRepository.LargestID--;
                throw new Exception("You must select a different room that is not occupied at the term date and time.");
            }

            _referralsService.Schedule(referral, appointment);
        }
    }
}
