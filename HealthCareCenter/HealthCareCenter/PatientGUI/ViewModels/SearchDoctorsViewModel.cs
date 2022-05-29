using HealthCareCenter.PatientGUI.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    class SearchDoctorsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<DoctorViewModel> _doctors;
        public IEnumerable<DoctorViewModel> Doctors => _doctors;

        public ICommand SearchDoctors { get; }
        public ICommand SortDoctors { get; }
        public ICommand SelectDoctor { get; }

        public SearchDoctorsViewModel(NavigationStore navigationStore)
        {

        }
    }
}
