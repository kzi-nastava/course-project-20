using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using HealthCareCenter.Enums;
using HealthCareCenter.Secretary.Controllers;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for ScheduleUrgentAppointmentWindow.xaml
    /// </summary>
    public partial class ScheduleUrgentAppointmentWindow : Window
    {
        private readonly Patient _patient;
        private List<Appointment> _occupiedAppointments;
        private Dictionary<int, Appointment> _newAppointmentsInfo;

        private readonly ScheduleUrgentAppointmentController _controller;

        public ScheduleUrgentAppointmentWindow()
        {
            InitializeComponent();
        }

        public ScheduleUrgentAppointmentWindow(Patient patient)
        {
            _patient = patient;
            _controller = new ScheduleUrgentAppointmentController();

            InitializeComponent();

            try
            {
                doctorTypesListBox.ItemsSource = _controller.GetTypesOfDoctors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EnteredData())
                return;

            TryScheduling();
        }

        private void TryScheduling()
        {
            AppointmentType type = GetSelectedAppointmentType();
            try
            {
                if (!_controller.TryScheduling(type, doctorTypesListBox.SelectedItem.ToString(), _patient, ref _occupiedAppointments, ref _newAppointmentsInfo))
                {
                    OpenPostponingWindow(type);
                }
                else
                {
                    MessageBox.Show("Successfully scheduled urgent appointment.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            Close();
        }

        private AppointmentType GetSelectedAppointmentType()
        {
            AppointmentType type = AppointmentType.Checkup;
            if ((bool)operationRadioButton.IsChecked)
                type = AppointmentType.Operation;
            return type;
        }

        private void OpenPostponingWindow(AppointmentType type)
        {
            if (_occupiedAppointments.Count == 0)
            {
                MessageBox.Show("No available term was found in the next 2 hours. Unfortunately, there are no terms to postpone at this time neither.");
                return;
            }
            MessageBox.Show("No available term was found in the next 2 hours. You can, however, postpone an occupied term in the next window.");
            OccupiedAppointmentsWindow window = new OccupiedAppointmentsWindow(_patient, type, _occupiedAppointments, _newAppointmentsInfo);
            window.ShowDialog();
            Close();
        }

        private bool EnteredData()
        {
            if (doctorTypesListBox.SelectedItem == null)
            {
                MessageBox.Show("You must select a type of doctor first.");
                return false;
            }
            if (!(bool)checkupRadioButton.IsChecked && !(bool)operationRadioButton.IsChecked)
            {
                MessageBox.Show("You must select a type of appointment first.");
                return false;
            }
            return true;
        }
    }
}
