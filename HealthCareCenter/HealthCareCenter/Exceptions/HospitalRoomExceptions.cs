using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Exceptions
{
    public class HospitalRoomNotFoundException : Exception
    {
        public HospitalRoomNotFoundException(string roomId) : base($"Hospital room with ID={roomId} not found!")
        {
        }
    }

    public class InvalideHospitalRoomNameException : Exception
    {
        public InvalideHospitalRoomNameException() : base("Hospital room name not entered!")
        {
        }
    }

    public class StorageIdException : Exception
    {
        public StorageIdException() : base("ID of storage has be entered!")
        {
        }
    }

    public class HospitalRoomAlreadyExistException : Exception
    {
        public HospitalRoomAlreadyExistException(string roomId) : base($"Hospital room with ID={roomId} already exist!")
        {
        }
    }

    public class InvalideHospitalRoomIdException : Exception
    {
        public InvalideHospitalRoomIdException(string roomId) : base($"Bad input of hospital room ID={roomId}!")
        {
        }
    }

    public class HospitalRoomContainsEquipmentException : Exception
    {
        public HospitalRoomContainsEquipmentException(string roomId) : base($"Hospital room with ID={roomId} contain equipment!")
        {
        }
    }

    public class HospitalRoomContainAppointmentException : Exception
    {
        public HospitalRoomContainAppointmentException(string roomId) : base($"Hospital room with ID={roomId} contains apointment!")
        {
        }
    }

    public class HospitalRoomContainEquipmentRearrangementException : Exception
    {
        public HospitalRoomContainEquipmentRearrangementException(string roomId) : base($"Hospital room with ID={roomId} contain rearrangement!")
        {
        }
    }

    public class HospitalRoomUnderRenovationException : Exception
    {
        public HospitalRoomUnderRenovationException(string roomId) : base($"Hospital room with ID={roomId} is renovating!")
        {
        }
    }

    public class RoomsMustBeDifferenteException : Exception
    {
        public RoomsMustBeDifferenteException() : base($"Rooms must be differente!")
        {
        }
    }
}