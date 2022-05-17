using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using HealthCareCenter.Enums;
using HealthCareCenter.Service;

namespace HealthCareCenter.SecretaryGUI
{
    /// <summary>
    /// Interaction logic for ScheduleUrgentAppointmentWindow.xaml
    /// </summary>
    public partial class ScheduleUrgentAppointmentWindow : Window
    {
        private Patient _patient;

        private List<string> _typesOfDoctors;
        private List<Appointment> _occupiedAppointments;
        private Dictionary<int, Appointment> _newAppointmentsInfo;

        public ScheduleUrgentAppointmentWindow()
        {
            InitializeComponent();
        }

        public ScheduleUrgentAppointmentWindow(Patient patient)
        {
            _patient = patient;

            AppointmentRepository.Load();

            InitializeComponent();

            Refresh();
        }

        private void Refresh()
        {
            _typesOfDoctors = new List<string>();
            _typesOfDoctors.AddRange(UserRepository.Doctors.Where(doctor => !_typesOfDoctors.Contains(doctor.Type)).Select(doctor => doctor.Type));
            doctorTypesListBox.ItemsSource = _typesOfDoctors;
        }

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            if (!EnteredData())
                return;

            TryScheduling();
        }

        private void TryScheduling()
        {
            List<Doctor> doctors = GetDoctorsOfSelectedType();
            AppointmentType type = AppointmentType.Checkup;
            if ((bool)operationRadioButton.IsChecked)
                type = AppointmentType.Operation;

            List<HospitalRoom> rooms = GetRoomsOfSelectedType(type);
            List<string> terms = GetTermsWithinTwoHours();

            _occupiedAppointments = new List<Appointment>();
            _newAppointmentsInfo = new Dictionary<int, Appointment>();

            if (!FindTermsAndSchedule(doctors, type, rooms, terms))
            {
                OpenPostponingWindow(doctors, type, rooms);
            }
        }

        private void OpenPostponingWindow(List<Doctor> doctors, AppointmentType type, List<HospitalRoom> rooms)
        {
            if (_occupiedAppointments.Count == 0)
            {
                MessageBox.Show("No available term was found in the next 2 hours. Unfortunately, there are no terms to postpone at this time neither.");
                return;
            }
            MessageBox.Show("No available term was found in the next 2 hours. You can, however, postpone an occupied term in the next window.");
            OccupiedAppointmentsWindow window = new OccupiedAppointmentsWindow(_patient, type, doctors, rooms, _occupiedAppointments, _newAppointmentsInfo);
            window.ShowDialog();
            this.Close();
        }

        private bool FindTermsAndSchedule(List<Doctor> doctors, AppointmentType type, List<HospitalRoom> rooms, List<string> terms)
        {
            foreach (string term in terms)
            {
                DateTime potentialTime = CreatePotentialTime(term);

                List<Doctor> availableDoctors = new List<Doctor>(doctors);
                List<HospitalRoom> availableRooms = new List<HospitalRoom>(rooms);

                bool isValid = CheckTermAndRemoveUnavailables(potentialTime, availableDoctors, availableRooms, AppointmentRepository.Appointments);

                if (isValid && availableDoctors.Count > 0 && availableRooms.Count > 0)
                {
                    ScheduleAppointment(type, potentialTime, availableDoctors, availableRooms);
                    this.Close();
                    return true;
                }
                else
                {
                    PrepareForPotentialPostponing(doctors, rooms, potentialTime);
                }
            }
            return false;
        }

        private void PrepareForPotentialPostponing(List<Doctor> doctors, List<HospitalRoom> rooms, DateTime potentialTime)
        {
            List<Appointment> appointments = new List<Appointment>(AppointmentRepository.Appointments);
            for (int i = 0; i < AppointmentRepository.Appointments.Count; i++)
            {
                if (AppointmentRepository.Appointments[i].ScheduledDate.CompareTo(potentialTime) != 0)
                    continue;

                appointments.Remove(AppointmentRepository.Appointments[i]);

                List<Doctor> availableDoctors = new List<Doctor>(doctors);
                List<HospitalRoom> availableRooms = new List<HospitalRoom>(rooms);

                bool isValid = CheckTermAndRemoveUnavailables(potentialTime, availableDoctors, availableRooms, appointments);
                if (isValid && availableDoctors.Count > 0 && availableRooms.Count > 0)
                {
                    AddPostponingInfo(availableDoctors, availableRooms, i);
                }

                appointments.Add(AppointmentRepository.Appointments[i]);
            }
        }

