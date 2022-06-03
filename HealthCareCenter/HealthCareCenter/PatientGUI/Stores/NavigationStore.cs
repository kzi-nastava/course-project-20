using System;
using HealthCareCenter.PatientGUI.ViewModels;

namespace HealthCareCenter.PatientGUI.Stores
{
    internal class NavigationStore
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
            if (instance == null)
            {
                instance = new NavigationStore();
            }
            return instance;
        }

        public event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
