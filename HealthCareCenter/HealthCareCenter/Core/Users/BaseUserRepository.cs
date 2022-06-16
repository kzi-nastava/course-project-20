using HealthCareCenter.Core.Patients;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Users
{
    public abstract class BaseUserRepository
    {
        public int LargestID { get; set; }
        public List<User> Users { get; set; }
        public List<Doctor> Doctors { get; set; }
        public List<Manager> Managers { get; set; }
        public List<Patient> Patients { get; set; }
        public List<Models.Secretary> Secretaries { get; set; }

        public abstract int CalculateMaxID();
        public abstract void LoadUsers();
        public abstract void SavePatients();
        public abstract void SaveDoctors();
    }
}
