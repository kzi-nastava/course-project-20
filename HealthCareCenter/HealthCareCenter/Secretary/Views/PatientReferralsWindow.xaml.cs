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

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for PatientReferralsWindow.xaml
    /// </summary>
    public partial class PatientReferralsWindow : Window
    {
        private readonly Patient _patient;
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

                Add(referral);
            }
            referralsDataGrid.ItemsSource = _referrals;
        }

        private void Add(Referral referral)
        {
            PatientReferral patientReferral = new PatientReferral(referral.ID);

            LinkDoctor(referral, patientReferral);
            _referrals.Add(patientReferral);
        }

        private static void LinkDoctor(Referral referral, PatientReferral patientReferral)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID != referral.DoctorID)
                {
                    continue;
                }
                patientReferral.DoctorUsername = doctor.Username;
                patientReferral.DoctorFirstName = doctor.FirstName;
                patientReferral.DoctorLastName = doctor.LastName;
                return;
            }
        }

        private void UseReferralButton_Click(object sender, RoutedEventArgs e)
        {
            if (referralsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must first select a referral from the table to use!");
                return;
            }

            Referral selectedReferral = FindReferral();

            ScheduleAppointmentReferralWindow window = new ScheduleAppointmentReferralWindow(_patient, selectedReferral);
            window.ShowDialog();
            Refresh();
        }

        private Referral FindReferral()
        {
            foreach (Referral referral in ReferralRepository.Referrals)
            {
                if (referral.ID == ((PatientReferral)referralsDataGrid.SelectedItem).ID)
                {
                    return referral;
                }
            }
            return null;
        }
    }
}
