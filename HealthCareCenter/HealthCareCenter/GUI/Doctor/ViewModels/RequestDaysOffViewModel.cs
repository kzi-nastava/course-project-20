using HealthCareCenter.Core;
using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Users.Models;
using HealthCareCenter.Core.VacationRequests.Models;
using HealthCareCenter.Core.VacationRequests.Repositories;
using HealthCareCenter.GUI.Doctor.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace HealthCareCenter.GUI.Doctor.ViewModels
{
    public class RequestDaysOffViewModel
    {
        private User _signedUser;
        private RequestDaysOffWindow _window;
        private BaseAppointmentRepository _appointmentRepository;
        private BaseVacationRequestRepository _vacationRequestRepository;
        public RequestDaysOffViewModel(User signedUser, AppointmentRepository appointmentRepository, VacationRequestRepository vacationRequestRepository)
        {
            _vacationRequestRepository = vacationRequestRepository;
            _appointmentRepository = appointmentRepository;
            _window = new RequestDaysOffWindow(this);
            _signedUser = signedUser;
            FillUpDateComboBoxes();
            _window.Show();

        }

        internal void SubmitVacaionRequest()
        {
            bool sucesfull = ValidateData();
            if (sucesfull)
            {
                MessageBox.Show("Added request sucesfully");
                _window.Close();
            }
        }
        private void FillUpDateComboBoxes()
        {
            for (int i = 1; i <= 31; i++)
            {
                string day = i.ToString();
                if (day.Length == 1)
                {
                    day = "0" + day;
                }

                _window.dayComboBox.Items.Add(day);
            }
            for (int i = 1; i <= 12; i++)
            {
                string month = i.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }

                _window.monthComboBox.Items.Add(month);
            }

            for (int i = 1; i <= 21; i++)
            {
                _window.numberOfDaysComboBox.Items.Add(i.ToString());
            }
            _window.yearComboBox.Items.Add("2022");
            _window.yearComboBox.Items.Add("2023");
            _window.yearComboBox.Items.Add("2024");

            _window.numberOfDaysComboBox.SelectedIndex = 0;
            _window.dayComboBox.SelectedIndex = 0;
            _window.monthComboBox.SelectedIndex = 0;
            _window.yearComboBox.SelectedIndex = 0;
        }

        private bool ValidateData()
        {
            int numberOfDays;
            string reasonForVacation, date;
            bool emergency;
            numberOfDays = _window.numberOfDaysComboBox.SelectedIndex + 1;
            
            emergency = (bool)_window.emergencyCheckBox.IsChecked;

            date = ParseTime();
   
            reasonForVacation = _window.requestReason.Text;
            if (reasonForVacation == "")
            {
                MessageBox.Show("Please enter a reason");
                return false;
            }
            bool sucsesfull = CheckIfPossible(emergency, date, numberOfDays, _appointmentRepository.Appointments, reasonForVacation);
            if(sucsesfull)
                return true;
            return false;
        }
        
        private string ParseTime()
        {

            int dayComboBoxUnparsed, monthComboBoxUnparsed, yearComboBoxUnparsed;
            string dayParsed, monthParsed, yearParsed;
            dayComboBoxUnparsed = _window.dayComboBox.SelectedIndex + 1;
            monthComboBoxUnparsed = _window.monthComboBox.SelectedIndex + 1;
            yearComboBoxUnparsed = _window.yearComboBox.SelectedIndex + 2022;

            if (dayComboBoxUnparsed.ToString().Length == 1)
                dayParsed = "0" + dayComboBoxUnparsed.ToString();
            else
                dayParsed = dayComboBoxUnparsed.ToString();

            if (monthComboBoxUnparsed.ToString().Length == 1)
                monthParsed = "0" + monthComboBoxUnparsed.ToString();
            else
                monthParsed = monthComboBoxUnparsed.ToString();

            yearParsed = yearComboBoxUnparsed.ToString();

            return dayParsed.ToString() + "/" + monthParsed.ToString() + "/" + yearParsed.ToString();
        }
        private bool CheckIfPossible(bool emergency,string date, int numberOfDays, List<Appointment> appointments, string reasonForVacation)
        {
            DateTime startDate, endDate;
            if (emergency)
            {
                startDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                if (numberOfDays > 5)
                {
                    endDate = startDate.AddDays(5);
                    MessageBox.Show("Number of vacation days has been reduced to 5");
                }
                else
                    endDate = startDate.AddDays(numberOfDays);
            }
            else
            {
                startDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                endDate = startDate.AddDays(numberOfDays);
            }

            foreach(Appointment appointment in appointments)
            {
                int firstCheck = DateTime.Compare(appointment.ScheduledDate, startDate);
                int secondCheck = DateTime.Compare(appointment.ScheduledDate, endDate);
                if(firstCheck > 0 && secondCheck < 0)
                {
                    MessageBox.Show("U have appoitments during this period");
                    return false;
                }
            }
            CreateVacationRequest(startDate, endDate, reasonForVacation, emergency);
            return true;
        }
        private void CreateVacationRequest(DateTime startDate, DateTime endDate, string reason, bool emergency)
        {
            VacationRequest vacationRequest = new VacationRequest();
            vacationRequest.ID = ++VacationRequestRepository.LargestID;
            vacationRequest.StartDate = startDate;
            vacationRequest.EndDate = endDate;
            vacationRequest.Emergency = emergency;
            vacationRequest.RequestReason = reason;
            vacationRequest.State = RequestState.Waiting;
            vacationRequest.DoctorID = _signedUser.ID;
            _vacationRequestRepository.Requests.Add(vacationRequest);
        }
    }
}
