using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    class Prescription
    {
        public List<Medicine> allMedicine { get; set; }
        public Doctor doctor { get; set; }
        public Instruction instruction { get; set; }
    }
}
