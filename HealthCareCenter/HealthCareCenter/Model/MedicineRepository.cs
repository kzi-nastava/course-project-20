using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class MedicineRepository
    {
        private static List<Medicine> _medicines;
        public static List<Medicine> Medicines
        {
            get
            {
                if (_medicines == null)
                {
                    Load();
                }
                return _medicines;
            }
            set => _medicines = value;
        }
        public static List<Medicine> Load()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.DateFormat
            };

            string JSONTextMedicines = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\medicines.json");
            _medicines = (List<Medicine>)JsonConvert.DeserializeObject<IEnumerable<Medicine>>(JSONTextMedicines, settings);
            return _medicines;
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
