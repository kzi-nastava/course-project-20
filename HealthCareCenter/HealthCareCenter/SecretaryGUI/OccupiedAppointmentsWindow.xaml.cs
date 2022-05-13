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
        private AppointmentType _appointmentType;
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

        public OccupiedAppointmentsWindow(Patient patient, AppointmentType appointmentType, List<Doctor> doctors, List<HospitalRoom> rooms, List<Appointment> occupiedAppointments, Dictionary<int, Appointment> newAppointmentsInfo)
        {
            _patient = patient;
            _appointmentType = appointmentType;
            _doctors = doctors;
            _rooms = rooms;
            _occupiedAppointments = occupiedAppointments;
            _newAppointmentsInfo = newAppointmentsInfo;

            InitializeComponent();

            Sort();
            Refresh();
        }

        private void Sort()
        {
            List<string> allPossibleTerms = Utils.GetPossibleDailyTerms();
            List<string> terms = FormTodaysPossibleTerms(allPossibleTerms);
            List<Appointment> sortedAppointments = new List<Appointment>();
            Dictionary<int, DateTime> newDateOf = new Dictionary<int, DateTime>();
            bool foundAll = false;
            DateTime startDate = DateTime.Now;

            for (int i = 0; i < 365; i++)
            {
                foreach (string term in terms)
                {
                    int hrs = int.Parse(term.Split(":")[0]);
                    int mins = int.Parse(term.Split(":")[1]);
                    DateTime newTime = startDate.Date.AddHours(hrs).AddMinutes(mins);

                    foreach (Appointment occupiedAppointment in _occupiedAppointments.ToList())
                    {
                        bool postponable = true;

                        foreach (Appointment appointment in AppointmentRepository.Appointments)
                        {
                            if (appointment.ScheduledDate.CompareTo(newTime) != 0)
                            {
                                continue;
                            }

                            if (appointment.DoctorID == occupiedAppointment.DoctorID || appointment.HospitalRoomID == occupiedAppointment.HospitalRoomID)
                            {
                                postponable = false;
                                break;
                            }
                        }
                        if (!postponable)
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

                startDate = startDate.AddDays(1);
                terms = new List<string>(allPossibleTerms);
            }
            _occupiedAppointments = new List<Appointment>(sortedAppointments);
            _newDateOf = new Dictionary<int, DateTime>(newDateOf);
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

        private void Refresh()
        {
            _appointmentsForDisplay = new List<AppointmentDisplay>();
            foreach (Appointment appointment in _occupiedAppointments)
            {
                AppointmentDisplay appointmentDisplay = new AppointmentDisplay
                {
                    ID = appointment.ID,
                    Type = appointment.Type,
                    ScheduledDate = appointment.ScheduledDate,
                    Emergency = appointment.Emergency,
                    PostponedTime = _newDateOf[appointment.ID]
                };
                foreach (Doctor doctor in UserRepository.Doctors)
                {
                    if (appointment.DoctorID == doctor.ID)
                    {
                        appointmentDisplay.DoctorName = doctor.FirstName + " " + doctor.LastName;
                        break;
                    }
                }
                foreach (Patient patient in UserRepository.Patients)
                {
                    if (appointment.HealthRecordID == patient.HealthRecordID)
                    {
                        appointmentDisplay.PatientName = patient.FirstName + " " + patient.LastName;
                        break;
                    }
                }
                _appointmentsForDisplay.Add(appointmentDisplay);
            }
            occupiedAppointmentsDataGrid.ItemsSource = _appointmentsForDisplay;
        }

        private void PostponeButton_Click(object sender, RoutedEventArgs e)
        {
            if (occupiedAppointmentsDataGrid.SelectedItem == null)
            {
                MessageBox.Show("You must select an appointment to postpone!");
                return;
            }

            AppointmentDisplay appointmentDisplay = (AppointmentDisplay)occupiedAppointmentsDataGrid.SelectedItem;
            Appointment postponedAppointment = null;
            foreach (Appointment app in AppointmentRepository.Appointments)
            {
                if (app.ID == appointmentDisplay.ID)
                {
                    postponedAppointment = app;
                    break;
                }
            }
            
            Appointment newAppointment = new Appointment(appointmentDisplay.ScheduledDate, _newAppointmentsInfo[postponedAppointment.ID].HospitalRoomID, _newAppointmentsInfo[postponedAppointment.ID].DoctorID, _patient.HealthRecordID, _appointmentType, true);
            AppointmentRepository.Appointments.Add(newAppointment);
            postponedAppointment.ScheduledDate = appointmentDisplay.PostponedTime;
            AppointmentRepository.Save();

            HospitalRoomService.Update(newAppointment.HospitalRoomID, newAppointment);
            HospitalRoomRepository.SaveRooms(HospitalRoomRepository.Rooms);

            HealthRecordService.Update(_patient.HealthRecordID, newAppointment);
            HealthRecordRepository.Save();

            UserService.UpdateDoctor(newAppointment.DoctorID, newAppointment);
            UserRepository.SaveDoctors();

            HealthRecord postponedPatientsRecord = FindHealthRecord(postponedAppointment);

            NotificationService.CalculateMaxID();
            Notification postponedPatientNotification = new Notification($"The appointment you had scheduled at {newAppointment.ScheduledDate} has been postponed to {postponedAppointment.ScheduledDate}.", postponedPatientsRecord.PatientID);
            Notification postponedDoctorNotification = new Notification($"The appointment you had scheduled at {newAppointment.ScheduledDate} has been postponed to {postponedAppointment.ScheduledDate}.", postponedAppointment.DoctorID);
            Notification newDoctorNotification = new Notification($"A new urgent appointment has been scheduled for you at {newAppointment.ScheduledDate} in room {HospitalRoomService.GetRoom(newAppointment.HospitalRoomID).Name}.", newAppointment.DoctorID);

            if (postponedPatientsRecord.PatientID == _patient.ID)
            {
                postponedPatientNotification.Opened = true;
                MessageBox.Show(postponedPatientNotification.Message);
            }

            NotificationRepository.Notifications.AddRange(new Notification[] { postponedPatientNotification, postponedDoctorNotification, newDoctorNotification });
            NotificationRepository.Save();

            MessageBox.Show($"Successfully postponed appointment {postponedAppointment.ID} and scheduled a new urgent appointment in its place.");
            this.Close();
        }

        private static HealthRecord FindHealthRecord(Appointment appointment)
        {
            foreach (HealthRecord record in HealthRecordRepository.Records)
            {
                if (record.ID == appointment.HealthRecordID)
                {
                    return record;
                }
            }
            return null;
        }
    }
}
