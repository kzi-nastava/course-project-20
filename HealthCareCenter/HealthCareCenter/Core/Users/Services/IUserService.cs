using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Users.Services
{
    public interface IUserService
    {
        string GetFullName(int id);
    }
}
