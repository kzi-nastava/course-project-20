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

namespace HealthCareCenter
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            try
            {
                UserManager.LoadUsers();
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
            foreach (User user in UserManager.Users)
            {
                if (user.Username == usernameTextBox.Text)
                {
                    foundUser = true;
                    if (user.Password == passwordBox.Password)
                    {
                        if (user.GetType() == typeof(Doctor))
                        {
                            ShowWindow(new DoctorWindow());
                        } 
                        else if (user.GetType() == typeof(Manager))
                        {
                            ShowWindow(new ManagerWindow());
                        }
                        else if (user.GetType() == typeof(Patient))
                        {
                            ShowWindow(new PatientWindow());
                        }
                        else if (user.GetType() == typeof(Secretary))
                        {
                            ShowWindow(new SecretaryWindow(user));
                        }
                    } else {
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

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(null, null);
            }
        }
    }
}
