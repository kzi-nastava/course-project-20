using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class PatientDataManager
    {
        public static List<Appointment> UnfinishedAppointments { get; set; }
        public static List<Appointment> AllAppointments { get; set; }
        public static List<Doctor> AllDoctors { get; set; }

        public static void Load(Patient patient)
        {
            LoadAppointments(patient);
            LoadDoctors();
        }

        public static void Write()
        {
            // implement writing of files
        }

        private static void LoadAppointments(Patient patient)
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

        private static void LoadDoctors()
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
