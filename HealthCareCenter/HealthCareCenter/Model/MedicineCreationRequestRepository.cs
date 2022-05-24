using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    internal class MedicineCreationRequestRepository
    {
        private const string _fileName = "medicineCreationRequests.json";
        public static List<Medicine> Medicines = Load();

        private static List<Medicine> Load()
        {
            try
            {
                List<Medicine> medicines = new List<Medicine>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextMedicine = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\" + _fileName);
                medicines = (List<Medicine>)JsonConvert.DeserializeObject<IEnumerable<Medicine>>(JSONTextMedicine, settings);
                return medicines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };

                using (StreamWriter sw = new StreamWriter(@"..\..\..\data\" + _fileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Medicines);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}