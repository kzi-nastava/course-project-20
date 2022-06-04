using HealthCareCenter.Model;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class DoctorViewModel : ViewModelBase
    {
        private readonly Doctor _doctor;

        public int DoctorID => _doctor.ID;
        public string DoctorFirstName => _doctor.FirstName;
        public string DoctorLastName => _doctor.LastName;
        public string DoctorProfessionalArea => _doctor.Type;
        public double DoctorRating => _doctor.GetAverageRating();

        public DoctorViewModel(Doctor doctor)
        {
            _doctor = doctor;
        }

    }
}
