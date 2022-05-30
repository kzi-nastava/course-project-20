using System.ComponentModel;
using System.Text.RegularExpressions;

namespace HealthCareCenter.PatientGUI.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            string typeName = GetType().Name[0..^9];
            return string.Join(" ", Regex.Split(typeName, @"(?<!^)(?=[A-Z])"));
        }
    }
}
