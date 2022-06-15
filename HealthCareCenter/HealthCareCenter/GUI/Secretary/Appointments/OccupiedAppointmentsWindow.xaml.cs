using HealthCareCenter.Core;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.Core.Appointments.Urgent;
using HealthCareCenter.Core.Appointments.Urgent.Controllers;
using HealthCareCenter.Core.Appointments.Urgent.DTO;
using HealthCareCenter.Core.Appointments.Urgent.Services;
using HealthCareCenter.Core.Notifications.Repositories;
using HealthCareCenter.Core.Notifications.Services;
using HealthCareCenter.Core.Patients;
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
        private List<OccupiedAppointment> _appointmentsForDisplay;
        private PostponableAppointmentsDTO _info;

        private readonly OccupiedAppointmentsController _controller;

        public OccupiedAppointmentsWindow()
        {
            InitializeComponent();
        }

        public OccupiedAppointmentsWindow(Patient patient, AppointmentType type, OccupiedAppointmentsDTO info)
        {
            _patient = patient;
            _type = type;
            _info = new PostponableAppointmentsDTO(info);
            BaseUrgentAppointmentService service = new UrgentAppointmentService(
                new TermsService(
                    new AppointmentRepository()),
                new NotificationService(
                    new NotificationRepository()),
                new AppointmentRepository(),
                new AppointmentService(
                    new AppointmentRepository(),
                    new AppointmentChangeRequestRepository(),
                    new AppointmentChangeRequestService(
                        new AppointmentRepository(),
                        new AppointmentChangeRequestRepository()))) { OccupiedInfo = _info };
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
                postponedAppointment = _controller.Postpone(ref notification, _patient, _type, (OccupiedAppointment)occupiedAppointmentsDataGrid.SelectedItem);
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
