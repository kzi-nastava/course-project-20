using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Urgent.DTO;
using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Urgent.Services
{
    public abstract class BaseUrgentAppointmentService
    {
        public OccupiedAppointmentsDTO UrgentInfo { get; set; }
        public PostponableAppointmentsDTO OccupiedInfo { get; set; }

        public abstract bool CheckTermAndRemoveUnavailables(DateTime potentialTime, List<Doctor> availableDoctors, List<HospitalRoom> availableRooms, List<Appointment> appointments, Patient patient);
        public abstract void PrepareForPotentialPostponing(List<Doctor> doctors, List<HospitalRoom> rooms, DateTime potentialTime, Patient patient);
        public abstract bool GetTermsAndSchedule(List<Doctor> doctors, AppointmentType type, Patient patient);
        public abstract bool TryScheduling(AppointmentType type, string doctorType, Patient patient);
        public abstract Appointment Postpone(ref string notification, Patient patient, AppointmentType type, OccupiedAppointment selectedAppointment);
        public abstract List<OccupiedAppointment> GetAppointmentsForDisplay();
        public abstract bool IsPostponableTo(DateTime newTime, Appointment occupiedAppointment);
        public abstract void SortPostponableAppointments();
    }
}
