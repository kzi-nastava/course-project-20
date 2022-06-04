using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Exceptions;
using HealthCareCenter.Service;

namespace HealthCareCenter.Controller
{
    public class ArrangingEquipmentController
    {
        public void SetRearrangement(string newRoomId, string equipmentForRearrangementId, string rearrangementDate, string rearrangementTime)
        {
            IsPossibleToCreateRearrangement(newRoomId, equipmentForRearrangementId, rearrangementDate, rearrangementTime);

            int parsedNewRoomId = Convert.ToInt32(newRoomId);
            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentForRearrangementId);
            Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);
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
            Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);
            EquipmentRearrangement rearrangement = EquipmentRearrangementService.Get(equipmentForRearrangement.RearrangementID);

            IsPossibleToUndoEquipmentRearrangement(rearrangement, equipmentForRearrangement);
            EquipmentRearrangementService.Remove(equipmentForRearrangement);
        }

        private bool IsNewRoomIdInputValide(string newRoomId)
        {
            return Int32.TryParse(newRoomId, out _);
        }

        private bool IsEquipmentForRearrangementIdInputValide(string equipmentForRearrangementId)
        {
            return Int32.TryParse(equipmentForRearrangementId, out _);
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

        private bool IsEquipmentForRearrangementFound(Equipment equipment)
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
            Room newRoom = RoomService.GetUsableHospitalPremesisForEquipmentTransfer(parsedNewRoomId);
            if (!RoomService.IsStorage(newRoom))
            {
                HospitalRoom newHospitalRoom = (HospitalRoom)newRoom;
                if (!IsNewRoomFound(newHospitalRoom)) { throw new HospitalRoomNotFoundException(newRoomId); }
            }
            else if (!IsNewRoomFound(newRoom)) { throw new HospitalRoomNotFoundException(newRoomId); }
        }

        private void IsEquipmentFroRearrangementValide(string equipmentId)
        {
            if (!IsEquipmentForRearrangementIdInputValide(equipmentId)) { throw new InvalideEquipmentIdExcpetion(equipmentId); }

            int parsedEquipmentForRearrangementId = Convert.ToInt32(equipmentId);
            Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);

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

            if (IsDateTimeBeforeCurrentDateTime(rearrangementDateTime)) { throw new DateIsBeforeTodaException(rearrangementDateTime.ToString()); }
        }

        private void IsPossibleToSetRearrangement(EquipmentRearrangement rearrangement)
        {
            if (WhetherRoomsAreSame(rearrangement.NewRoomID, rearrangement.OldRoomID))
            {
                throw new RoomsMustBeDifferenteException();
            }

            // Checking are rooms available
            Room currentRoom = RoomService.GetUsableHospitalPremesisForEquipmentTransfer(rearrangement.OldRoomID);
            Room newRoom = RoomService.GetUsableHospitalPremesisForEquipmentTransfer(rearrangement.NewRoomID);

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
            Equipment equipmentForRearrangement = EquipmentService.Get(parsedEquipmentForRearrangementId);

            if (!IsEquipmentForRearrangementFound(equipmentForRearrangement)) { throw new EquipmentNotFoundException(equipmentId); }
        }

        private void IsPossibleToUndoEquipmentRearrangement(EquipmentRearrangement rearrangement, Equipment equipment)
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
            IsEquipmentFroRearrangementValide(equipmentForRearrangementId);
            IsDateTimeValide(rearrangementDate, rearrangementTime);
        }
    }
}