using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Model
{
    public class PatientRepository
    {
        private Patient patient;
        public List<Appointment> UnfinishedAppointments { get; set; }  // unfinished appointments of the currently signed patient
        public List<Appointment> AllAppointments { get; set; }
        public List<Doctor> AllDoctors { get; set; }
        public List<AppointmentChangeRequest> AllChangeRequests { get; set; }

        public PatientRepository(Patient patient)
        {
            this.patient = patient;
        }

        public void Load()
        {
            LoadAppointments();
            LoadAppointmentChangeRequests();
            LoadDoctors();
        }

        public int GenerateAppointmentID()
        {
            int allAppointmentsSize = AllAppointments.Count;
            return allAppointmentsSize <= 0 ? 1 : AllAppointments[allAppointmentsSize - 1].ID + 1;
        }

        public int GenerateAppointmentChangeRequestID()
        {
            int allChangeRequestsSize = AllChangeRequests.Count;
            //return allChangeRequestsSize <= 0 ? 1 : PatientDataManager.AllChangeRequests[allChangeRequestsSize - 1].ID + 1;
            return -1;
        }

        public void WriteAll()
        {
            // implement writing of files

            // call after every action (modification or creation)
        }

        private void LoadAppointments()
        {
            // loads all appointments for the patient and adds them to the "Appointments" list property

            UnfinishedAppointments = new List<Appointment>();

            // loading all appointments
            //==============================================================================
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextAllAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\appointments.json");
                AllAppointments = (List<Appointment>)JsonConvert.DeserializeObject<IEnumerable<Appointment>>(JSONTextAllAppointments, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            //==============================================================================

            foreach (Appointment potentialAppointment in AllAppointments)
            {
                if (potentialAppointment.HealthRecordID == patient.HealthRecordID)
                {
                    if (potentialAppointment.AppointmentDate.CompareTo(DateTime.Now) > 0)
                    {
                        UnfinishedAppointments.Add(potentialAppointment);
                    }
                }
            }

        }

        private void LoadAppointmentChangeRequests()
        {
            // loading all appointment change requests
            //==============================================================================
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateTimeFormat
                };

                string JSONTextAllAppointments = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\changerequests.json");
                AllChangeRequests = (List<AppointmentChangeRequest>)JsonConvert.DeserializeObject<IEnumerable<AppointmentChangeRequest>>(JSONTextAllAppointments, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            //==============================================================================
        }

        public string GetDoctorFullNameFromAppointment(Appointment appointment)
        {
            foreach (Doctor potentialDoctor in AllDoctors)
            {
                if (potentialDoctor.ID == appointment.DoctorID)
                {
                    return potentialDoctor.FirstName + " " + potentialDoctor.LastName;
                }
            }

            return null;
        }

        private void LoadDoctors()
        {
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextDoctors = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\doctors.json");
                AllDoctors = (List<Doctor>)JsonConvert.DeserializeObject<IEnumerable<Doctor>>(JSONTextDoctors, settings);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
