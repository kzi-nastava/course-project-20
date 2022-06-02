using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Secretary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCareCenter.Service
{
    public class AppointmentService
    {
        public static Appointment Find(AppointmentDisplay appointmentDisplay)
        {
            if (AppointmentRepository.Appointments == null)
            {
                return null;
            }

            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID == appointmentDisplay.ID)
                {
                    return appointment;
                }
            }
            return null;
        }

        public static List<Appointment> GetPatientUnfinishedAppointments(int patientHealthRecordID)
        {
            if (AppointmentRepository.Appointments == null)
            {
                return null;
            }

            List<Appointment> unfinishedAppointments = new List<Appointment>();
            foreach (Appointment potentialAppointment in AppointmentRepository.Appointments)
            {
                if (potentialAppointment.HealthRecordID == patientHealthRecordID)
                {
                    if (potentialAppointment.ScheduledDate.CompareTo(DateTime.Now) > 0)
                    {
                        unfinishedAppointments.Add(potentialAppointment);
                    }
                }
            }

            return unfinishedAppointments;
        }

        public static List<Appointment> GetPatientFinishedAppointments(int patientHealthRecordID)
        {
            if (AppointmentRepository.Appointments == null)
            {
                return null;
            }

            List<Appointment> finishedAppointments = new List<Appointment>();
            foreach (Appointment potentialAppointment in AppointmentRepository.Appointments)
            {
                if (potentialAppointment.HealthRecordID == patientHealthRecordID)
                {
                    if (potentialAppointment.ScheduledDate.CompareTo(DateTime.Now) < 0)
                    {
                        finishedAppointments.Add(potentialAppointment);
                    }
                }
            }

            return finishedAppointments;
        }

        public static List<string> GetAvailableTerms(int doctorID, DateTime when)
        {
            List<string> terms = TermsService.GetPossibleDailyTerms();
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.DoctorID != doctorID || appointment.ScheduledDate.Date.CompareTo(when) != 0)
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
            return terms;
        }

        public static bool CheckTermAndRemoveUnavailables(DateTime potentialTime, List<Doctor> availableDoctors, List<HospitalRoom> availableRooms, List<Appointment> appointments, Patient patient)
        {
            foreach (Appointment appointment in appointments)
            {
                if (appointment.ScheduledDate.CompareTo(potentialTime) != 0)
                {
                    continue;
                }
                if (appointment.HealthRecordID == patient.HealthRecordID)
                {
                    return false;
                }
                UserService.RemoveUnavailableDoctors(availableDoctors, appointment);
                HospitalRoomService.RemoveUnavailableRooms(availableRooms, appointment);
            }
            return true;
        }

        public static void Schedule(AppointmentType type, DateTime potentialTime, Doctor doctor, HospitalRoom room, Patient patient)
        {
            Appointment appointment = new Appointment(potentialTime, room.ID, doctor.ID, patient.HealthRecordID, type, true);
            AppointmentRepository.Appointments.Add(appointment);
            AppointmentRepository.Save();

            HospitalRoomService.Update(room.ID, appointment);
            HospitalRoomRepository.Save();
        }

        public static void PrepareForPotentialPostponing(List<Doctor> doctors, List<HospitalRoom> rooms, DateTime potentialTime, Patient patient, ref List<Appointment> occupiedAppointments, ref Dictionary<int, Appointment> newAppointmentsInfo)
        {
            List<Appointment> appointments = new List<Appointment>(AppointmentRepository.Appointments);
            for (int i = 0; i < AppointmentRepository.Appointments.Count; i++)
            {
                if (AppointmentRepository.Appointments[i].ScheduledDate.CompareTo(potentialTime) != 0)
                    continue;

                appointments.Remove(AppointmentRepository.Appointments[i]);

                List<Doctor> availableDoctors = new List<Doctor>(doctors);
                List<HospitalRoom> availableRooms = new List<HospitalRoom>(rooms);

                bool isValid = AppointmentService.CheckTermAndRemoveUnavailables(potentialTime, availableDoctors, availableRooms, appointments, patient);
                if (isValid && availableDoctors.Count > 0 && availableRooms.Count > 0)
                {
                    AddPostponingInfo(availableDoctors, availableRooms, i, ref occupiedAppointments, ref newAppointmentsInfo);
                }

                appointments.Add(AppointmentRepository.Appointments[i]);
            }
        }

        private static void AddPostponingInfo(List<Doctor> availableDoctors, List<HospitalRoom> availableRooms, int i, ref List<Appointment> occupiedAppointments, ref Dictionary<int, Appointment> newAppointmentsInfo)
        {
            occupiedAppointments.Add(AppointmentRepository.Appointments[i]);
            newAppointmentsInfo.Add(AppointmentRepository.Appointments[i].ID,
                new Appointment { DoctorID = availableDoctors[0].ID, HospitalRoomID = availableRooms[0].ID });
        }

        public static bool FindTermsAndSchedule(List<Doctor> doctors, AppointmentType type, List<HospitalRoom> rooms, List<string> terms, Patient patient, ref List<Appointment> occupiedAppointments, ref Dictionary<int, Appointment> newAppointmentsInfo)
        {
            foreach (string term in terms)
            {
                DateTime potentialTime = TermsService.CreatePotentialTime(term);

                List<Doctor> availableDoctors = new List<Doctor>(doctors);
                List<HospitalRoom> availableRooms = new List<HospitalRoom>(rooms);

                bool isValid = CheckTermAndRemoveUnavailables(potentialTime, availableDoctors, availableRooms, AppointmentRepository.Appointments, patient);

                if (isValid && availableDoctors.Count > 0 && availableRooms.Count > 0)
                {
                    Schedule(type, potentialTime, availableDoctors[0], availableRooms[0], patient);
                    return true;
                }
                else
                {
                    PrepareForPotentialPostponing(doctors, rooms, potentialTime, patient, ref occupiedAppointments, ref newAppointmentsInfo);
                }
            }
            return false;
        }

        public static bool TryScheduling(AppointmentType type, string doctorType, Patient patient, ref List<Appointment> occupiedAppointments, ref Dictionary<int, Appointment> newAppointmentsInfo)
        {
            List<Doctor> doctors = UserService.GetDoctorsOfType(doctorType);

            List<HospitalRoom> rooms = HospitalRoomService.GetRoomsOfType(type);
            List<string> terms = TermsService.GetTermsWithinTwoHours();

            occupiedAppointments = new List<Appointment>();
            newAppointmentsInfo = new Dictionary<int, Appointment>();

            if (!FindTermsAndSchedule(doctors, type, rooms, terms, patient, ref occupiedAppointments, ref newAppointmentsInfo))
            {
                return false;
            }
            return true;
        }

        public static Appointment Postpone(ref string notification, Patient patient, Dictionary<int, Appointment> newAppointmentsInfo, AppointmentType type, AppointmentDisplay appointmentDisplay)
        {
            Appointment postponedAppointment = AppointmentService.Find(appointmentDisplay);

            Appointment newAppointment = new Appointment(appointmentDisplay.ScheduledDate, newAppointmentsInfo[postponedAppointment.ID].HospitalRoomID, newAppointmentsInfo[postponedAppointment.ID].DoctorID, patient.HealthRecordID, type, true);
            AppointmentRepository.Appointments.Add(newAppointment);
            postponedAppointment.ScheduledDate = appointmentDisplay.PostponedTime;
            AppointmentRepository.Save();

            HospitalRoomService.Update(newAppointment.HospitalRoomID, newAppointment);
            HospitalRoomRepository.Save();

            notification = NotificationService.SendNotifications(postponedAppointment, newAppointment, patient);
            return postponedAppointment;
        }

        public static List<AppointmentDisplay> GetAppointmentsForDisplay(List<Appointment> occupiedAppointments, Dictionary<int, DateTime> newDateOf)
        {
            List<AppointmentDisplay> appointmentsForDisplay = new List<AppointmentDisplay>();
            foreach (Appointment appointment in occupiedAppointments)
            {
                AppointmentDisplay appointmentDisplay = new AppointmentDisplay(appointment.ID, appointment.Type, appointment.ScheduledDate, appointment.Emergency, newDateOf[appointment.ID]);
                LinkDoctor(appointment, appointmentDisplay);
                LinkPatient(appointment, appointmentDisplay);
                appointmentsForDisplay.Add(appointmentDisplay);
            }
            return appointmentsForDisplay;
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

        public static bool IsPostponableTo(DateTime newTime, Appointment occupiedAppointment)
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

        public static void SortAppointments(ref List<Appointment> occupiedAppointments, ref Dictionary<int, DateTime> dateOf)
        {
            List<string> allPossibleTerms = TermsService.GetPossibleDailyTerms();
            List<string> terms = TermsService.FormTodaysPossibleTerms(allPossibleTerms);
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

                    foreach (Appointment occupiedAppointment in occupiedAppointments.ToList())
                    {
                        if (!AppointmentService.IsPostponableTo(newTime, occupiedAppointment))
                            continue;

                        sortedAppointments.Add(occupiedAppointment);
                        newDateOf.Add(occupiedAppointment.ID, newTime);
                        occupiedAppointments.Remove(occupiedAppointment);

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
            occupiedAppointments = new List<Appointment>(sortedAppointments);
            dateOf = new Dictionary<int, DateTime>(newDateOf);
        }
    }
}
