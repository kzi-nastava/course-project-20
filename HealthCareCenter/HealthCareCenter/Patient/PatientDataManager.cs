using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace HealthCareCenter.Patient
{
    public static class PatientDataManager
    {
        public static List<Model.Appointment> Appointments { get; set; }

        public static void Load(Model.Patient patient)
        {
            LoadAppointments(patient);
        }

        private static Model.HealthRecord LoadHealthRecord(Model.Patient patient)
        {
            // loads and returns the patient's health record

            List<Model.HealthRecord> allHealthRecords;

            // loading all health records
            //==============================================================================
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Model.Constants.DateFormat
                };

                String JSONTextHealthRecords = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\healthrecords.json");
                allHealthRecords = (List<Model.HealthRecord>)JsonConvert.DeserializeObject<IEnumerable<Model.HealthRecord>>(JSONTextHealthRecords, settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //==============================================================================

            foreach (Model.HealthRecord potentialHealthRecord in allHealthRecords)
            {
                if (patient.HealthRecordID == potentialHealthRecord.ID)
                {
                    return potentialHealthRecord;
                }
            }

            return null;
        }

        private static void LoadAppointments(Model.Patient patient)
        {
            // loads all appointments for the patient and adds them to the "Appointments" list property

            Model.HealthRecord healthRecord = LoadHealthRecord(patient);
            List<Model.Appointment> allAppointments;

            // loading all appointments
            //==============================================================================
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Model.Constants.DateFormat
                };

                String JSONTextAllAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json");
                allAppointments = (List<Model.Appointment>)JsonConvert.DeserializeObject<IEnumerable<Model.Appointment>>(JSONTextAllAppointments, settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //==============================================================================

            foreach (Model.Appointment potentialAppointment in allAppointments)
            {
                if (healthRecord.AppointmentIDs.Contains(potentialAppointment.ID))
                {
                    Appointments.Add(potentialAppointment);
                }
            }

        }

        public static string GetDoctorFullName(Model.Appointment appointment)
        {
            List<Model.Doctor> allDoctors;

            // loading all appointments
            //==============================================================================
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Model.Constants.DateFormat
                };

                String JSONTextDoctors = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\doctors.json");
                allDoctors = (List<Model.Doctor>)JsonConvert.DeserializeObject<IEnumerable<Model.Doctor>>(JSONTextDoctors, settings);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //==============================================================================

            foreach (Model.Doctor potentialDoctor in allDoctors)
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
