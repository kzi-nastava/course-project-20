using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Interaction logic for PatientCreateWindow.xaml
    /// </summary>
    public partial class PatientCreateWindow : Window
    {
        public PatientCreateWindow()
        {
            InitializeComponent();
        }
        private void Reset()
        {
            firstNameTextBox.Clear();
            usernameTextBox.Clear();
            lastNameTextBox.Clear();
            passwordTextBox.Clear();
            heightTextBox.Clear();
            weightTextBox.Clear();
            previousDiseaseTextBox.Clear();
            allergenTextBox.Clear();
            previousDiseasesListBox.Items.Clear();
            allergensListBox.Items.Clear();
            idTextBox.Text = (UserService.maxID + 1).ToString();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void AddPreviousDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(previousDiseaseTextBox.Text))
            {
                MessageBox.Show("You must enter a disease.");
                return;
            }
            previousDiseasesListBox.Items.Add(previousDiseaseTextBox.Text);
        }

        private void AddAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(allergenTextBox.Text))
            {
                MessageBox.Show("You must enter an allergen.");
                return;
            }
            allergensListBox.Items.Add(allergenTextBox.Text);
        }

        private void DeletePreviousDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (previousDiseasesListBox.SelectedItem != null) 
            {
                previousDiseasesListBox.Items.Remove(previousDiseasesListBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("You need to select a disease from the list first.");
            }
        }

        private void DeleteAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (allergensListBox.SelectedItem != null)
            {
                allergensListBox.Items.Remove(allergensListBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("You need to select an allergen from the list first.");
            }
        }

        private void EditPreviousDiseaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(previousDiseaseTextBox.Text))
            {
                MessageBox.Show("You must enter a disease.");
                return;
            }
            if (previousDiseasesListBox.SelectedItem != null)
            {
                previousDiseasesListBox.Items[previousDiseasesListBox.SelectedIndex] = previousDiseaseTextBox.Text;
            }
            else
            {
                MessageBox.Show("You need to select a disease from the list first.");
            }
        }

        private void EditAllergenButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(allergenTextBox.Text))
            {
                MessageBox.Show("You must enter an allergen.");
                return;
            }
            if (allergensListBox.SelectedItem != null)
            {
                allergensListBox.Items[allergensListBox.SelectedIndex] = allergenTextBox.Text;
            }
            else
            {
                MessageBox.Show("You need to select an allergen from the list first.");
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            idTextBox.Text = (UserService.maxID + 1).ToString();
        }

        private void PreviousDiseasesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (previousDiseasesListBox.SelectedItem != null)
            {
                previousDiseaseTextBox.Text = previousDiseasesListBox.SelectedValue.ToString();
            }
            else
            {
                previousDiseaseTextBox.Clear();
            }
        }

        private void AllergensListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (allergensListBox.SelectedItem != null)
            {
                allergenTextBox.Text = allergensListBox.SelectedValue.ToString();
            }
            else
            {
                allergenTextBox.Clear();
            }
        }

        private bool EnteredData()
        {
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                MessageBox.Show("You must enter a first name.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text))
            {
                MessageBox.Show("You must enter a username.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                MessageBox.Show("You must enter a last name.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                MessageBox.Show("You must enter a password.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(heightTextBox.Text))
            {
                MessageBox.Show("You must enter a height.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(weightTextBox.Text))
            {
                MessageBox.Show("You must enter a weight.");
                return false;
            }
            if (birthDatePicker.SelectedDate == null)
            {
                MessageBox.Show("You must enter a date of birth.");
                return false;
            }
            return true;
        }

        private bool UsernameInUse()
        {
            foreach (User user in UserRepository.Users)
            {
                if (user.Username == usernameTextBox.Text)
                {
                    MessageBox.Show("Username is already in use. Choose a different one.");
                    return true;
                }
            }
            return false;
        }

        private bool BirthDateInFuture()
        {
            if (birthDatePicker.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("You cannot enter a date in the future.");
                return true;
            }
            return false;
        }

        private bool ValidHeight()
        {
            if (!Double.TryParse(heightTextBox.Text, out _))
            {
                MessageBox.Show("Height must be a number.");
                return false;
            }
            return true;
        }

        private bool ValidWeight()
        {
            if (!Double.TryParse(weightTextBox.Text, out _))
            {
                MessageBox.Show("Weight must be a number.");
                return false;
            }
            return true;
        }

        private bool ValidData()
        {
            if (BirthDateInFuture())
                return false;

            if (UsernameInUse())
                return false;

            if (!ValidHeight())
                return false;

            if (!ValidWeight())
                return false;

            return true;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EnteredData())
                return;

            if (!ValidData())
                return;

            double height = Double.Parse(heightTextBox.Text);
            double weight = Double.Parse(weightTextBox.Text);

            HealthRecordService.maxID++;
            UserService.maxID++;
            
            HealthRecord record = new HealthRecord(HealthRecordService.maxID, height, weight, previousDiseasesListBox.Items.Cast<String>().ToList(), allergensListBox.Items.Cast<String>().ToList(), UserService.maxID);
            Patient patient = new Patient(UserService.maxID, usernameTextBox.Text, passwordTextBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, (DateTime)birthDatePicker.SelectedDate, false, Enums.Blocker.None, new List<int>(), HealthRecordService.maxID);

            AddToRepositories(record, patient);
            SaveRepositories();

            Reset();
            MessageBox.Show("Successfully created the patient and health record.");
        }

        private static void SaveRepositories()
        {
            HealthRecordRepository.Save();
            UserRepository.SavePatients();
        }

        private static void AddToRepositories(HealthRecord record, Patient patient)
        {
            HealthRecordRepository.Records.Add(record);
            UserRepository.Patients.Add(patient);
            UserRepository.Users.Add(patient);
        }
    }
}
