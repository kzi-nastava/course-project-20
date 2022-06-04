using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HealthCareCenter.PatientGUI.Models
{
    class PatientFunctionality
    {
        private const int _creationTrollLimit = 100;
        private const int _modificationTrollLimit = 100;

        private static readonly PatientFunctionality instance;

        private PatientFunctionality() { }

        public static PatientFunctionality GetInstance()
        {
            return instance is null ? new PatientFunctionality() : instance;
        }

        public List<AppointmentTerm> GetAllPossibleTermsForCreateAppointment(int chosenDoctorID, DateTime chosenScheduleDate)
        {
            List<AppointmentTerm> allPossibleTerms = AppointmentTermService.GetDailyTermsFromRange(Constants.StartWorkTime, 0, Constants.EndWorkTime, 0);
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.DoctorID == chosenDoctorID &&
                    appointment.ScheduledDate.Date.CompareTo(chosenScheduleDate.Date) == 0)
                {
                    AppointmentTerm unavailableSchedule = new AppointmentTerm(appointment.ScheduledDate.Hour, appointment.ScheduledDate.Minute);
                    allPossibleTerms.Remove(unavailableSchedule);
                }
            }

            return allPossibleTerms;
        }

        public bool IsAvailable(DateTime scheduleDate, int doctorID)
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

        public void ScheduleAppointment(DateTime scheduleDate, int doctorID, int healthRecordID, int hospitalRoomID)
        {
            if (CheckCreationTroll(UserService.GetPatientByHealthRecordID(healthRecordID)))
            {
                return;
            }

            Appointment newAppointment = new Appointment
            {
                ID = AppointmentRepository.GetLargestID() + 1,
                Type = Enums.AppointmentType.Checkup,
                CreatedDate = DateTime.Now,
                ScheduledDate = scheduleDate,
                Emergency = false,
                DoctorID = doctorID,
                HealthRecordID = healthRecordID,
                HospitalRoomID = hospitalRoomID,
                PatientAnamnesis = null
            };
            HospitalRoomService.AddAppointmentToRoom(hospitalRoomID, newAppointment.ID);
            AppointmentRepository.Appointments.Add(newAppointment);

            AppointmentRepository.Save();
        }

        public void ScheduleAppointment(Appointment newAppointment)
        {
            if (CheckCreationTroll(UserService.GetPatientByHealthRecordID(newAppointment.HealthRecordID)))
            {
                return;
            }

            HospitalRoomService.AddAppointmentToRoom(newAppointment.HospitalRoomID, newAppointment.ID);
            AppointmentRepository.Appointments.Add(newAppointment);

            AppointmentRepository.Save();
        }

        private bool CheckCreationTroll(Patient possibleTroll)
        {
            int creationCount = 0;
            foreach (Appointment appointment in AppointmentRepository.Appointments)
            {
                if (appointment.HealthRecordID == possibleTroll.HealthRecordID)
                {
                    TimeSpan timePassedSinceScheduling = DateTime.Now.Subtract(appointment.CreatedDate);
                    if (timePassedSinceScheduling.TotalDays < 30)
                    {
                        ++creationCount;
                    }
                }
            }

            if (creationCount >= _creationTrollLimit)
            {
                MessageBox.Show("You have been blocked for excessive amounts of appointments created in the last 30 days");
                foreach (Patient patient in UserRepository.Patients)
                {
                    if (possibleTroll.ID == patient.ID)
                    {
                        patient.IsBlocked = true;
                        patient.BlockedBy = Enums.Blocker.System;
                        break;
                    }

                }
                return true;
            }

            return false;
        }

        public bool ShouldSendToSecretary(DateTime scheduleDate)
        {
            TimeSpan timeTillAppointment = scheduleDate.Date.Subtract(DateTime.Now.Date);
            return timeTillAppointment.TotalDays <= 2;
        }

        public void ModifyAppointment(DateTime scheduleDate, DateTime oldScheduleDate, int appointmentID, int doctorID, int patientID, int hospitalRoomID)
        {
            if (CheckModificationTroll(UserService.GetPatientByID(patientID)))
            {
                return;
            }

            if (AppointmentChangeRequestRepository.Requests == null)
            {
                AppointmentChangeRequestRepository.Load();
            }

            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest()
            {
                ID = AppointmentChangeRequestRepository.GetLargestID() + 1,
                AppointmentID = appointmentID,
                RequestType = Enums.RequestType.MakeChanges,
                State = Enums.RequestState.Waiting,
                NewDate = scheduleDate,
                NewAppointmentType = Enums.AppointmentType.Checkup,
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
                        return;
                    }
                }
                AppointmentChangeRequestRepository.Requests.Add(newChangeRequest);
                AppointmentChangeRequestRepository.Save();
                return;
            }
            else
            {
                HospitalRoomService.AddAppointmentToRoom(hospitalRoomID, newChangeRequest.AppointmentID);
                AppointmentChangeRequestService.EditAppointment(newChangeRequest);
            }
        }

        public void CancelAppointment(int appointmentID, int patientID, DateTime appointmentScheduleDate)
        {
            if (CheckModificationTroll(UserService.GetPatientByID(patientID)))
            {
                return;
            }

            if (AppointmentChangeRequestRepository.Requests == null)
            {
                AppointmentChangeRequestRepository.Load();
            }
            AppointmentChangeRequest newChangeRequest = new AppointmentChangeRequest
            {
                ID = AppointmentChangeRequestRepository.GetLargestID() + 1,
                AppointmentID = appointmentID,
                RequestType = Enums.RequestType.Delete,
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
                        return;
                    }
                }
                AppointmentChangeRequestRepository.Requests.Add(newChangeRequest);
                AppointmentChangeRequestRepository.Save();
                return;
            }
            else
            {
                AppointmentChangeRequestService.DeleteAppointment(newChangeRequest);
                AppointmentRepository.Save();
            }
        }

        private bool CheckModificationTroll(Patient possibleTroll)
        {
            int modificationCount = 0;
            foreach (AppointmentChangeRequest changeRequest in AppointmentChangeRequestRepository.Requests)
            {
                if (changeRequest.PatientID == possibleTroll.ID)
                {
                    TimeSpan timePassedSinceScheduling = DateTime.Now.Subtract(changeRequest.DateSent);
                    if (timePassedSinceScheduling.TotalDays < 30)
                    {
                        ++modificationCount;
                    }
                }
            }

            if (modificationCount >= _modificationTrollLimit)
            {
                MessageBox.Show("You have been blocked for excessive amounts of change requests sent in the last 30 days");
                foreach (Patient patient in UserRepository.Patients)
                {
                    if (possibleTroll.ID == patient.ID)
                    {
                        patient.IsBlocked = true;
                        patient.BlockedBy = Enums.Blocker.System;
                        break;
                    }
                }

                return true;
            }

            return false;
        }

        public List<Doctor> SearchDoctorByKeyword(string searchKeyword, string searchCriteria)
        {
            List<Doctor> doctorsByKeyword;
            switch (searchCriteria)
            {
                case "First name":
                    doctorsByKeyword = SearchDoctorByFirstName(searchKeyword);
                    break;
                case "Last name":
                    doctorsByKeyword = SearchDoctorByLastName(searchKeyword);
                    break;
                case "Professional area":
                    doctorsByKeyword = SearchDoctorByProfessionalArea(searchKeyword);
                    break;
                default:
                    doctorsByKeyword = new List<Doctor>();
                    break;
            }

            return doctorsByKeyword;

        }

        private List<Doctor> SearchDoctorByFirstName(string firstName)
        {
            List<Doctor> doctorsByKeyword = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.FirstName.ToLower().Contains(firstName))
                {
                    doctorsByKeyword.Add(doctor);
                }
            }

            return doctorsByKeyword;
        }

        private List<Doctor> SearchDoctorByLastName(string lastName)
        {
            List<Doctor> doctorsByKeyword = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.LastName.ToLower().Contains(lastName))
                {
                    doctorsByKeyword.Add(doctor);
                }
            }

            return doctorsByKeyword;
        }

        private List<Doctor> SearchDoctorByProfessionalArea(string professionalArea)
        {
            List<Doctor> doctorsByKeyword = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type.ToString().ToLower().Contains(professionalArea))
                {
                    doctorsByKeyword.Add(doctor);
                }
            }

            return doctorsByKeyword;
        }

        public List<Doctor> GetSortedDoctorsByCriteria(List<Doctor> doctors, string sortCriteria, string searchCriteria)
        {
            switch (sortCriteria)
            {
                case "Search criteria":
                    switch (searchCriteria)
                    {
                        case "First name":
                            doctors.Sort(new DoctorFirstNameCompare());
                            break;
                        case "Last name":
                            doctors.Sort(new DoctorLastNameCompare());
                            break;
                        case "Professional area":
                            doctors.Sort(new DoctorProfessionalAreaCompare());
                            break;
                    }
                    break;
                case "Rating":
                    doctors.Sort(new DoctorRatingCompare());
                    break;
            }

            return doctors;
        }

        public List<Appointment> SortAppointments(List<Appointment> appointments, string sortCriteria)
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

        public List<Appointment> GetAppointmentsByAnamnesisKeyword(string searchKeyword, int healthRecordID)
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

        public Appointment GetPriorityAppointment(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            Appointment newAppointment = isDoctorPriority
                ? GetDoctorPriorityAppointment(doctorID, healthRecordID, finalScheduleDate, startRange, endRange)
                : GetTimePriorityAppointment(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);
            return newAppointment;
        }

        private Appointment GetDoctorPriorityAppointment(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            Appointment newAppointment = BothPrioritiesSearch(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);

            if (newAppointment == null)
            {
                newAppointment = SameDoctorDifferentTimeSearch(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);
            }

            return newAppointment;
        }

        private Appointment GetTimePriorityAppointment(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            Appointment newAppointment = BothPrioritiesSearch(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);

            if (newAppointment == null)
            {
                newAppointment = DifferentDoctorSameTimeSearch(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);
            }

            return newAppointment;
        }

        private Appointment PrioritySearch(int doctorID, DateTime finalScheduleDate, int healthRecordID, List<AppointmentTerm> possibleTerms)
        {
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            while (date.Date.CompareTo(finalScheduleDate.Date) <= 0)
            {
                foreach (AppointmentTerm term in possibleTerms)
                {
                    bool isAvailable = true;
                    foreach (Appointment appointment in AppointmentRepository.Appointments)
                    {
                        if (doctorID == appointment.DoctorID &&
                            term.Hours == appointment.ScheduledDate.Hour &&
                            term.Minutes == appointment.ScheduledDate.Minute &&
                            appointment.ScheduledDate.Date.CompareTo(date.Date) == 0)
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        string scheduleDateParse = date.ToString().Split(" ")[0] + " " + term.ToString();
                        DateTime scheduleDate = Convert.ToDateTime(scheduleDateParse);

                        int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(scheduleDate, Enums.RoomType.Checkup);
                        if (hospitalRoomID == -1)
                        {
                            continue;
                        }

                        if (AppointmentRepository.Appointments == null)
                        {
                            AppointmentRepository.Load();
                        }

                        Appointment newAppointment = new Appointment()
                        {
                            ID = AppointmentRepository.GetLargestID() + 1,
                            Type = Enums.AppointmentType.Checkup,
                            ScheduledDate = scheduleDate,
                            CreatedDate = DateTime.Now,
                            Emergency = false,
                            DoctorID = doctorID,
                            HealthRecordID = healthRecordID,
                            HospitalRoomID = hospitalRoomID,
                        };

                        return newAppointment;
                    }
                }
                date = date.AddDays(1);
            }

            return null;
        }

        private Appointment BothPrioritiesSearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            // searches for an appointment based on both priorities
            List<AppointmentTerm> possibleSchedules = AppointmentTermService.GetDailyTermsFromRange(startRange, endRange);

            return PrioritySearch(doctorID, finalScheduleDate, healthRecordID, possibleSchedules);

        }

        private Appointment DifferentDoctorSameTimeSearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            // searches for an available appointment in the selected time span
            // for any doctor except the doctor that the patient chose

            List<AppointmentTerm> possibleTerms = AppointmentTermService.GetDailyTermsFromRange(startRange, endRange);
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == doctorID)
                {
                    continue;
                }

                Appointment newAppointment = PrioritySearch(doctor.ID, finalScheduleDate, healthRecordID, possibleTerms);
                if (newAppointment != null)
                {
                    return newAppointment;
                }
            }

            return null;
        }

        private Appointment SameDoctorDifferentTimeSearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            // searches the chosen doctor with every time range except the one given

            List<AppointmentTerm> possibleTerms = AppointmentTermService.GetDailyTermsOppositeOfRange(startRange, endRange);

            return PrioritySearch(doctorID, finalScheduleDate, healthRecordID, possibleTerms);
        }

        public List<Appointment> GetAppointmentsSimilarToPriorites(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            List<Appointment> similarAppointments = new List<Appointment>();

            List<AppointmentTerm> possibleTerms = AppointmentTermService.GetDailyTermsFromRange(startRange, endRange);
            List<AppointmentTerm> oppositePossibleTerms = AppointmentTermService.GetDailyTermsOppositeOfRange(startRange, endRange);

            if (isDoctorPriority)
            {
                // similar to DifferentDoctorSameTimeSearch except it adds the found appointments
                // to a list instead of returning one appointment

                foreach (Doctor doctor in UserRepository.Doctors)
                {
                    similarAppointments.AddRange(AppointmentsSimilarToPrioritySearch(doctor.ID, healthRecordID, finalScheduleDate, possibleTerms));
                    if (similarAppointments.Count >= 3)
                    {
                        break;
                    }
                }
            }
            else
            {
                // similar to SameDoctorDifferentTimeSearch except it adds the found appointments
                // to a list instead of returning one appointment

                similarAppointments.AddRange(AppointmentsSimilarToPrioritySearch(doctorID, healthRecordID, finalScheduleDate, oppositePossibleTerms));
            }

            return similarAppointments;
        }

        private List<Appointment> AppointmentsSimilarToPrioritySearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, List<AppointmentTerm> possibleTerms)
        {
            List<Appointment> similarAppointments = new List<Appointment>();
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            while (date.Date.CompareTo(finalScheduleDate.Date) <= 0 && similarAppointments.Count < 3)
            {
                foreach (AppointmentTerm term in possibleTerms)
                {
                    bool isAvailable = true;
                    foreach (Appointment appointment in AppointmentRepository.Appointments)
                    {
                        if (doctorID == appointment.DoctorID &&
                            term.Hours == appointment.ScheduledDate.Hour &&
                            term.Minutes == appointment.ScheduledDate.Minute &&
                            appointment.ScheduledDate.Date.CompareTo(date.Date) == 0)
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        string scheduleDateParse = date.ToString().Split(" ")[0] + " " + term.ToString();
                        DateTime scheduleDate = Convert.ToDateTime(scheduleDateParse);

                        int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(scheduleDate, Enums.RoomType.Checkup);
                        if (hospitalRoomID == -1)
                        {
                            continue;
                        }

                        if (AppointmentRepository.Appointments == null)
                        {
                            AppointmentRepository.Load();
                        }

                        Appointment possibleAppointment = new Appointment()
                        {
                            ID = AppointmentRepository.GetLargestID() + 1,
                            Type = Enums.AppointmentType.Checkup,
                            ScheduledDate = scheduleDate,
                            CreatedDate = DateTime.Now,
                            Emergency = false,
                            DoctorID = doctorID,
                            HealthRecordID = healthRecordID,
                            HospitalRoomID = hospitalRoomID,
                        };

                        similarAppointments.Add(possibleAppointment);
                        if (similarAppointments.Count >= 3)
                        {
                            break;
                        }
                    }
                }
                date = date.AddDays(1);
            }

            return similarAppointments;
        }

        public string GetMedicineInstructionInfo(MedicineInstruction instruction)
        {
            string medicineInstructionInfo = "";
            medicineInstructionInfo += "Comment:\n";
            medicineInstructionInfo += "- " + instruction.Comment + "\n";
            medicineInstructionInfo += "Consumption time:\n";
            foreach (DateTime consumptionTime in instruction.ConsumptionTime)
            {
                medicineInstructionInfo += "- " + consumptionTime.ToString("t") + "h\n";
            }
            medicineInstructionInfo += "Daily consumption amount:\n";
            medicineInstructionInfo += "- " + instruction.DailyConsumption + "\n";
            medicineInstructionInfo += "Consumption period:\n";
            medicineInstructionInfo += "- " + instruction.ConsumptionPeriod;

            return medicineInstructionInfo;
        }

        public Dictionary<int, Dictionary<int, int>> GetNotificationsSentDict(List<Prescription> patientPrescriptions)
        {
            // dictionary notificationsSent contains int key and Dictionary<int, int> as value
            // key is prescription id, and value is Dictionary<int, int> where the key is medicine instruction
            // id and value is the index of the time in MedicineInstruction.ConsumptionTime that should be checked
            // if it fulfills the criteria for sending a notification

            Dictionary<int, Dictionary<int, int>> notificationsFromPrescriptionsToSend = new Dictionary<int, Dictionary<int, int>>();
            foreach (Prescription prescription in patientPrescriptions)
            {
                Dictionary<int, int> notificationsToSend = new Dictionary<int, int>();
                foreach (int medicineInstructionID in prescription.MedicineInstructionIDs)
                {
                    MedicineInstruction instruction = MedicineInstructionService.GetSingle(medicineInstructionID);
                    notificationsToSend.Add(instruction.ID, 0);
                    foreach (DateTime takingTime in instruction.ConsumptionTime)
                    {
                        TimeSpan timePassedTakingMedicine = takingTime.TimeOfDay.Subtract(DateTime.Now.TimeOfDay);
                        int hoursTilConsumption = (int)Math.Round(timePassedTakingMedicine.TotalHours);
                        if (hoursTilConsumption < 0)
                        {
                            ++notificationsToSend[instruction.ID];
                            continue;
                        }
                        break;
                    }
                }

                notificationsFromPrescriptionsToSend.Add(prescription.ID, notificationsToSend);
            }

            return notificationsFromPrescriptionsToSend;
        }

        public List<string> GetNotifications(Dictionary<int, Dictionary<int, int>> notificationsFromPrescriptionsToSend, Patient patient)
        {
            List<string> notificationsToSend = new List<string>();
            foreach (KeyValuePair<int, Dictionary<int, int>> kvp in notificationsFromPrescriptionsToSend)
            {
                foreach (KeyValuePair<int, int> intructionNotificationTimeIndex in kvp.Value)
                {
                    MedicineInstruction instruction = MedicineInstructionService.GetSingle(intructionNotificationTimeIndex.Key);
                    if (intructionNotificationTimeIndex.Value >= instruction.ConsumptionTime.Count)
                    {
                        continue;
                    }

                    DateTime takingTime = instruction.ConsumptionTime[intructionNotificationTimeIndex.Value];
                    TimeSpan timePassedTakingMedicine = takingTime.TimeOfDay.Subtract(DateTime.Now.TimeOfDay);
                    int hoursTilConsumption = (int)Math.Round(timePassedTakingMedicine.TotalHours);
                    if (hoursTilConsumption > 0 && hoursTilConsumption <= patient.NotificationReceiveTime)
                    {
                        string medicineName = MedicineService.GetName(instruction.MedicineID);
                        string notificationInfo = $"Medicine consumption notification! Medicine: {medicineName}, Time to take: {takingTime.TimeOfDay}, Prescription ID: {kvp.Key}";
                        notificationsToSend.Add(notificationInfo);
                        ++kvp.Value[intructionNotificationTimeIndex.Key];
                    }
                    break;
                }
            }

            return notificationsToSend;
        }

    }
}
