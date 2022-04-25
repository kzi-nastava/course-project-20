using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class PatientDataManager
    {
        public static List<Appointment> Appointments { get; set; }

        public static void Load(Patient patient)
        {
            LoadAppointments(patient);
        }

        public static void Write()
        {
            // implement writing of files
        }

        private static HealthRecord LoadHealthRecord(Patient patient)
        {
            // loads and returns the patient's health record

            List<HealthRecord> allHealthRecords;

            // loading all health records
            //==============================================================================
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextHealthRecords = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\healthrecords.json");
                allHealthRecords = (List<HealthRecord>)JsonConvert.DeserializeObject<IEnumerable<HealthRecord>>(JSONTextHealthRecords, settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //==============================================================================

            foreach (HealthRecord potentialHealthRecord in allHealthRecords)
            {
                if (patient.HealthRecordID == potentialHealthRecord.ID)
                {
                    return potentialHealthRecord;
                }
            }

            return null;
        }

        private static void LoadAppointments(Patient patient)
        {
            // loads all appointments for the patient and adds them to the "Appointments" list property

            Appointments = new List<Appointment>();
            HealthRecord healthRecord = LoadHealthRecord(patient);
            List<Appointment> allAppointments;

            // loading all appointments
            //==============================================================================
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextAllAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json");
                allAppointments = (List<Appointment>)JsonConvert.DeserializeObject<IEnumerable<Appointment>>(JSONTextAllAppointments, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            //==============================================================================

            foreach (Appointment potentialAppointment in allAppointments)
            {
                if (healthRecord.AppointmentIDs.Contains(potentialAppointment.ID))
                {
                    Appointments.Add(potentialAppointment);
                }
            }

        }

        public static string GetDoctorFullName(Appointment appointment)
        {
            List<Doctor> allDoctors;

            // loading all appointments
            //==============================================================================
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextDoctors = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\doctors.json");
                allDoctors = (List<Doctor>)JsonConvert.DeserializeObject<IEnumerable<Doctor>>(JSONTextDoctors, settings);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //==============================================================================

            foreach (Doctor potentialDoctor in allDoctors)
            {
                if (potentialDoctor.ID == appointment.DoctorID)
                {
                    return potentialDoctor.FirstName + " " + potentialDoctor.LastName;
                }
            }

            return null;
        }
    }
}
