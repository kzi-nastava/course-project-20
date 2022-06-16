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
    /// Interaction logic for RequestOffDaysWindow.xaml
    /// </summary>
    public partial class RequestDaysOffWindow : Window
    {
        private RequestDaysOffViewModel _windowViewModel;
        public RequestDaysOffWindow(RequestDaysOffViewModel viewModel)
        {
            _windowViewModel = viewModel;
            InitializeComponent();
        }

        private void submitDaysOffRequest_Click(object sender, RoutedEventArgs e)
        {
            _windowViewModel.SubmitVacaionRequest();
        }
    }
}
