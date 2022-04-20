using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class MedicineCreationRequest
    {
        public string denyComment { get; set; }
        public RequestState requestState { get; set; }
        public Medicine medicine { get; set; }
    }
}