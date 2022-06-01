using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;
using HealthCareCenter.Service;

namespace HealthCareCenter.Model
{
    public class Equipment
    {
        public int ID { get; set; }
        public int CurrentRoomID { get; set; }
        public EquipmentType Type { get; set; }
        public string Name { get; set; }
        public int RearrangementID { get; set; }

        public Equipment()
        { }

        /// <summary>
        /// Constructor used for adding new equipment.
        /// </summary>
        public Equipment(EquipmentType type, string name)
        {
            this.ID = EquipmentService.GetLargestEquipmentId() + 1;
            this.CurrentRoomID = 0;
            this.Type = type;
            this.Name = name;
            this.RearrangementID = -1;
        }

        /// <summary>
        /// Add all fields to list.
        /// </summary>
        /// <returns>Equipment object like list</returns>
        public List<string> ToList()
        {
            return new List<string> { this.ID.ToString(), this.CurrentRoomID.ToString(), this.Type.ToString(), this.Name };
        }

        /// <summary>
        /// Checking does equipment contains rearrangment.
        /// </summary>
        /// <returns></returns>
        public bool IsScheduledRearrangement()
        {
            return RearrangementID != -1;
        }

        /// <summary>
        /// Remove equipment rearrangement (if rearrangemnt is scheduled).
        /// </summary>
        public void RemoveRearrangement()
        {
            Room currentRoom = RoomService.Get(CurrentRoomID);

            if (this.IsScheduledRearrangement())
            {
                // Get rearrangement
                EquipmentRearrangement rearrangement = EquipmentRearrangementService.GetRearrangement(this.RearrangementID);
                Room newRoomOfRearrangement = RoomService.Get(CurrentRoomID);

                // Romeve rearrangement id from list RearrangemetIDs of newRoomOfPreviusRearrangement and currenRoom
                newRoomOfRearrangement.EquipmentRearrangementsIDs.Remove(rearrangement.ID);
                currentRoom.EquipmentRearrangementsIDs.Remove(rearrangement.ID);

                // Remove rearrangement from file
                EquipmentRearrangementService.DeleteRearrangement(rearrangement.ID);

                // Update rooms
                //********************************************
                RoomService.Update(newRoomOfRearrangement);
                RoomService.Update(currentRoom);
                //********************************************

                // Rearrangement removed
                this.RearrangementID = -1;
                EquipmentService.Update(this);
            }
        }

        /// <summary>
        /// Setting rearrangement for equipment. (if rearrangemnt is not scheduled).
        /// </summary>
        /// <param name="rearrangement"></param>
        public void SetRearrangement(EquipmentRearrangement rearrangement)
        {
            Room currentRoom = RoomService.Get(CurrentRoomID);
            Room newRoomOfCurrentRearrangement = RoomService.Get(rearrangement.NewRoomID);

            // Equipment already has rearrangemt
            if (this.IsScheduledRearrangement())
            {
                this.RemoveRearrangement();
            }

            this.RearrangementID = rearrangement.ID;

            // Add new rearrangement id to list RearrangemetIDs of currenRoom and newRoomOfCurrentRearrangement
            currentRoom.EquipmentRearrangementsIDs.Add(rearrangement.ID);
            newRoomOfCurrentRearrangement.EquipmentRearrangementsIDs.Add(rearrangement.ID);

            // Add new rearrangemnt to file
            EquipmentRearrangementService.AddRearrangement(rearrangement);

            // Update Rooms
            //********************************************
            RoomService.Update(newRoomOfCurrentRearrangement);
            RoomService.Update(currentRoom);
            //********************************************

            // Update equipment information
            EquipmentService.Update(this);
        }

        private bool IsDateTimeBeforeCurrentTime(DateTime rearrangementDate)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(rearrangementDate, now);
            return value < 0;
        }

        /// <summary>
        /// Check if equipment has rearrangement, if is time for rerarrangement than motehod do rearrangement.
        /// </summary>
        public void Rearrange()
        {
            if (this.IsScheduledRearrangement())
            {
                EquipmentRearrangement rearrangement = EquipmentRearrangementService.GetRearrangement(this.RearrangementID);

                if (IsDateTimeBeforeCurrentTime(rearrangement.MoveTime))
                {
                    Room currentRoom = RoomService.Get(this.CurrentRoomID);
                    Room newRoomOfRearrangement = RoomService.Get(rearrangement.NewRoomID);

                    // update data about equipment number in rooms
                    currentRoom.EquipmentAmounts[this.Name]--;

                    if (newRoomOfRearrangement.EquipmentAmounts.ContainsKey(Name))
                    {
                        newRoomOfRearrangement.EquipmentAmounts[this.Name]++;
                    }
                    else
                    {
                        newRoomOfRearrangement.EquipmentAmounts.Add(this.Name, 1);
                    }

                    RoomService.Update(currentRoom);
                    RoomService.Update(newRoomOfRearrangement);
                    this.CurrentRoomID = rearrangement.NewRoomID;
                    this.RemoveRearrangement();
                    EquipmentService.Update(this);
                }
            }
        }
    }
}