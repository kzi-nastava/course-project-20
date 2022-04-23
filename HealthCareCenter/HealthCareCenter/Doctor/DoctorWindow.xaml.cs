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

namespace HealthCareCenter
{
    public partial class DoctorWindow : Window
    {
        private Doctor signedUser;
        private DataTable dataTable;
        private bool tableIsFilled = false;
        private List<Appointment> Appointments;
        public DoctorWindow(Model.User user)
        {
            signedUser = (Doctor) user;
            InitializeComponent();
        }

        private void scheduleRewiewButton_Click(object sender, RoutedEventArgs e)
        {
            startingGrid.Visibility = Visibility.Collapsed;
            scheduleGrid.Visibility = Visibility.Visible;
            if (tableIsFilled)
            {
                return;
            }
            dataTable = new DataTable("emp");

            DataColumn dc1 = new DataColumn("Id", typeof(int));
            DataColumn dc2 = new DataColumn("Type of appointment", typeof(string));
            DataColumn dc3 = new DataColumn("Appointment date", typeof(string));
            DataColumn dc4 = new DataColumn("Creation date", typeof(string));
            DataColumn dc5 = new DataColumn("Emergency", typeof(bool));
            DataColumn dc6 = new DataColumn("Doctors first and last name", typeof(string));
            DataColumn dc7 = new DataColumn("Room", typeof(string));
            dataTable.Columns.Add(dc1);
            dataTable.Columns.Add(dc2);
            dataTable.Columns.Add(dc3);
            dataTable.Columns.Add(dc4);
            dataTable.Columns.Add(dc5);
            dataTable.Columns.Add(dc6);
            dataTable.Columns.Add(dc7);
            AppointmentsMenager appointmentsMenager = new AppointmentsMenager();
            List<Appointment> appointments = appointmentsMenager.loadAppointments();
            DataRow dr;
            foreach(Appointment appointment in appointments)
            {
                dr = dataTable.NewRow();
                dr[0] = appointment.ID;
                dr[1] = appointment.Type;
                dr[2] = appointment.AppointmentDate;
                dr[3] = appointment.CreatedDate;
                dr[4] = appointment.Emergency;
                dr[5] = appointment.DoctorID;
                dr[6] = appointment.HospitalRoomID;
                dataTable.Rows.Add(dr);
            }
            scheduleDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void drugMenagmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
