using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Medicine
    {
        public string _name { get; set; }
        public DateTime _creation { get; set; }
        public DateTime _expiration { get; set; }
        public List<string> _ingredients { get; set; }
        public string _manufacturer { get; set; } // remove???
    }
}