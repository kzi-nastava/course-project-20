using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter
{
    public class Prescription
    {
        public List<Medicine> _medicines { get; set; }
        public Doctor _doctor { get; set; }
        public Instruction _instruction { get; set; }
    }
}