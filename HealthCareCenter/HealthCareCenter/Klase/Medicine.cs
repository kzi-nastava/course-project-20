using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    class Medicine
    {
        public string medicineName { get; set; }
        public Date creationDate { get; set; }
        public Date expirationDate { get; set; }
        public List<string> ingredients { get; set; }
        public string manufacturer { get; set; }
    }
}
