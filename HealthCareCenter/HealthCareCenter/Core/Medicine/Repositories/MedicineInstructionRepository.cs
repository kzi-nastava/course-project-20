using HealthCareCenter.Core.Medicine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Repositories
{
    public static class MedicineInstructionRepository
    {
        private static List<MedicineInstruction> _medicineInstructions;
        public static List<MedicineInstruction> MedicineInstructions
        {
            get
            {
                if (_medicineInstructions == null)
                {
                    Load();
                }
                return _medicineInstructions;
            }
            set => _medicineInstructions = value;
        }
        public static int LargestID { get; set; }
        public static List<MedicineInstruction> Load()
        {
            var settings = new JsonSerializerSettings
            {
                DateFormatString = Constants.DateTimeFormat
            };

            string JSONTextMedicineInstructions = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\medicineInstructions.json");
            _medicineInstructions = (List<MedicineInstruction>)JsonConvert.DeserializeObject<IEnumerable<MedicineInstruction>>(JSONTextMedicineInstructions, settings);
            LargestID = _medicineInstructions.Count == 0 ? 0 : MedicineInstructions[^1].ID;
            return _medicineInstructions;
        }
        public static void Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented,
                    DateFormatString = Constants.DateTimeFormat
                };
                using (StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\medicineInstructions.json"))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, MedicineInstructions);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
