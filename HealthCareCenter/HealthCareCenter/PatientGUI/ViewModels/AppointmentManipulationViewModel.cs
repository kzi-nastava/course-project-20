using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Stores;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal abstract class AppointmentManipulationViewModel : ViewModelBase
    {
        public Patient Patient { get; }

        private List<DoctorViewModel> _doctors;
        public List<DoctorViewModel> Doctors
        {
            get => _doctors;
            set
            {
                _doctors = value;
                OnPropertyChanged(nameof(Doctors));
            }
        }

        private DoctorViewModel _chosenDoctor;
        public DoctorViewModel ChosenDoctor
        {
            get => _chosenDoctor;
            set
            {
                _chosenDoctor = value;
                AvailableSchedules = new List<AppointmentScheduleViewModel>();
                OnPropertyChanged(nameof(ChosenDoctor));
            }
        }

        private List<AppointmentScheduleViewModel> _availableSchedules;
        public List<AppointmentScheduleViewModel> AvailableSchedules
        {
            get => _availableSchedules;
            set
            {
                _availableSchedules = value;
                OnPropertyChanged(nameof(AvailableSchedules));
            }
        }

        private DateTime _chosenDate;
        public DateTime ChosenDate
        {
            get => _chosenDate;
            set
            {
                _chosenDate = value;
                OnPropertyChanged(nameof(ChosenDate));
            }
        }

        private AppointmentScheduleViewModel _chosenSchedule;
        public AppointmentScheduleViewModel ChosenSchedule
        {
            get => _chosenSchedule;
            set
            {
                _chosenSchedule = value;
                OnPropertyChanged(nameof(ChosenSchedule));
            }
        }

        public ICommand FindTerms { get; }

        public AppointmentManipulationViewModel(NavigationStore navigationStore, Patient patient)
        {
            Patient = patient;

            _doctors = new List<DoctorViewModel>();
            List<Doctor> allDoctors = UserRepository.Doctors;
            foreach (Doctor doctor in allDoctors)
            {
                _doctors.Add(new DoctorViewModel(doctor));
            }

            ChosenDate = DateTime.Now.Date;
            _availableSchedules = new List<AppointmentScheduleViewModel>();

            FindTerms = new FindSchedulesCommand(this);
        }

    }
}
