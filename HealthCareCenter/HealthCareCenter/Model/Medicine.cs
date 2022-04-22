using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Medicine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime Creation { get; set; }
        public DateTime Expiration { get; set; }
        public List<string> Ingredients { get; set; }
        public string Manufacturer { get; set; }
    }
}