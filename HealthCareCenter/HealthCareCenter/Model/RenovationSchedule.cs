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
            this.ID = RenovationScheduleService.GetLargestRenovationId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.Room1ID = -1;
            this.Room2ID = -1;
            this.MainRoomID = roomForRenovation.ID;
            this.RenovationType = Enums.RenovationType.Simple;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, int roomForRenovationId)
        {
            this.ID = RenovationScheduleService.GetLargestRenovationId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.Room1ID = -1;
            this.Room2ID = -1;
            this.MainRoomID = roomForRenovationId;
            this.RenovationType = Enums.RenovationType.Simple;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, HospitalRoom firstRoomForRenovation, HospitalRoom secondRoomForRenovation, HospitalRoom newRoom, Enums.RenovationType renovationType)
        {
            this.ID = RenovationScheduleService.GetLargestRenovationId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.Room1ID = firstRoomForRenovation.ID;
            this.Room2ID = secondRoomForRenovation.ID;
            this.MainRoomID = newRoom.ID;
            this.RenovationType = renovationType;
        }

        public RenovationSchedule(DateTime startDate, DateTime finishDate, int firstRoomForRenovationId, int secondRoomForRenovationId, int newRoomId, Enums.RenovationType renovationType)
        {
            this.ID = RenovationScheduleService.GetLargestRenovationId() + 1;
            this.StartDate = startDate;
            this.FinishDate = finishDate;
            this.Room1ID = firstRoomForRenovationId;
            this.Room2ID = secondRoomForRenovationId;
            this.MainRoomID = newRoomId;
            this.RenovationType = renovationType;
        }

        public bool IsComplexRenovation()
        {
            return (RenovationType != Enums.RenovationType.Simple);
        }

        public void ScheduleSimpleRenovation(HospitalRoom roomForRenovation)
        {
            RenovationScheduleService.AddRenovation(this);
            HospitalRoomService.DeleteRoom(roomForRenovation);
            HospitalRoomForRenovationService.AddRoom(roomForRenovation);
        }

        public void ScheduleMergeRenovation(HospitalRoom room1, HospitalRoom room2, HospitalRoom newRoom)
        {
            HospitalRoomUnderConstructionService.AddRoom(newRoom);

            HospitalRoomService.DeleteRoom(room1);
            HospitalRoomService.DeleteRoom(room2);

            HospitalRoomForRenovationService.AddRoom(room1);
            HospitalRoomForRenovationService.AddRoom(room2);

            RenovationScheduleService.AddRenovation(this);
        }

        public void ScheduleSplitRenovation(HospitalRoom newRoom1, HospitalRoom newRoom2, HospitalRoom splitRoom)
        {
            HospitalRoomForRenovationService.AddRoom(splitRoom);
            HospitalRoomService.DeleteRoom(splitRoom);

            HospitalRoomUnderConstructionService.AddRoom(newRoom1);
            HospitalRoomUnderConstructionService.AddRoom(newRoom2);

            RenovationScheduleService.AddRenovation(this);
        }

        private bool IsDateBeforeToday(DateTime date)
        {
            int value = DateTime.Compare(date, DateTime.Now);
            return value < 0;
        }

        private void FinishSimpleRenovation()
        {
            HospitalRoom renovatedRoom = HospitalRoomForRenovationService.GetRoom(this.MainRoomID);
            HospitalRoomService.InsertRoom(renovatedRoom);
            HospitalRoomForRenovationService.DeleteRoom(renovatedRoom);
            RenovationScheduleService.DeleteRenovation(this);
        }

        private void FinishMergeRenovation()
        {
            HospitalRoom newRoom = HospitalRoomUnderConstructionService.GetRoom(this.MainRoomID);
            HospitalRoom room1 = HospitalRoomForRenovationService.GetRoom(this.Room1ID);
            HospitalRoom room2 = HospitalRoomForRenovationService.GetRoom(this.Room2ID);
            HospitalRoomService.InsertRoom(newRoom);
            // -----
            room1.TransferAllEquipment(newRoom);
            room2.TransferAllEquipment(newRoom);
            // -----
            HospitalRoomForRenovationService.DeleteRoom(room1);
            HospitalRoomForRenovationService.DeleteRoom(room2);
            HospitalRoomUnderConstructionService.DeleteRoom(newRoom.ID);
            RenovationScheduleService.DeleteRenovation(this);
        }

        private void FinishSplitRenovation()
        {
            HospitalRoom mainRoom = HospitalRoomForRenovationService.GetRoom(this.MainRoomID);
            HospitalRoom room1 = HospitalRoomUnderConstructionService.GetRoom(this.Room1ID);
            HospitalRoom room2 = HospitalRoomUnderConstructionService.GetRoom(this.Room2ID);

            HospitalRoomUnderConstructionService.DeleteRoom(room1.ID);
            HospitalRoomUnderConstructionService.DeleteRoom(room2.ID);
            HospitalRoomForRenovationService.DeleteRoom(mainRoom.ID);

            HospitalRoomService.InsertRoom(room1);
            HospitalRoomService.InsertRoom(room2);

            RenovationScheduleService.DeleteRenovation(this);
        }

        // dodati za split
        public void FinishRenovation()
        {
            if (IsDateBeforeToday(this.FinishDate))
            {
                if (this.RenovationType == Enums.RenovationType.Simple)
                {
                    FinishSimpleRenovation();
                }
                else if (this.RenovationType == Enums.RenovationType.Merge)
                {
                    FinishMergeRenovation();
                }
                else if (this.RenovationType == Enums.RenovationType.Split)
                {
                    FinishSplitRenovation();
                }
            }
        }
    }
}