using HealthCareCenter.Core;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Appointments.Services.Priority;
using HealthCareCenter.Core.Surveys.Services;
using HealthCareCenter.Core.Users;
using HealthCareCenter.GUI.Patient.AppointmentCRUD.Commands;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.GUI.Patient.AppointmentCRUD.ViewModels
{
    internal class PrioritySchedulingViewModel : ViewModelBase
    {
        private readonly IAppointmentTermService _termService;

        public Core.Patients.Patient Patient { get; }

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

        public PrioritySchedulingViewModel(
            IAppointmentTermService termService,
            Core.Patients.Patient patient, 
            NavigationStore navigationStore)
        {
            _termService = termService;

            Patient = patient;

            Doctors = new List<DoctorViewModel>();
            List<Core.Users.Models.Doctor> allDoctors = UserRepository.Doctors;
            foreach (Core.Users.Models.Doctor doctor in allDoctors)
            {
                Doctors.Add(new DoctorViewModel(doctor, new DoctorSurveyRatingService()));
            }

            PriorityNotFoundChoices = new List<PriorityNotFoundChoiceViewModel>();

            IsDoctorPriority = true;

            ChosenDate = DateTime.Now.Date;

            AllPossibleTerms = _termService.GetDailyTermsFromRange(Constants.StartWorkTime, 0, Constants.EndWorkTime, 0);

            StartRange = AllPossibleTerms[0];
            EndRange = AllPossibleTerms[^1];

            PriorityScheduleAppointment = new PriorityScheduleAppointmentCommand(
                this,
                navigationStore,
                new AppointmentPrioritySearchService(
                    new PriorityAppointmentFinder(
                        new AppointmentTermService(),
                        new AppointmentRepository()),
                    new SimilarToPriorityAppointmentsFinder(
                        new AppointmentRepository()),
                    new AppointmentTermService()),
                new AppointmentService(
                    new AppointmentRepository(),
                    new AppointmentChangeRequestRepository(),
                    new AppointmentChangeRequestService(
                        new AppointmentRepository(),
                        new AppointmentChangeRequestRepository())));
        }
    }
}