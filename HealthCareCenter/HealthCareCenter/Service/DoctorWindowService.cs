using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HealthCareCenter.Model;
using System.Data;
using HealthCareCenter.Enums;
using System.Globalization;
using System.ComponentModel;

namespace HealthCareCenter.Service
{
    public class DoctorWindowService
    {
        private User _signedUser;
        private DoctorWindow window;
        public DoctorWindowService(User signedUser) {
            _signedUser = signedUser;
            window = new DoctorWindow(signedUser, this);
            window.Show();
            FillMedicineRequestsTable();
            FillPatientsTable();
            FillMedicinesTable();
        }
        private void FillMedicineRequestsTable()
        {
            window.medicineCreationRequestDataTable.Rows.Clear();
            foreach (MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
            {
                if (request.State != RequestState.Waiting)
                    continue;
                window.AddMedicineRequestToTable(request);
            }
            window.medicineCreationRequestDataGrid.ItemsSource = window.medicineCreationRequestDataTable.DefaultView;
        }
        private void FillPatientsTable()
        {
            window.patientsDataTable.Rows.Clear();
            foreach (Patient patient in UserRepository.Patients)
            {
                window.AddPatientToPatientsTable(patient);
            }
            window.patientsDataGrid.ItemsSource = window.patientsDataTable.DefaultView;
        }
        private void FillMedicinesTable()
        {
            window.medicineDataTable.Rows.Clear();
            foreach (Medicine medicine in MedicineRepository.Medicines)
            {
                window.AddMedicineToMedicineTable(medicine);
            }
            window.medicationDataGrid.ItemsSource = window.medicineDataTable.DefaultView;
        }

        private void FillEquipmentTable(Room room)
        {
            window.equipmentDataTable.Rows.Clear();
            foreach (string name in room.EquipmentAmounts.Keys)
            {
                window.AddEquipmentToEquipmentTable(name);
            }
            window.equipmentDataGrid.ItemsSource = window.equipmentDataTable.DefaultView;
        }
        private void FillDoctorsTable(List<Doctor> doctors)
        {
            window.doctorsDataTable.Rows.Clear();
            foreach (Doctor doctor in doctors)
            {
                window.AddDoctorToDoctorsTable(doctor);
            }
            window.doctorsDataGrid.ItemsSource = window.doctorsDataTable.DefaultView;
        }

        private void FillDateTimeComboBoxes()
        {
            for (int i = 1; i <= 31; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                window.dayComboBox.Items.Add(s);
                window.dayChoiceComboBox.Items.Add(s);
            }
            for (int i = 1; i <= 12; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                window.monthComboBox.Items.Add(s);
                window.monthChoiceComboBox.Items.Add(s);
            }
            window.yearComboBox.Items.Add("2022");
            window.yearComboBox.Items.Add("2023");
            window.yearComboBox.Items.Add("2024");
            window.yearChoiceComboBox.Items.Add("2022");
            window.yearChoiceComboBox.Items.Add("2023");
            window.yearChoiceComboBox.Items.Add("2024");
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

        private void FillMedicineTakingComboBoxes()
        {
            for (int i = 0; i <= 24; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                    s = "0" + s;
                window.hourOfMedicineTakingComboBox.Items.Add(s);
            }
            for (int i = 0; i <= 59; i += 1)
            {
                string s = i.ToString();
                if (s.Length == 1)
                    s = "0" + s;
                window.minuteOfMedicineTakingComboBox.Items.Add(s);
            }
        }

        private void FillAppointmentsTable(List<Appointment> appointments)
        {
            window.appointmentsDataTable.Rows.Clear();
            if (AppointmentRepository.Appointments == null)
            {
                appointments = AppointmentRepository.Load();
            }

            foreach (Appointment appointment in appointments)
            {
                if (appointment.DoctorID != _signedUser.ID)
                {
                    continue;
                }
                int patientID = -1;
                HealthRecord patientsHealthRecord = HealthRecordService.FindRecord(appointment.HealthRecordID);
                if (patientsHealthRecord != null)
                    window.AddAppointmentToAppointmentsTable(appointment, patientsHealthRecord.PatientID);
                else
                    window.AddAppointmentToAppointmentsTable(appointment, -1);
            }
            window.scheduleDataGrid.ItemsSource = window.appointmentsDataTable.DefaultView;
        }
        private bool ParseAppointmentData(Appointment appointment, bool isBeingCreated)
        {
            int id;
            string selectedValue, unparsedDate;
            bool emergency, error, sucessfull; 
            DateTime currentDate;
            DateTime displayedDate;

            id = getRowItemID(window.patientsDataGrid, "Id");
            if (id == -1) return false;

            selectedValue = getComboBoxItem(window.appointmentTypeComboBox);
            if (selectedValue == "") return false;

            emergency = (bool)window.emergencyCheckBox.IsChecked;

            error = ValidateDateTimeComboBoxes();
            if (error) return false;

            unparsedDate = ParseDateTimeComboBoxes();

            displayedDate = DateTime.ParseExact(unparsedDate, Constants.DateTimeFormat, CultureInfo.InvariantCulture);

            sucessfull = TimeIsAvailable(appointment, displayedDate);
            if (!sucessfull) return false;

            Patient patient = PatientService.FindPatient(id);
            if (patient != null) return false;

            currentDate = DateTime.Today;

            appointment.Type = (AppointmentType)Enum.Parse(typeof(AppointmentType), selectedValue);

            appointment.Emergency = emergency;

            appointment.DoctorID = _signedUser.ID;

            appointment.HealthRecordID = patient.HealthRecordID;

            appointment.HospitalRoomID = 1;

            appointment.ScheduledDate = displayedDate;

            appointment.CreatedDate = currentDate;

            if (isBeingCreated)
            {
                appointment.ID = ++AppointmentRepository.LargestID;
                AppointmentRepository.Appointments.Add(appointment);
            }
            FillAppointmentsTable(AppointmentRepository.Appointments);
            window.appointmentCreationGrid.Visibility = Visibility.Collapsed;
            window.scheduleGrid.Visibility = Visibility.Visible;
            return true;
        }

        private bool TimeIsAvailable(Appointment appointment,DateTime displayedDate)
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
        private bool ValidateDateTimeComboBoxes()
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
        private string ParseDateTimeComboBoxes()
        {
            string day, month, year, hour, minute;
            day = window.dayComboBox.SelectedItem.ToString();
            month = window.monthComboBox.SelectedItem.ToString();
            year = window.yearComboBox.SelectedItem.ToString();
            hour = window.hourComboBox.SelectedItem.ToString();
            minute = window.minuteComboBox.SelectedItem.ToString();
            return day + "/" + month + "/" + year + " " + hour + ":" + minute;
        }
        private void ParseAppointmentData(Appointment appointment)
        {
            int year,month,day,hour,minute,appointmentTypeIndex, patientIndex;
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

            patientIndex = PatientService.FindPatientIndex(appointment.HealthRecordID);

        }
        private void ParseHealthRecordData(HealthRecord healthRecord)
        {
            int appointmentIndex;
            string height, weight, alergens, previousDiseases, healthRecordID, anamnesis;
            appointmentIndex = window.scheduleDataGrid.SelectedIndex;
            if(appointmentIndex == -1)
            {
                MessageBox.Show("No row is selected");
                return;
            }
            alergens = HealthRecordService.CheckAlergens(healthRecord);
            previousDiseases = HealthRecordService.CheckPreviousDiseases(healthRecord);
            healthRecordID = healthRecord.ID.ToString();
            height = healthRecord.Height.ToString();
            weight = healthRecord.Weight.ToString();
            if (AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis.Comment == "")
            {
                anamnesis = "No anamnesis";
                window.createAPrescription.IsEnabled = false;
            }
            else
            {
                anamnesis = AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis.Comment;
                window.createAPrescription.IsEnabled = true;
            }
        }
        private int getRowItemID(DataGrid grid, string key)
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
        private string getRowItem(DataGrid grid, string key)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)grid.SelectedItems[0];
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return "";
            }
            return (string)row[key];
        }

        private string getComboBoxItem(ComboBox comboBox)
        {
            string selectedValue = "";
            try
            {
                selectedValue = ((ComboBoxItem)window.appointmentTypeComboBox.SelectedItem).Content.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return selectedValue;
        }
    }
}
