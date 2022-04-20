using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Secretary : User
    {
        public List<ChangeRequest> changedRequests { get; set; } // look again
        public List<HospitalRoom> _hospitalRooms { get; set; } // look again
        public List<Equipment> _allEquipment { get; set; } // look again
        public List<DynamicEquipmentRequest> _dynamicEquipmentRequests { get; set; } // look again
        public List<VacationRequest> _vacationRequests { get; set; } // look again, same???
        public List<Patient> _patients { get; set; } // look again
    }
}