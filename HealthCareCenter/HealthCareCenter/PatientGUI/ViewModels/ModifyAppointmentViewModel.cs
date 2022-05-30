using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Stores;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class ModifyAppointmentViewModel : AppointmentManipulationViewModel
    {
        public AppointmentViewModel ChosenAppointment { get; }

        private readonly List<AppointmentViewModel> _oldAppointment;
        public List<AppointmentViewModel> OldAppointment => _oldAppointment;

        public ICommand ModifyAppointment { get; }

        public ModifyAppointmentViewModel(NavigationStore navigationStore, Patient patient, AppointmentViewModel chosenAppointment) :
            base(navigationStore, patient)
        {
            ChosenAppointment = chosenAppointment;
            List<AppointmentViewModel> oldAppointment = new List<AppointmentViewModel>()
            {
                ChosenAppointment
            };
            _oldAppointment = oldAppointment;
            OnPropertyChanged(nameof(OldAppointment));

            ModifyAppointment = new ModifyAppointmentCommand(navigationStore, this);
        }
    }
}
