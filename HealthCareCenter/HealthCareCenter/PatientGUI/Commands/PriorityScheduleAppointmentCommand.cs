using HealthCareCenter.PatientGUI.Stores;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.PatientGUI.Commands
{
    class PriorityScheduleAppointmentCommand : CommandBase
    {
        private NavigationStore _navigationStore;

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public PriorityScheduleAppointmentCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}
