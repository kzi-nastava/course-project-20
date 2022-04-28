﻿using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Enums;

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
            this.ID = EquipmentRepository.GetLastEquipmentId() + 1;
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
        /// Checking does equipment contain rearrangment.
        /// </summary>
        /// <returns></returns>
        public bool IsScheduledRearrangement()
        {
            if (RearrangementID == -1)
                return false;

            return true;
        }

        /// <summary>
        /// Remove equipment rearrangement (if rearrangemnt is scheduled).
        /// </summary>
        public void RemoveRearrangement()
        {
            Room currentRoom;

            // Is current room storage
            if (this.CurrentRoomID == 0)
                currentRoom = StorageRepository.GetStorage();

            // Is current room storage
            else
                currentRoom = HospitalRoomRepository.GetRoomById(this.CurrentRoomID);
            if (this.IsScheduledRearrangement())
            {
                Room newRoomOfRearrangement;

                // Get rearrangement
                EquipmentRearrangement rearrangement = EquipmentRearrangementRepository.GetRearrangementById(this.RearrangementID);

                // Is newRoomOfPreviusRearrangement storage
                if (rearrangement.NewRoomID == 0)
                    newRoomOfRearrangement = StorageRepository.GetStorage();

                // Is newRoomOfPreviusRearrangement hospital room
                else
                    newRoomOfRearrangement = HospitalRoomRepository.GetRoomById(rearrangement.NewRoomID);

                // Romeve rearrangement id from list RearrangemetIDs of newRoomOfPreviusRearrangement and currenRoom
                newRoomOfRearrangement.EquipmentRearrangementsIDs.Remove(rearrangement.ID);
                currentRoom.EquipmentRearrangementsIDs.Remove(rearrangement.ID);

                // Remove rearrangement from file
                EquipmentRearrangementRepository.DeleteRearrangement(rearrangement.ID);

                // Update rooms
                //********************************************
                RoomRepository.UpdateRoom(newRoomOfRearrangement);
                RoomRepository.UpdateRoom(currentRoom);
                //********************************************

                // Rearrangement removed
                this.RearrangementID = -1;
                EquipmentRepository.UpdateEquipment(this);
            }
        }

        /// <summary>
        /// Setting rearrangement for equipment.
        /// </summary>
        /// <param name="rearrangement"></param>
        public void SetRearrangement(EquipmentRearrangement rearrangement)
        {
            Room currenRoom;
            Room newRoomOfCurrentRearrangement;

            // Is current room storage
            if (this.CurrentRoomID == 0)
                currenRoom = StorageRepository.GetStorage();

            // Is current room storage
            else
                currenRoom = HospitalRoomRepository.GetRoomById(this.CurrentRoomID);

            // Is new room of current rerrangement storage
            if (rearrangement.NewRoomID == 0)
                newRoomOfCurrentRearrangement = StorageRepository.GetStorage();

            // Is new room of current rerrangement hospital room
            else
                newRoomOfCurrentRearrangement = HospitalRoomRepository.GetRoomById(rearrangement.NewRoomID);

            // Equipment already has rearrangemt
            if (this.IsScheduledRearrangement())
            {
                this.RemoveRearrangement();
            }

            this.RearrangementID = rearrangement.ID;

            // Add new rearrangement id to list RearrangemetIDs of currenRoom and newRoomOfCurrentRearrangement
            currenRoom.EquipmentRearrangementsIDs.Add(rearrangement.ID);
            newRoomOfCurrentRearrangement.EquipmentRearrangementsIDs.Add(rearrangement.ID);

            // Add new rearrangemnt to file
            EquipmentRearrangementRepository.AddRearrangement(rearrangement);

            // Update Rooms
            //********************************************
            RoomRepository.UpdateRoom(newRoomOfCurrentRearrangement);
            RoomRepository.UpdateRoom(currenRoom);
            //********************************************

            // Update equipment information
            EquipmentRepository.UpdateEquipment(this);
        }

        private bool IsEquipmentRearrangementTimeBeforeCurrentTime(DateTime rearrangementDate)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(rearrangementDate, now);
            if (value < 0)
                return true;

            return false;
        }

        /// <summary>
        /// Check if equipment has rearrangement, if is time for rerarrangement than motehod do rearrangement.
        /// </summary>
        public void DoRearrangement()
        {
            if (this.IsScheduledRearrangement())
            {
                EquipmentRearrangement rearrangement = EquipmentRearrangementRepository.GetRearrangementById(this.RearrangementID);

                if (IsEquipmentRearrangementTimeBeforeCurrentTime(rearrangement.MoveTime))
                {
                    Room currentRoom;
                    Room newRoomOfRearrangement;

                    // Is current room storage
                    if (this.CurrentRoomID == 0)
                        currentRoom = StorageRepository.GetStorage();

                    // Is current room storage
                    else
                        currentRoom = HospitalRoomRepository.GetRoomById(this.CurrentRoomID);

                    // Is newRoomOfPreviusRearrangement storage
                    if (rearrangement.NewRoomID == 0)
                        newRoomOfRearrangement = StorageRepository.GetStorage();

                    // Is newRoomOfPreviusRearrangement hospital room
                    else
                        newRoomOfRearrangement = HospitalRoomRepository.GetRoomById(rearrangement.NewRoomID);

                    currentRoom.EquipmentAmounts[this.Name]--;
                    if (newRoomOfRearrangement.EquipmentAmounts.ContainsKey(Name))
                        newRoomOfRearrangement.EquipmentAmounts[this.Name]++;
                    else
                        newRoomOfRearrangement.EquipmentAmounts.Add(this.Name, 1);
                    RoomRepository.UpdateRoom(currentRoom);
                    RoomRepository.UpdateRoom(newRoomOfRearrangement);
                    this.CurrentRoomID = rearrangement.NewRoomID;
                    this.RemoveRearrangement();
                    EquipmentRepository.UpdateEquipment(this);
                }
            }
        }
    }
}