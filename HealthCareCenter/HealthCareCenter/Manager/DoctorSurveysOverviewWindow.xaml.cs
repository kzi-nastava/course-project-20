using System;
using System.Collections.Generic;
using System.Dynamic;
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
using HealthCareCenter.Controller;
using HealthCareCenter.Model;
using HealthCareCenter.Service;

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for DoctorSurveysOverviewWindow.xaml
    /// </summary>
    public partial class DoctorSurveysOverviewWindow : Window
    {
        private Manager _signedManager;
        private string[] _doctorSurveysHeader = { "DoctorID", "PatientID", "Comment", "Rating" };
        private string[] _best3DoctorsHeader = { "DoctorID", "First Name", "Second Name", "Rating" };
        private string[] _worst3DoctorsHeader = { "DoctorID", "First Name", "Second Name", "Rating" };
        private DoctorSurveyOverviewController _controller = new DoctorSurveyOverviewController();

        public DoctorSurveysOverviewWindow(Manager manager)
        {
            _signedManager = manager;
            InitializeComponent();
            FillDataGridDoctorsSurveys();
            FillDataGridBest3Dctors();
            FillDataGridWorst3Doctors();
        }

        private void AddDataGridHeader(DataGrid dataGrid, string[] header)
        {
            foreach (string label in header)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = label;
                column.Binding = new Binding(label.Replace(' ', '_'));
                dataGrid.Columns.Add(column);
            }
        }

        private void AddDataGridRow(DataGrid dataGrid, string[] header, List<string> equipmentAttributesToDisplay)
        {
            dynamic row = new ExpandoObject();

            for (int i = 0; i < header.Length; i++)
            {
                ((IDictionary<String, Object>)row)[header[i].Replace(' ', '_')] = equipmentAttributesToDisplay[i];
            }

            dataGrid.Items.Add(row);
        }

        private void FillDataGridDoctorsSurveys()
        {
            AddDataGridHeader(DataGridSurveys, _doctorSurveysHeader);
            List<DoctorSurveyRating> doctorSurveys = _controller.GetDoctorSurveys();
            foreach (DoctorSurveyRating survey in doctorSurveys)
            {
                AddDataGridRow(DataGridSurveys, _doctorSurveysHeader, survey.ToList());
            }
        }

        private void FillDataGridBest3Dctors()
        {
            AddDataGridHeader(DataGridBest3Doctors, _best3DoctorsHeader);

            foreach (List<string> doctor in _controller.GetBest3Doctors())
            {
                AddDataGridRow(DataGridBest3Doctors, _best3DoctorsHeader, doctor);
            }
        }

        private void FillDataGridWorst3Doctors()
        {
            AddDataGridHeader(DataGridWorst3Doctors, _worst3DoctorsHeader);
            foreach (List<string> doctor in _controller.GetWorst3Doctors())
            {
                AddDataGridRow(DataGridWorst3Doctors, _worst3DoctorsHeader, doctor);
            }
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