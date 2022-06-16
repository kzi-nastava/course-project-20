using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Rooms.Repositories;
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
            LoginWindow win = new LoginWindow(
                new DynamicEquipmentService(
                    new DynamicEquipmentRequestRepository(),
                    new StorageRepository()));
            win.Show();
            base.OnStartup(e);
        }
    }
}