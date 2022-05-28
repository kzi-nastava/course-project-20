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
using HealthCareCenter.SecretaryGUI;
using HealthCareCenter.Service;

namespace HealthCareCenter
{
    public partial class LoginWindow : Window
    {
        private void DoEquipmentRearrangements()
        {
            List<Equipment> equipments = EquipmentService.GetEquipments();
            for (int i = 0; i < equipments.Count; i++)
            {
                equipments[i].Rearrange();
            }
        }

        private void FinshPossibleRenovation()
        {
            List<RenovationSchedule> renovations = RenovationScheduleService.GetRenovations();
            for (int i = 0; i < renovations.Count; i++)
            {
                renovations[i].FinishRenovation();
            }
        }

        public LoginWindow()
        {
            InitializeComponent();
            DoEquipmentRearrangements();
            FinshPossibleRenovation();

            try
            {
                UserRepository.LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            NotificationRepository.Load();
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private bool Login(User user)
        {
            if (user.GetType() == typeof(Doctor))
            {
                ShowWindow(new DoctorWindow(user));
            }
            else if (user.GetType() == typeof(Manager))
            {
                ShowWindow(new CrudHospitalRoomWindow((Manager)user));
            }
            else if (user.GetType() == typeof(Patient))
            {
                Patient patient = (Patient)user;
                if (patient.IsBlocked)
                {
                    MessageBox.Show("This user is blocked");
                    usernameTextBox.Clear();
                    passwordBox.Clear();
                    return false;
                }

                PatientGUI.Stores.NavigationStore navStore = PatientGUI.Stores.NavigationStore.GetInstance();
                navStore.CurrentViewModel = new PatientGUI.ViewModels.MyAppointmentsViewModel(navStore);
                PatientGUI.MainWindow win = new PatientGUI.MainWindow()
                {
                    DataContext = new PatientGUI.ViewModels.MainViewModel(navStore)
                };
                ShowWindow(win);
            }
            else if (user.GetType() == typeof(Secretary))
            {
                ShowWindow(new SecretaryWindow(user));
            }
            return true;
        }

        private void TryLogin()
        {
            bool foundUser = false;
            foreach (User user in UserRepository.Users)
            {
                if (user.Username != usernameTextBox.Text)
                {
                    continue;
                }
                foundUser = true;
                if (user.Password == passwordBox.Password)
                {
                    if (!Login(user))
                        return;
                }
                else
                {
                    passwordBox.Clear();
                    MessageBox.Show("Invalid password.");
                }
            }
            if (!foundUser)
            {
                MessageBox.Show("Invalid username.");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TryLogin();
            }
        }
    }
}