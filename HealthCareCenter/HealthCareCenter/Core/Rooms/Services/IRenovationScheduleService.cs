using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Services
{
    public interface IRenovationScheduleService
    {
        List<RenovationSchedule> GetRenovations();

        void Add(RenovationSchedule newRenovation);

        bool Delete(RenovationSchedule renovation);

        void ScheduleSimpleRenovation(RenovationSchedule renovationSchedule, HospitalRoom roomForRenovation);

        void ScheduleMergeRenovation(RenovationSchedule renovationSchedule, HospitalRoom room1, HospitalRoom room2, HospitalRoom newRoom);

        void ScheduleSplitRenovation(RenovationSchedule renovationSchedule, HospitalRoom newRoom1, HospitalRoom newRoom2, HospitalRoom splitRoom);

        void FinishRenovation(RenovationSchedule renovationSchedule);
    }
}