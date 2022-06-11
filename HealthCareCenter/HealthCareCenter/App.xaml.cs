using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System.Windows;

namespace HealthCareCenter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            LoginWindow win = new LoginWindow(new DynamicEquipmentService(new DynamicEquipmentRequestRepository()));
            win.Show();
            base.OnStartup(e);
        }
    }
}
