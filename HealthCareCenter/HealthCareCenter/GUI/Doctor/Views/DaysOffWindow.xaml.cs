using HealthCareCenter.Core.VacationRequests.Repositories;
using HealthCareCenter.GUI.Doctor.ViewModels;
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

namespace HealthCareCenter.GUI.Doctor.Views
{
    /// <summary>
    /// Interaction logic for DaysOffWindow.xaml
    /// </summary>
    public partial class DaysOffWindow : Window
    {
        private DaysOffViewModel _windowViewModel;
        public DaysOffWindow(DaysOffViewModel ViewModel)
        {
            _windowViewModel = ViewModel;
            InitializeComponent();
        }

        private void allDaysOffRequests_Click(object sender, RoutedEventArgs e)
        {
            _windowViewModel.OpenRequestsPreview();
        }

        private void requestDaysOff_Click(object sender, RoutedEventArgs e)
        {
            _windowViewModel.OpenRequestDaysOff();
        }
    }
}
