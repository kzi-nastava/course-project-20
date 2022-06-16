using HealthCareCenter.Core.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Users.Services
{
    public interface IDoctorSearchService
    {
        List<Doctor> SearchByFirstName(string firstName);
        List<Doctor> SearchByLastName(string lastName);
        List<Doctor> SearchByProfessionalArea(string professionalArea);
    }
}
