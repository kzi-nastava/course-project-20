using HealthCareCenter.Core.Appointments.Models;
using System;

namespace HealthCareCenter.GUI.Patient.SharedViewModels
{
    internal class PriorityNotFoundChoiceViewModel : ViewModelBase
    {
        private readonly Appointment _appointment;

        public int DoctorID => _appointment.DoctorID;
        public DateTime AppointmentDate => _appointment.ScheduledDate;

        public PriorityNotFoundChoiceViewModel(Appointment appointment)
        {
            _appointment = appointment;
        }
    }
}
