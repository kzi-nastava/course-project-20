using HealthCareCenter.Core;
using HealthCareCenter.Core.Appointments.Models;
using System;

namespace HealthCareCenter.GUI.Patient.SharedViewModels
{
    internal class AppointmentViewModel : ViewModelBase
    {
        private readonly Appointment _appointment;

        public int AppointmentID => _appointment.ID;
        public AppointmentType AppointmentType => _appointment.Type;
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
