using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Secretary.Controllers;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HealthCareCenter.Secretary
{
    /// <summary>
    /// Interaction logic for OccupiedAppointmentsWindow.xaml
    /// </summary>
    public partial class OccupiedAppointmentsWindow : Window
    {
        private readonly Patient _patient;
        private readonly AppointmentType _type;
        private List<Appointment> _occupiedAppointments;
        private List<AppointmentDisplay> _appointmentsForDisplay;
        private Dictionary<int, DateTime> _newDateOf;
        private readonly Dictionary<int, Appointment> _newAppointmentsInfo;

        private readonly OccupiedAppointmentsController _controller;

        public OccupiedAppointmentsWindow()
        {
            InitializeComponent();
        }

        public OccupiedAppointmentsWindow(Patient patient, AppointmentType type, List<Appointment> occupiedAppointments, Dictionary<int, Appointment> newAppointmentsInfo)
        {
            _patient = patient;
            _type = type;
            _occupiedAppointments = occupiedAppointments;
            _newAppointmentsInfo = newAppointmentsInfo;
            _controller = new OccupiedAppointmentsController();

            InitializeComponent();
            try
            {
                _controller.SortAppointments(ref _occupiedAppointments, ref _newDateOf);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                _appointmentsForDisplay = _controller.GetAppointmentsForDisplay(_occupiedAppointments, _newDateOf);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            occupiedAppointmentsDataGrid.ItemsSource = _appointmentsForDisplay;
        }

        private void PostponeButton_Click(object sender, RoutedEventArgs e)
        {
            if (occupiedAppointmentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select an appointment to postpone!");
                return;
            }

            string notification = null;
            Appointment postponedAppointment;
            try
            {
                postponedAppointment = _controller.Postpone(ref notification, _patient, _newAppointmentsInfo, _type, (AppointmentDisplay)occupiedAppointmentsDataGrid.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (!string.IsNullOrEmpty(notification))
                MessageBox.Show(notification);

            MessageBox.Show($"Successfully postponed appointment {postponedAppointment.ID} and scheduled a new urgent appointment in its place.");
            Close();
        }
    }
}
