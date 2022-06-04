using HealthCareCenter.Enums;
using HealthCareCenter.Model;
using HealthCareCenter.Secretary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCareCenter.Service
{
    public static class AppointmentService
    {
        public static UrgentAppointmentInfo UrgentInfo { get; set; }
        public static OccupiedAppointmentInfo OccupiedInfo { get; set; }

        public static Appointment Get(AppointmentDisplay appointmentDisplay)
        {
            if (AppointmentRepository.Appointments == null)
            {
                AppointmentRepository.Load();
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
      
        public static Appointment Get(int appointmentID)
        {
            if (AppointmentRepository.Appointments == null)
            {
                AppointmentRepository.Load();
            }

            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ID == appointmentID)
                {
                    return appointment;
                }
            }
            return null;
        }

        public static List<Appointment> GetAppointmentsInTheFollowingDays(DateTime date, int numberOfDays)
        {
            List<Appointment> appointments = new List<Appointment>();
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                TimeSpan timeSpan = appointment.ScheduledDate.Subtract(date);
                if (timeSpan.TotalDays <= 3 && timeSpan.TotalDays >= 0)
                {
                    appointments.Add(appointment);
                }
            }
            return appointments;
        } 

        public static List<Appointment> GetPatientUnfinishedAppointments(int patientHealthRecordID)
        {
            if (AppointmentRepository.Appointments == null)
            {
                AppointmentRepository.Load();
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
                AppointmentRepository.Load();
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
                DoctorService.RemoveUnavailableDoctors(availableDoctors, appointment);
                HospitalRoomService.RemoveUnavailableRooms(availableRooms, appointment);
            }
            return true;
        }

        public static bool Schedule(Appointment appointment)
        {
            if (PatientService.CheckCreationTroll(UserService.GetPatientByHealthRecordID(appointment.HealthRecordID)))
            {
                return false;
            }

            AppointmentRepository.Appointments.Add(appointment);
            AppointmentRepository.Save();

            HospitalRoomService.Update(appointment.HospitalRoomID, appointment);
            HospitalRoomRepository.Save();

            return true;
        }

        public static bool Schedule(DateTime scheduleDate, int doctorID, int healthRecordID, int hospitalRoomID)
        {
            if (PatientService.CheckCreationTroll(UserService.GetPatientByHealthRecordID(healthRecordID)))
            {
                return false;
            }

            Appointment appointment = new Appointment
            {
                ID = AppointmentRepository.GetLargestID() + 1,
                Type = AppointmentType.Checkup,
                CreatedDate = DateTime.Now,
                ScheduledDate = scheduleDate,
                Emergency = false,
                DoctorID = doctorID,
                HealthRecordID = healthRecordID,
                HospitalRoomID = hospitalRoomID,
                PatientAnamnesis = null
            };

            AppointmentRepository.Appointments.Add(appointment);
            AppointmentRepository.Save();

            HospitalRoomService.Update(appointment.HospitalRoomID, appointment);
            HospitalRoomRepository.Save();

            return true;
        }

        public static bool Modify(DateTime scheduleDate, DateTime oldScheduleDate, int appointmentID, int doctorID, int patientID, int hospitalRoomID)
        {
            if (PatientService.CheckModificationTroll(UserService.GetPatientByID(patientID)))
            {
                return false;
            }

            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest()
            {
                ID = AppointmentChangeRequestRepository.GetLargestID() + 1,
                AppointmentID = appointmentID,
                RequestType = RequestType.MakeChanges,
                State = RequestState.Waiting,
                NewDate = scheduleDate,
                NewAppointmentType = AppointmentType.Checkup,
                NewDoctorID = doctorID,
                DateSent = DateTime.Now,
                PatientID = patientID
            };
            if (ShouldSendToSecretary(oldScheduleDate))
            {
                foreach (AppointmentChangeRequest changeRequest in AppointmentChangeRequestRepository.Requests)
                {
                    if (changeRequest.AppointmentID == appointmentID)
                    {
                        changeRequest.State = newChangeRequest.State;
                        changeRequest.NewDate = newChangeRequest.NewDate;
                        changeRequest.NewDoctorID = newChangeRequest.NewDoctorID;
                        changeRequest.RequestType = newChangeRequest.RequestType;
                        AppointmentChangeRequestRepository.Save();
                        return true;
                    }
                }
                AppointmentChangeRequestRepository.Requests.Add(newChangeRequest);
                AppointmentChangeRequestRepository.Save();
                return true;
            }
            else
            {
                HospitalRoomService.AddAppointmentToRoom(hospitalRoomID, newChangeRequest.AppointmentID);
                AppointmentChangeRequestService.EditAppointment(newChangeRequest);
            }

            return true;
        }

        public static bool ShouldSendToSecretary(DateTime scheduleDate)
        {
            TimeSpan timeTillAppointment = scheduleDate.Date.Subtract(DateTime.Now.Date);
            return timeTillAppointment.TotalDays <= 2;
        }

        public static bool CancelAppointment(int appointmentID, int patientID, DateTime appointmentScheduleDate)
        {
            if (PatientService.CheckModificationTroll(UserService.GetPatientByID(patientID)))
            {
                return false;
            }

            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest
            {
                ID = AppointmentChangeRequestRepository.GetLargestID() + 1,
                AppointmentID = appointmentID,
                RequestType = RequestType.Delete,
                DateSent = DateTime.Now,
                PatientID = patientID
            };

            if (ShouldSendToSecretary(appointmentScheduleDate))
            {
                foreach (AppointmentChangeRequest changeRequest in AppointmentChangeRequestRepository.Requests)
                {
                    if (changeRequest.AppointmentID == newChangeRequest.AppointmentID)
                    {
                        changeRequest.State = Enums.RequestState.Waiting;
                        changeRequest.NewDate = newChangeRequest.NewDate;
                        changeRequest.NewDoctorID = newChangeRequest.NewDoctorID;
                        changeRequest.RequestType = newChangeRequest.RequestType;
                        AppointmentChangeRequestRepository.Save();
                        return true;
                    }
                }
                AppointmentChangeRequestRepository.Requests.Add(newChangeRequest);
                AppointmentChangeRequestRepository.Save();
                return true;
            }
            else
            {
                AppointmentChangeRequestService.DeleteAppointment(newChangeRequest);
                AppointmentRepository.Save();
            }

            return true;
        }

        public static void PrepareForPotentialPostponing(List<Doctor> doctors, List<HospitalRoom> rooms, DateTime potentialTime, Patient patient)
        {
            List<Appointment> appointments = new List<Appointment>(AppointmentRepository.Appointments);
            for (int i = 0; i < AppointmentRepository.Appointments.Count; i++)
            {
                if (AppointmentRepository.Appointments[i].ScheduledDate.CompareTo(potentialTime) != 0)
                    continue;

                appointments.Remove(AppointmentRepository.Appointments[i]);

                List<Doctor> availableDoctors = new List<Doctor>(doctors);
                List<HospitalRoom> availableRooms = new List<HospitalRoom>(rooms);

                bool isValid = CheckTermAndRemoveUnavailables(potentialTime, availableDoctors, availableRooms, appointments, patient);
                if (isValid && availableDoctors.Count > 0 && availableRooms.Count > 0)
                {
                    AddPostponingInfo(availableDoctors, availableRooms, i);
                }

                appointments.Add(AppointmentRepository.Appointments[i]);
            }
        }

        private static void AddPostponingInfo(List<Doctor> availableDoctors, List<HospitalRoom> availableRooms, int index)
        {
            UrgentInfo.OccupiedAppointments.Add(AppointmentRepository.Appointments[index]);
            UrgentInfo.NewAppointmentsInfo.Add(AppointmentRepository.Appointments[index].ID,
                new Appointment { DoctorID = availableDoctors[0].ID, HospitalRoomID = availableRooms[0].ID });
        }

        public static bool FindTermsAndSchedule(List<Doctor> doctors, AppointmentType type, Patient patient)
        {
            List<HospitalRoom> rooms = HospitalRoomService.GetRoomsOfType(type);
            foreach (string term in TermsService.GetTermsWithinTwoHours())
            {
                DateTime potentialTime = TermsService.CreateTime(term);

                List<Doctor> availableDoctors = new List<Doctor>(doctors);
                List<HospitalRoom> availableRooms = new List<HospitalRoom>(rooms);

                bool isValid = CheckTermAndRemoveUnavailables(potentialTime, availableDoctors, availableRooms, AppointmentRepository.Appointments, patient);

                if (isValid && availableDoctors.Count > 0 && availableRooms.Count > 0)
                {
                    Schedule(new Appointment(potentialTime, availableRooms[0].ID, availableDoctors[0].ID, patient.HealthRecordID, type, true));
                    return true;
                }
                else
                {
                    PrepareForPotentialPostponing(doctors, rooms, potentialTime, patient);
                }
            }
            return false;
        }

        public static bool TryScheduling(AppointmentType type, string doctorType, Patient patient)
        {
            List<Doctor> doctors = DoctorService.GetDoctorsOfType(doctorType);

            UrgentInfo.OccupiedAppointments = new List<Appointment>();
            UrgentInfo.NewAppointmentsInfo = new Dictionary<int, Appointment>();

            if (!FindTermsAndSchedule(doctors, type, patient))
            {
                return false;
            }
            return true;
        }

        public static Appointment Postpone(ref string notification, Patient patient, AppointmentType type, AppointmentDisplay selectedAppointment)
        {
            Appointment postponedAppointment = Get(selectedAppointment);

            Appointment newAppointment = new Appointment(selectedAppointment.ScheduledDate, OccupiedInfo.NewAppointmentsInfo[postponedAppointment.ID].HospitalRoomID, OccupiedInfo.NewAppointmentsInfo[postponedAppointment.ID].DoctorID, patient.HealthRecordID, type, true);
            AppointmentRepository.Appointments.Add(newAppointment);
            postponedAppointment.ScheduledDate = selectedAppointment.PostponedTime;
            AppointmentRepository.Save();

            HospitalRoomService.Update(newAppointment.HospitalRoomID, newAppointment);
            HospitalRoomRepository.Save();

            notification = NotificationService.Send(postponedAppointment, newAppointment, patient);
            return postponedAppointment;
        }

        public static List<AppointmentDisplay> GetAppointmentsForDisplay()
        {
            List<AppointmentDisplay> appointments = new List<AppointmentDisplay>();
            foreach (Appointment appointment in OccupiedInfo.OccupiedAppointments)
            {
                AppointmentDisplay appointmentDisplay = new AppointmentDisplay(appointment.ID, appointment.Type, appointment.ScheduledDate, appointment.Emergency, OccupiedInfo.NewDateOf[appointment.ID]);
                LinkDoctor(appointment, appointmentDisplay);
                LinkPatient(appointment, appointmentDisplay);
                appointments.Add(appointmentDisplay);
            }
            return appointments;
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

        public static List<Appointment> GetByAnamnesisKeyword(string searchKeyword, int healthRecordID)
        {
            List<Appointment> finishedAppointments = AppointmentService.GetPatientFinishedAppointments(healthRecordID);
            if (string.IsNullOrEmpty(searchKeyword))
            {
                return finishedAppointments;
            }

            List<Appointment> appointmentsByKeyword = new List<Appointment>();
            foreach (Appointment appointment in finishedAppointments)
            {
                if (appointment.PatientAnamnesis != null &&
                    appointment.PatientAnamnesis.Comment.ToLower().Contains(searchKeyword))
                {
                    appointmentsByKeyword.Add(appointment);
                }
            }

            return appointmentsByKeyword;
        }

        public static List<Appointment> SortAppointments(List<Appointment> appointments, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "Date":
                    AppointmentDateCompare appointmentDateComparison = new AppointmentDateCompare();
                    appointments.Sort(appointmentDateComparison);
                    break;
                case "Doctor":
                    AppointmentDoctorCompare appointmentDoctorComparison = new AppointmentDoctorCompare();
                    appointments.Sort(appointmentDoctorComparison);
                    break;
                case "Professional area":
                    AppointmentDoctorCompare appointmentProfessionalAreaComparison = new AppointmentDoctorCompare();
                    appointments.Sort(appointmentProfessionalAreaComparison);
                    break;
            }

            return appointments;
        }

        public static bool IsAvailable(DateTime scheduleDate, int doctorID)
        {
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.ScheduledDate.CompareTo(scheduleDate) == 0 && doctorID == appointment.DoctorID)
                {
                    return false;
                }
            }

            return true;
        }
      
        public static void SortPostponableAppointments()
        {
            List<string> allPossibleTerms = TermsService.GetPossibleDailyTerms();
            List<string> terms = TermsService.GetTermsAfterTwoHours(allPossibleTerms);
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

                    foreach (Appointment occupiedAppointment in OccupiedInfo.OccupiedAppointments.ToList())
                    {
                        if (!IsPostponableTo(newTime, occupiedAppointment))
                            continue;

                        sortedAppointments.Add(occupiedAppointment);
                        newDateOf.Add(occupiedAppointment.ID, newTime);
                        OccupiedInfo.OccupiedAppointments.Remove(occupiedAppointment);

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
            OccupiedInfo.OccupiedAppointments = new List<Appointment>(sortedAppointments);
            OccupiedInfo.NewDateOf = new Dictionary<int, DateTime>(newDateOf);
        }
    }
}
