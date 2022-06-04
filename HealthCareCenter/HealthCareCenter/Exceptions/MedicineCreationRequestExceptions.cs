using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Exceptions
{
    public class InvalideMedicineNameException : Exception
    {
        public InvalideMedicineNameException(string medicineName) : base($"Bad input for medicine name={medicineName}!")
        {
        }
    }

    public class InvalideMedicineManugacturerException : Exception
    {
        public InvalideMedicineManugacturerException(string medicineManufacturer) : base($"Bad input for manufacturer={medicineManufacturer}")
        {
        }
    }

    public class IngredientsNotFoundException : Exception
    {
        public IngredientsNotFoundException() : base("Medicine must have ingredients")
        {
        }
    }

    public class InvalideMedicineCreationRequestException : Exception
    {
        public InvalideMedicineCreationRequestException(string requestId) : base($"Bad input for change request id={requestId}!")
        {
        }
    }

    public class MedicineCreationRequestNotFoundException : Exception
    {
        public MedicineCreationRequestNotFoundException(string requestId) : base($" request with id={requestId} not found!")
        {
        }
    }
}