using System;
using System.Collections.Generic;
using HealthCareCenter.Enums;

namespace HealthCareCenter.Model
{
    public class Patient : User
    {
        public bool IsBlocked { get; set; }
        public Blocker BlockedBy { get; set; }
        public List<int> PrescriptionIDs { get; set; }
        public int HealthRecordID { get; set; }
        public int NotificationReceiveTime { get; set; }  // how many hours before the patient should take the medicine should they recieve the notification

        //public List<Survey> Surveys { get; set; }
        public Patient() : base() { }
        public Patient(int id, string username, string password, string firstName, string lastName, DateTime dateOfBirth, bool isBlocked, Blocker blockedBy, List<int> prescriptionIDs, int healthRecordID) : base(id, username, password, firstName, lastName, dateOfBirth)
        {
            IsBlocked = isBlocked;
            BlockedBy = blockedBy;
            PrescriptionIDs = prescriptionIDs;
            HealthRecordID = healthRecordID;
            NotificationReceiveTime = 2;
        }
    }
}