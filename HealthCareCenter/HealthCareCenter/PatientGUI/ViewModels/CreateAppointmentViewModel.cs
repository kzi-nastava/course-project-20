using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Stores;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class CreateAppointmentViewModel : AppointmentManipulationViewModel
    {
        public ICommand ScheduleAppointment { get; }

        public CreateAppointmentViewModel(NavigationStore navigationStore, Patient patient, DoctorViewModel chosenDoctor) :
            base(navigationStore, patient)
        {
            ChosenDoctor = chosenDoctor;
            if (ChosenDoctor != null)
            {
                Doctors = new List<DoctorViewModel>();
            }

            ScheduleAppointment = new ScheduleAppointmentCommand(navigationStore, this);
        }
    }
}
