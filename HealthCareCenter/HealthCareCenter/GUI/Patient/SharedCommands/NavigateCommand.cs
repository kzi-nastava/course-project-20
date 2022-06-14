using HealthCareCenter.Core;
using HealthCareCenter.Core.Appointments.Services;
using HealthCareCenter.GUI.Patient.AppointmentCRUD.ViewModels;
using HealthCareCenter.GUI.Patient.DoctorSearch;
using HealthCareCenter.GUI.Patient.Profile.ViewModels;
using HealthCareCenter.GUI.Patient.SharedViewModels;
using HealthCareCenter.GUI.Patient.Survey.ViewModels;

namespace HealthCareCenter.GUI.Patient.SharedCommands
{
    internal class NavigateCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            switch (_viewType)
            {
                case ViewType.MyAppointments:
                    _navigationStore.CurrentViewModel = new MyAppointmentsViewModel(_navigationStore, _patient);
                    break;
                case ViewType.PriorityScheduling:
                    _navigationStore.CurrentViewModel = new PrioritySchedulingViewModel(_patient, _navigationStore);
                    break;
                case ViewType.MyHealthRecord:
                    _navigationStore.CurrentViewModel = new MyHealthRecordViewModel(_patient);
                    break;
                case ViewType.MyPrescriptions:
                    _navigationStore.CurrentViewModel = new MyPrescriptionsViewModel(_navigationStore, _patient);
                    break;
                case ViewType.DoctorSurvey:
                    _navigationStore.CurrentViewModel = new DoctorSurveyViewModel(_patient);
                    break;
                case ViewType.HealthCenterSurvey:
                    _navigationStore.CurrentViewModel = new HealthCenterSurveyViewModel(_patient);
                    break;
                case ViewType.SearchDoctors:
                    _navigationStore.CurrentViewModel = new SearchDoctorsViewModel(_navigationStore, _patient);
                    break;
            }
        }

        private readonly NavigationStore _navigationStore;
        private readonly ViewType _viewType;
        private readonly Core.Patients.Patient _patient;

        public NavigateCommand(NavigationStore navigationStore, ViewType viewType, Core.Patients.Patient patient)
        {
            _navigationStore = navigationStore;
            _viewType = viewType;
            _patient = patient;
        }
    }
}
