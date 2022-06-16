using HealthCareCenter.Core.VacationRequests.Models;
using HealthCareCenter.Core.VacationRequests.Repositories;
using HealthCareCenter.GUI.Doctor.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthCareCenter.GUI.Doctor.Views
{
    /// <summary>
    /// Interaction logic for RequestsPreviewWindow.xaml
    /// </summary>
    public partial class RequestsPreviewWindow : Window
    {
        private RequestsPreviewViewModel windowViewModel;
        private BaseVacationRequestRepository _vacationRequestRepository;
        private DataRow dr;
        public DataTable vacationRequestsDataTable;
        public RequestsPreviewWindow(RequestsPreviewViewModel requestsPreviewViewModel, VacationRequestRepository vacationRequestRepository)
        {
            _vacationRequestRepository = vacationRequestRepository;
            windowViewModel = requestsPreviewViewModel;
            InitializeComponent();
            CreateTheTable();
        }

        private void CreateTheTable()
        {
            vacationRequestsDataTable = new DataTable("Vacation requests");
            DataColumn dc1 = new DataColumn("Id", typeof(int));
            DataColumn dc2 = new DataColumn("Start Date", typeof(string));
            DataColumn dc3 = new DataColumn("End Date", typeof(string));
            DataColumn dc4 = new DataColumn("Emergency", typeof(string));
            vacationRequestsDataTable.Columns.Add(dc1);
            vacationRequestsDataTable.Columns.Add(dc2);
            vacationRequestsDataTable.Columns.Add(dc3);
            vacationRequestsDataTable.Columns.Add(dc4);

        }
        public void AddRequestToTable(VacationRequest request)
        {
            dr = vacationRequestsDataTable.NewRow();
            dr[0] = request.ID;
            dr[1] = request.StartDate;
            dr[2] = request.EndDate;
            dr[3] = request.Emergency;
            vacationRequestsDataTable.Rows.Add(dr);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
