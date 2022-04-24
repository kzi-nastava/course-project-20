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

namespace HealthCareCenter
{
    public partial class PatientWindow : Window
    {
        private Patient signedUser;
        private int activeWindow = 0;  // should be enum, add enum later, should show which grids should be collapsed and which should be visible

        // appointment datagrid
        private DataTable appointmentsDataTable;
        private bool isAppointmentDataTableFilled = false;

        public PatientWindow(User user)
        {
            signedUser = (Patient)user;
            PatientDataManager.Load(signedUser);
            InitializeComponent();

            // creating the appointment table
            CreateAppointmentTable();
            FillAppointmentTable();
        }

        // creation of appointment tables
        //==============================================================================
        private void CreateAppointmentTable()
        {
            currentActionTextBlock.Text = "My appointments";

            appointmentsDataTable = new DataTable("Appointments");
            appointmentsDataTable.Columns.Add(new DataColumn("Id", typeof(int)));
            appointmentsDataTable.Columns.Add(new DataColumn("Type of appointment", typeof(string)));
            appointmentsDataTable.Columns.Add(new DataColumn("Appointment date", typeof(string)));
            appointmentsDataTable.Columns.Add(new DataColumn("Creation date", typeof(string)));
            appointmentsDataTable.Columns.Add(new DataColumn("Emergency", typeof(bool)));
            appointmentsDataTable.Columns.Add(new DataColumn("Doctor's full name", typeof(string)));
            appointmentsDataTable.Columns.Add(new DataColumn("Room", typeof(string)));

        }

        private void FillAppointmentTable()
        {
            DataRow row;
            foreach (Appointment appointment in PatientDataManager.Appointments)
            {
                row = appointmentsDataTable.NewRow();
                row[0] = appointment.ID;
                row[1] = appointment.Type;
                row[2] = appointment.AppointmentDate;
                row[3] = appointment.CreatedDate;
                row[4] = appointment.Emergency;
                row[5] = PatientDataManager.GetDoctorFullName(appointment);
                row[6] = appointment.HospitalRoomID;  // replace with hospital room type
                appointmentsDataTable.Rows.Add(row);
            }
            myAppointmentsDataGrid.ItemsSource = appointmentsDataTable.DefaultView;
            isAppointmentDataTableFilled = true;
        }
        //==============================================================================

        // action menu click methods
        //=======================================================================================
        private void myAppointmentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // changing the visibility based on activeWindow attribute comes here
            currentActionTextBlock.Text = "My appointments";
            if (isAppointmentDataTableFilled)
            {
                return;
            }
        }

        private void searchDoctorsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            currentActionTextBlock.Text = "Search for doctor";
        }

        private void myNotificationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            currentActionTextBlock.Text = "My notifications";
        }

        private void myHealthRecordMenuItem_Click(object sender, RoutedEventArgs e)
        {
            currentActionTextBlock.Text = "My health record";
        }

        private void doctorSurveyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            currentActionTextBlock.Text = "Doctor survey";
        }

        private void healthCenterMenuItem_Click(object sender, RoutedEventArgs e)
        {
            currentActionTextBlock.Text = "Health center survey";
        }
        //=======================================================================================
    }
}
