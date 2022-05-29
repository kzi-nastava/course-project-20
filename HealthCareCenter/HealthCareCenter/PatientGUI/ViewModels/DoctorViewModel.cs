using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    class DoctorViewModel : ViewModelBase
    {
        private readonly Doctor _doctor;

        public string DoctorID => _doctor.ID.ToString();
        public string DoctorFirstName => _doctor.FirstName.ToString();
        public string DoctorLastName => _doctor.LastName.ToString();
        public string DoctorProfessionalArea => _doctor.Type.ToString();

        public DoctorViewModel(Doctor doctor)
        {
            _doctor = doctor;
        }

    }
}
