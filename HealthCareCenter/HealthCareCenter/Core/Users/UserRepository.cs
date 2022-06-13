using HealthCareCenter.Core;
using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Users.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Users
{
    public static class UserRepository
    {
        public static List<User> Users { get; set; }
        public static List<Doctor> Doctors { get; set; }
        public static List<Manager> Managers { get; set; }
        public static List<Patient> Patients { get; set; }
        public static List<Models.Secretary> Secretaries { get; set; }

        public static int maxID = -1;

        public static void CalculateMaxID()
        {
            maxID = -1;
            foreach (User user in Users)
            {
                if (user.ID > maxID)
                {
                    maxID = user.ID;
                }
            }
        }

        public static void LoadUsers()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextDoctors = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\doctors.json");
                Doctors = (List<Doctor>)JsonConvert.DeserializeObject<IEnumerable<Doctor>>(JSONTextDoctors, settings);
                string JSONTextManagers = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\managers.json");
                Managers = (List<Manager>)JsonConvert.DeserializeObject<IEnumerable<Manager>>(JSONTextManagers, settings);
                string JSONTextPatients = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\patients.json");
                Patients = (List<Patient>)JsonConvert.DeserializeObject<IEnumerable<Patient>>(JSONTextPatients, settings);
                string JSONTextSecretaries = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\secretaries.json");
                Secretaries = (List<Models.Secretary>)JsonConvert.DeserializeObject<IEnumerable<Models.Secretary>>(JSONTextSecretaries, settings);

                Users = new List<User>();
                Users.AddRange(Doctors);
                Users.AddRange(Managers);
                Users.AddRange(Patients);
                Users.AddRange(Secretaries);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SavePatients()
        {
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
                    serializer.Serialize(writer, Patients);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveDoctors()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\doctors.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Doctors);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
