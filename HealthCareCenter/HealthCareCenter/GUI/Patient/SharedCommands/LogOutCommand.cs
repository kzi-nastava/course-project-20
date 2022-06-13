using System.Windows;

namespace HealthCareCenter.GUI.Patient.SharedCommands
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
