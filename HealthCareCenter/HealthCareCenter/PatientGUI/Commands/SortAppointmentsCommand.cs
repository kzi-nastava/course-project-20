using HealthCareCenter.PatientGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HealthCareCenter.PatientGUI.Commands
{
    class SortAppointmentsCommand : CommandBase
    {
        ObservableCollection<AppointmentViewModel> _appointments;

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public SortAppointmentsCommand(ref ObservableCollection<AppointmentViewModel> appointments)
        {
            _appointments = appointments;
        }
    }
}
