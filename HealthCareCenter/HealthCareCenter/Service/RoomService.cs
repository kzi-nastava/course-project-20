using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public class RoomService
    {
        /// <summary>
        /// Get every equipment amount like dictionary, where key is name of equipment and value amount.
        /// </summary>
        /// <returns>equipments amount</returns>
        public static Dictionary<string, int> GetEquipmentsAmount()
        {
            List<HospitalRoom> rooms = HospitalRoomService.GetRooms();
            Room storage = StorageRepository.Load();

            List<Room> hospitalPremises = new List<Room>();

            hospitalPremises.Add(storage);
            foreach (HospitalRoom room in rooms)
            {
                hospitalPremises.Add(room);
            }

            Dictionary<string, int> equipmentsAmount = new Dictionary<string, int>();
            foreach (Room room in hospitalPremises)
            {
                foreach (KeyValuePair<string, int> entry in room.EquipmentAmounts)
                {
                    if (equipmentsAmount.ContainsKey(entry.Key))
                    {
                        equipmentsAmount[entry.Key] = equipmentsAmount[entry.Key] + entry.Value;
                    }
                    else
                    {
                        equipmentsAmount[entry.Key] = entry.Value;
                    }
                }
            }
            return equipmentsAmount;
        }

        /// <summary>
        /// Update room by type, if is storage than update storage or if is hospital room than update hospital room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool Update(Room room)
        {
            try
            {
                if (IsStorage(room))
                {
                    StorageRepository.Save(room);
                }
                else
                {
                    HospitalRoom hospitalRoom = (HospitalRoom)room;
                    if (HospitalRoomService.IsCurrentlyRenovating(hospitalRoom))
                    {
                        HospitalRoomForRenovationService.Update(hospitalRoom);
                    }
                    else
                    {
                        HospitalRoomService.Update(hospitalRoom);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Room Get(int roomId)
        {
            try
            {
                Room room = null;
                if (roomId == 0)
                {
                    room = StorageRepository.Load();
                }
                if (room == null)
                {
                    room = HospitalRoomService.Get(roomId);
                }
                if (room == null)
                {
                    room = HospitalRoomForRenovationService.Get(roomId);
                }
                if (room == null)
                {
                    room = HospitalRoomUnderConstructionService.Get(roomId);
                }
                if (room == null)
                {
                    return null;
                }

                return room;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method return usable hospital room or storage
        /// </summary>
        public static Room GetUsableHospitalPremesisForEquipmentTransfer(int roomId)
        {
            try
            {
                Room room = null;
                if (roomId == 0)
                {
                    room = StorageRepository.Load();
                }
                if (room == null)
                {
                    room = HospitalRoomService.Get(roomId);
                }

                return room;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsStorage(Room room)
        {
            if (room.ID == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checking does room contains any equipment.
        /// </summary>
        /// <returns></returns>
        public static bool ContainAnyEquipment(Room room)
        {
            if (room.EquipmentAmounts.Count != 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checking does room contains equipment. For example is room contains chair.
        /// </summary>
        /// <returns>True if contains or false if not</returns>
        public static bool ContainsEquipment(Room room, string equipmentName)
        {
            if (!room.EquipmentAmounts.ContainsKey(equipmentName) || (room.EquipmentAmounts[equipmentName] == 0))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// For example, returns how much chairs are in room.
        /// </summary>
        /// <param name="equipmentName"></param>
        /// <returns></returns>
        public static int GetEquipmentAmount(Room room, string equipmentName)
        {
            if (!ContainsEquipment(room, equipmentName))
            {
                return 0;
            }

            return room.EquipmentAmounts[equipmentName];
        }

        /// <summary>
        /// Check if room contain specific equipment. For example check if room contain equipment with id=1.
        /// </summary>
        /// <param name="specificEquipment"></param>
        /// <returns></returns>
        public static bool Contains(Room room, Equipment specificEquipment)
        {
            List<Equipment> equipments = EquipmentService.GetEquipments();
            foreach (Equipment equipment in equipments)
            {
                if (equipment.CurrentRoomID == room.ID)
                {
                    if (equipment.ID == specificEquipment.ID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Reduce equipment amount in dictionary
        /// </summary>
        /// <param name="equipment"></param>
        private static void ReduceEquipmentAmount(Equipment equipment, Room room)
        {
            try
            {
                if (Contains(room, equipment))
                {
                    room.EquipmentAmounts[equipment.Name]--;
                    RoomService.Update(room);
                }
                else
                {
                    throw new EquipmentNotFound();
                }
            }
            catch (EquipmentNotFound ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void IncreaseEquipmentAmount(Equipment equipment, Room room)
        {
            if (ContainsEquipment(room, equipment.Name))
            {
                room.EquipmentAmounts[equipment.Name]++;
            }
            else
            {
                if (room.EquipmentAmounts.ContainsKey(equipment.Name) == false)
                {
                    room.EquipmentAmounts.Add(equipment.Name, 1);
                }
                else
                {
                    room.EquipmentAmounts[equipment.Name]++;
                }
            }
        }

        private static void AddEquipment(Room currentEquipmentRoom, Equipment equipment, Room newEquipmentRoom)
        {
            IncreaseEquipmentAmount(equipment, newEquipmentRoom);
            RoomService.Update(currentEquipmentRoom);
            equipment.CurrentRoomID = newEquipmentRoom.ID;
            EquipmentService.Update(equipment);
        }

        private static void TransferEquipment(Room currnetRoom, Equipment equipment, Room newEquipmentRoom)
        {
            ReduceEquipmentAmount(equipment, currnetRoom);
            AddEquipment(currnetRoom, equipment, newEquipmentRoom);
        }

        public static void TransferAllEquipment(Room currentEquipmentRoom, Room newEquipmentRoom)
        {
            List<Equipment> equipments = EquipmentService.GetEquipments();
            for (int i = 0; i < equipments.Count; i++)
            {
                if (equipments[i].CurrentRoomID == currentEquipmentRoom.ID)
                {
                    TransferEquipment(currentEquipmentRoom, equipments[i], newEquipmentRoom);
                }
            }
        }

        public static bool ContaninsAnyRearrangement(Room room)
        {
            List<EquipmentRearrangement> rearrangements = EquipmentRearrangementService.GetRearrangements();
            foreach (EquipmentRearrangement rearrangement in rearrangements)
            {
                if (rearrangement.OldRoomID == room.ID || rearrangement.NewRoomID == room.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Equipment> GetAllEquipments(Room room)
        {
            List<Equipment> roomEquipments = new List<Equipment>();
            List<Equipment> equipments = EquipmentService.GetEquipments();
            foreach (Equipment equipment in equipments)
            {
                if (equipment.CurrentRoomID == room.ID)
                {
                    roomEquipments.Add(equipment);
                }
            }

            return roomEquipments;
        }
    }
}