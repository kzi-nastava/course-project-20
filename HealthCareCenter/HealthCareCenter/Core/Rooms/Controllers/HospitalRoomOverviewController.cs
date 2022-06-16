using HealthCareCenter.Core.Equipment.Models;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Rooms.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Controllers
{
    public class HospitalRoomOverviewController
    {
        private readonly IEquipmentRearrangementService _equipmentRearrangementService;
        private readonly IRoomService _roomService;
        private readonly IEquipmentService _equipmentService;
        private readonly BaseStorageRepository _storageRepository;

        public HospitalRoomOverviewController(
            IEquipmentRearrangementService equipmentRearrangementService,
            IRoomService roomService,
            IEquipmentService equipmentService,
            BaseStorageRepository storageRepository)
        {
            _equipmentRearrangementService = equipmentRearrangementService;
            _roomService = roomService;
            _equipmentService = equipmentService;
            _storageRepository = storageRepository;
        }

        public List<List<string>> GetFilteredEquipmentSearchResult(string searchContent, string amount, string equipmentType, string roomType)
        {
            List<Equipment.Models.Equipment> equipments = _equipmentService.GetEquipments();
            List<List<string>> equipmentsForDisplay = new List<List<string>>();
            foreach (Equipment.Models.Equipment equipment in equipments)
            {
                if (!_equipmentService.HasScheduledRearrangement(equipment))
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    AddEmptyFieldsForEquipmentDisplay(ref equipmentAttributesToDisplay);
                    FilterEquipment(ref equipmentsForDisplay, equipmentAttributesToDisplay, searchContent, amount, equipmentType, roomType);
                }
                else
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    EquipmentRearrangement rearrangement = _equipmentRearrangementService.Get(equipment.RearrangementID);
                    equipmentAttributesToDisplay.Add(rearrangement.MoveTime.ToString(Constants.DateFormat));
                    equipmentAttributesToDisplay.Add(rearrangement.NewRoomID.ToString());
                    FilterEquipment(ref equipmentsForDisplay, equipmentAttributesToDisplay, searchContent, amount, equipmentType, roomType);
                }
            }

            return equipmentsForDisplay;
        }

        public List<List<string>> GetAllEquipmentsForDisplay()
        {
            List<Equipment.Models.Equipment> equipments = _equipmentService.GetEquipments();
            List<List<string>> equipmentForDisplay = new List<List<string>>();
            foreach (Equipment.Models.Equipment equipment in equipments)
            {
                if (!_equipmentService.HasScheduledRearrangement(equipment))
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    AddEmptyFieldsForEquipmentDisplay(ref equipmentAttributesToDisplay);
                    equipmentForDisplay.Add(equipmentAttributesToDisplay);
                }
                else
                {
                    List<string> equipmentAttributesToDisplay = equipment.ToList();
                    EquipmentRearrangement rearrangement = _equipmentRearrangementService.Get(equipment.RearrangementID);
                    equipmentAttributesToDisplay.Add(rearrangement.MoveTime.ToString(Constants.DateFormat));
                    equipmentAttributesToDisplay.Add(rearrangement.NewRoomID.ToString());
                    equipmentForDisplay.Add(equipmentAttributesToDisplay);
                }
            }

            return equipmentForDisplay;
        }

        private void AddEmptyFieldsForEquipmentDisplay(ref List<string> equipmentAttributesToDisplay)
        {
            equipmentAttributesToDisplay.Add("");
            equipmentAttributesToDisplay.Add("");
        }

        public bool FilterEquipmentsBySearchTextBox(List<string> equipmentAttributesToDisplay, string searchContent)
        {
            if (searchContent == "") { return true; }

            foreach (string attribute in equipmentAttributesToDisplay)
            {
                if (attribute.Contains(searchContent)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Filter content by type of current room (the room in which equipment) is located
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to fileter</param>
        /// <returns>True if content pass criterion</returns>
        private bool FilterEquipmentsByCurrentRoomType(List<string> equipmentAttributesToDisplay, string roomType)
        {
            string currentRoomId = equipmentAttributesToDisplay[1];

            if (roomType == "") { return true; }

            if (roomType == "Storage" && currentRoomId == "0") { return true; }
            else if (currentRoomId != "0")
            {
                HospitalRoom room = (HospitalRoom)_roomService.Get(Convert.ToInt32(currentRoomId));
                if (roomType == room.Type.ToString()) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Filter content by equipment type
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Contet we want to fileter</param>
        /// <returns>True if content pass criterion</returns>
        private bool FilterEquipmentsByEquipmentType(List<string> equipmentAttributesToDisplay, string equipmentType)
        {
            if (equipmentType == "") { return true; }

            if (equipmentAttributesToDisplay[2].Contains(equipmentType)) { return true; }

            return false;
        }

        /// <summary>
        /// Filter content by his ammount in storage
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to filter</param>
        /// <returns>True if content pass criterion</returns>
        private bool FilterEquipmentsByAmount(List<string> equipmentAttributesToDisplay, string amount)
        {
            string equipmentName = equipmentAttributesToDisplay[3];

            if (amount == "") { return true; }

            Room storage = _storageRepository.Load();

            if (amount == "Out of stock") { if (!_roomService.ContainsEquipment(storage, equipmentName)) { return true; } }

            if (amount == "0-10")
            {
                if (!_roomService.ContainsEquipment(storage, equipmentName)) { return false; }

                if (_roomService.GetEquipmentAmount(storage, equipmentName) > 0 &&
                    _roomService.GetEquipmentAmount(storage, equipmentName) < 10) { return true; }
            }

            if (amount == "10+")
            {
                if (!_roomService.ContainsEquipment(storage, equipmentName)) { return false; }

                if (_roomService.GetEquipmentAmount(storage, equipmentName) > 10) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Here are called all filter metheods and if all filter methods return true, content is added.
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to add</param>
        private void FilterEquipment(ref List<List<string>> roomsForDisplay, List<string> equipmentAttributesToDisplay, string searchContent, string amount, string equipmentType, string roomType)
        {
            if (FilterEquipmentsBySearchTextBox(equipmentAttributesToDisplay, searchContent))
            {
                if (FilterEquipmentsByCurrentRoomType(equipmentAttributesToDisplay, roomType))
                {
                    if (FilterEquipmentsByEquipmentType(equipmentAttributesToDisplay, equipmentType))
                    {
                        if (FilterEquipmentsByAmount(equipmentAttributesToDisplay, amount))
                        {
                            roomsForDisplay.Add(equipmentAttributesToDisplay);
                        }
                    }
                }
            }
        }
    }
}