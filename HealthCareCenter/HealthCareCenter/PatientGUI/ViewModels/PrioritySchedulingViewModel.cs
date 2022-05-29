using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    class PrioritySchedulingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<DoctorViewModel> _doctors;
        public IEnumerable<DoctorViewModel> Doctors => _doctors;

        public ICommand PriorityScheduleAppointment { get; }

        public PrioritySchedulingViewModel(NavigationStore navigationStore)
        {
            _doctors = new ObservableCollection<DoctorViewModel>();
            List<Doctor> allDoctors = UserRepository.Doctors;
            foreach (Doctor doctor in allDoctors)
            {
                _doctors.Add(new DoctorViewModel(doctor));
            }

            PriorityScheduleAppointment = new PriorityScheduleAppointmentCommand(navigationStore);
        }
    }
}
