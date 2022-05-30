using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class PrioritySchedulingViewModel : ViewModelBase
    {
        public Patient Patient { get; }

        public List<DoctorViewModel> Doctors { get; }

        private DoctorViewModel _chosenDoctor;
        public DoctorViewModel ChosenDoctor
        {
            get => _chosenDoctor;
            set
            {
                _chosenDoctor = value;
                OnPropertyChanged(nameof(ChosenDoctor));
            }
        }

        private List<PriorityNotFoundChoiceViewModel> _priorityNotFoundChoices;
        public List<PriorityNotFoundChoiceViewModel> PriorityNotFoundChoices
        {
            get => _priorityNotFoundChoices;
            set
            {
                _priorityNotFoundChoices = value;
                OnPropertyChanged(nameof(PriorityNotFoundChoices));
            }
        }

        private PriorityNotFoundChoiceViewModel _priorityNotFoundChoice;
        public PriorityNotFoundChoiceViewModel PriorityNotFoundChoice
        {
            get => _priorityNotFoundChoice;
            set
            {
                _priorityNotFoundChoice = value;
                OnPropertyChanged(nameof(PriorityNotFoundChoice));
            }
        }

        private bool _isDoctorPriority;
        public bool IsDoctorPriority
        {
            get => _isDoctorPriority;
            set
            {
                _isDoctorPriority = value;
                OnPropertyChanged(nameof(IsDoctorPriority));
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

        private List<AppointmentTerm> _allPossibleTerms;
        public List<AppointmentTerm> AllPossibleTerms
        {
            get => _allPossibleTerms;
            set
            {
                _allPossibleTerms = value;
                OnPropertyChanged(nameof(AllPossibleTerms));
            }
        }

        private AppointmentTerm _startRange;
        public AppointmentTerm StartRange
        {
            get => _startRange;
            set
            {
                _startRange = value;
                OnPropertyChanged(nameof(StartRange));
            }
        }

        private AppointmentTerm _endRange;
        public AppointmentTerm EndRange
        {
            get => _endRange;
            set
            {
                _endRange = value;
                OnPropertyChanged(nameof(EndRange));
            }
        }

        public ICommand PriorityScheduleAppointment { get; }

        public PrioritySchedulingViewModel(NavigationStore navigationStore, Patient patient)
        {
            Patient = patient;

            Doctors = new List<DoctorViewModel>();
            List<Doctor> allDoctors = UserRepository.Doctors;
            foreach (Doctor doctor in allDoctors)
            {
                Doctors.Add(new DoctorViewModel(doctor));
            }

            PriorityNotFoundChoices = new List<PriorityNotFoundChoiceViewModel>();

            IsDoctorPriority = true;

            ChosenDate = DateTime.Now.Date;

            AllPossibleTerms = AppointmentTermService.GetDailyTermsFromRange(Constants.StartWorkTime, 0, Constants.EndWorkTime, 0);

            StartRange = AllPossibleTerms[0];
            EndRange = AllPossibleTerms[^1];

            PriorityScheduleAppointment = new PriorityScheduleAppointmentCommand(this, navigationStore);
        }
    }
}
