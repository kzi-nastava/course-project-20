using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class HospitalRoomNotFound : Exception
    {
        public HospitalRoomNotFound()
        {
            Console.WriteLine("Error, hospital room not found!");
        }
    }

    public class EquipmentNotFound : Exception
    {
        public EquipmentNotFound()
        {
            Console.WriteLine("Error, equipment not found!");
        }
    }

    public class EquipmentRearrangementNotFound : Exception
    {
        public EquipmentRearrangementNotFound()
        {
            Console.WriteLine("Error, equipment rearrangement not found!");
        }
    }
}