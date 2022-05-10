using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;
using HealthCareCenter.Service;

namespace HealthCareCenter.Model
{
    public class HospitalRoom : Room
    {
        public RoomType Type { get; set; }

        public string Name { get; set; }

        public List<int> AppointmentIDs { get; set; }

        public HospitalRoom()
        { }

        /// <summary>
        /// Constructor used for adding new hospital room.
        /// </summary>
        public HospitalRoom(RoomType type, string name)
        {
            if (HospitalRoomService.GetLargestRoomId() > HospitalRoomForRenovationService.GetLargestRoomId())
            {
                this.ID = HospitalRoomService.GetLargestRoomId() + 1;
            }
            else
            {
                this.ID = HospitalRoomForRenovationService.GetLargestRoomId() + 1;
            }
            this.Name = name;
            this.EquipmentAmounts = new Dictionary<string, int>();
            this.EquipmentRearrangementsIDs = new List<int>();
            this.Type = type;
            this.AppointmentIDs = new List<int>();
        }

        /// <summary>
        /// Check if room contains any appointment
        /// </summary>
        /// <returns></returns>
        public bool ContainsAnyAppointment()
        {
            if (AppointmentIDs.Count != 0)
            {
                return true;
            }

            return false;
        }

        public bool IsCurrentlyRenovating()
        {
            foreach (HospitalRoom room in HospitalRoomForRenovationService.GetRooms())
            {
                if (room.ID == this.ID)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsDateBeforeToday(DateTime date)
        {
            int value = DateTime.Compare(date, DateTime.Now);
            return value < 0;
        }

        /// <summary>
        /// If hospital room is scheduled for renovation, if finish date is passe, then just set room to be avalible
        /// </summary>
        public void SetToBeAvailable()
        {
            if (IsCurrentlyRenovating())
            {
                RenovationSchedule renovation = RenovationScheduleService.GetRenovation(this);
                if (!renovation.IsComplexRenovation())
                {
                    if (IsDateBeforeToday(renovation.FinishDate))
                    {
                        HospitalRoomService.InsertRoom(this);
                        HospitalRoomForRenovationService.DeleteRoom(this);
                        RenovationScheduleService.DeleteRenovation(renovation);
                    }
                }
            }
        }
    }
}