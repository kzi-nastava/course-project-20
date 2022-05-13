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
            List<HospitalRoom> roomsForRenovation = HospitalRoomForRenovationService.GetRooms();
            for (int i = 0; i < roomsForRenovation.Count; i++)
            {
                roomsForRenovation[i].SetToBeAvailable();
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

        private void Login()
        {
            bool foundUser = false;
            foreach (User user in UserRepository.Users)
            {
                if (user.Username == usernameTextBox.Text)
                {
                    foundUser = true;
                    if (user.Password == passwordBox.Password)
                    {
                        if (user.GetType() == typeof(Doctor))
                        {
                            ShowWindow(new DoctorWindow(user));
                        }
                        else if (user.GetType() == typeof(Manager))
                        {
                            ShowWindow(new HospitalRoomRenovationWindow((Manager)user));
                        }
                        else if (user.GetType() == typeof(Patient))
                        {
                            Patient patient = (Patient)user;
                            if (patient.IsBlocked)
                            {
                                MessageBox.Show("This user is blocked");
                                usernameTextBox.Clear();
                                passwordBox.Clear();
                                return;
                            }
                            ShowWindow(new PatientWindow(user));
                        }
                        else if (user.GetType() == typeof(Secretary))
                        {
                            ShowWindow(new SecretaryWindow(user));
                        }
                    }
                    else
                    {
                        passwordBox.Clear();

                        MessageBox.Show("Invalid password.");
                    }
                }
            }
            if (!foundUser)
            {
                MessageBox.Show("Invalid username.");
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login();
            }
        }
    }
}