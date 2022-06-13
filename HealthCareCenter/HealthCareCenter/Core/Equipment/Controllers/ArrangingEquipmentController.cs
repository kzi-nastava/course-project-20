using System;
using System.Collections.Generic;
using HealthCareCenter.Core.Equipment.Models;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Equipment.Exceptions;
using HealthCareCenter.Core.Rooms;
using HealthCareCenter.Core.Exceptions;

namespace HealthCareCenter.Core.Equipment.Controllers
{
    public class ArrangingEquipmentController
    {
        public void SetRearrangement(string newRoomId, string equipmentForRearrangementId, string rearrangementDate, string rearrangementTime)
        {
            IsPossibleToCreateRearrangement(newRoomId, equipmentForRearrangementId, rearrangementDate, rearrangementTime);

            int parsedNewRoomId = Convert.ToInt32(newRoomId);
            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentForRearrangementId);
            Models.Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);
            DateTime rearrangementDateTime = Convert.ToDateTime(rearrangementDate + " " + rearrangementTime);

            EquipmentRearrangement rearrangement = new EquipmentRearrangement(
                equipmentForRearrangement.ID, rearrangementDateTime,
                equipmentForRearrangement.CurrentRoomID, parsedNewRoomId);

            IsPossibleToSetRearrangement(rearrangement);
            EquipmentRearrangementService.Set(rearrangement, equipmentForRearrangement);
        }

        public void UndoRearrangement(string equipmentForRearrangementId)
        {
            IsEqipmentForUndoigRearrangementValide(equipmentForRearrangementId);

            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentForRearrangementId);
            Models.Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);
            EquipmentRearrangement rearrangement = EquipmentRearrangementService.Get(equipmentForRearrangement.RearrangementID);

            IsPossibleToUndoEquipmentRearrangement(rearrangement, equipmentForRearrangement);
            EquipmentRearrangementService.Remove(equipmentForRearrangement);
        }

        public List<List<string>> GetEquipmentsForDisplay()
        {
            List<List<string>> equipmentsForDisplay = new List<List<string>>();

            List<Models.Equipment> equipments = EquipmentService.GetEquipments();
            foreach (Models.Equipment equipment in equipments)
            {
                if (!EquipmentService.HasScheduledRearrangement(equipment))
                {
                    List<string> equipmentAttributesToDisplay = GetUnscheduledEquipmentAttributes(equipment);
                    equipmentsForDisplay.Add(equipmentAttributesToDisplay);
                }
                else
                {
                    List<string> equipmentAttributesToDisplay = GetScheduledEquipmentAttributes(equipment);
                    equipmentsForDisplay.Add(equipmentAttributesToDisplay);
                }
            }
            return equipmentsForDisplay;
        }

        private List<string> GetUnscheduledEquipmentAttributes(Models.Equipment equipment)
        {
            List<string> equipmentAttributesToDisplay = equipment.ToList();
            AddEmptyFieldsForEquipmentDisplay(ref equipmentAttributesToDisplay);
            return equipmentAttributesToDisplay;
        }

        private List<string> GetScheduledEquipmentAttributes(Models.Equipment equipment)
        {
            List<string> equipmentAttributesToDisplay = equipment.ToList();
            EquipmentRearrangement rearrangement = EquipmentRearrangementService.Get(equipment.RearrangementID);
            equipmentAttributesToDisplay.Add(rearrangement.MoveTime.ToString(Constants.DateFormat));
            equipmentAttributesToDisplay.Add(rearrangement.NewRoomID.ToString());
            return equipmentAttributesToDisplay;
        }

        /// <summary>
        /// When equipment object don't have rearrangement we add 2 empty strings for "Move Time" and for "New Room Id"
        /// </summary>
        /// <param name="equipmentAttributesToDisplay">Content we want to display in DataGridEquipment</param>
        private void AddEmptyFieldsForEquipmentDisplay(ref List<string> equipmentAttributesToDisplay)
        {
            equipmentAttributesToDisplay.Add("");
            equipmentAttributesToDisplay.Add("");
        }

        private bool IsNewRoomIdInputValide(string newRoomId)
        {
            return int.TryParse(newRoomId, out _);
        }

        private bool IsEquipmentForRearrangementIdInputValide(string equipmentForRearrangementId)
        {
            return int.TryParse(equipmentForRearrangementId, out _);
        }

        private bool IsNewRoomFound(Room mewRoom)
        {
            return mewRoom != null;
        }

        private bool IsEquipmentRearrangementFound(EquipmentRearrangement rearrangement)
        {
            if (rearrangement == null)
            {
                return false;
            }
            return true;
        }

        private bool IsEquipmentForRearrangementFound(Models.Equipment equipment)
        {
            return equipment != null;
        }

        private bool IsDateTimeInputValide(string date, string time)
        {
            return DateTime.TryParse(date + " " + time, out _);
        }

        private bool WhetherRoomsAreSame(int newRoomId, int currentRoomId)
        {
            return currentRoomId == newRoomId;
        }

        private bool IsDateTimeBeforeCurrentDateTime(DateTime rearrangementDateTime)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(rearrangementDateTime, now);
            return value < 0;
        }

        private bool IsRoomAvailable(Room room, EquipmentRearrangement rearrangement)
        {
            return room != null;
        }

        private void IsNewRoomValide(string newRoomId)
        {
            if (!IsNewRoomIdInputValide(newRoomId)) { throw new InvalideHospitalRoomIdException(newRoomId); }
            int parsedNewRoomId = Convert.ToInt32(newRoomId);
            Room newRoom = RoomService.GetPremesisForEquipmentTransfer(parsedNewRoomId);
            if (!RoomService.IsStorage(newRoom))
            {
                HospitalRoom newHospitalRoom = (HospitalRoom)newRoom;
                if (!IsNewRoomFound(newHospitalRoom)) { throw new HospitalRoomNotFoundException(newRoomId); }
            }
            else if (!IsNewRoomFound(newRoom)) { throw new HospitalRoomNotFoundException(newRoomId); }
        }

        private void IsEquipmentForRearrangementValide(string equipmentId)
        {
            if (!IsEquipmentForRearrangementIdInputValide(equipmentId)) { throw new InvalideEquipmentIdExcpetion(equipmentId); }

            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentId);
            Models.Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);

            if (!IsEquipmentForRearrangementFound(equipmentForRearrangement)) { throw new EquipmentNotFoundException(equipmentId); }

            if (EquipmentService.HasScheduledRearrangement(equipmentForRearrangement))
            {
                throw new EquipmentAlreadyHasScheduledRearrangementException(equipmentId);
            }
        }

        private void IsDateTimeValide(string rearrangementDate, string rearrangementTime)
        {
            if (!IsDateTimeInputValide(rearrangementDate, rearrangementTime)) { throw new InvalideDateException(rearrangementTime + " " + rearrangementTime); }

            DateTime rearrangementDateTime = Convert.ToDateTime(rearrangementDate + " " + rearrangementTime);

            if (IsDateTimeBeforeCurrentDateTime(rearrangementDateTime)) { throw new DateIsBeforeTodayException(rearrangementDateTime.ToString()); }
        }

        private void IsPossibleToSetRearrangement(EquipmentRearrangement rearrangement)
        {
            if (WhetherRoomsAreSame(rearrangement.NewRoomID, rearrangement.OldRoomID))
            {
                throw new RoomsMustBeDifferenteException();
            }

            // Checking are rooms available
            Room currentRoom = RoomService.GetPremesisForEquipmentTransfer(rearrangement.OldRoomID);
            Room newRoom = RoomService.GetPremesisForEquipmentTransfer(rearrangement.NewRoomID);

            if (!IsRoomAvailable(currentRoom, rearrangement))
            {
                throw new HospitalRoomUnderRenovationException(rearrangement.OldRoomID.ToString());
            }

            if (!IsRoomAvailable(newRoom, rearrangement))
            {
                throw new HospitalRoomUnderRenovationException(rearrangement.NewRoomID.ToString());
            }
        }

        private void IsEqipmentForUndoigRearrangementValide(string equipmentId)
        {
            if (!IsEquipmentForRearrangementIdInputValide(equipmentId)) { throw new InvalideEquipmentIdExcpetion(equipmentId); }

            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentId);
            Models.Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);

            if (!IsEquipmentForRearrangementFound(equipmentForRearrangement)) { throw new EquipmentNotFoundException(equipmentId); }
        }

        private void IsPossibleToUndoEquipmentRearrangement(EquipmentRearrangement rearrangement, Models.Equipment equipment)
        {
            if (!IsEquipmentRearrangementFound(rearrangement))
            {
                throw new EquipmentDesntContainScheduledRearrangementException(equipment.ID.ToString());
            }
            if (EquipmentRearrangementService.IsIrrevocable(rearrangement))
            {
                throw new EquipmentRearrangementIsIrevocableException(rearrangement.ID.ToString());
            }
        }

        private void IsPossibleToCreateRearrangement(string newRoomId, string equipmentForRearrangementId, string rearrangementDate, string rearrangementTime)
        {
            IsNewRoomValide(newRoomId);
            IsEquipmentForRearrangementValide(equipmentForRearrangementId);
            IsDateTimeValide(rearrangementDate, rearrangementTime);
        }
    }
}