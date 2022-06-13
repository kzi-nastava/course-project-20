using HealthCareCenter.GUI.Patient.SharedViewModels;
using HealthCareCenter.GUI.Patient.Survey.Commands;
using System.Windows.Input;

namespace HealthCareCenter.GUI.Patient.Survey.ViewModels
{
    class HealthCenterSurveyViewModel : ViewModelBase
    {
        public Core.Patients.Patient Patient { get; }

        public bool ServiceQualityTicked1 { get; set; }
        public bool ServiceQualityTicked2 { get; set; }
        public bool ServiceQualityTicked3 { get; set; }
        public bool ServiceQualityTicked4 { get; set; }
        public bool ServiceQualityTicked5 { get; set; }

        public bool HygieneQualityTicked1 { get; set; }
        public bool HygieneQualityTicked2 { get; set; }
        public bool HygieneQualityTicked3 { get; set; }
        public bool HygieneQualityTicked4 { get; set; }
        public bool HygieneQualityTicked5 { get; set; }

        public bool SatisfiedTicked1 { get; set; }
        public bool SatisfiedTicked2 { get; set; }
        public bool SatisfiedTicked3 { get; set; }
        public bool SatisfiedTicked4 { get; set; }
        public bool SatisfiedTicked5 { get; set; }

        public bool WouldRecommendTicked1 { get; set; }
        public bool WouldRecommendTicked2 { get; set; }
        public bool WouldRecommendTicked3 { get; set; }
        public bool WouldRecommendTicked4 { get; set; }
        public bool WouldRecommendTicked5 { get; set; }

        private string _commentOnHealthcareCenter;
        public string CommentOnHealthcareCenter
        {
            get => _commentOnHealthcareCenter;
            set
            {
                _commentOnHealthcareCenter = value;
                OnPropertyChanged(nameof(CommentOnHealthcareCenter));
            }
        }

        public ICommand SubmitReview { get; }

        public HealthCenterSurveyViewModel(Core.Patients.Patient patient)
        {
            Patient = patient;

            ServiceQualityTicked5 = true;
            HygieneQualityTicked5 = true;
            SatisfiedTicked5 = true;
            WouldRecommendTicked5 = true;

            SubmitReview = new SubmitHealthcareCenterReviewCommand(this);
        }
    }
}
