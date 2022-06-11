using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Secretary.DTO
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public UserDTO() { }
        public UserDTO(int id, string username, string password, string firstName, string lastName, DateTime? dateOfBirth)
        {
            ID = id;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }
    }
}
