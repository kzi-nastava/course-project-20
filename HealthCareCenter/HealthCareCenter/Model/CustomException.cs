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

    public class RenovationScheduleNotFound : Exception
    {
        public RenovationScheduleNotFound()
        {
            Console.WriteLine("Error, renovation schedule not found!");
        }
    }

    public class HospitalPremisesNotFound : Exception
    {
        public HospitalPremisesNotFound()
        {
            Console.WriteLine("Error, hospital premises not found!");
        }
    }

    public class MedicineNotFound : Exception
    {
        public MedicineNotFound()
        {
            Console.WriteLine("Error, medicine not found!");
        }
    }
}