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
            RenovationScheduleService.Add(this);
            HospitalRoomService.Delete(roomForRenovation);
            HospitalRoomForRenovationService.Add(roomForRenovation);
        }

        public void ScheduleMergeRenovation(HospitalRoom room1, HospitalRoom room2, HospitalRoom newRoom)
        {
            HospitalRoomUnderConstructionService.Add(newRoom);

            HospitalRoomService.Delete(room1);
            HospitalRoomService.Delete(room2);

            HospitalRoomForRenovationService.Add(room1);
            HospitalRoomForRenovationService.Add(room2);

            RenovationScheduleService.Add(this);
        }

        public void ScheduleSplitRenovation(HospitalRoom newRoom1, HospitalRoom newRoom2, HospitalRoom splitRoom)
        {
            HospitalRoomForRenovationService.Add(splitRoom);
            HospitalRoomService.Delete(splitRoom);

            HospitalRoomUnderConstructionService.Add(newRoom1);
            HospitalRoomUnderConstructionService.Add(newRoom2);

            RenovationScheduleService.Add(this);
        }

        private bool IsDateBeforeToday(DateTime date)
        {
            int value = DateTime.Compare(date, DateTime.Now);
            return value < 0;
        }

        private void FinishSimpleRenovation()
        {
            HospitalRoom renovatedRoom = HospitalRoomForRenovationService.Get(this.MainRoomID);
            HospitalRoomService.Insert(renovatedRoom);
            HospitalRoomForRenovationService.Delete(renovatedRoom);
            RenovationScheduleService.Delete(this);
        }

        private void FinishMergeRenovation()
        {
            HospitalRoom newRoom = HospitalRoomUnderConstructionService.Get(this.MainRoomID);
            HospitalRoom room1 = HospitalRoomForRenovationService.Get(this.Room1ID);
            HospitalRoom room2 = HospitalRoomForRenovationService.Get(this.Room2ID);
            HospitalRoomService.Insert(newRoom);
            // -----
            room1.TransferAllEquipment(newRoom);
            room2.TransferAllEquipment(newRoom);
            // -----
            HospitalRoomForRenovationService.Delete(room1);
            HospitalRoomForRenovationService.Delete(room2);
            HospitalRoomUnderConstructionService.Delete(newRoom.ID);
            RenovationScheduleService.Delete(this);
        }

        private void FinishSplitRenovation()
        {
            HospitalRoom mainRoom = HospitalRoomForRenovationService.Get(this.MainRoomID);
            HospitalRoom room1 = HospitalRoomUnderConstructionService.Get(this.Room1ID);
            HospitalRoom room2 = HospitalRoomUnderConstructionService.Get(this.Room2ID);

            HospitalRoomUnderConstructionService.Delete(room1.ID);
            HospitalRoomUnderConstructionService.Delete(room2.ID);
            HospitalRoomForRenovationService.Delete(mainRoom.ID);

            HospitalRoomService.Insert(room1);
            HospitalRoomService.Insert(room2);

            RenovationScheduleService.Delete(this);
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