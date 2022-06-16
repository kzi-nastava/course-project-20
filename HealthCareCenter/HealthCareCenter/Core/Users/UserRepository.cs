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
    public class UserRepository : BaseUserRepository
    {
        public UserRepository()
        {
            LoadUsers();
            CalculateMaxID();
        }

        public override int CalculateMaxID()
        {
            LargestID = -1;
            foreach (User user in Users)
            {
                if (user.ID > LargestID)
                {
                    LargestID = user.ID;
                }
            }

            return LargestID;
        }

        public override void LoadUsers()
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

        public override void SavePatients()
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

        public override void SaveDoctors()
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
