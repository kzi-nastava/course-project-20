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
        DataRow dr;
        public DoctorWindow(Model.User user)
        {
            signedUser = (Doctor) user;
            createAppointmentTable();
            createPatientTable();
            InitializeComponent();
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
            appointmentsDataTable.Columns.Add(dc1);
            appointmentsDataTable.Columns.Add(dc2);
            appointmentsDataTable.Columns.Add(dc3);
            appointmentsDataTable.Columns.Add(dc4);
            appointmentsDataTable.Columns.Add(dc5);
            appointmentsDataTable.Columns.Add(dc6);
            appointmentsDataTable.Columns.Add(dc7);
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

        private void loadAppointmentTable()
        {
            appointmentsDataTable.Rows.Clear();
            if (AppointmentsMenager.Appointments == null)
                AppointmentsMenager.loadAppointments();
            foreach (Appointment appointment in AppointmentsMenager.Appointments)
            {
                dr = appointmentsDataTable.NewRow();
                dr[0] = appointment.ID;
                dr[1] = appointment.Type;
                dr[2] = appointment.AppointmentDate;
                dr[3] = appointment.CreatedDate;
                dr[4] = appointment.Emergency;
                dr[5] = appointment.DoctorID;
                dr[6] = appointment.HospitalRoomID;
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
        private void scheduleRewiewButton_Click(object sender, RoutedEventArgs e)
        {
            startingGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
            if (appointmentsTableIsFilled)
            {
                return;
            }
            loadAppointmentTable();
        }

        private void drugMenagmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            appointmentCreationGrid.Visibility = Visibility.Visible;
            scheduleGrid.Visibility = Visibility.Collapsed;
            if (patientsTableIsFilled)
            {
                return;
            }
            loadPatientTable();

        }

        private void submitAppointment_Click(object sender, RoutedEventArgs e)
        {
            Appointment appointment = new Appointment();
            DataRowView row = (DataRowView)patientsDataGrid.SelectedItems[0];
            int id = (int)row["Id"];
            string selectedValue = ((ComboBoxItem)appointmentComboBox.SelectedItem).Content.ToString();
            bool emergency = (bool)emergencyCheckBox.IsChecked;
            DateTime date = dateCalendar.DisplayDate;
            DateTime current_date = DateTime.Today;
            appointment.ID = AppointmentsMenager.Appointments.Count() + 1;
            appointment.Type = (AppointmentType)Enum.Parse(typeof(AppointmentType), selectedValue);
            appointment.Emergency = emergency;
            appointment.DoctorID = signedUser.ID;
            appointment.HealthRecordID = id;
            appointment.HospitalRoomID = 1;
            appointment.AppointmentDate = date;
            appointment.CreatedDate = current_date;
            AppointmentsMenager.Appointments.Add(appointment);
            loadAppointmentTable();
            appointmentCreationGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
        }
    }
}