        private static DateTime CreatePotentialTime(string term)
        {
            int termHour = int.Parse(term.Split(":")[0]);
            int termMinute = int.Parse(term.Split(":")[1]);
            DateTime potentialTime = DateTime.Now.Date.AddHours(termHour).AddMinutes(termMinute);
            return potentialTime;
        }

        private void AddPostponingInfo(List<Doctor> availableDoctors, List<HospitalRoom> availableRooms, int i)
        {
            _occupiedAppointments.Add(AppointmentRepository.Appointments[i]);
            _newAppointmentsInfo.Add(AppointmentRepository.Appointments[i].ID,
                new Appointment { DoctorID = availableDoctors[0].ID, HospitalRoomID = availableRooms[0].ID });
        }

        private void ScheduleAppointment(AppointmentType type, DateTime potentialTime, List<Doctor> availableDoctors, List<HospitalRoom> availableRooms)
        {
            Doctor doctor = availableDoctors[0];
            HospitalRoom room = availableRooms[0];

            Appointment appointment = new Appointment(potentialTime, room.ID, doctor.ID, _patient.HealthRecordID, type, true);
            AppointmentRepository.Appointments.Add(appointment);
            AppointmentRepository.Save();

            HospitalRoomService.Update(room.ID, appointment);
            HospitalRoomRepository.SaveRooms(HospitalRoomRepository.Rooms);

            MessageBox.Show($"Successfully scheduled urgent appointment at {potentialTime.ToShortTimeString()} with doctor {doctor.FirstName} {doctor.LastName} in room {room.Name}.");
        }

        private bool CheckTermAndRemoveUnavailables(DateTime potentialTime, List<Doctor> availableDoctors, List<HospitalRoom> availableRooms, List<Appointment> appointments)
        {
            foreach (Appointment appointment in appointments)
            {
                if (appointment.ScheduledDate.CompareTo(potentialTime) != 0)
                {
                    continue;
                }
                if (appointment.HealthRecordID == _patient.HealthRecordID)
                {
                    return false;
                }
                RemoveUnavailableDoctors(availableDoctors, appointment);
                RemoveUnavailableRooms(availableRooms, appointment);
            }
            return true;
        }

        private static void RemoveUnavailableRooms(List<HospitalRoom> availableRooms, Appointment appointment)
        {
            foreach (HospitalRoom room in availableRooms)
            {
                if (room.ID == appointment.HospitalRoomID)
                {
                    availableRooms.Remove(room);
                    break;
                }
            }
        }

        private void RemoveUnavailableDoctors(List<Doctor> availableDoctors, Appointment appointment)
        {
            foreach (Doctor doctor in availableDoctors)
            {
                if (doctor.ID == appointment.DoctorID)
                {
                    availableDoctors.Remove(doctor);
                    break;
                }
            }
        }

        private List<string> GetTermsWithinTwoHours()
        {
            List<string> possibleTerms = Utils.GetPossibleDailyTerms();
            List<string> termsWithinTwoHours = new List<string>();
            int currHour = DateTime.Now.Hour;
            int currMinute = DateTime.Now.Minute;
            foreach (string term in possibleTerms)
            {
                int termHour = int.Parse(term.Split(":")[0]);
                int termMinute = int.Parse(term.Split(":")[1]);
                int diff = termHour - currHour;

                bool withinTwoHours = diff == 1 || (diff == 0 && termMinute >= currMinute) || (diff == 2 && termMinute <= currMinute);
                if (withinTwoHours)
                {
                    termsWithinTwoHours.Add(term);
                }
            }
            return termsWithinTwoHours;
        }

        private List<HospitalRoom> GetRoomsOfSelectedType(AppointmentType type)
        {
            List<HospitalRoom> rooms = new List<HospitalRoom>();
            foreach (HospitalRoom room in HospitalRoomRepository.Rooms)
            {
                bool correctRoom = (type == AppointmentType.Checkup && room.Type == RoomType.Checkup) || (type == AppointmentType.Operation && room.Type == RoomType.Operation);
                if (correctRoom)
                {
                    rooms.Add(room);
                }
            }
            return rooms;
        }

        
        private List<Doctor> GetDoctorsOfSelectedType()
        {
            List<Doctor> doctors = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type == doctorTypesListBox.SelectedItem.ToString())
                {
                    doctors.Add(doctor);
                }
            }
            return doctors;
        }

        private bool EnteredData()
        {
            if (doctorTypesListBox.SelectedItem == null)
            {
                MessageBox.Show("You must select a type of doctor first.");
                return false;
            }
            if (!(bool)checkupRadioButton.IsChecked && !(bool)operationRadioButton.IsChecked)
            {
                MessageBox.Show("You must select a type of appointment first.");
                return false;
            }
            return true;
        }
    }
}
