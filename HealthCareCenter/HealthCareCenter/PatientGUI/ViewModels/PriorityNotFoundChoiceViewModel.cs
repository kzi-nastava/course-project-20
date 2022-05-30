using HealthCareCenter.Model;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class PriorityNotFoundChoiceViewModel : ViewModelBase
    {
        private readonly Appointment _appointment;

        public string DoctorID => _appointment.DoctorID.ToString();
        public string AppointmentDate => _appointment.ScheduledDate.ToString("g");

        public PriorityNotFoundChoiceViewModel(Appointment appointment)
        {
            _appointment = appointment;
        }
    }
}
