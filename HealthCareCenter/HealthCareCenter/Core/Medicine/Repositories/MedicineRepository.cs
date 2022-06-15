using HealthCareCenter.Core.Medicine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Repositories
{
    public class MedicineRepository : BaseMedicineRepository
    {
        public override List<Models.Medicine> Load()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.DateFormat
            };

            string JSONTextMedicines = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\medicines.json");
            _medicines = (List<Models.Medicine>)JsonConvert.DeserializeObject<IEnumerable<Models.Medicine>>(JSONTextMedicines, settings);
            return _medicines;
        }
        public override void Save()
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
