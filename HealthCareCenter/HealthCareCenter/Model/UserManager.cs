using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class UserManager
    {
        public static List<User> Users { get; set; }
        public static List<Doctor> Doctors { get; set; }
        public static List<Manager> Managers { get; set; }
        public static List<Patient> Patients { get; set; }
        public static List<Secretary> Secretaries { get; set; }

        public static void LoadUsers()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                String JSONTextDoctors = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\doctors.json");
                Doctors = (List<Doctor>) JsonConvert.DeserializeObject<IEnumerable<Doctor>>(JSONTextDoctors, settings);
                String JSONTextManagers = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\managers.json");
                Managers = (List<Manager>) JsonConvert.DeserializeObject<IEnumerable<Manager>>(JSONTextManagers, settings);
                String JSONTextPatients = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\patients.json");
                Patients = (List<Patient>) JsonConvert.DeserializeObject<IEnumerable<Patient>>(JSONTextPatients, settings);
                String JSONTextSecretaries = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\secretaries.json");
                Secretaries = (List<Secretary>) JsonConvert.DeserializeObject<IEnumerable<Secretary>>(JSONTextSecretaries, settings);

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
    }
}
