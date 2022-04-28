using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HealthCareCenter.Model;
using System.Data;
using System.Data.SqlClient;
using HealthCareCenter.Enums;
using System.Globalization;

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
            HealthRecordsMenager.LoadHealthRecords();
            InitializeComponent();
            FillDateTimeComboBoxes();
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
            foreach (Patient patient in UserManager.Patients)
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
                    s = "0" + s;
                dayComboBox.Items.Add(s);
                dayChoiceComboBox.Items.Add(s);
            }
            for (int i = 1; i <= 12; i++)
            {
                string s = i.ToString();
                if (s.Length == 1)
                    s = "0" + s;
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
                    s = "0" + s;
                hourComboBox.Items.Add(s);
            }
            for (int i = 0; i <= 45; i += 15)
            {
                string s = i.ToString();
                if (s.Length == 1)
                    s = "0" + s;
                minuteComboBox.Items.Add(s);
            }
        }
        private void FillAppointmentsTable(List<Appointment> appointments)
        {
            appointmentsDataTable.Rows.Clear();
            if (AppointmentsMenager.Appointments == null)
                appointments = AppointmentsMenager.LoadAppointments();

            foreach (Appointment appointment in appointments)
            {
                dr = appointmentsDataTable.NewRow();
                dr[0] = appointment.ID;
                dr[1] = appointment.Type;
                dr[2] = appointment.AppointmentDate;
                dr[3] = appointment.CreatedDate;
                dr[4] = appointment.Emergency;
                dr[5] = appointment.DoctorID;
                dr[6] = appointment.HospitalRoomID;
                dr[7] = appointment.HealthRecordID;
                appointmentsDataTable.Rows.Add(dr);
            }
            scheduleDataGrid.ItemsSource = appointmentsDataTable.DefaultView;
        }
        private void FillApointmentWithData(Appointment appointment)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)patientsDataGrid.SelectedItems[0];
            }
            catch
            {
                throw;
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
            try
            {
                day = dayComboBox.SelectedItem.ToString();
                month = monthComboBox.SelectedItem.ToString();
                year = yearComboBox.SelectedItem.ToString();
                hour = hourComboBox.SelectedItem.ToString();
                minute = minuteComboBox.SelectedItem.ToString();
            }
            catch
            {
                MessageBox.Show("Please select date and time");
                return;
            }
            string unparsedDate = day + "/" + month + "/" + year + " " + hour + ":" + minute;
            DateTime date = DateTime.ParseExact(unparsedDate,
                    Constants.DateTimeFormat,
                    CultureInfo.InvariantCulture);
            foreach (Appointment appointments in AppointmentsMenager.Appointments)
            {
                TimeSpan timeSpan = appointments.AppointmentDate.Subtract(date);
                if (Math.Abs(timeSpan.TotalMinutes) < 15)
                {
                    throw new ArgumentException("Termin je zauzet");
                }
            }
            DateTime currentDate = DateTime.Today;
            appointment.ID = AppointmentsMenager.HighestIndex + 1;
            appointment.Type = (AppointmentType)Enum.Parse(typeof(AppointmentType), selectedValue);
            appointment.Emergency = emergency;
            appointment.DoctorID = signedUser.ID;
            appointment.HealthRecordID = id;
            appointment.HospitalRoomID = 1;
            appointment.AppointmentDate = date;
            appointment.CreatedDate = currentDate;
            AppointmentsMenager.Appointments.Add(appointment);
            FillAppointmentsTable(AppointmentsMenager.Appointments);
            appointmentCreationGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
        }
        private void FillAppointmentWithDefaultValues(Appointment appointment)
        {
            string[] timeFragments = appointment.AppointmentDate.ToString().Split("/");
            dayComboBox.SelectedItem = timeFragments[0];
            string[] yearAndTime = timeFragments[2].Split(" ");
            string[] time = yearAndTime[1].Split(":");
            dayComboBox.SelectedIndex = int.Parse(timeFragments[1]) - 1;
            monthComboBox.SelectedIndex = int.Parse(timeFragments[0]) - 1;
            yearComboBox.SelectedIndex = int.Parse(yearAndTime[0]) - 2022;
            int hour = int.Parse(time[0]);
            if (hour <= 12 && timeFragments[2] == "AM")
                hour -= 9;
            else
                hour += 4;
            hourComboBox.SelectedIndex = hour;                               
            minuteComboBox.SelectedIndex = int.Parse(time[1]) / 15;
            emergencyCheckBox.IsChecked = appointment.Emergency;
            if (appointment.Type == AppointmentType.Checkup)
                appointmentTypeComboBox.SelectedIndex = 0;
            else
                appointmentTypeComboBox.SelectedIndex = 1;
            int patientIndex = 0;
            foreach (Patient patient in UserManager.Patients)
            {
                patientIndex++;
                if (patient.ID == appointment.HealthRecordID)
                    break;
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

                previousDiseasesTextBox.Text = previousDiseases.Substring(1, previousDiseases.Length - 1);
            }
            else
                previousDiseasesTextBox.Text = "none";
            string alergens = "";
            if (healthRecord.Allergens != null)
            {
                foreach (string s in healthRecord.Allergens)
                {
                    alergens += "," + s;
                }
                alergensTextBox.Text = alergens.Substring(1, alergens.Length - 1);
            }
            else
            {
                alergensTextBox.Text = "none";
            }
            if(AppointmentsMenager.Appointments[appointmentIndex].PatientAnamnesis == null)
            {
                anamnesisLabel.Content = "No anamnesis";
            }
            else
            {
                anamnesisLabel.Content = AppointmentsMenager.Appointments[appointmentIndex].PatientAnamnesis.Comment;
            }
        }

        //---------------------------------------------------------------------------------------
        //First menu buttons
        private void ScheduleRewiewButton_Click(object sender, RoutedEventArgs e)
        {
            startingGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
            FillAppointmentsTable(AppointmentsMenager.Appointments);
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
            Appointment appointment = AppointmentsMenager.Appointments[rowIndex];
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
            AppointmentsMenager.Appointments.RemoveAt(rowIndex);
            FillAppointmentsTable(AppointmentsMenager.Appointments);
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string day, month, year;
            try
            {
                day = dayChoiceComboBox.SelectedItem.ToString();
                month = monthChoiceComboBox.SelectedItem.ToString();
                year = yearChoiceComboBox.SelectedItem.ToString();
            }
            catch 
            {
                MessageBox.Show("Select a date");
                return;
            }
            string unparsedDate = day + "/" + month + "/" + year;
            DateTime date = DateTime.ParseExact(unparsedDate,
                    Constants.DateFormat,
                    CultureInfo.InvariantCulture);
            List<Appointment> appointmentsResult = new List<Appointment>();
            foreach (Appointment appointment in AppointmentsMenager.Appointments)
            {
                TimeSpan timeSpan = appointment.AppointmentDate.Subtract(date);
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

            foreach (HealthRecord healthRecord in HealthRecordsMenager.HealthRecords)
            {          
                if (healthRecord.ID == id)
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
            int id = (int)row["Id"];
            healthRecordGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            updateHealthRecord.Visibility = Visibility.Visible;
            createAnamnesis.Visibility = Visibility.Visible;
            foreach (HealthRecord healthRecord in HealthRecordsMenager.HealthRecords)
            {
                if (healthRecord.ID == id)
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
                FillApointmentWithData(appointment);
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
            Appointment appointment = AppointmentsMenager.Appointments[appointmentIndex];
            try
            {
                FillApointmentWithData(appointment);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            AppointmentsMenager.Appointments.RemoveAt(appointmentIndex);
            FillAppointmentsTable(AppointmentsMenager.Appointments);
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
            HealthRecordsMenager.HealthRecords[healthRecordIndex].Height = double.Parse(heightTextBox.Text);
            HealthRecordsMenager.HealthRecords[healthRecordIndex].Weight = double.Parse(weigthTextBox.Text);
            HealthRecordsMenager.HealthRecords[healthRecordIndex].PreviousDiseases.Clear();
            string[] previousDiseases = previousDiseasesTextBox.Text.Split(",");
            foreach(string disease in previousDiseases)
            {
                HealthRecordsMenager.HealthRecords[healthRecordIndex].PreviousDiseases.Add(disease);
            }
            HealthRecordsMenager.HealthRecords[healthRecordIndex].Allergens.Clear();
            string[] allergens = alergensTextBox.Text.Split(",");
            foreach (string allergen in allergens)
            {
                HealthRecordsMenager.HealthRecords[healthRecordIndex].Allergens.Add(allergen);
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
            AppointmentsMenager.Appointments[appointmentIndex].PatientAnamnesis = anamnesis;
            anamnesisGrid.Visibility = Visibility.Collapsed;
            healthRecordGrid.Visibility = Visibility.Visible;
            anamnesisLabel.Content = anamnesisComment;
        }

        //---------------------------------------------------------------------------------------

    }
}
