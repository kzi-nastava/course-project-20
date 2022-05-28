using System;
using HealthCareCenter.PatientGUI.ViewModels;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.PatientGUI.Stores
{
    class NavigationStore
    {
        public ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private static NavigationStore instance;

        private NavigationStore() { }

        public static NavigationStore GetInstance()
        {
            return instance is null ? new NavigationStore() : instance;
        }

        public event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
