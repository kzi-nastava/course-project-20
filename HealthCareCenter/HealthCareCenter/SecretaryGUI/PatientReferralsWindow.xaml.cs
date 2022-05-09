using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Interaction logic for PatientReferralsWindow.xaml
    /// </summary>
    public partial class PatientReferralsWindow : Window
    {
        private Patient _patient;
        private List<PatientReferral> _referrals;

        public PatientReferralsWindow()
        {
            InitializeComponent();
        }

        public PatientReferralsWindow(Patient patient)
        {
            _patient = patient;
            ReferralRepository.Load();

            InitializeComponent();

            referralsDataGrid.IsReadOnly = true;
            Refresh();
        }

        private void Refresh()
        {
            _referrals = new List<PatientReferral>();
            foreach (Referral referral in ReferralRepository.Referrals)
            {
                if (referral.PatientID != _patient.ID)
                {
                    continue;
                }

                PatientReferral patientReferral = new PatientReferral
                {
                    ID = referral.ID
                };

                foreach (Doctor doctor in UserRepository.Doctors)
                {
                    if (doctor.ID != referral.DoctorID)
                    {
                        continue;
                    }
                    patientReferral.DoctorUsername = doctor.Username;
                    patientReferral.DoctorFirstName = doctor.FirstName;
                    patientReferral.DoctorLastName = doctor.LastName;
                    break;
                }
                _referrals.Add(patientReferral);
            }

            referralsDataGrid.ItemsSource = _referrals;
        }
    }
}
