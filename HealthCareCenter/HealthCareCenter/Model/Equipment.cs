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
            Room currentRoom = RoomService.GetRoom(CurrentRoomID);

            if (this.IsScheduledRearrangement())
            {
                // Get rearrangement
                EquipmentRearrangement rearrangement = EquipmentRearrangementService.GetRearrangement(this.RearrangementID);
                Room newRoomOfRearrangement = RoomService.GetRoom(CurrentRoomID);

                // Romeve rearrangement id from list RearrangemetIDs of newRoomOfPreviusRearrangement and currenRoom
                newRoomOfRearrangement.EquipmentRearrangementsIDs.Remove(rearrangement.ID);
                currentRoom.EquipmentRearrangementsIDs.Remove(rearrangement.ID);

                // Remove rearrangement from file
                EquipmentRearrangementService.DeleteRearrangement(rearrangement.ID);

                // Update rooms
                //********************************************
                RoomService.UpdateRoom(newRoomOfRearrangement);
                RoomService.UpdateRoom(currentRoom);
                //********************************************

                // Rearrangement removed
                this.RearrangementID = -1;
                EquipmentService.UpdateEquipment(this);
            }
        }

        /// <summary>
        /// Setting rearrangement for equipment. (if rearrangemnt is not scheduled).
        /// </summary>
        /// <param name="rearrangement"></param>
        public void SetRearrangement(EquipmentRearrangement rearrangement)
        {
            Room currentRoom = RoomService.GetRoom(CurrentRoomID);
            Room newRoomOfCurrentRearrangement = RoomService.GetRoom(rearrangement.NewRoomID);

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
            RoomService.UpdateRoom(newRoomOfCurrentRearrangement);
            RoomService.UpdateRoom(currentRoom);
            //********************************************

            // Update equipment information
            EquipmentService.UpdateEquipment(this);
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
                    Room currentRoom = RoomService.GetRoom(this.CurrentRoomID);
                    Room newRoomOfRearrangement = RoomService.GetRoom(rearrangement.NewRoomID);

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

                    RoomService.UpdateRoom(currentRoom);
                    RoomService.UpdateRoom(newRoomOfRearrangement);
                    this.CurrentRoomID = rearrangement.NewRoomID;
                    this.RemoveRearrangement();
                    EquipmentService.UpdateEquipment(this);
                }
            }
        }
    }
}