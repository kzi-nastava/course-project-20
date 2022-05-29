using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HealthCareCenter.PatientGUI.Commands
{
    class SortAppointmentsCommand : CommandBase
    {
        ObservableCollection<AppoinmentViewModel> _appointments;

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public SortAppointmentsCommand(ref ObservableCollection<AppoinmentViewModel> appointments)
        {
            _appointments = appointments;
        }
    }
}
