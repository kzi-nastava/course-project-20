using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public class PatientRepository
    {
        private Patient patient;
        public List<Appointment> UnfinishedAppointments { get; set; }  // unfinished appointments of the currently signed patient
        public List<Appointment> AllAppointments { get; set; }
        public List<Doctor> AllDoctors { get; set; }
        public List<AppointmentChangeRequest> AllChangeRequests { get; set; }

        public PatientRepository(Patient patient)
        {
            this.patient = patient;
        }

        public void Load()
        {
            LoadAppointments();
            LoadAppointmentChangeRequests();
            LoadDoctors();
        }

        public int GenerateAppointmentID()
        {
            int allAppointmentsSize = AllAppointments.Count;
            return allAppointmentsSize <= 0 ? 1 : AllAppointments[allAppointmentsSize - 1].ID + 1;
        }

        public int GenerateAppointmentChangeRequestID()
        {
            int allChangeRequestsSize = AllChangeRequests.Count;
            return allChangeRequestsSize <= 0 ? 1 : AllChangeRequests[allChangeRequestsSize - 1].ID + 1;
        }

        public void WritePatient()
        {
            List<Patient> allPatients;

            // loading all patients
            //==============================================================================
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextAllAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\patients.json");
                allPatients = (List<Patient>)JsonConvert.DeserializeObject<IEnumerable<Patient>>(JSONTextAllAppointments, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            //==============================================================================

            // changing the patient info inside of allPatients list
            //==============================================================================
            foreach (Patient loadedPatient in allPatients)
            {
                if (loadedPatient.ID == patient.ID)
                {
                    loadedPatient.IsBlocked = patient.IsBlocked;
                    loadedPatient.BlockedBy = patient.BlockedBy;
                    loadedPatient.ReferralIDs = patient.ReferralIDs;
                    loadedPatient.HealthRecordID = patient.HealthRecordID;
                    break;
                }
            }
            //==============================================================================

            // writing all patients
            //==============================================================================
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\patients.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, allPatients);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //==============================================================================
        }

        public void WriteAppointments()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateTimeFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, AllAppointments);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteChangeRequests()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateTimeFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\changerequests.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, AllChangeRequests);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadAppointments()
        {
            // loads all appointments to AllAppointments and filters the patient's unfinished appointments and it
            // adds them to UnfinishedAppointments

            UnfinishedAppointments = new List<Appointment>();

            // loading all appointments
            //==============================================================================
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextAllAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json");
                AllAppointments = (List<Appointment>)JsonConvert.DeserializeObject<IEnumerable<Appointment>>(JSONTextAllAppointments, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            //==============================================================================

            foreach (Appointment potentialAppointment in AllAppointments)
            {
                if (potentialAppointment.HealthRecordID == patient.HealthRecordID)
                {
                    if (potentialAppointment.AppointmentDate.CompareTo(DateTime.Now) > 0)
                    {
                        UnfinishedAppointments.Add(potentialAppointment);
                    }
                }
            }

        }

        private void LoadAppointmentChangeRequests()
        {
            // loading all appointment change requests
            //==============================================================================
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextAllAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\changerequests.json");
                AllChangeRequests = (List<AppointmentChangeRequest>)JsonConvert.DeserializeObject<IEnumerable<AppointmentChangeRequest>>(JSONTextAllAppointments, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            //==============================================================================
        }

        public string GetDoctorFullNameFromAppointment(Appointment appointment)
        {
            foreach (Doctor potentialDoctor in AllDoctors)
            {
                if (potentialDoctor.ID == appointment.DoctorID)
                {
                    return potentialDoctor.FirstName + " " + potentialDoctor.LastName;
                }
            }

            return null;
        }

        private void LoadDoctors()
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextDoctors = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\doctors.json");
                AllDoctors = (List<Doctor>)JsonConvert.DeserializeObject<IEnumerable<Doctor>>(JSONTextDoctors, settings);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
