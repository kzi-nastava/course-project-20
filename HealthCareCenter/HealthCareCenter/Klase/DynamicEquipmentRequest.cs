using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    class DynamicEquipmentRequest
    {
        public RequestState state { get; set; }
        public Secretary secretary { get; set; }
        public List<Equipment> equipment { get; set; }
        public List<int> amount { get; set; }
    }
}
