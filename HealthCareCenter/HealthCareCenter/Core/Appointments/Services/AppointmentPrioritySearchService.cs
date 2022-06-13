using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Appointments.Repository;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Users;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Appointments.Services
{
    class AppointmentPrioritySearchService
    {
        public static Appointment GetPriorityAppointment(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            Appointment newAppointment = isDoctorPriority
                ? GetDoctorPriorityAppointment(doctorID, healthRecordID, finalScheduleDate, startRange, endRange)
                : GetTimePriorityAppointment(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);
            return newAppointment;
        }

        private static Appointment GetDoctorPriorityAppointment(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            Appointment newAppointment = BothPrioritiesSearch(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);

            if (newAppointment == null)
            {
                newAppointment = SameDoctorDifferentTimeSearch(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);
            }

            return newAppointment;
        }

        private static Appointment GetTimePriorityAppointment(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            Appointment newAppointment = BothPrioritiesSearch(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);

            if (newAppointment == null)
            {
                newAppointment = DifferentDoctorSameTimeSearch(doctorID, healthRecordID, finalScheduleDate, startRange, endRange);
            }

            return newAppointment;
        }

        private static Appointment PrioritySearch(int doctorID, DateTime finalScheduleDate, int healthRecordID, List<AppointmentTerm> possibleTerms)
        {
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            while (date.Date.CompareTo(finalScheduleDate.Date) <= 0)
            {
                foreach (AppointmentTerm term in possibleTerms)
                {
                    bool isAvailable = true;
                    foreach (Appointment appointment in AppointmentRepository.Appointments)
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

                        if (AppointmentRepository.Appointments == null)
                        {
                            AppointmentRepository.Load();
                        }

                        Appointment newAppointment = new Appointment()
                        {
                            ID = AppointmentRepository.GetLargestID() + 1,
                            Type = AppointmentType.Checkup,
                            ScheduledDate = scheduleDate,
                            CreatedDate = DateTime.Now,
                            Emergency = false,
                            DoctorID = doctorID,
                            HealthRecordID = healthRecordID,
                            HospitalRoomID = hospitalRoomID,
                        };

                        return newAppointment;
                    }
                }
                date = date.AddDays(1);
            }

            return null;
        }

        private static Appointment BothPrioritiesSearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            // searches for an appointment based on both priorities
            List<AppointmentTerm> possibleSchedules = AppointmentTermService.GetDailyTermsFromRange(startRange, endRange);

            return PrioritySearch(doctorID, finalScheduleDate, healthRecordID, possibleSchedules);

        }

        private static Appointment DifferentDoctorSameTimeSearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            // searches for an available appointment in the selected time span
            // for any doctor except the doctor that the patient chose

            List<AppointmentTerm> possibleTerms = AppointmentTermService.GetDailyTermsFromRange(startRange, endRange);
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.ID == doctorID)
                {
                    continue;
                }

                Appointment newAppointment = PrioritySearch(doctor.ID, finalScheduleDate, healthRecordID, possibleTerms);
                if (newAppointment != null)
                {
                    return newAppointment;
                }
            }

            return null;
        }

        private static Appointment SameDoctorDifferentTimeSearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            // searches the chosen doctor with every time range except the one given

            List<AppointmentTerm> possibleTerms = AppointmentTermService.GetDailyTermsOppositeOfRange(startRange, endRange);

            return PrioritySearch(doctorID, finalScheduleDate, healthRecordID, possibleTerms);
        }

        public static List<Appointment> GetAppointmentsSimilarToPriorites(bool isDoctorPriority, int doctorID, int healthRecordID, DateTime finalScheduleDate, AppointmentTerm startRange, AppointmentTerm endRange)
        {
            List<Appointment> similarAppointments = new List<Appointment>();

            List<AppointmentTerm> possibleTerms = AppointmentTermService.GetDailyTermsFromRange(startRange, endRange);
            List<AppointmentTerm> oppositePossibleTerms = AppointmentTermService.GetDailyTermsOppositeOfRange(startRange, endRange);

            if (isDoctorPriority)
            {
                // similar to DifferentDoctorSameTimeSearch except it adds the found appointments
                // to a list instead of returning one appointment

                foreach (Doctor doctor in UserRepository.Doctors)
                {
                    similarAppointments.AddRange(AppointmentsSimilarToPrioritySearch(doctor.ID, healthRecordID, finalScheduleDate, possibleTerms));
                    if (similarAppointments.Count >= 3)
                    {
                        break;
                    }
                }
            }
            else
            {
                // similar to SameDoctorDifferentTimeSearch except it adds the found appointments
                // to a list instead of returning one appointment

                similarAppointments.AddRange(AppointmentsSimilarToPrioritySearch(doctorID, healthRecordID, finalScheduleDate, oppositePossibleTerms));
            }

            return similarAppointments;
        }

        private static List<Appointment> AppointmentsSimilarToPrioritySearch(int doctorID, int healthRecordID, DateTime finalScheduleDate, List<AppointmentTerm> possibleTerms)
        {
            List<Appointment> similarAppointments = new List<Appointment>();
            DateTime date = DateTime.Now;
            date = date.AddDays(1);
            while (date.Date.CompareTo(finalScheduleDate.Date) <= 0 && similarAppointments.Count < 3)
            {
                foreach (AppointmentTerm term in possibleTerms)
                {
                    bool isAvailable = true;
                    foreach (Appointment appointment in AppointmentRepository.Appointments)
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

                        if (AppointmentRepository.Appointments == null)
                        {
                            AppointmentRepository.Load();
                        }

                        Appointment possibleAppointment = new Appointment()
                        {
                            ID = AppointmentRepository.GetLargestID() + 1,
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
