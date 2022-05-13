using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HealthCareCenter.Model;
using System.Data;
using HealthCareCenter.Enums;
using System.Globalization;
using System.ComponentModel;
using HealthCareCenter.Service;

namespace HealthCareCenter
{
    public partial class DoctorWindow : Window
    {
        private Doctor signedUser;
        private DataTable appointmentsDataTable;
        private DataTable patientsDataTable;
        private bool patientsTableIsFilled = false;
        private int appointmentIndex;
        private int healthRecordIndex;
        DataRow dr;
        public DoctorWindow(Model.User user)
        {
            signedUser = (Doctor)user;
            CreateAppointmentTable();
            CreatePatientsTable();
            HealthRecordRepository.Load();
            AppointmentRepository.Load();
            InitializeComponent();
            FillDateTimeComboBoxes();

            DisplayNotifications();
        }

        private void DisplayNotifications()
        {
            List<Notification> notifications = NotificationService.FindUnopened(signedUser);
            if (notifications.Count == 0)
            {
                return;
            }
            MessageBox.Show("You have new notifications.");
            foreach (Notification notification in notifications)
            {
                MessageBox.Show(notification.Message);
            }
        }
        //Creating tables(table headers)
        private void CreateAppointmentTable()
        {
            appointmentsDataTable = new DataTable("Appointments");
            DataColumn dc1 = new DataColumn("Id", typeof(int));
            DataColumn dc2 = new DataColumn("Type of appointment", typeof(string));
            DataColumn dc3 = new DataColumn("Appointment date", typeof(string));
            DataColumn dc4 = new DataColumn("Creation date", typeof(string));
            DataColumn dc5 = new DataColumn("Emergency", typeof(bool));
            DataColumn dc6 = new DataColumn("Doctors first and last name", typeof(string));
            DataColumn dc7 = new DataColumn("Room", typeof(string));
            DataColumn dc8 = new DataColumn("Patient ID", typeof(string));
            appointmentsDataTable.Columns.Add(dc1);
            appointmentsDataTable.Columns.Add(dc2);
            appointmentsDataTable.Columns.Add(dc3);
            appointmentsDataTable.Columns.Add(dc4);
            appointmentsDataTable.Columns.Add(dc5);
            appointmentsDataTable.Columns.Add(dc6);
            appointmentsDataTable.Columns.Add(dc7);
            appointmentsDataTable.Columns.Add(dc8);
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

        //---------------------------------------------------------------------------------------
        //Putting data into tables
        private void FillPatientsTable()
        {
            patientsDataTable.Rows.Clear();
            foreach (Patient patient in UserRepository.Patients)
            {
                dr = patientsDataTable.NewRow();
                dr[0] = patient.ID;
                dr[1] = patient.FirstName;
                dr[2] = patient.LastName;
                patientsDataTable.Rows.Add(dr);
            }
            patientsTableIsFilled = true;
            patientsDataGrid.ItemsSource = patientsDataTable.DefaultView;
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

                dayComboBox.Items.Add(s);
                dayChoiceComboBox.Items.Add(s);
            }
            for (int i = 1; i <= 12; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                monthComboBox.Items.Add(s);
                monthChoiceComboBox.Items.Add(s);
            }
            yearComboBox.Items.Add("2022");
            yearComboBox.Items.Add("2023");
            yearComboBox.Items.Add("2024");
            yearChoiceComboBox.Items.Add("2022");
            yearChoiceComboBox.Items.Add("2023");
            yearChoiceComboBox.Items.Add("2024");
            for (int i = 8; i <= 21; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                hourComboBox.Items.Add(s);
            }
            for (int i = 0; i <= 45; i += 15)
            {
                string s = i.ToString();
                if (s.Length == 1)
                {
                    s = "0" + s;
                }

                minuteComboBox.Items.Add(s);
            }
        }
        private void FillAppointmentsTable(List<Appointment> appointments)
        {
            appointmentsDataTable.Rows.Clear();
            if (AppointmentRepository.Appointments == null)
            {
                appointments = AppointmentRepository.Load();
            }

            foreach (Appointment appointment in appointments)
            {
                if (appointment.DoctorID != signedUser.ID)
                {
                    continue;
                }
                dr = appointmentsDataTable.NewRow();
                dr[0] = appointment.ID;
                dr[1] = appointment.Type;
                dr[2] = appointment.ScheduledDate;
                dr[3] = appointment.CreatedDate;
                dr[4] = appointment.Emergency;
                dr[5] = appointment.DoctorID;
                dr[6] = appointment.HospitalRoomID;
                foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
                {
                    if (healthRecord.ID == appointment.HealthRecordID)
                    {
                        dr[7] = healthRecord.PatientID;
                        break;
                    }
                }
                appointmentsDataTable.Rows.Add(dr);
            }
            scheduleDataGrid.ItemsSource = appointmentsDataTable.DefaultView;
        }
        private void FillApointmentWithData(Appointment appointment, bool isBeingCreated)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)patientsDataGrid.SelectedItems[0];
            }
            catch
            {
                throw new Exception("Choose a patient from the table");
            }
            int id = (int)row["Id"];
            string selectedValue;
            try
            {
                selectedValue = ((ComboBoxItem)appointmentTypeComboBox.SelectedItem).Content.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            bool emergency = (bool)emergencyCheckBox.IsChecked;
            string day, month, year, hour, minute;
            if(dayComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select day from combo box");
                return;
            }
            if (monthComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select month from combo box");
                return;
            }
            if (yearComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select year from combo box");
                return;
            }
            if (hourComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select hour from combo box");
                return;
            }
            if (minuteComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select minute from combo box");
                return;
            }

            day = dayComboBox.SelectedItem.ToString();
            month = monthComboBox.SelectedItem.ToString();
            year = yearComboBox.SelectedItem.ToString();
            hour = hourComboBox.SelectedItem.ToString();
            minute = minuteComboBox.SelectedItem.ToString();
            string unparsedDate = day + "/" + month + "/" + year + " " + hour + ":" + minute;
            DateTime date = DateTime.ParseExact(unparsedDate,
                    Constants.DateTimeFormat,
                    CultureInfo.InvariantCulture);
            foreach (Appointment appointments in AppointmentRepository.Appointments)
            {
                if (appointments.ID == appointment.ID)
                {
                    continue;
                }

                TimeSpan timeSpan = appointments.ScheduledDate.Subtract(date);
                if (Math.Abs(timeSpan.TotalMinutes) < 15)
                {
                    throw new ArgumentException("Termin je zauzet");
                }
            }
            DateTime currentDate = DateTime.Today;
            appointment.Type = (AppointmentType)Enum.Parse(typeof(AppointmentType), selectedValue);
            appointment.Emergency = emergency;
            appointment.DoctorID = signedUser.ID;
            foreach(Patient patient in UserRepository.Patients)
            {
                if (patient.ID == id)
                {
                    appointment.HealthRecordID = patient.HealthRecordID;
                    break;
                }
            }
            appointment.HospitalRoomID = 1;
            appointment.ScheduledDate = date;
            appointment.CreatedDate = currentDate;
            if (isBeingCreated)
            {
                appointment.ID = ++AppointmentRepository.LargestID;
                AppointmentRepository.Appointments.Add(appointment);
            }
            FillAppointmentsTable(AppointmentRepository.Appointments);
            appointmentCreationGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
        }
        private void FillAppointmentWithDefaultValues(Appointment appointment)
        {
            string[] timeFragments = appointment.ScheduledDate.ToString().Split("/");
            dayComboBox.SelectedItem = timeFragments[0];
            string[] yearAndTime = timeFragments[2].Split(" ");
            string[] time = yearAndTime[1].Split(":");
            dayComboBox.SelectedIndex = int.Parse(timeFragments[1]) - 1;
            monthComboBox.SelectedIndex = int.Parse(timeFragments[0]) - 1;
            yearComboBox.SelectedIndex = int.Parse(yearAndTime[0]) - 2022;
            int hour = int.Parse(time[0]);
            if (hour <= 12 && yearAndTime[2] == "AM")
            {
                hour -= 8;
            }
            else
            {
                hour += 4;
            }

            hourComboBox.SelectedIndex = hour;                               
            minuteComboBox.SelectedIndex = int.Parse(time[1]) / 15;
            emergencyCheckBox.IsChecked = appointment.Emergency;
            if (appointment.Type == AppointmentType.Checkup)
            {
                appointmentTypeComboBox.SelectedIndex = 0;
            }
            else
            {
                appointmentTypeComboBox.SelectedIndex = 1;
            }

            int patientIndex = 0;
            foreach (Patient patient in UserRepository.Patients)
            {
                patientIndex++;
                if (patient.ID == appointment.HealthRecordID)
                {
                    break;
                }
            }
            patientsDataGrid.SelectedIndex = patientIndex;

        }
        private void FillHealthRecordData(HealthRecord healthRecord)
        {
            idLable.Content = healthRecord.ID.ToString();
            heightTextBox.Text = healthRecord.Height.ToString();
            weigthTextBox.Text = healthRecord.Weight.ToString();
            string previousDiseases = "";
            if (healthRecord.PreviousDiseases != null)
            {
                foreach (string s in healthRecord.PreviousDiseases)
                {
                    previousDiseases += "," + s;
                }
                
                if (healthRecord.PreviousDiseases.Count == 0)
                {
                    previousDiseasesTextBox.Text = "";
                }
                else
                {
                    previousDiseasesTextBox.Text = previousDiseases.Substring(1, previousDiseases.Length - 1);
                }
            }
            else
            {
                previousDiseasesTextBox.Text = "none";
            }

            string alergens = "";
            if (healthRecord.Allergens != null)
            {
                foreach (string s in healthRecord.Allergens)
                {
                    alergens += "," + s;
                }

                if (healthRecord.Allergens.Count == 0)
                {
                    alergensTextBox.Text = "";
                }
                else
                {
                    alergensTextBox.Text = alergens.Substring(1, alergens.Length - 1);
                }
            }
            else
            {
                alergensTextBox.Text = "none";
            }
            if(AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis == null)
            {
                anamnesisLabel.Content = "No anamnesis";
            }
            else
            {
                anamnesisLabel.Content = AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis.Comment;
            }
        }

        //---------------------------------------------------------------------------------------
        //First menu buttons
        private void ScheduleRewiewButton_Click(object sender, RoutedEventArgs e)
        {
            startingGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
            FillAppointmentsTable(AppointmentRepository.Appointments);
        }
        private void DrugMenagmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        //---------------------------------------------------------------------------------------
        //Buttons on schedule rewiew menu
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            appointmentCreationGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            alterAppointment.Visibility = Visibility.Collapsed;
            sumbitAppointment.Visibility = Visibility.Visible;
            if (patientsTableIsFilled)
            {
                return;
            }
            FillPatientsTable();

        }
        private void Alter_Click(object sender, RoutedEventArgs e)
        {

            int rowIndex;
            rowIndex = scheduleDataGrid.SelectedIndex;
            if (rowIndex == -1)
            {
                MessageBox.Show("Choose an appointment from the table");
                return;
            }
            appointmentCreationGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            alterAppointment.Visibility = Visibility.Visible;
            sumbitAppointment.Visibility = Visibility.Collapsed;
            if (!patientsTableIsFilled)
            {
                FillPatientsTable();
            }
            appointmentIndex = rowIndex;
            Appointment appointment = AppointmentRepository.Appointments[rowIndex];
            FillAppointmentWithDefaultValues(appointment);
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int rowIndex;
            try
            {
                rowIndex = scheduleDataGrid.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("Select an appointment");
                return;
            }
            AppointmentRepository.Appointments.RemoveAt(rowIndex);
            FillAppointmentsTable(AppointmentRepository.Appointments);
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string day, month, year;
            if(dayChoiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a day");
                return;
            }
            if (monthChoiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a month");
                return;
            }
            if (yearChoiceComboBox.SelectedItem == null)
            {
                MessageBox.Show("select a year");
                return;
            }

            day = dayChoiceComboBox.SelectedItem.ToString();
            month = monthChoiceComboBox.SelectedItem.ToString();
            year = yearChoiceComboBox.SelectedItem.ToString();

            string unparsedDate = day + "/" + month + "/" + year;
            DateTime date = DateTime.ParseExact(unparsedDate,
                    Constants.DateFormat,
                    CultureInfo.InvariantCulture);
            List<Appointment> appointmentsResult = new List<Appointment>();
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                TimeSpan timeSpan = appointment.ScheduledDate.Subtract(date);
                if (timeSpan.TotalDays <= 3 && timeSpan.TotalDays >= 0)
                {
                    appointmentsResult.Add(appointment);
                }
            }
            FillAppointmentsTable(appointmentsResult);
        }
        private void HealthRecord_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)scheduleDataGrid.SelectedItems[0];
                appointmentIndex = scheduleDataGrid.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("Select an appointment");
                return;
            }
            int id = int.Parse(row["Patient Id"].ToString());
            healthRecordGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Visible;

            int counter = 0;

            foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
            {
                if (healthRecord.PatientID == id)
                {
                    healthRecordIndex = counter;
                    FillHealthRecordData(healthRecord);
                    break;
                }
                counter++;
            }
        }
        private void StartAppointment_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)scheduleDataGrid.SelectedItems[0];
            }
            catch 
            {
                MessageBox.Show("Select an appointment");
                return;
            }
            int patientID = int.Parse(row["Patient ID"].ToString());
            appointmentIndex = scheduleDataGrid.SelectedIndex;  
            healthRecordGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            updateHealthRecord.Visibility = Visibility.Visible;
            createAnamnesis.Visibility = Visibility.Visible;
            foreach (HealthRecord healthRecord in HealthRecordRepository.Records)
            {
                if (healthRecord.PatientID == patientID)
                {
                    FillHealthRecordData(healthRecord);
                    break;
                }
            }
        }

        //---------------------------------------------------------------------------------------
        //Buttons on appointment creation menu
        private void SubmitAppointment_Click(object sender, RoutedEventArgs e)
        {

            Appointment appointment = new Appointment();
            try
            {
                FillApointmentWithData(appointment,true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        
        //---------------------------------------------------------------------------------------
        //Buttons on appointment altering menu
        private void AlterAppointment_Click(object sender, RoutedEventArgs e)
        {
            Appointment appointment = AppointmentRepository.Appointments[appointmentIndex];
            try
            {
                FillApointmentWithData(appointment,false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
                return;
            }
            FillAppointmentsTable(AppointmentRepository.Appointments);
        }

        //---------------------------------------------------------------------------------------
        //Health record menu buttons
        private void CreateAnamnesis_Click(object sender, RoutedEventArgs e)
        {
            healthRecordGrid.Visibility = Visibility.Collapsed;
            anamnesisGrid.Visibility = Visibility.Visible;
        }
        private void UpdateHealthRecord_Click(object sender, RoutedEventArgs e)
        {
            healthRecordGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
            updateHealthRecord.Visibility = Visibility.Collapsed;
            createAnamnesis.Visibility = Visibility.Collapsed;
            HealthRecordRepository.Records[healthRecordIndex].Height = double.Parse(heightTextBox.Text);
            HealthRecordRepository.Records[healthRecordIndex].Weight = double.Parse(weigthTextBox.Text);
            HealthRecordRepository.Records[healthRecordIndex].PreviousDiseases.Clear();
            string[] previousDiseases = previousDiseasesTextBox.Text.Split(",");
            foreach(string disease in previousDiseases)
            {
                if (string.IsNullOrWhiteSpace(disease))
                {
                    continue;
                }

                HealthRecordRepository.Records[healthRecordIndex].PreviousDiseases.Add(disease);
            }
            HealthRecordRepository.Records[healthRecordIndex].Allergens.Clear();
            string[] allergens = alergensTextBox.Text.Split(",");
            foreach (string allergen in allergens)
            {
                if (string.IsNullOrWhiteSpace(allergen))
                {
                    continue;
                }

                HealthRecordRepository.Records[healthRecordIndex].Allergens.Add(allergen);
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            scheduleGrid.Visibility = Visibility.Visible;
            healthRecordGrid.Visibility = Visibility.Collapsed;
            backButton.Visibility = Visibility.Collapsed;
        }

        //---------------------------------------------------------------------------------------
        //Anamnesis creation buttons
        private void SubmitAnamnesis_Click(object sender, RoutedEventArgs e)
        {
            string anamnesisComment = anamnesisTextBox.Text;
            Anamnesis anamnesis = new Anamnesis();
            anamnesis.Comment = anamnesisComment;
            anamnesis.ID = appointmentIndex;
            AppointmentRepository.Appointments[appointmentIndex].PatientAnamnesis = anamnesis;
            anamnesisGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;
            anamnesisLabel.Content = anamnesisComment;
        }

        //---------------------------------------------------------------------------------------
        //Window closing
        private void WindowClosing(object sender, CancelEventArgs e)
        {
            HealthRecordRepository.Save();
            AppointmentRepository.Save();
            LogOut();
        }

        private void LogOut()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }

    }
}
