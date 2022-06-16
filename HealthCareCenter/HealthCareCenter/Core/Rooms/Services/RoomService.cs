using HealthCareCenter.Core.Equipment.Models;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
using System;
using System.Collections.Generic;

namespace HealthCareCenter.Core.Rooms.Services
{
    public class RoomService : IRoomService
    {
        private readonly IEquipmentRearrangementService _equipmentRearrangementService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;
        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly BaseStorageRepository _storageRepository;
        private readonly IEquipmentService _equipmentService;

        public RoomService(
            IEquipmentRearrangementService equipmentRearrangementService,
            BaseStorageRepository storageRepository,
            IEquipmentService equipmentService,
            IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService,
            IHospitalRoomForRenovationService hospitalRoomForRenovationService)
        {
            _equipmentRearrangementService = equipmentRearrangementService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _storageRepository = storageRepository;
            _equipmentService = equipmentService;
        }

        public RoomService(
            BaseStorageRepository storageRepository,
            IEquipmentService equipmentService,
            IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService,
            IHospitalRoomForRenovationService hospitalRoomForRenovationService)
        {
            _equipmentRearrangementService = new EquipmentRearrangementService(
                this, 
                new EquipmentService(
                    new EquipmentRepository()),
                new HospitalRoomUnderConstructionService(
                    new HospitalRoomUnderConstructionRepository()));
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _storageRepository = storageRepository;
            _equipmentService = equipmentService;
        }

        /// <summary>
        /// Get every equipment amount like dictionary, where key is name of equipment and value amount.
        /// </summary>
        /// <returns>equipments amount</returns>
        public Dictionary<string, int> GetEquipmentsAmount()
        {
            List<HospitalRoom> rooms = HospitalRoomService.GetRooms();
            Room storage = _storageRepository.Load();

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
        public bool Update(Room room)
        {
            try
            {
                if (IsStorage(room))
                {
                    _storageRepository.Save(room);
                }
                else
                {
                    HospitalRoom hospitalRoom = (HospitalRoom)room;
                    if (HospitalRoomService.IsCurrentlyRenovating(hospitalRoom))
                    {
                        _hospitalRoomForRenovationService.Update(hospitalRoom);
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

        public Room Get(int roomId)
        {
            try
            {
                Room room = null;
                if (roomId == 0)
                {
                    room = _storageRepository.Load();
                }
                if (room == null)
                {
                    room = HospitalRoomService.Get(roomId);
                }
                if (room == null)
                {
                    room = _hospitalRoomForRenovationService.Get(roomId);
                }
                if (room == null)
                {
                    room = _hospitalRoomUnderConstructionService.Get(roomId);
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
        public Room GetPremesisForEquipmentTransfer(int roomId)
        {
            try
            {
                Room room = null;
                if (roomId == 0)
                {
                    room = _storageRepository.Load();
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

        public bool IsStorage(Room room)
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
        public bool ContainAnyEquipment(Room room)
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
        public bool ContainsEquipment(Room room, string equipmentName)
        {
            if (!room.EquipmentAmounts.ContainsKey(equipmentName) || room.EquipmentAmounts[equipmentName] == 0)
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
        public int GetEquipmentAmount(Room room, string equipmentName)
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
        public bool Contains(Room room, Equipment.Models.Equipment specificEquipment)
        {
            List<Equipment.Models.Equipment> equipments = _equipmentService.GetEquipments();
            foreach (Equipment.Models.Equipment equipment in equipments)
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

        public void TransferAllEquipment(Room currentEquipmentRoom, Room newEquipmentRoom)
        {
            List<Equipment.Models.Equipment> equipments = _equipmentService.GetEquipments();
            for (int i = 0; i < equipments.Count; i++)
            {
                if (equipments[i].CurrentRoomID == currentEquipmentRoom.ID)
                {
                    TransferEquipment(currentEquipmentRoom, equipments[i], newEquipmentRoom);
                }
            }
        }

        public bool ContainsAnyRearrangement(Room room)
        {
            List<EquipmentRearrangement> rearrangements = _equipmentRearrangementService.GetRearrangements();
            foreach (EquipmentRearrangement rearrangement in rearrangements)
            {
                if (rearrangement.OldRoomID == room.ID || rearrangement.NewRoomID == room.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Equipment.Models.Equipment> GetAllEquipment(Room room)
        {
            List<Equipment.Models.Equipment> roomEquipments = new List<Equipment.Models.Equipment>();
            List<Equipment.Models.Equipment> equipments = _equipmentService.GetEquipments();
            foreach (Equipment.Models.Equipment equipment in equipments)
            {
                if (equipment.CurrentRoomID == room.ID)
                {
                    roomEquipments.Add(equipment);
                }
            }

            return roomEquipments;
        }

        /// <summary>
        /// Reduce equipment amount in dictionary
        /// </summary>
        /// <param name="equipment"></param>
        private void ReduceEquipmentAmount(Equipment.Models.Equipment equipment, Room room)
        {
            try
            {
                if (Contains(room, equipment))
                {
                    room.EquipmentAmounts[equipment.Name]--;
                    Update(room);
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

        private void IncreaseEquipmentAmount(Equipment.Models.Equipment equipment, Room room)
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

        private void AddEquipment(Room currentEquipmentRoom, Equipment.Models.Equipment equipment, Room newEquipmentRoom)
        {
            IncreaseEquipmentAmount(equipment, newEquipmentRoom);
            Update(currentEquipmentRoom);
            equipment.CurrentRoomID = newEquipmentRoom.ID;
            _equipmentService.Update(equipment);
        }

        private void TransferEquipment(Room currnetRoom, Equipment.Models.Equipment equipment, Room newEquipmentRoom)
        {
            ReduceEquipmentAmount(equipment, currnetRoom);
            AddEquipment(currnetRoom, equipment, newEquipmentRoom);
        }
    }
}