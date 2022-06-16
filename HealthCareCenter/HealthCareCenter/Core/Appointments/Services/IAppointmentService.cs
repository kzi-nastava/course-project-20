using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Urgent;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services
{
    public interface IAppointmentService
    {
        Appointment Get(OccupiedAppointment appointmentDisplay);
        List<Appointment> GetPatientUnfinishedAppointments(int patientHealthRecordID);
        List<Appointment> GetPatientFinishedAppointments(int patientHealthRecordID);
        bool Schedule(Appointment appointment, bool checkTroll);
        bool Schedule(DateTime scheduleDate, int doctorID, int healthRecordID, int hospitalRoomID);
        bool Edit(DateTime scheduleDate, DateTime oldScheduleDate, int appointmentID, int doctorID, int patientID, int hospitalRoomID);
        bool ShouldSendToSecretary(DateTime scheduleDate);
        bool Cancel(int appointmentID, int patientID, DateTime appointmentScheduleDate);
        List<Appointment> GetByAnamnesisKeyword(string searchKeyword, int healthRecordID);
        bool IsAvailable(DateTime scheduleDate, int doctorID);
        List<Appointment> Sort(List<Appointment> appointments, string sortCriteria);
        List<Appointment> GetAppointmentsInTheFollowingDays(DateTime date, int numberOfDays);
        Appointment Get(int appointmentID);
    }
}
