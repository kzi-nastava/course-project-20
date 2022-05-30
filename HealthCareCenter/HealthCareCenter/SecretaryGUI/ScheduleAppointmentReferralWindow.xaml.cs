using HealthCareCenter.Controller;
using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Interaction logic for ScheduleAppointmentReferralWindow.xaml
    /// </summary>
    public partial class ScheduleAppointmentReferralWindow : Window
    {
        private readonly Patient _patient;
        private readonly Referral _referral;

        private List<HospitalRoomDisplay> _rooms;

        public ScheduleAppointmentReferralWindow()
        {
            InitializeComponent();
        }

        public ScheduleAppointmentReferralWindow(Patient patient, Referral referral)
        {
            _patient = patient;
            _referral = referral;

            AppointmentRepository.Load();

            InitializeComponent();

            roomsDataGrid.IsReadOnly = true;
            Refresh();
        }

        private void Refresh()
        {
            RefreshTerms();
            RefreshRooms();
        }

        private void RefreshTerms()
        {
            if (termDatePicker.SelectedDate == null)
            {
                return;
            }

            List<string> terms = Utils.GetPossibleDailyTerms();

            RemoveOccupiedTerms(terms);

            termsListBox.ItemsSource = terms;
        }

        private void RemoveOccupiedTerms(List<string> terms)
        {
            if (VacationRequestController.OnVacation(_referral.DoctorID, (DateTime)termDatePicker.SelectedDate))
            {
                terms.Clear();
                MessageBox.Show("The doctor is on vacation at this time.");
                return;
            }
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.DoctorID != _referral.DoctorID || appointment.ScheduledDate.Date.CompareTo(termDatePicker.SelectedDate) != 0)
                {
                    continue;
                }
                string unavailableTerm = appointment.ScheduledDate.Hour.ToString();
                if (appointment.ScheduledDate.Minute != 0)
                    unavailableTerm += ":" + appointment.ScheduledDate.Minute;
                else
                    unavailableTerm += ":" + appointment.ScheduledDate.Minute + "0";
                terms.Remove(unavailableTerm);
            }
        }

        private void RefreshRooms()
        {
            if (checkupRadioButton == null || operationRadioButton == null)
            {
                return;
            }

            _rooms = new List<HospitalRoomDisplay>();
            foreach (HospitalRoom room in HospitalRoomRepository.Rooms)
            {
                bool correctRoom = (room.Type == Enums.RoomType.Checkup && (bool)checkupRadioButton.IsChecked) || (room.Type == Enums.RoomType.Operation && (bool)operationRadioButton.IsChecked);
                if (correctRoom)
                {
                    _rooms.Add(new HospitalRoomDisplay() { ID = room.ID, Name = room.Name });
                }
            }

            roomsDataGrid.ItemsSource = _rooms;
        }

        private void AppointmentTypeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RefreshRooms();
        }

        private void TermDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshTerms();
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EnteredData())
                return;

            DateTime scheduledDate = GetScheduledDate();

            if (!TimePassed(scheduledDate))
                return;

            if (RoomOccupied(scheduledDate))
                return;

            ScheduleAppointment(scheduledDate);

            MessageBox.Show("Successfully scheduled appointment via referral.");
            this.Close();
        }

        private void ScheduleAppointment(DateTime scheduledDate)
        {
            int roomID = ((HospitalRoomDisplay)roomsDataGrid.SelectedItem).ID;
            Appointment appointment = new Appointment(scheduledDate, roomID, _referral.DoctorID, _patient.HealthRecordID, SelectedAppointmentType(), false);
            AppointmentRepository.Appointments.Add(appointment);
            AppointmentRepository.Save();

            HospitalRoomService.Update(roomID, appointment);
            HospitalRoomRepository.Save();

            ReferralRepository.Referrals.Remove(_referral);
            ReferralRepository.Save();
        }

        private AppointmentType SelectedAppointmentType()
        {
            if ((bool)checkupRadioButton.IsChecked)
                return AppointmentType.Checkup;
            else
                return AppointmentType.Operation;
        }

        private bool RoomOccupied(DateTime time)
        {
            HospitalRoomDisplay room = (HospitalRoomDisplay)roomsDataGrid.SelectedItem;
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.HospitalRoomID != room.ID)
                {
                    continue;
                }
                if (appointment.ScheduledDate.CompareTo(time) == 0)
                {
                    MessageBox.Show("You must select a different room that is not occupied at the term date and time.");
                    return true;
                }
            }
            return false;
        }

        private static bool TimePassed(DateTime scheduledDate)
        {
            if (scheduledDate <= DateTime.Now)
            {
                MessageBox.Show("Your term date has to be in the future.");
                return false;
            }
            return true;
        }

        private DateTime GetScheduledDate()
        {
            string[] scheduledHoursMinutes = termsListBox.SelectedItem.ToString().Split(":");
            int hours = int.Parse(scheduledHoursMinutes[0]);
            int minutes = int.Parse(scheduledHoursMinutes[1]);
            DateTime scheduledDate = ((DateTime)termDatePicker.SelectedDate).Date.AddHours(hours).AddMinutes(minutes);
            return scheduledDate;
        }

        private bool EnteredData()
        {
            if (roomsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select a room first.");
                return false;
            }
            if (termsListBox.SelectedItem == null)
            {
                MessageBox.Show("You must select a term first.");
                return false;
            }
            if (!(bool)checkupRadioButton.IsChecked && !(bool)operationRadioButton.IsChecked)
            {
                MessageBox.Show("You must select an appointment type first.");
                return false;
            }
            if (termDatePicker.SelectedDate == null)
            {
                MessageBox.Show("You must select a term date first.");
                return false;
            }
            return true;
        }
    }
}
