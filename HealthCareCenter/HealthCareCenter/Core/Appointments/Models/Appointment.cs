using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using HealthCareCenter.Core.Appointments.Repository;

namespace HealthCareCenter.Core.Appointments.Models
{
    public class Appointment
    {
        public int ID { get; set; }
        public AppointmentType Type { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Emergency { get; set; }
        public int DoctorID { get; set; }
        public int HealthRecordID { get; set; }
        public int HospitalRoomID { get; set; }
        public Anamnesis PatientAnamnesis { get; set; }

        public Appointment() { }
        public Appointment(DateTime scheduledDate, int roomID, int doctorID, int healthRecordID, AppointmentType type, bool emergency)
        {
            ID = ++AppointmentRepository.LargestID;
            CreatedDate = DateTime.Now;
            ScheduledDate = scheduledDate;
            DoctorID = doctorID;
            HospitalRoomID = roomID;
            Emergency = emergency;
            Type = type;
            HealthRecordID = healthRecordID;
            PatientAnamnesis = new Anamnesis();
        }
    }
}