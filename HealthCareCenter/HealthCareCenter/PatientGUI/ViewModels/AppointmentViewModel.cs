using HealthCareCenter.Model;
using System;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class AppointmentViewModel : ViewModelBase
    {
        private readonly Appointment _appointment;

        public int AppointmentID => _appointment.ID;
        public Enums.AppointmentType AppointmentType => _appointment.Type;
        public DateTime AppointmentDate => _appointment.ScheduledDate;
        public DateTime CreationDate => _appointment.CreatedDate;
        public bool IsEmergency => _appointment.Emergency;
        public int DoctorID => _appointment.DoctorID;
        public int RoomID => _appointment.HospitalRoomID;

        public AppointmentViewModel(Appointment appointment)
        {
            _appointment = appointment;
        }
    }
}
