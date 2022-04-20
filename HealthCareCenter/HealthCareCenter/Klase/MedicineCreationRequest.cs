using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

namespace HealthCareCenter
{
    public class MedicineCreationRequest
    {
        public string _denyComment { get; set; }
        public RequestState _state { get; set; }
        public Medicine _medicine { get; set; } // look again remove???
    }
}