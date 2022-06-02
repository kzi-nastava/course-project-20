using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class MedicineChangeRequest
    {
        public Medicine Medicine { get; set; }
        public string Comment { get; set; }

        public MedicineChangeRequest(Medicine medicine, string comment)
        {
            Medicine = medicine;
            Comment = comment;
        }
    }
}