using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Globalization;
using System.ComponentModel;
using HealthCareCenter.GUI.Doctor.ViewModels;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Patients;

namespace HealthCareCenter.DoctorGUI
{
    /// <summary>
    /// Interaction logic for AddAlterAppointmentWindow.xaml
    /// </summary>
    public partial class AddAlterAppointmentWindow : Window
    {
        private AddDeleteAppointmentViewModel windowService;
        private readonly BaseAppointmentRepository _appointmentRepository;

        public DataTable patientsDataTable;
        private DataRow dr;
        public AddAlterAppointmentWindow(
            AddDeleteAppointmentViewModel _windowService,
            BaseAppointmentRepository appointmentRepository)
        {
            windowService = _windowService;
            _appointmentRepository = appointmentRepository;
            CreatePatientsTable();
            InitializeComponent();
        }
        private void CreatePatientsTable()
        {
            patientsDataTable = new DataTable("Patients");
            DataColumn dc1 = new DataColumn("Id", typeof(int));
            DataColumn dc2 = new DataColumn("First name", typeof(string));
            DataColumn dc3 = new DataColumn("Last name", typeof(string));
            patientsDataTable.Columns.Add(dc1);
            patientsDataTable.Columns.Add(dc2);
            patientsDataTable.Columns.Add(dc3);
        }

        public void AddPatientToPatientsTable(Patient patient)
        {
            dr = patientsDataTable.NewRow();
            dr[0] = patient.ID;
            dr[1] = patient.FirstName;
            dr[2] = patient.LastName;
            patientsDataTable.Rows.Add(dr);
        }

        public void FillAppointmentWithDefaultValues(int year, int month, int day, int hour, int minute, int appointmentTypeIndex, int patientIndex, bool emergency)
        {
            yearComboBox.SelectedItem = year;
            monthComboBox.SelectedItem = month;
            dayComboBox.SelectedItem = day;
            hourComboBox.SelectedItem = hour;
            minuteComboBox.SelectedItem = minute;
            emergencyCheckBox.IsChecked = emergency;
            appointmentTypeComboBox.SelectedIndex = appointmentTypeIndex;
            patientsDataGrid.SelectedIndex = patientIndex;
        }


        private void sumbitAppointment_Click(object sender, RoutedEventArgs e)
        {
            bool sucessfull = windowService.ParseAppointmentData(true);
            if (!sucessfull)
                return;
            Close();
        }

        private void AlterAppointment_Click(object sender, RoutedEventArgs e)
        {
            bool sucessfull = windowService.ParseAppointmentData(false);
            if (!sucessfull)
                return;
            windowService.UpdateAppointmentsTable(_appointmentRepository.Appointments);
            Close();
        }
    }
}
