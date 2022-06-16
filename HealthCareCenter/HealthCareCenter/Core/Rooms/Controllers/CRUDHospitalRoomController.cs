using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Controllers
{
    public class CRUDHospitalRoomController
    {
        private IRoomService _roomService;
        private IHospitalRoomForRenovationService _hospitalRoomForRenovationService;

        public CRUDHospitalRoomController(IRoomService roomService, IHospitalRoomForRenovationService hospitalRoomForRenovationService)
        {
            _roomService = roomService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
        }

        public void Create(RoomType roomType, string roomName)
        {
            if (!IsHospitalRoomNameInputValide(roomName)) { throw new InvalideHospitalRoomNameException(); }
            HospitalRoom room = new HospitalRoom(roomType, roomName);
            HospitalRoomService.Add(room);
        }

        public void Delete(string roomId)
        {
            IsPossibleToDeleteHospitalRoom(roomId);
            int parsedRoomId = Convert.ToInt32(roomId);
            HospitalRoom hospitalRoom = HospitalRoomService.Get(parsedRoomId);
            HospitalRoomService.Delete(hospitalRoom);
        }

        public void Update(string newRoomName, RoomType newRoomType, string roomId)
        {
            IsPossibleRoomToUpdate(newRoomName, roomId);

            int parsedRoomId = Convert.ToInt32(roomId);
            HospitalRoom hospitalRoom = HospitalRoomService.Get(parsedRoomId);
            hospitalRoom.Name = newRoomName;
            hospitalRoom.Type = newRoomType;
            _roomService.Update(hospitalRoom);
        }

        public List<HospitalRoom> GetRoomsToDisplay()
        {
            List<HospitalRoom> availableRooms = HospitalRoomService.GetRooms();
            List<HospitalRoom> roomsUnderRenovationProcess = _hospitalRoomForRenovationService.GetRooms();
            List<HospitalRoom> roomsForDisplay = new List<HospitalRoom>();
            roomsForDisplay.AddRange(availableRooms);
            roomsForDisplay.AddRange(roomsUnderRenovationProcess);
            return roomsForDisplay;
        }

        private bool IsHospitalRoomNameInputValide(string roomName)
        {
            return roomName != "";
        }

        private bool IsHospitalRoomIdInputValide(string roomId)
        {
            return int.TryParse(roomId, out int _);
        }

        private bool IsHospitalRoomFound(Room room)
        {
            return room != null;
        }

        private void IsPossibleToDeleteHospitalRoom(string roomId)
        {
            if (!IsHospitalRoomIdInputValide(roomId)) { throw new InvalideHospitalRoomIdException(roomId); }

            int parsedRoomId = Convert.ToInt32(roomId);
            Room room = _roomService.Get(parsedRoomId);

            if (!IsHospitalRoomFound(room)) { throw new HospitalRoomNotFoundException(roomId); }

            if (_roomService.IsStorage(room)) { throw new StorageIdException(); }

            HospitalRoom hospitalRoom = (HospitalRoom)room;

            if (_roomService.ContainAnyEquipment(hospitalRoom)) { throw new HospitalRoomContainsEquipmentException(roomId); }

            if (HospitalRoomService.ContainsAnyAppointment(hospitalRoom)) { throw new HospitalRoomContainsEquipmentException(roomId); }

            if (HospitalRoomService.IsCurrentlyRenovating(hospitalRoom)) { throw new HospitalRoomUnderRenovationException(roomId); }
        }

        private void IsPossibleRoomToUpdate(string newRoomName, string roomId)
        {
            if (!IsHospitalRoomIdInputValide(roomId)) { throw new InvalideHospitalRoomIdException(roomId); }
            int parsedRoomId = Convert.ToInt32(roomId);

            if (!IsHospitalRoomNameInputValide(newRoomName)) { throw new InvalideHospitalRoomNameException(); }

            Room room = _roomService.Get(parsedRoomId);
            if (_roomService.IsStorage(room)) { throw new StorageIdException(); }

            HospitalRoom hospitalRoom = (HospitalRoom)room;
            if (!IsHospitalRoomFound(hospitalRoom)) { throw new HospitalRoomNotFoundException(roomId); }
        }
    }
}