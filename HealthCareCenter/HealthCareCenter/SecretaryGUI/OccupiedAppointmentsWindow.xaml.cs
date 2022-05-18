using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for OccupiedAppointmentsWindow.xaml
    /// </summary>
    public partial class OccupiedAppointmentsWindow : Window
    {
        private Patient _patient;
        private AppointmentType _type;
        private List<Doctor> _doctors;
        private List<HospitalRoom> _rooms;
        private List<Appointment> _occupiedAppointments;
        private List<AppointmentDisplay> _appointmentsForDisplay;
        private Dictionary<int, DateTime> _newDateOf;
        private Dictionary<int, Appointment> _newAppointmentsInfo;

        public OccupiedAppointmentsWindow()
        {
            InitializeComponent();
        }

        public OccupiedAppointmentsWindow(Patient patient, AppointmentType type, List<Doctor> doctors, List<HospitalRoom> rooms, List<Appointment> occupiedAppointments, Dictionary<int, Appointment> newAppointmentsInfo)
        {
            _patient = patient;
            _type = type;
            _doctors = doctors;
            _rooms = rooms;
            _occupiedAppointments = occupiedAppointments;
            _newAppointmentsInfo = newAppointmentsInfo;

            InitializeComponent();

            SortAppointments();
            RefreshTable();
        }

        private void SortAppointments()
        {
            List<string> allPossibleTerms = Utils.GetPossibleDailyTerms();
            List<string> terms = FormTodaysPossibleTerms(allPossibleTerms);
            List<Appointment> sortedAppointments = new List<Appointment>();
            Dictionary<int, DateTime> newDateOf = new Dictionary<int, DateTime>();
            bool foundAll = false;
            DateTime current = DateTime.Now;

            for (int i = 0; i < 365; i++)
            {
                foreach (string term in terms)
                {
                    int hours = int.Parse(term.Split(":")[0]);
                    int minutes = int.Parse(term.Split(":")[1]);
                    DateTime newTime = current.Date.AddHours(hours).AddMinutes(minutes);

                    foreach (Appointment occupiedAppointment in _occupiedAppointments.ToList())
                    {
                        if (!IsPostponableTo(newTime, occupiedAppointment))
                            continue;

                        sortedAppointments.Add(occupiedAppointment);
                        newDateOf.Add(occupiedAppointment.ID, newTime);
                        _occupiedAppointments.Remove(occupiedAppointment);

                        if (sortedAppointments.Count == 5)
                        {
                            foundAll = true;
                            break;
                        }
                    }
                    if (foundAll)
                        break;
                }
                if (foundAll)
                    break;

                current = current.AddDays(1);
                terms = new List<string>(allPossibleTerms);
            }
            _occupiedAppointments = new List<Appointment>(sortedAppointments);
            _newDateOf = new Dictionary<int, DateTime>(newDateOf);
        }

        private static bool IsPostponableTo(DateTime newTime, Appointment occupiedAppointment)
        {
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ScheduledDate.CompareTo(newTime) != 0)
                {
                    continue;
                }

                if (appointment.DoctorID == occupiedAppointment.DoctorID || appointment.HospitalRoomID == occupiedAppointment.HospitalRoomID || appointment.HealthRecordID == occupiedAppointment.HealthRecordID)
                {
                    return false;
                }
            }
            return true;
        }

        private static List<string> FormTodaysPossibleTerms(List<string> allPossibleTerms)
        {
            List<string> terms = new List<string>();
            foreach (string term in allPossibleTerms)
            {
                int hrs = int.Parse(term.Split(":")[0]);
                int mins = int.Parse(term.Split(":")[1]);
                if (hrs > DateTime.Now.Hour + 2 || (hrs == DateTime.Now.Hour + 2 && mins > DateTime.Now.Minute))
                {
                    terms.Add(term);
                }
            }
            return terms;
        }

        private void RefreshTable()
        {
            _appointmentsForDisplay = new List<AppointmentDisplay>();
            foreach (Appointment appointment in _occupiedAppointments)
            {
                AppointmentDisplay appointmentDisplay = new AppointmentDisplay(appointment.ID, appointment.Type, appointment.ScheduledDate, appointment.Emergency, _newDateOf[appointment.ID]);
                LinkDoctor(appointment, appointmentDisplay);
                LinkPatient(appointment, appointmentDisplay);
                _appointmentsForDisplay.Add(appointmentDisplay);
            }
            occupiedAppointmentsDataGrid.ItemsSource = _appointmentsForDisplay;
        }

        private static void LinkPatient(Appointment appointment, AppointmentDisplay appointmentDisplay)
        {
            foreach (Patient patient in UserRepository.Patients)
            {
                if (appointment.HealthRecordID == patient.HealthRecordID)
                {
                    appointmentDisplay.PatientName = patient.FirstName + " " + patient.LastName;
                    return;
                }
            }
        }

        private static void LinkDoctor(Appointment appointment, AppointmentDisplay appointmentDisplay)
        {
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (appointment.DoctorID == doctor.ID)
                {
                    appointmentDisplay.DoctorName = doctor.FirstName + " " + doctor.LastName;
                    return;
                }
            }
        }

        private void PostponeButton_Click(object sender, RoutedEventArgs e)
        {
            if (occupiedAppointmentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select an appointment to postpone!");
                return;
            }

            Appointment postponedAppointment = Postpone();

            MessageBox.Show($"Successfully postponed appointment {postponedAppointment.ID} and scheduled a new urgent appointment in its place.");
            this.Close();
        }

        private Appointment Postpone()
        {
            AppointmentDisplay appointmentDisplay = (AppointmentDisplay)occupiedAppointmentsDataGrid.SelectedItem;
            Appointment postponedAppointment = AppointmentService.Find(appointmentDisplay);

            Appointment newAppointment = new Appointment(appointmentDisplay.ScheduledDate, _newAppointmentsInfo[postponedAppointment.ID].HospitalRoomID, _newAppointmentsInfo[postponedAppointment.ID].DoctorID, _patient.HealthRecordID, _type, true);
            AppointmentRepository.Appointments.Add(newAppointment);
            postponedAppointment.ScheduledDate = appointmentDisplay.PostponedTime;
            AppointmentRepository.Save();

            HospitalRoomService.Update(newAppointment.HospitalRoomID, newAppointment);
            HospitalRoomRepository.Save();
            SendNotifications(postponedAppointment, newAppointment);
            return postponedAppointment;
        }

        private void SendNotifications(Appointment postponedAppointment, Appointment newAppointment)
        {
            HealthRecord postponedRecord = HealthRecordService.FindRecord(postponedAppointment);

            NotificationService.CalculateMaxID();
            Notification postponedPatientNotification = new Notification($"The appointment you had scheduled at {newAppointment.ScheduledDate} has been postponed to {postponedAppointment.ScheduledDate}.", postponedRecord.PatientID);
            Notification postponedDoctorNotification = new Notification($"The appointment you had scheduled at {newAppointment.ScheduledDate} has been postponed to {postponedAppointment.ScheduledDate}.", postponedAppointment.DoctorID);
            Notification newDoctorNotification = new Notification($"A new urgent appointment has been scheduled for you at {newAppointment.ScheduledDate} in room {HospitalRoomService.Get(newAppointment.HospitalRoomID).Name}.", newAppointment.DoctorID);

            if (postponedRecord.PatientID == _patient.ID)
            {
                postponedPatientNotification.Opened = true;
                MessageBox.Show(postponedPatientNotification.Message);
            }

            NotificationRepository.Notifications.AddRange(new Notification[] { postponedPatientNotification, postponedDoctorNotification, newDoctorNotification });
            NotificationRepository.Save();
        }
    }
}
