using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Models;
using HealthCareCenter.PatientGUI.Stores;
using HealthCareCenter.PatientGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.PatientGUI.Commands
{
    class NavigateCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly ViewType _viewType;
        private readonly Patient _patient;

        public NavigateCommand(NavigationStore navigationStore, ViewType viewType, Patient patient)
        {
            _navigationStore = navigationStore;
            _viewType = viewType;
            _patient = patient;
        }

        public override void Execute(object parameter)
        {
            switch (_viewType)
            {
                case ViewType.MyAppointments:
                    _navigationStore.CurrentViewModel = new MyAppointmentsViewModel(_navigationStore, _patient);
                    break;
                case ViewType.CreateAppointment:
                    _navigationStore.CurrentViewModel = new CreateAppointmentViewModel(_navigationStore, _patient, null);
                    break;
                case ViewType.PriorityScheduling:
                    _navigationStore.CurrentViewModel = new PrioritySchedulingViewModel(_navigationStore);
                    break;
                case ViewType.ModifyAppointment:

                    break;
                case ViewType.MyHealthRecord:
                    _navigationStore.CurrentViewModel = new MyHealthRecordViewModel(_navigationStore);
                    break;
                case ViewType.MyPrescriptions:
                    _navigationStore.CurrentViewModel = new MyPrescriptionsViewModel(_navigationStore);
                    break;
                case ViewType.DoctorSurvey:

                    break;
                case ViewType.HealthCenterSurvey:

                    break;
                case ViewType.SearchDoctors:
                    _navigationStore.CurrentViewModel = new SearchDoctorsViewModel(_navigationStore, _patient);
                    break;
            }
        }
    }
}
