using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Enums
{
    public enum Blocker
    {
        SYSTEM,
        SECRETARY
    }

    public enum RequestType
    {
        DELETE,
        MAKE_CHANGES
    }

    public enum FurnitureType
    {
        TABLE,
        CHAIR,
        BED,
        CLOSET,
        OTHER
    }

    public enum EquipmentType
    {
        FOR_CHECKUP,
        FOR_SURGERY,
        FURNITURE,
        FOR_HALLWAY
    }

    public enum RoomType
    {
        REST,
        OPERATION,
        CHECKUP,
        STORAGE,
        OTHER
    }

    public enum AppointmentType
    {
        OPERATION,
        CHECKUP
    }

    public enum ConsumptionPeriod
    {
        AFTER_EATING,
        BEFORE_EATING,
        ANY
    }

    public enum RequestState
    {
        DENIED,
        APPROVED,
        WAITING
    }
}