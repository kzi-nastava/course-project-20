using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.VacationRequests.Models;
using HealthCareCenter.Core.VacationRequests.Repositories;
using HealthCareCenter.GUI.Doctor.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.GUI.Doctor.ViewModels
{
    public class RequestsPreviewViewModel
    {
        private User _signedUser;
        RequestsPreviewWindow _window;
        VacationRequestRepository _vacationRepository;
        public RequestsPreviewViewModel(User signedUser, VacationRequestRepository vacationRepository)
        {
            _vacationRepository = vacationRepository;
            _signedUser = signedUser;
            _window = new RequestsPreviewWindow(this, _vacationRepository);
            FillTheTable();
            _window.Show();
        }

        private void FillTheTable()
        {
            foreach (VacationRequest request in _vacationRepository.Requests)
            {
                _window.AddRequestToTable(request);
            }
            _window.daysOffRequestsDataGrid.ItemsSource = _window.vacationRequestsDataTable.DefaultView;
        }
    }
}
