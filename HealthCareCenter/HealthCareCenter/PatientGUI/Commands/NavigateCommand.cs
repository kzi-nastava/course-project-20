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

        public NavigateCommand(NavigationStore navigationStore, ViewType viewType)
        {
            _navigationStore = navigationStore;
            _viewType = viewType;
        }

        public override void Execute(object parameter)
        {
            switch (_viewType)
            {
                case ViewType.MyAppointments:
                    _navigationStore.CurrentViewModel = new MyAppointmentsViewModel(_navigationStore);
                    break;
                case ViewType.CreateAppointment:
                    _navigationStore.CurrentViewModel = new CreateAppointmentViewModel();
                    break;
                case ViewType.PriorityScheduling:
                    _navigationStore.CurrentViewModel = new PrioritySchedulingViewModel();
                    break;
                case ViewType.ModifyAppointment:

                    break;
                case ViewType.MyHealthRecord:

                    break;
                case ViewType.MyPrescriptions:

                    break;
                case ViewType.DoctorSurvey:

                    break;
                case ViewType.HealthCenterSurvey:

                    break;
            }
        }
    }
}
