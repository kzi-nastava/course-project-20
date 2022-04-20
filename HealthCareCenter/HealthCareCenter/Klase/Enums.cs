using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Enums
{
    enum Blocker
    {
        SYSTEM,
        SECRETARY
    }
    enum RequestType
    {
        DELETE,
        MAKE_CHANGES
    }
    enum FurnitureType
    {
        TABLE,
        CHAIR,
        BED,
        CLOSET,
        OTHER
    }
    enum EquipmentType
    {
        FOR_CHECKUP,
        FOR_SURGERY,
        FURNITURE,
        FOR_HALLWAY
    }
    enum RoomType
    {
        REST,
        OPERATION,
        CHECKUP,
        STORAGE,
        OTHER
    }
    enum AppointmentType
    {
        OPERATION,
        CHECKUP
    }
    enum ConsumptionPeriod
    {
        AFTER_EATING,
        BEFORE_EATING,
        ANY
    }
    enum RequestState
    {
        DENIED,
        APPROVED,
        WAITING
    }
}
