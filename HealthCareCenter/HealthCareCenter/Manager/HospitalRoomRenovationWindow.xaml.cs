using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for HospitalRoomRenovationWindow.xaml
    /// </summary>
    public partial class HospitalRoomRenovationWindow : Window
    {
        private Manager _signedManager;

        private bool IsHospitalRoomIdInputValide(string roomId)
        {
            return Int32.TryParse(roomId, out _);
        }

        private bool IsHospitalRoomFound(HospitalRoom room)
        {
            return room != null;
        }

        private bool IsDateInputValide(string date)
        {
            return DateTime.TryParse(date, out DateTime _);
        }

        private bool IsDateBeforeCurrentTime(DateTime date)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(date, now);
            return value < 0;
        }

        private bool IsFinishDateBeforeStartDate(DateTime startDate, DateTime finishDate)
        {
            int value = DateTime.Compare(finishDate, startDate);
            return value < 0;
        }

        private bool IsDateValide(string startDate, string finishDate)
        {
            if (!IsDateInputValide(startDate))
            {
                MessageBox.Show("Error, bad input for start date!");
                return false;
            }
            DateTime parsedStartDate = Convert.ToDateTime(startDate);

            if (!IsDateInputValide(finishDate))
            {
                MessageBox.Show("Error, bad input for end date!");
                return false;
            }
            DateTime parsedFinishDate = Convert.ToDateTime(finishDate);

            if (IsDateBeforeCurrentTime(parsedStartDate))
            {
                MessageBox.Show("Error, bad input for start date!");
                return false;
            }

            if (IsDateBeforeCurrentTime(parsedFinishDate))
            {
                MessageBox.Show("Error, bad input for end date!");
                return false;
            }

            if (IsFinishDateBeforeStartDate(parsedStartDate, parsedFinishDate))
            {
                MessageBox.Show("Error, end date is before start date!");
                return false;
            }

            return true;
        }

        private bool IsHospitalRoomValide(string roomId)
        {
            if (!IsHospitalRoomIdInputValide(roomId))
            {
                MessageBox.Show("Error, bad hospital room ID input!");
                return false;
            }

            int parsedHospitalRoomId = Convert.ToInt32(roomId);

            HospitalRoom roomForRenovation = HospitalRoomService.GetRoom(parsedHospitalRoomId);

            if (!IsHospitalRoomFound(roomForRenovation))
            {
                MessageBox.Show($"Error, hospital room with id={parsedHospitalRoomId} not found!");
                return false;
            }

            return true;
        }

        private bool IsPossibleRenovation(HospitalRoom roomForRenovation)
        {
            if (roomForRenovation.ContainsAnyAppointment())
            {
                MessageBox.Show("Error, hospital room contains appointments!");
                return false;
            }
            if (roomForRenovation.ContaninsAnyRearrangement())
            {
                MessageBox.Show("Error, hospital room contains rearrangements!");
                return false;
            }

            return true;
        }

        private void FillDataGridHospitalRooms()
        {
            DataGridHospitalRooms.Items.Clear();
            List<HospitalRoom> rooms = HospitalRoomService.GetRooms();
            foreach (HospitalRoom room in rooms)
            {
                DataGridHospitalRooms.Items.Add(room);
            }
        }

        private void FillDataGridHospitalRoomsRenovation()
        {
            DataGridHospitalRoomsRenovation.Items.Clear();
            List<RenovationSchedule> renovations = RenovationScheduleService.GetRenovations();
            foreach (RenovationSchedule renovation in renovations)
            {
                DataGridHospitalRoomsRenovation.Items.Add(renovation);
            }
        }

        public HospitalRoomRenovationWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillDataGridHospitalRooms();
            FillDataGridHospitalRoomsRenovation();
            
            DisplayNotifications();
        }
        
        private void DisplayNotifications()
        {
            List<Notification> notifications = NotificationService.FindUnopened(_signedManager);
            if (notifications.Count == 0)
            {
                return;
            }
            MessageBox.Show("You have new notifications.");
            foreach (Notification notification in notifications)
            {
                MessageBox.Show(notification.Message);
            }
        }

        private void ScheduleRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            string hospitalRoomForRenovationId = HospitalRoomIdTextBox.Text;
            string startDate = StartDatePicker.Text;
            string finishDate = EndDatePicker.Text;

            if (!IsHospitalRoomValide(hospitalRoomForRenovationId))
            {
                return;
            }
            int parsedHospitalRoomForRenovationId = Convert.ToInt32(hospitalRoomForRenovationId);
            HospitalRoom roomForRenovation = HospitalRoomService.GetRoom(parsedHospitalRoomForRenovationId);

            if (!IsDateValide(startDate, finishDate))
            {
                return;
            }
            DateTime parsedStartDate = Convert.ToDateTime(startDate);
            DateTime parsedFinishDate = Convert.ToDateTime(finishDate);

            if (!IsPossibleRenovation(roomForRenovation))
            {
                return;
            }
            RenovationSchedule renovation = new RenovationSchedule(parsedStartDate, parsedFinishDate, roomForRenovation);
            renovation.ScheduleSimpleRenovation(roomForRenovation);

            FillDataGridHospitalRoomsRenovation();
            FillDataGridHospitalRooms();
        }

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
        }

        private void CrudHospitalRoomMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new CrudHospitalRoomWindow(_signedManager));
        }

        private void EquipmentReviewMenuItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalEquipmentReviewWindow(_signedManager));
        }

        private void ArrangingEquipmentItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ArrangingEquipmentWindow(_signedManager));
        }

        private void SimpleRenovationItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HospitalRoomRenovationWindow(_signedManager));
        }

        private void ComplexRenovationMergeItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationMergeWindow(_signedManager));
        }

        private void ComplexRenovationSplitItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ComplexHospitalRoomRenovationSplitWindow(_signedManager));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}