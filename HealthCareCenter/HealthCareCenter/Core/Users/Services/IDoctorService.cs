using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Users.Services
{
    public interface IDoctorService
    {
        List<Doctor> GetDoctorsOfType(string type);
        List<string> GetTypesOfDoctors();
        Doctor Get(int id);
        void RemoveUnavailableDoctors(List<Doctor> availableDoctors, Appointment appointment);
        List<Doctor> SearchByKeyword(string searchKeyword, string searchCriteria);
        List<Doctor> GetSortedByCriteria(List<Doctor> doctors, string sortCriteria, string searchCriteria);
    }
}
