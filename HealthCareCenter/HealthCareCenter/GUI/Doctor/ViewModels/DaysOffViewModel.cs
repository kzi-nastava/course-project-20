using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.VacationRequests.Repositories;
using HealthCareCenter.GUI.Doctor.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.GUI.Doctor.ViewModels
{
    public class DaysOffViewModel
    {
        private User _signedUser;
        private DaysOffWindow _window;
        private BaseVacationRequestRepository _vacationRequestRepository;
        private BaseAppointmentRepository _appointmentRepository;
        public DaysOffViewModel(User signedUser, VacationRequestRepository vacationRequestRepository, AppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _vacationRequestRepository = vacationRequestRepository;
            _window = new DaysOffWindow(this); 
            _signedUser = signedUser;
            _window.Show();
        }

        internal void OpenRequestsPreview()
        {
            RequestsPreviewViewModel requestsPreviewViewModel = new RequestsPreviewViewModel(_signedUser, (VacationRequestRepository)_vacationRequestRepository);
        }

        internal void OpenRequestDaysOff()
        {
            RequestDaysOffViewModel requestDaysOffViewModel = new RequestDaysOffViewModel(_signedUser, (AppointmentRepository)_appointmentRepository, (VacationRequestRepository)_vacationRequestRepository);
        }
    }
}
