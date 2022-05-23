using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Secretary : User
    {
        public List<int> DynamicEquipmentRequestIDs { get; set; }
    }
}