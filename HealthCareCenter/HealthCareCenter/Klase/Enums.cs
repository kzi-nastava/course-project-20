using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Enums
{
    public enum Blocker
    {
        System,
        Secretary
    }

    public enum RequestType
    {
        Delete,
        MakeChanges
    }

    public enum FurnitureType
    {
        Table,
        Chair,
        Bed,
        Closet,
        Other
    }

    public enum EquipmentType
    {
        ForCheckup,
        ForSurgery,
        Furniture,
        ForHallway
    }

    public enum RoomType
    {
        Rest,
        Operation,
        Checkup,
        Storage,
        Other
    }

    public enum AppointmentType
    {
        Operation,
        Checkup
    }

    public enum ConsumptionPeriod
    {
        AfterEating,
        BeforeEating,
        Any
    }

    public enum RequestState
    {
        Denied,
        Approved,
        Waiting
    }
}