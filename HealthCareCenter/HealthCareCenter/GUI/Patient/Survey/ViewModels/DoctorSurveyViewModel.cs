using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using HealthCareCenter.GUI.Patient.Survey.Commands;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.GUI.Patient.Survey.ViewModels
{
    class DoctorSurveyViewModel : ViewModelBase
    {
        public Core.Patients.Patient Patient { get; }

        private List<AppointmentViewModel> _appointments;
        public List<AppointmentViewModel> Appointments
        {
            get => _appointments;
            set
            {
                _appointments = value;
                OnPropertyChanged(nameof(Appointments));
            }
        }

        private AppointmentViewModel _chosenAppointment;
        public AppointmentViewModel ChosenAppointment
        {
            get => _chosenAppointment;
            set
            {
                _chosenAppointment = value;
                OnPropertyChanged(nameof(ChosenAppointment));
            }
        }

        private string _doctorFullName;
        public string DoctorFullName
        {
            get => _doctorFullName;
            set
            {
                _doctorFullName = value;
                OnPropertyChanged(nameof(DoctorFullName));
            }
        }

        private string _commentOnDoctor;
        public string CommentOnDoctor
        {
            get => _commentOnDoctor;
            set
            {
                _commentOnDoctor = value;
                OnPropertyChanged(nameof(CommentOnDoctor));
            }
        }

        public bool ServiceQualityTicked1 { get; set; }
        public bool ServiceQualityTicked2 { get; set; }
        public bool ServiceQualityTicked3 { get; set; }
        public bool ServiceQualityTicked4 { get; set; }
        public bool ServiceQualityTicked5 { get; set; }

        public bool WouldRecommendTicked1 { get; set; }
        public bool WouldRecommendTicked2 { get; set; }
        public bool WouldRecommendTicked3 { get; set; }
        public bool WouldRecommendTicked4 { get; set; }
        public bool WouldRecommendTicked5 { get; set; }

        public ICommand ChooseDoctorFromAppointment { get; }
        public ICommand SubmitReview { get; }

        public DoctorSurveyViewModel(
            IAppointmentService appointmentService,
            Core.Patients.Patient patient)
        {
            Patient = patient;

            _appointments = new List<AppointmentViewModel>();
            List<Appointment> finishedAppointment = appointmentService.GetPatientFinishedAppointments(Patient.HealthRecordID);
            foreach (Appointment appointment in finishedAppointment)
            {
                _appointments.Add(new AppointmentViewModel(appointment));
            }

            DoctorFullName = "N/A";

            ServiceQualityTicked5 = true;
            WouldRecommendTicked5 = true;

            ChooseDoctorFromAppointment = new ChooseDoctorFromAppointmentCommand(this);
            SubmitReview = new SubmitDoctorReviewCommand(this);
        }

    }
}
