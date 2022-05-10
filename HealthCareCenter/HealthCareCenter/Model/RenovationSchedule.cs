using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class RenovationSchedule
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int PrimaryRoomID { get; set; }
        public int SecondaryRoomID { get; set; } // if is complex

        public RenovationSchedule()
        { }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, HospitalRoom roomForRenovation)
        {
            this.ID = RenovationScheduleService.GetLargestRenovationId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.PrimaryRoomID = roomForRenovation.ID;
            this.SecondaryRoomID = -1;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, int roomForRenovationId)
        {
            this.ID = RenovationScheduleService.GetLargestRenovationId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.PrimaryRoomID = roomForRenovationId;
            this.SecondaryRoomID = -1;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, HospitalRoom firstRoomForRenovation, HospitalRoom secondRoomForRenovation)
        {
            this.ID = RenovationScheduleService.GetLargestRenovationId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.PrimaryRoomID = firstRoomForRenovation.ID;
            this.SecondaryRoomID = secondRoomForRenovation.ID;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, int firstRoomForRenovationId, int secondRoomForRenovationId)
        {
            this.ID = RenovationScheduleService.GetLargestRenovationId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.PrimaryRoomID = firstRoomForRenovationId;
            this.SecondaryRoomID = secondRoomForRenovationId;
        }

        public bool IsComplexRenovation()
        {
            return SecondaryRoomID != -1;
        }
    }
}