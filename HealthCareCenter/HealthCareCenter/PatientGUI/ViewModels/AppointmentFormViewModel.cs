using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class AppointmentFormViewModel : ViewModelBase
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
                OnPropertyChanged(nameof(ChosenDoctor));
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

        private List<AppointmentTerm> _allTerms;
        public List<AppointmentTerm> AllTerms
        {
            get => _allTerms;
            set
            {
                _allTerms = value;
                OnPropertyChanged(nameof(AllTerms));
            }
        }

        private AppointmentTerm _chosenTerm;
        public AppointmentTerm ChosenTerm
        {
            get => _chosenTerm;
            set
            {
                _chosenTerm = value;
                OnPropertyChanged(nameof(ChosenTerm));
            }
        }

        public AppointmentViewModel ChosenAppointment { get; }

        public ICommand SubmitAppointment { get; }

        public AppointmentFormViewModel(NavigationStore navigationStore, Patient patient, AppointmentViewModel chosenAppointment, bool isModification, DoctorViewModel chosenDoctor)
        {
            Patient = patient;

            ChosenAppointment = isModification ? chosenAppointment : null;
            ChosenDate = ChosenAppointment == null ? DateTime.Now.Date : ChosenAppointment.AppointmentDate;
            FillDoctorListView(chosenDoctor);
            FillAppointmentSchedulesListView();

            SubmitAppointment = new SubmitAppointmentCommand(navigationStore, this);
        }

        private void FillDoctorListView(DoctorViewModel chosenDoctor)
        {
            _doctors = new List<DoctorViewModel>();

            if (chosenDoctor == null)
            {
                List<Doctor> allDoctors = UserRepository.Doctors;
                foreach (Doctor doctor in allDoctors)
                {
                    DoctorViewModel doctorViewModel = new DoctorViewModel(doctor);
                    _doctors.Add(doctorViewModel);
                    if (ChosenAppointment != null && doctorViewModel.DoctorID == ChosenAppointment.DoctorID)
                    {
                        ChosenDoctor = doctorViewModel;
                    }
                }
            }
            else
            {
                _doctors.Add(chosenDoctor);
                ChosenDoctor = chosenDoctor;
            }

        }

        private void FillAppointmentSchedulesListView()
        {
            _allTerms = new List<AppointmentTerm>();

            List<AppointmentTerm> allPossibleTerms = AppointmentTermService.GetDailyTermsFromRange(
                Constants.StartWorkTime, 0, Constants.EndWorkTime, 0);
            foreach (AppointmentTerm term in allPossibleTerms)
            {
                _allTerms.Add(term);
                if (ChosenAppointment != null && ChosenDate.ToString("t") == term.ToString())
                {
                    ChosenTerm = term;
                }
            }

            if (_chosenTerm == null)
            {
                ChosenTerm = AllTerms[0];
            }
        }

    }
}
