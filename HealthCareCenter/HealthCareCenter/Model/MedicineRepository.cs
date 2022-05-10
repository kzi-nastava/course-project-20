using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class MedicineRepository
    {
        public static List<Medicine> Medicines { get; set; }
        public static List<Medicine> Load()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.DateFormat
            };

            string JSONTextMedicines = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\medicines.json");
            Medicines = (List<Medicine>)JsonConvert.DeserializeObject<IEnumerable<Medicine>>(JSONTextMedicines, settings);
            return Medicines;
        }
        public static void Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\medicines.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Medicines);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
