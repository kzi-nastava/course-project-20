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

        private void ShowWindow(Window window)
        {
            window.Show();
            Close();
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

        private bool IsDateInputValide(string date)
        {
            return DateTime.TryParse(date, out DateTime _);
        }

        private bool IsDateInputBeforeCurrentTime(DateTime date)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(date, now);
            return value < 0;
        }

        private bool IsEndDateBeforeStartDate(DateTime startDate, DateTime endDate)
        {
            int value = DateTime.Compare(endDate, startDate);
            return value < 0;
        }

        public HospitalRoomRenovationWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillDataGridHospitalRooms();
            FillDataGridHospitalRoomsRenovation();
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

        private void ScheduleRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            string hospitalRoomId = HospitalRoomIdTextBox.Text;

            if (!IsHospitalRoomIdInputValide(hospitalRoomId))
            {
                MessageBox.Show("Error, bad hospital room ID input!");
                return;
            }

            int parsedHospitalRoomId = Convert.ToInt32(hospitalRoomId);

            HospitalRoom roomForRenovation = HospitalRoomService.GetRoom(parsedHospitalRoomId);

            if (!IsHospitalRoomFound(roomForRenovation))
            {
                MessageBox.Show($"Error, hospital room with id={parsedHospitalRoomId} not found!");
                return;
            }

            string startDate = StartDatePicker.Text;
            if (!IsDateInputValide(startDate))
            {
                MessageBox.Show("Error, bad input for start date!");
                return;
            }
            DateTime parsedStartDate = Convert.ToDateTime(startDate);

            string endDate = EndDatePicker.Text;
            if (!IsDateInputValide(endDate))
            {
                MessageBox.Show("Error, bad input for end date!");
                return;
            }
            DateTime parsedEndDate = Convert.ToDateTime(endDate);

            if (IsDateInputBeforeCurrentTime(parsedStartDate))
            {
                MessageBox.Show("Error, bad input for start date!");
                return;
            }

            if (IsDateInputBeforeCurrentTime(parsedEndDate))
            {
                MessageBox.Show("Error, bad input for end date!");
                return;
            }

            if (IsEndDateBeforeStartDate(parsedStartDate, parsedEndDate))
            {
                MessageBox.Show("Error, end date is before start date!");
                return;
            }

            if (roomForRenovation.ContainsAnyAppointment())
            {
                MessageBox.Show("Error, hospital room contains appointments!");
                return;
            }

            HospitalRoomService.DeleteRoom(roomForRenovation);
            HospitalRoomForRenovationService.AddRoom(roomForRenovation);
            RenovationSchedule renovation = new RenovationSchedule(parsedStartDate, parsedEndDate, roomForRenovation);
            RenovationScheduleService.AddRenovation(renovation);
            FillDataGridHospitalRoomsRenovation();
            FillDataGridHospitalRooms();
        }
    }
}