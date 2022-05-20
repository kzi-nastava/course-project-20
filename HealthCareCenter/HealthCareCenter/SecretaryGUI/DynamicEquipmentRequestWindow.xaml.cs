using HealthCareCenter.Model;
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

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentRequestWindow.xaml
    /// </summary>
    public partial class DynamicEquipmentRequestWindow : Window
    {
        private Secretary _secretary;
        public DynamicEquipmentRequestWindow()
        {
            InitializeComponent();
        }

        public DynamicEquipmentRequestWindow(Secretary secretary)
        {
            _secretary = secretary;

            InitializeComponent();
        }
    }
}
