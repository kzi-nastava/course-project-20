using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Controller
{
    public static class VacationRequestController
    {
        public static bool OnVacation(int doctorID, DateTime when)
        {
            foreach (VacationRequest request in VacationRequestRepository.Requests)
            {
                if (request.DoctorID == doctorID && request.State == RequestState.Approved 
                    && request.StartDate.CompareTo(when) <= 0 && request.EndDate.CompareTo(when) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
