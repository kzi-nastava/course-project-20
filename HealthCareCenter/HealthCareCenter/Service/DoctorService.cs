using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HealthCareCenter.Model;
using System.Data;
using HealthCareCenter.Enums;
using System.Globalization;
using System.ComponentModel;

namespace HealthCareCenter.Service
{
    public static class DoctorService
    {
        public static List<Doctor> GetDoctorsByType(string chosenType)
        {
            List<Doctor> doctors = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type.Equals(chosenType))
                    doctors.Add(doctor);
            }
            return doctors;
        }
        public static List<Doctor> GetDoctorsBySpecialization(string chosenType)
        {
            List<Doctor>doctors = new List<Doctor>();
            foreach (Doctor doctor in UserRepository.Doctors)
            {
                if (doctor.Type.Equals(chosenType))
                    doctors.Add(doctor);
            }
            return doctors;
        }
    }
}
