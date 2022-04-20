using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Medicine
    {
<<<<<<< HEAD
        public string _name { get; set; }
        public DateTime _creation { get; set; }
        public DateTime _expiration { get; set; }
        public List<string> _ingredients { get; set; }
        public string _manufacturer { get; set; } // remove???
=======
        public string medicineName { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime expirationDate { get; set; }
        public List<string> ingredients { get; set; }
        public string manufacturer { get; set; }
>>>>>>> 6ee85423f23a9fee7945477461b9c57c0719c897
    }
}