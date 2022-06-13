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
using HealthCareCenter.Controller;

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for HospitalRoomRenovationWindow.xaml
    /// </summary>
    public partial class HospitalRoomRenovationWindow : Window
    {
        private Manager _signedManager;
        private HospitalRoomRenovaitonController _controller = new HospitalRoomRenovaitonController();

        private void FillDataGridHospitalRooms()
        {
            DataGridHospitalRooms.Items.Clear();
            List<HospitalRoom> rooms = _controller.GetRoomsForDisplay();
            foreach (HospitalRoom room in rooms)
            {
                DataGridHospitalRooms.Items.Add(room);
            }
        }

        private void FillDataGridHospitalRoomsRenovation()
        {
            DataGridHospitalRoomsRenovation.Items.Clear();
            List<RenovationSchedule> renovations = _controller.GetRenovationsForDisplay();
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
        }

        private void ScheduleRenovationButton_Click(object sender, RoutedEventArgs e)
        {
            string hospitalRoomForRenovationId = HospitalRoomIdTextBox.Text;
            string startDate = StartDatePicker.Text;
            string finishDate = EndDatePicker.Text;
            _controller.ScheduleRenovation(hospitalRoomForRenovationId, startDate, finishDate);
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
            ShowWindow(new CrudHospitalRoomWindow(_signedManager, new NotificationService(new NotificationRepository())));
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

        private void CreateMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new MedicineCreationRequestWindow(_signedManager));
        }

        private void ReffusedMedicineClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new ChangedMedicineCreationRequestWindow(_signedManager));
        }

        private void HealthcareSurveysClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new HealthcareSurveysOverviewWindow(_signedManager));
        }

        private void DoctorSurveysClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new DoctorSurveysOverviewWindow(_signedManager));
        }

        private void LogOffItemClick(object sender, RoutedEventArgs e)
        {
            ShowWindow(new LoginWindow());
        }
    }
}