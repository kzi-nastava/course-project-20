using HealthCareCenter.Model;
using HealthCareCenter.PatientGUI.Commands;
using HealthCareCenter.PatientGUI.Stores;
using System.Collections.Generic;
using System.Windows.Input;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class SearchDoctorsViewModel : ViewModelBase
    {
        public Patient Patient { get; }

        private List<DoctorViewModel> _doctors;
        public List<DoctorViewModel> Doctors
        {
            get => _doctors;
            set
            {
                _doctors = value;
                OnPropertyChanged(nameof(Doctors));
            }
        }

        public List<string> SearchCriteria { get; }

        private string _chosenSearchCriteria;
        public string ChosenSearchCriteria
        {
            get => _chosenSearchCriteria;
            set
            {
                _chosenSearchCriteria = value;
                OnPropertyChanged(nameof(ChosenSearchCriteria));
            }
        }

        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged(nameof(SearchKeyword));
            }
        }

        private readonly List<string> _sortCriteria;
        public List<string> SortCriteria => _sortCriteria;

        private string _chosenSortCriteria;
        public string ChosenSortCriteria
        {
            get => _chosenSortCriteria;
            set
            {
                _chosenSortCriteria = value;
                OnPropertyChanged(nameof(ChosenSortCriteria));
            }
        }

        private DoctorViewModel _chosenDoctor;
        public DoctorViewModel ChosenDoctor
        {
            get => _chosenDoctor;
            set
            {
                _chosenDoctor = value;
                OnPropertyChanged(nameof(ChosenDoctor));
            }
        }

        public ICommand SearchDoctors { get; }
        public ICommand SortDoctors { get; }
        public ICommand SelectDoctor { get; }

        public SearchDoctorsViewModel(NavigationStore navigationStore, Patient patient)
        {
            Patient = patient;

            _doctors = new List<DoctorViewModel>();

            SearchCriteria = new List<string>
            {
                "First name",
                "Last name",
                "Professional area"
            };
            _chosenSearchCriteria = SearchCriteria[0];

            _sortCriteria = new List<string>
            {
                "Search criteria",
                "Rating"
            };
            _chosenSortCriteria = _sortCriteria[0];

            SearchDoctors = new SearchDoctorsCommand(this);
            SortDoctors = new SortDoctorsCommand(this);
            SelectDoctor = new SelectDoctorCommand(this, navigationStore);
        }
    }
}
