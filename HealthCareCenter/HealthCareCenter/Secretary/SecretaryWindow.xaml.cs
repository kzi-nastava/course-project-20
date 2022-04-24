﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace HealthCareCenter
{
    public partial class SecretaryWindow : Window
    {
        private Secretary _signedUser;
        public SecretaryWindow(User user)
        {
            _signedUser = (Secretary)user;
            InitializeComponent();
        }
    }
}
