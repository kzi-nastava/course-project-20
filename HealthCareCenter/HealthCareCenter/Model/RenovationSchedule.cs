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
        public int Room1ID { get; set; }
        public int Room2ID { get; set; } // if is complex
        public int MainRoomID { get; set; }
        public Enums.RenovationType RenovationType { get; set; }

        public RenovationSchedule()
        { }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, HospitalRoom roomForRenovation)
        {
            this.ID = RenovationScheduleService.GetLargestId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.Room1ID = -1;
            this.Room2ID = -1;
            this.MainRoomID = roomForRenovation.ID;
            this.RenovationType = Enums.RenovationType.Simple;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, int roomForRenovationId)
        {
            this.ID = RenovationScheduleService.GetLargestId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.Room1ID = -1;
            this.Room2ID = -1;
            this.MainRoomID = roomForRenovationId;
            this.RenovationType = Enums.RenovationType.Simple;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, HospitalRoom firstRoomForRenovation, HospitalRoom secondRoomForRenovation, HospitalRoom newRoom, Enums.RenovationType renovationType)
        {
            this.ID = RenovationScheduleService.GetLargestId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.Room1ID = firstRoomForRenovation.ID;
            this.Room2ID = secondRoomForRenovation.ID;
            this.MainRoomID = newRoom.ID;
            this.RenovationType = renovationType;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, int firstRoomForRenovationId, int secondRoomForRenovationId, int newRoomId, Enums.RenovationType renovationType)
        {
            this.ID = RenovationScheduleService.GetLargestId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.Room1ID = firstRoomForRenovationId;
            this.Room2ID = secondRoomForRenovationId;
            this.MainRoomID = newRoomId;
            this.RenovationType = renovationType;
        }
    }
}