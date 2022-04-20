using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Enums
{
    enum Blocker
    {
        System,
        Secretary
    }
    enum RequestType
    {
        Delete,
        MakeChanges
    }
    enum FurnitureType
    {
        Table,
        Chair,
        Bed,
        Closet,
        Other
    }
    enum EquipmentType
    {
        ForCheckup,
        ForSurgery,
        Furniture,
        ForHallway
    }
    enum RoomType
    {
        Rest,
        Operation,
        Checkup,
        Storage,
        Other
    }
    enum AppointmentType
    {
        Operation,
        Checkup
    }
    enum ConsumptionPeriod
    {
        AfterEating,
        BeforeEating,
        Any
    }
    enum RequestState
    {
        Denied,
        Approved,
        Waiting
    }
}
