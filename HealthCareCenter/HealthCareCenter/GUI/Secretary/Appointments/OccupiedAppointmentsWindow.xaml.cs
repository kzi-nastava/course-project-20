using HealthCareCenter.Core;
using HealthCareCenter.Core.Appointments.Controllers;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Patients.Models;
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
        private List<AppointmentDisplay> _appointmentsForDisplay;
        private OccupiedAppointmentInfo _info;

        private readonly OccupiedAppointmentsController _controller;

        public OccupiedAppointmentsWindow()
        {
            InitializeComponent();
        }

        public OccupiedAppointmentsWindow(Patient patient, AppointmentType type, UrgentAppointmentInfo info)
        {
            _patient = patient;
            _type = type;
            _info = new OccupiedAppointmentInfo(info);
            BaseUrgentAppointmentService service = new UrgentAppointmentService(new TermsService(), new NotificationService(new NotificationRepository())) { OccupiedInfo = _info };
            _controller = new OccupiedAppointmentsController(service);

            InitializeComponent();
            try
            {
                _controller.SortAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                _appointmentsForDisplay = _controller.GetAppointmentsForDisplay();
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
                postponedAppointment = _controller.Postpone(ref notification, _patient, _type, (AppointmentDisplay)occupiedAppointmentsDataGrid.SelectedItem);
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
