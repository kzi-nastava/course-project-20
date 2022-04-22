using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter.Model
{
    public class MedicineCreationRequest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }
        public string Manufacturer { get; set; }
        public string DenyComment { get; set; }
        public RequestState State { get; set; }
    }
}