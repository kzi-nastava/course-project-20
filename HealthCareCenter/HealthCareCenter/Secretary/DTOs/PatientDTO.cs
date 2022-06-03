using HealthCareCenter.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.DTOs
{
    public class PatientDTO : UserDTO
    {
        public bool IsBlocked { get; set; }
        public Blocker BlockedBy { get; set; }
        public List<int> PrescriptionIDs { get; set; }
        public int HealthRecordID { get; set; }
        public int NotificationReceiveTime { get; set; }

        public PatientDTO() : base() { }
        public PatientDTO(int id, string username, string password, string firstName, string lastName, DateTime? dateOfBirth, bool isBlocked, Blocker blockedBy, List<int> prescriptionIDs, int healthRecordID) : base(id, username, password, firstName, lastName, dateOfBirth)
        {
            IsBlocked = isBlocked;
            BlockedBy = blockedBy;
            PrescriptionIDs = prescriptionIDs;
            HealthRecordID = healthRecordID;
            NotificationReceiveTime = 2;
        }
    }
}
