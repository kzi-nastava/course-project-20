using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace HealthCareCenter.PatientGUI.Commands
{
    class LogOutCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            LoginWindow win = new LoginWindow();
            win.Show();
            Application.Current.Windows[0].Close();
        }
    }
}
