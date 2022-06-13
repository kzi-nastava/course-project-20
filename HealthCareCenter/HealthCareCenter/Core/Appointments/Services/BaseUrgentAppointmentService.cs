using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Patients.Models;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services
{
    public abstract class BaseUrgentAppointmentService
    {
        public UrgentAppointmentInfo UrgentInfo { get; set; }
        public OccupiedAppointmentInfo OccupiedInfo { get; set; }

        public abstract bool CheckTermAndRemoveUnavailables(DateTime potentialTime, List<Doctor> availableDoctors, List<HospitalRoom> availableRooms, List<Appointment> appointments, Patient patient);
        public abstract void PrepareForPotentialPostponing(List<Doctor> doctors, List<HospitalRoom> rooms, DateTime potentialTime, Patient patient);
        public abstract bool GetTermsAndSchedule(List<Doctor> doctors, AppointmentType type, Patient patient);
        public abstract bool TryScheduling(AppointmentType type, string doctorType, Patient patient);
        public abstract Appointment Postpone(ref string notification, Patient patient, AppointmentType type, AppointmentDisplay selectedAppointment);
        public abstract List<AppointmentDisplay> GetAppointmentsForDisplay();
        public abstract bool IsPostponableTo(DateTime newTime, Appointment occupiedAppointment);
        public abstract void SortPostponableAppointments();
    }
}
