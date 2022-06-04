using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Exceptions
{
    public class EquipmentNotFoundException : Exception
    {
        public EquipmentNotFoundException(string equipmentId) : base($"Equipment with ID={equipmentId} not found!")
        {
        }
    }

    public class InvalideEquipmentIdExcpetion : Exception
    {
        public InvalideEquipmentIdExcpetion(string equipmentId) : base($"Bad input for equipment ID={equipmentId}!")
        {
        }
    }

    public class EquipmentAlreadyHasScheduledRearrangementException : Exception
    {
        public EquipmentAlreadyHasScheduledRearrangementException(string equipmentId) : base($"Eequipment with ID={equipmentId} already has scheduled rearrangement!")
        {
        }
    }

    public class EquipmentDesntContainScheduledRearrangementException : Exception
    {
        public EquipmentDesntContainScheduledRearrangementException(string equipmentId) : base($"Rearrangement for eqiupment with id={equipmentId} is not found!")
        {
        }
    }
}