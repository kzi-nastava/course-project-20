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

namespace HealthCareCenter
{
    public partial class PatientWindow : Window
    {
        private Model.Patient signedUser;
        private int activeWindow = 0;  // should be enum, add enum later

        public PatientWindow(Model.User user)
        {
            signedUser = (Model.Patient)user;
            InitializeComponent();
            CreateAppointmentTable();
        }

        // action menu click methods
        //=======================================================================================

        private void CreateAppointmentTable()
        {
            currentActionTextBlock.Text = "My appointments";

        }

        private void myAppointmentsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CreateAppointmentTable();
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
