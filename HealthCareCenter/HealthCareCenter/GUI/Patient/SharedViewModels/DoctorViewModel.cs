using HealthCareCenter.Core.Surveys.Services;

namespace HealthCareCenter.GUI.Patient.SharedViewModels
{
    internal class DoctorViewModel : ViewModelBase
    {
        private readonly Core.Users.Models.Doctor _doctor;

        public int DoctorID => _doctor.ID;
        public string DoctorFirstName => _doctor.FirstName;
        public string DoctorLastName => _doctor.LastName;
        public string DoctorProfessionalArea => _doctor.Type;
        public double DoctorRating => DoctorSurveyRatingService.GetAverageRating(_doctor.ID);

        public DoctorViewModel(Core.Users.Models.Doctor doctor)
        {
            _doctor = doctor;
        }

    }
}
