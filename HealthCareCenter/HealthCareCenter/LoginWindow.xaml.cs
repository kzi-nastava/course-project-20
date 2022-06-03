﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using HealthCareCenter.DoctorServices;
using HealthCareCenter.DoctorGUI;
using HealthCareCenter.Service;

namespace HealthCareCenter
{
    public partial class LoginWindow : Window
    {
        private static BackgroundWorker _backgroundWorker = null;

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
            DynamicEquipmentRequestRepository.Load();
            StartBackgroundWorkerIfNeeded();
        }

        private void StartBackgroundWorkerIfNeeded()
        {
            if (_backgroundWorker == null)
            {
                _backgroundWorker = new BackgroundWorker();
                _backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
                _backgroundWorker.RunWorkerAsync(30 * Constants.Minute);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int timeBetweenWork = (int)e.Argument;
            BackgroundWork(timeBetweenWork);
        }

        private void BackgroundWork(int timeBetweenWork)
        {
            while (true)
            {
                DynamicEquipmentRequestService.FulfillRequestsIfNeeded();
                Thread.Sleep(timeBetweenWork);
            }
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
                            DoctorWindowService doctorWindowService = new DoctorWindowService((Doctor)user);
                            Close();
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