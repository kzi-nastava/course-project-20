using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Medicine
    {
        public string medicineName { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime expirationDate { get; set; }
        public List<string> ingredients { get; set; }
        public string manufacturer { get; set; }
    }
}
