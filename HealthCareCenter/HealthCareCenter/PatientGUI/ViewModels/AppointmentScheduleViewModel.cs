using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    class AppointmentScheduleViewModel : ViewModelBase
    {
        private string _availableDate;
        private string _availableTerm;

        public string AvailableDate => _availableDate;
        public string AvailableTerm => _availableTerm;

        public AppointmentScheduleViewModel(string availableDate, string availableTerm)
        {
            _availableDate = availableDate;
            _availableTerm = availableTerm;
        }
    }
}
