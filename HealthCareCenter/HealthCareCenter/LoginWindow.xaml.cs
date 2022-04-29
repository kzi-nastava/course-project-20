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

namespace HealthCareCenter
{
    public partial class LoginWindow : Window
    {
        private void DoEquipmentRearrangements()
        {
            List<Equipment> equipments = EquipmentRepository.GetEquipments();
            for (int i = 0; i < equipments.Count; i++)
            {
                equipments[i].DoRearrangement();
            }
        }

        public LoginWindow()
        {
            InitializeComponent();
            DoEquipmentRearrangements();

            try
            {
                UserRepository.LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool foundUser = false;
            foreach (User user in UserRepository.Users)
            {
                if (user.Username == usernameTextBox.Text)
                {
                    foundUser = true;
                    if (user.Password == passwordTextBox.Password)
                    {
                        if (user.GetType() == typeof(Doctor))
                        {
                            ShowWindow(new DoctorWindow(user));
                        }
                        else if (user.GetType() == typeof(Manager))
                        {
                            ShowWindow(new CrudHospitalRoomWindow(user));
                        }
                        else if (user.GetType() == typeof(Patient))
                        {
                            Patient patient = (Patient)user;
                            if (patient.IsBlocked)
                            {
                                MessageBox.Show("This user is blocked");
                                usernameTextBox.Clear();
                                passwordTextBox.Clear();
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
                        passwordTextBox.Clear();

                        MessageBox.Show("Invalid password.");
                    }
                }
            }
            if (!foundUser)
            {
                MessageBox.Show("Invalid username.");
            }
        }
    }
}