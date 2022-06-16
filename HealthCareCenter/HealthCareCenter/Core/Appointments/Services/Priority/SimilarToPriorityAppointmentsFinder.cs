using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Rooms.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services.Priority
{
    class SimilarToPriorityAppointmentsFinder : ISimilarToPriorityAppointmentsFinder
    {
        // create attributes for AppointmentRepository and HospitalRoomService
        private readonly BaseAppointmentRepository _appointmentRepository;

        public SimilarToPriorityAppointmentsFinder(BaseAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public List<Appointment> AppointmentsSimilarToPrioritySearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, List<AppointmentTerm> possibleTerms)
        {
            List<Appointment> similarAppointments = new List<Appointment>();
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            while (date.Date.CompareTo(finalScheduleDate.Date) <= 0 && similarAppointments.Count < 3)
            {
                foreach (AppointmentTerm term in possibleTerms)
                {
                    bool isAvailable = true;
                    foreach (Appointment appointment in _appointmentRepository.Appointments)
                    {
                        if (doctorID == appointment.DoctorID &&
                            term.Hours == appointment.ScheduledDate.Hour &&
                            term.Minutes == appointment.ScheduledDate.Minute &&
                            appointment.ScheduledDate.Date.CompareTo(date.Date) == 0)
                        {
                            isAvailable = false;
                            break;
                        }
                    }

                    if (isAvailable)
                    {
                        string scheduleDateParse = date.ToString().Split(" ")[0] + " " + term.ToString();
                        DateTime scheduleDate = Convert.ToDateTime(scheduleDateParse);

                        int hospitalRoomID = HospitalRoomService.GetAvailableRoomID(scheduleDate, RoomType.Checkup);
                        if (hospitalRoomID == -1)
                        {
                            continue;
                        }

                        Appointment possibleAppointment = new Appointment()
                        {
                            ID = _appointmentRepository.GetLargestID() + 1,
                            Type = AppointmentType.Checkup,
                            ScheduledDate = scheduleDate,
                            CreatedDate = DateTime.Now,
                            Emergency = false,
                            DoctorID = doctorID,
                            HealthRecordID = healthRecordID,
                            HospitalRoomID = hospitalRoomID,
                        };

                        similarAppointments.Add(possibleAppointment);
                        if (similarAppointments.Count >= 3)
                        {
                            break;
                        }
                    }
                }
                date = date.AddDays(1);
            }

            return similarAppointments;
        }
    }
}
