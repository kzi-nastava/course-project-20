using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Globalization;
using HealthCareCenter.DoctorGUI;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Users.Services;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core;
using HealthCareCenter.Core.Users;

namespace HealthCareCenter.GUI.Doctor.ViewModels
{
    public class AddDeleteAppointmentViewModel
    {
        private User signedUser;
        private AddAlterAppointmentWindow window;
        private DoctorWindow previousWindow;
        private DoctorWindowViewModel previousService;
        private int selectedAppointmentIndex;
        public AddDeleteAppointmentViewModel(Core.Users.Models.Doctor _signedUser, DoctorWindow scheduleWindow, bool add, DoctorWindowViewModel service, int rowIndex = -1)
        {
            previousService = service;
            previousWindow = scheduleWindow;
            window = new AddAlterAppointmentWindow(this);
            signedUser = _signedUser;
            FillPatientsTable();
            FillDateTimeComboBoxes();

            if (add)
                window.sumbitAppointment.Visibility = Visibility.Visible;
            else
            {
                window.alterAppointment.Visibility = Visibility.Visible;
                CommitAlteringChanges(rowIndex);
            }
            window.Show();
        }
        public void CommitAlteringChanges(int rowIndex)
        {
            selectedAppointmentIndex = rowIndex;
            Appointment appointment = AppointmentRepository.Appointments[rowIndex];
            ParseAppointmentData(appointment);
        }

        public bool ParseAppointmentData(bool isBeingCreated)
        {
            Appointment appointment;
            int id;
            string selectedValue, unparsedDate;
            bool emergency, error, sucessfull;
            DateTime currentDate;
            DateTime displayedDate;

            if (isBeingCreated)
                appointment = new Appointment();
            else
                appointment = AppointmentRepository.Appointments[selectedAppointmentIndex];

            id = GetRowItemID(window.patientsDataGrid, "Id");
            if (id == -1) return false;

            selectedValue = GetComboBoxItem(window.appointmentTypeComboBox);
            if (selectedValue == "") return false;

            emergency = (bool)window.emergencyCheckBox.IsChecked;

            error = ValidateDateTimeComboBoxes();
            if (error) return false;

            unparsedDate = ParseDateTimeComboBoxes();

            displayedDate = DateTime.ParseExact(unparsedDate, Constants.DateTimeFormat, CultureInfo.InvariantCulture);

            sucessfull = TimeIsAvailable(appointment, displayedDate);
            if (!sucessfull) return false;

            Core.Patients.Models.Patient patient = PatientService.Get(id);
            if (patient == null) return false;

            currentDate = DateTime.Today;

            appointment.Type = (AppointmentType)Enum.Parse(typeof(AppointmentType), selectedValue);

            appointment.Emergency = emergency;

            appointment.DoctorID = signedUser.ID;

            appointment.HealthRecordID = patient.HealthRecordID;

            appointment.HospitalRoomID = 1;

            appointment.ScheduledDate = displayedDate;

            appointment.CreatedDate = currentDate;

            if (isBeingCreated)
            {
                appointment.ID = ++AppointmentRepository.LargestID;
                AppointmentRepository.Appointments.Add(appointment);
            }
            UpdateAppointmentsTable(AppointmentRepository.Appointments);
            return true;
        }

        public bool TimeIsAvailable(Appointment appointment, DateTime displayedDate)
        {
            foreach (Appointment appointments in AppointmentRepository.Appointments)
            {
                if (appointments.ID == appointment.ID)
                {
                    continue;
                }

                TimeSpan timeSpan = appointments.ScheduledDate.Subtract(displayedDate);
                if (Math.Abs(timeSpan.TotalMinutes) < 15)
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateAppointmentsTable(List<Appointment> appointments)
        {
            previousService.FillAppointmentsTable(appointments);
        }
        public void ParseAppointmentData(Appointment appointment)
        {
            int year, month, day, hour, minute, appointmentTypeIndex, patientIndex;
            bool emergency;
            string[] timeFragments = appointment.ScheduledDate.ToString().Split("/");
            string[] yearAndTime = timeFragments[2].Split(" ");
            string[] time = yearAndTime[1].Split(":");
            day = int.Parse(timeFragments[1]) - 1;
            month = int.Parse(timeFragments[0]) - 1;
            year = int.Parse(yearAndTime[0]) - 2022;
            hour = int.Parse(time[0]);
            if (hour <= 12 && yearAndTime[2] == "AM")
            {
                hour -= 8;
            }
            else
            {
                hour += 4;
            }

            minute = int.Parse(time[1]) / 15;
            emergency = appointment.Emergency;
            if (appointment.Type == AppointmentType.Checkup)
            {
                appointmentTypeIndex = 0;
            }
            else
            {
                appointmentTypeIndex = 1;
            }

            patientIndex = PatientService.GetIndex(appointment.HealthRecordID);
            window.FillAppointmentWithDefaultValues(year, month, day, hour, minute, appointmentTypeIndex, patientIndex, emergency);

        }

        public string ParseDateTimeComboBoxes()
        {
            string day, month, year, hour, minute;
            day = window.dayComboBox.SelectedItem.ToString();
            month = window.monthComboBox.SelectedItem.ToString();
            year = window.yearComboBox.SelectedItem.ToString();
            hour = window.hourComboBox.SelectedItem.ToString();
            minute = window.minuteComboBox.SelectedItem.ToString();
            return day + "/" + month + "/" + year + " " + hour + ":" + minute;
        }

        public bool ValidateDateTimeComboBoxes()
        {
            if (window.dayComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select day from combo box");
                return true;
            }
            if (window.monthComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select month from combo box");
                return true;
            }
            if (window.yearComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select year from combo box");
                return true;
            }
            if (window.hourComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select hour from combo box");
                return true;
            }
            if (window.minuteComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select minute from combo box");
                return true;
            }
            return false;
        }

        public void FillPatientsTable()
        {
            window.patientsDataTable.Rows.Clear();
            foreach (Core.Patients.Models.Patient patient in UserRepository.Patients)
            {
                window.AddPatientToPatientsTable(patient);
            }
            window.patientsDataGrid.ItemsSource = window.patientsDataTable.DefaultView;
        }
        public void FillDateTimeComboBoxes()
        {
            for (int i = 1; i <= 31; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                window.dayComboBox.Items.Add(s);
            }
            for (int i = 1; i <= 12; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                window.monthComboBox.Items.Add(s);
            }
            window.yearComboBox.Items.Add("2022");
            window.yearComboBox.Items.Add("2023");
            window.yearComboBox.Items.Add("2024");

            for (int i = 8; i <= 21; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                window.hourComboBox.Items.Add(s);
            }
            for (int i = 0; i <= 45; i += 15)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                window.minuteComboBox.Items.Add(s);
            }
        }
        public int GetSelectedIndex(DataGrid dataGrid)
        {
            int rowIndex;
            try
            {
                rowIndex = dataGrid.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return -1;
            }
            return rowIndex;
        }

        public int GetRowItemID(DataGrid grid, string key)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)grid.SelectedItems[0];
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return -1;
            }
            return (int)row[key];
        }

        public string GetComboBoxItem(ComboBox comboBox)
        {
            string selectedValue = "";
            try
            {
                selectedValue = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return selectedValue;
        }

    }
}