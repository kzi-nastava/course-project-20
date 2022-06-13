using HealthCareCenter.Core.Rooms.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Models
{
    public class RenovationSchedule
    {
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int Room1ID { get; set; }
        public int Room2ID { get; set; } // if is complex
        public int MainRoomID { get; set; }
        public RenovationType RenovationType { get; set; }

        public RenovationSchedule()
        { }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, HospitalRoom roomForRenovation)
        {
            ID = RenovationScheduleRepository.GetLargestId() + 1;
            StartDate = startDate;
            FinishDate = finishDate;
            Room1ID = -1;
            Room2ID = -1;
            MainRoomID = roomForRenovation.ID;
            RenovationType = RenovationType.Simple;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, int roomForRenovationId)
        {
            ID = RenovationScheduleRepository.GetLargestId() + 1;
            StartDate = startDate;
            FinishDate = finishDate;
            Room1ID = -1;
            Room2ID = -1;
            MainRoomID = roomForRenovationId;
            RenovationType = RenovationType.Simple;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, HospitalRoom firstRoomForRenovation, HospitalRoom secondRoomForRenovation, HospitalRoom newRoom, RenovationType renovationType)
        {
            ID = RenovationScheduleRepository.GetLargestId() + 1;
            StartDate = startDate;
            FinishDate = finishDate;
            Room1ID = firstRoomForRenovation.ID;
            Room2ID = secondRoomForRenovation.ID;
            MainRoomID = newRoom.ID;
            RenovationType = renovationType;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, int firstRoomForRenovationId, int secondRoomForRenovationId, int newRoomId, RenovationType renovationType)
        {
            ID = RenovationScheduleRepository.GetLargestId() + 1;
            StartDate = startDate;
            FinishDate = finishDate;
            Room1ID = firstRoomForRenovationId;
            Room2ID = secondRoomForRenovationId;
            MainRoomID = newRoomId;
            RenovationType = renovationType;
        }
    }
}