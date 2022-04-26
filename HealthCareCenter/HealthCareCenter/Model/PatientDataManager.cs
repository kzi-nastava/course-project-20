using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class PatientDataManager
    {
        public static List<Appointment> UnfinishedAppointments { get; set; }  // unfinished appointments of the currently signed patient
        public static List<Appointment> AllAppointments { get; set; }
        public static List<Doctor> AllDoctors { get; set; }
        public static List<AppointmentChangeRequest> AllChangeRequests { get; set; }

        public static void Load(Patient patient)
        {
            s_loadAppointments(patient);
            s_loadAppointmentChangeRequests(patient);
            s_loadDoctors();
        }

        public static int GenerateAppointmentID()
        {
            int allAppointmentsSize = PatientDataManager.AllAppointments.Count;
            return allAppointmentsSize <= 0 ? 1 : PatientDataManager.AllAppointments[allAppointmentsSize - 1].ID + 1;
        }

        public static int GenerateAppointmentChangeRequestID()
        {
            int allChangeRequestsSize = PatientDataManager.AllChangeRequests.Count;
            //return allChangeRequestsSize <= 0 ? 1 : PatientDataManager.AllChangeRequests[allChangeRequestsSize - 1].ID + 1;
            return -1;
        }

        public static void WriteAll()
        {
            // implement writing of files

            // call after every action (modification or creation)
        }

        private static void s_loadAppointments(Patient patient)
        {
            // loads all appointments for the patient and adds them to the "Appointments" list property

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

        private static void s_loadAppointmentChangeRequests(Patient patient)
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

        public static string GetDoctorFullNameFromAppointment(Appointment appointment)
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

        private static void s_loadDoctors()
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
