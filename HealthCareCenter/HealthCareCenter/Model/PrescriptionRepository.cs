using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class PrescriptionRepository
    {
        private static List<Prescription> _prescriptions;
        public static List<Prescription> Prescriptions
        {
            get
            {
                if (_prescriptions == null)
                {
                    Load();
                }
                return _prescriptions;
            }
            set => _prescriptions = value;
        }
        public static List<MedicineInstruction> MedicineInstructions { get; set; }
        public static int LargestID { get; set; }
        public static List<Prescription> Load()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.TimeFormat
            };

            string JSONTextPrescriptions = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\prescriptions.json");
            _prescriptions = (List<Prescription>)JsonConvert.DeserializeObject<IEnumerable<Prescription>>(JSONTextPrescriptions, settings);
            LargestID = _prescriptions.Count == 0 ? 0 :Prescriptions[^1].ID;
            return _prescriptions;
        }
        public static void Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.TimeFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\prescriptions.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Prescriptions);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
