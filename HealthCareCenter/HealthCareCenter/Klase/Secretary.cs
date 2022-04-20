using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    class Secretary : User
    {
        public List<ChangeRequest> allChangeRequests { get; set; }
        public List<HospitalRoom> hospitalRooms { get; set; }
        public List<Equipment> allEquipment { get; set; }
        public List<DynamicEquipmentRequest> dynamicEquipmentRequests { get; set; }
        public List<VacationRequest> vacationRequests { get; set; }
        public List<Patient> patients { get; set; }
    }
}
