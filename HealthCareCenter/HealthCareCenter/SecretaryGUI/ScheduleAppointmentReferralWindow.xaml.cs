using HealthCareCenter.Enums;
using HealthCareCenter.Model;
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
        private Patient _patient;
        private Referral _referral;

        private List<HospitalRoomDisplay> _rooms;
        private DataTable _availableTerms;

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
                if ((room.Type == Enums.RoomType.Checkup && (bool)checkupRadioButton.IsChecked) || (room.Type == Enums.RoomType.Operation && (bool)operationRadioButton.IsChecked))
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
            Appointment appointment = CreateAppointment(scheduledDate, roomID);
            AppointmentRepository.Appointments.Add(appointment);
            AppointmentRepository.Save();

            UpdateRoom(roomID, appointment);
            HospitalRoomRepository.SaveRooms(HospitalRoomRepository.Rooms);

            UpdateHealthRecord(appointment);
            HealthRecordRepository.Save();

            _patient.ReferralIDs.Remove(_referral.ID);
            UpdateDoctor(appointment);
            UserRepository.SavePatients();
            UserRepository.SaveDoctors();

            ReferralRepository.Referrals.Remove(_referral);
            ReferralRepository.Save();
        }

        private void UpdateDoctor(Appointment appointment)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == _referral.DoctorID)
                {
                    doctor.AppointmentIDs.Add(appointment.ID);
                    break;
                }
            }
        }

        private void UpdateHealthRecord(Appointment appointment)
        {
            foreach (HealthRecord record in HealthRecordRepository.Records)
            {
                if (record.ID == _patient.HealthRecordID)
                {
                    record.AppointmentIDs.Add(appointment.ID);
                    break;
                }
            }
        }

        private Appointment CreateAppointment(DateTime scheduledDate, int roomID)
        {
            return new Appointment()
            {
                ID = ++AppointmentRepository.LargestID,
                CreatedDate = DateTime.Now,
                ScheduledDate = scheduledDate,
                DoctorID = _referral.DoctorID,
                HospitalRoomID = roomID,
                Emergency = false,
                Type = SelectedAppointmentType(),
                HealthRecordID = _patient.HealthRecordID,
                PatientAnamnesis = new Anamnesis()
            };
        }

        private static void UpdateRoom(int roomID, Appointment appointment)
        {
            foreach (HospitalRoom room in HospitalRoomRepository.Rooms)
            {
                if (room.ID == roomID)
                {
                    room.AppointmentIDs.Add(appointment.ID);
                    break;
                }
            }
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
