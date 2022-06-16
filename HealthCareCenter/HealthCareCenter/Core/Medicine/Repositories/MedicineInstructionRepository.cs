using HealthCareCenter.Core.Medicine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Repositories
{
    public class MedicineInstructionRepository : BaseMedicineInstructionRepository
    {
        
        public override List<MedicineInstruction> Load()
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
        public override void Save()
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

        public override int CalculateMaxID()
        {
            LargestID = -1;
            foreach (MedicineInstruction instruction in MedicineInstructions)
            {
                if (instruction.ID > LargestID)
                {
                    LargestID = instruction.ID;
                }
            }
            return LargestID;
        }
    }
}
