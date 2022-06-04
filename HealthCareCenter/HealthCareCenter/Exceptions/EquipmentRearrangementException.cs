using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Exceptions
{
    public class EquipmentRearrangementNotFoundException : Exception
    {
        public EquipmentRearrangementNotFoundException(string rerrangementId) : base($"Equipment rearrangement with id={rerrangementId} not found!")
        {
        }
    }

    public class EquipmentRearrangementIsIrevocableException : Exception
    {
        public EquipmentRearrangementIsIrevocableException(string rerrangementId) : base($"Rerrangement with id={rerrangementId} is irrevocable")
        {
        }
    }
}