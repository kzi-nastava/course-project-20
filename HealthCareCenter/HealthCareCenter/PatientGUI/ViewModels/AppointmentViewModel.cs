using HealthCareCenter.Model;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class AppointmentViewModel : ViewModelBase
    {
        private readonly Appointment _appointment;

        public string AppointmentID => _appointment.ID.ToString();
        public string AppointmentType => _appointment.Type.ToString();
        public string AppointmentDate => _appointment.ScheduledDate.ToString("g");
        public string CreationDate => _appointment.CreatedDate.ToString("g");
        public string IsEmergency => _appointment.Emergency.ToString();
        public string DoctorID => _appointment.DoctorID.ToString();
        public string RoomID => _appointment.HospitalRoomID.ToString();

        public AppointmentViewModel(Appointment appointment)
        {
            _appointment = appointment;
        }
    }
}
