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
        private bool appointmentsTableIsFilled = false;
        private bool patientsTableIsFilled = false;
        private List<Appointment> Appointments;
        private int appointmentIndex;
        DataRow dr;
        public DoctorWindow(Model.User user)
        {
            signedUser = (Doctor)user;
            createAppointmentTable();
            createPatientTable();
            HealthRecordsMenager.loadHealthRecords();
            InitializeComponent();
            fillDateComboBox();
        }

        private void createAppointmentTable()
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
        private void createPatientTable()
        {
            patientsDataTable = new DataTable("Patients");
            DataColumn dc1 = new DataColumn("Id", typeof(int));
            DataColumn dc2 = new DataColumn("First name", typeof(string));
            DataColumn dc3 = new DataColumn("Last name", typeof(string));
            patientsDataTable.Columns.Add(dc1);
            patientsDataTable.Columns.Add(dc2);
            patientsDataTable.Columns.Add(dc3);
        }
        private void fillDateComboBox()
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
        private void loadAppointmentTable(List<Appointment> appointments)
        {
            appointmentsDataTable.Rows.Clear();
            if (AppointmentsMenager.Appointments == null)
                appointments = AppointmentsMenager.loadAppointments();

            foreach (Appointment appointment in appointments)
            {
                DateTime now = DateTime.Now;

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
            appointmentsTableIsFilled = true;
            scheduleDataGrid.ItemsSource = appointmentsDataTable.DefaultView;
        }
        private void loadPatientTable()
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
        private void fillApointmentWithData(Appointment appointment)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)patientsDataGrid.SelectedItems[0];
            }
            catch (Exception ex)
            {
                throw;
            }
            int id = (int)row["Id"];
            string selectedValue;
            try
            {
                selectedValue = ((ComboBoxItem)appointmentComboBox.SelectedItem).Content.ToString();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            string dateStr = day + "/" + month + "/" + year + " " + hour + ":" + minute;
            DateTime date = DateTime.ParseExact(dateStr,
                    Constants.DateTimeFormat,
                    CultureInfo.InvariantCulture);
            foreach (Appointment appointments in AppointmentsMenager.Appointments)
            {
                TimeSpan t = appointments.AppointmentDate.Subtract(date);
                if (Math.Abs(t.TotalMinutes) < 15)
                {
                    throw new ArgumentException("Termin je zauzet");
                }
            }
            DateTime current_date = DateTime.Today;
            appointment.ID = AppointmentsMenager.HighestIndex + 1;
            appointment.Type = (AppointmentType)Enum.Parse(typeof(AppointmentType), selectedValue);
            appointment.Emergency = emergency;
            appointment.DoctorID = signedUser.ID;
            appointment.HealthRecordID = id;
            appointment.HospitalRoomID = 1;
            appointment.AppointmentDate = date;
            appointment.CreatedDate = current_date;
            AppointmentsMenager.Appointments.Add(appointment);
            loadAppointmentTable(AppointmentsMenager.Appointments);
            appointmentCreationGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
        }
        private void fillAppointmentWithDefaultValues(Appointment appointment)
        {
            string[] timeFragments = appointment.AppointmentDate.ToString().Split("/");
            dayComboBox.SelectedItem = timeFragments[0];
            string[] yearAndTime = timeFragments[2].Split(" ");
            string[] time = yearAndTime[1].Split(":");
            dayComboBox.SelectedIndex = int.Parse(timeFragments[1]) - 1;
            monthComboBox.SelectedIndex = int.Parse(timeFragments[0]) - 1;
            yearComboBox.SelectedIndex = int.Parse(yearAndTime[0]) - 2022;
            hourComboBox.SelectedIndex = int.Parse(time[0]) - 9;
            minuteComboBox.SelectedIndex = int.Parse(time[1]) / 15;
            emergencyCheckBox.IsChecked = appointment.Emergency;
            if (appointment.Type == AppointmentType.Checkup)
                appointmentComboBox.SelectedIndex = 0;
            else
                appointmentComboBox.SelectedIndex = 1;
            int patientIndex = 0;
            foreach (Patient patient in UserManager.Patients)
            {
                patientIndex++;
                if (patient.ID == appointment.HealthRecordID)
                    break;
            }
            patientsDataGrid.SelectedIndex = patientIndex;

        }
        private void fillHealthRecordData(HealthRecord healthRecord)
        {
            idInfoLable.Content = healthRecord.ID.ToString();
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
        }
        private void scheduleRewiewButton_Click(object sender, RoutedEventArgs e)
        {
            startingGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
            if (appointmentsTableIsFilled)
            {
                return;
            }
            loadAppointmentTable(AppointmentsMenager.Appointments);
        }
        private void add_Click(object sender, RoutedEventArgs e)
        {
            appointmentCreationGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            alterAppointment.Visibility = Visibility.Collapsed;
            sumbitAppointment.Visibility = Visibility.Visible;
            if (patientsTableIsFilled)
            {
                return;
            }
            loadPatientTable();

        }
        private void alter_Click(object sender, RoutedEventArgs e)
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
                loadPatientTable();
            }
            appointmentIndex = rowIndex;
            Appointment appointment = AppointmentsMenager.Appointments[rowIndex];
            fillAppointmentWithDefaultValues(appointment);
        }
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row;
            int rowIndex;
            try
            {
                rowIndex = scheduleDataGrid.SelectedIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            AppointmentsMenager.Appointments.RemoveAt(rowIndex);
            loadAppointmentTable(AppointmentsMenager.Appointments);
        }
        private void search_Click(object sender, RoutedEventArgs e)
        {
            string day, month, year, hour, minute;
            try
            {
                day = dayChoiceComboBox.SelectedItem.ToString();
                month = monthChoiceComboBox.SelectedItem.ToString();
                year = yearChoiceComboBox.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            string dateStr = day + "/" + month + "/" + year;
            DateTime date = DateTime.ParseExact(dateStr,
                    Constants.DateFormat,
                    CultureInfo.InvariantCulture);
            List<Appointment> appointmentsResult = new List<Appointment>();
            foreach (Appointment appointment in AppointmentsMenager.Appointments)
            {
                TimeSpan t = appointment.AppointmentDate.Subtract(date);
                if (t.TotalDays <= 3 && t.TotalDays >= 0)
                {
                    appointmentsResult.Add(appointment);
                }
            }
            loadAppointmentTable(appointmentsResult);
        }
        private void submitAppointment_Click(object sender, RoutedEventArgs e)
        {

            Appointment appointment = new Appointment();
            try
            {
                fillApointmentWithData(appointment);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void alterAppointment_Click(object sender, RoutedEventArgs e)
        {
            Appointment appointment = AppointmentsMenager.Appointments[appointmentIndex];
            try
            {
                fillApointmentWithData(appointment);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            AppointmentsMenager.Appointments.RemoveAt(appointmentIndex);
            loadAppointmentTable(AppointmentsMenager.Appointments);
        }
        private void drugMenagmentButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void healthRecord_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)scheduleDataGrid.SelectedItems[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select an appointment");
                return;
            }
            int id = (int)row["Id"];
            healthRecordGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;

            foreach (HealthRecord healthRecord in HealthRecordsMenager.HealthRecords)
            {
                if (healthRecord.ID == id)
                {
                    fillHealthRecordData(healthRecord);
                    break;
                }
            }
        }

        private void startAppointment_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)scheduleDataGrid.SelectedItems[0];
            }
            catch (Exception ex)
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
                    fillHealthRecordData(healthRecord);
                    break;
                }
            }
        }
        private void createAnamnesia_Click(object sender, RoutedEventArgs e)
        {

        }
        private void updateHealthRecord_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
