using HealthCareCenter.Model;
using System;

namespace HealthCareCenter.PatientGUI.ViewModels
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
