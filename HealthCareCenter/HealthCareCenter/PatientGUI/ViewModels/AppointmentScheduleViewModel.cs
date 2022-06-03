namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class AppointmentScheduleViewModel : ViewModelBase
    {
        public string AvailableDate { get; }
        public string AvailableTerm { get; }

        public AppointmentScheduleViewModel(string availableDate, string availableTerm)
        {
            AvailableDate = availableDate;
            AvailableTerm = availableTerm;
        }
    }
}
