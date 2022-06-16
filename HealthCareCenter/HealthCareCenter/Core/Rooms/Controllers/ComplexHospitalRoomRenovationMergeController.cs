using System;
using System.Collections.Generic;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Exceptions;

namespace HealthCareCenter.Core.Rooms.Controllers
{
    internal class ComplexHospitalRoomRenovationMergeController
    {
        private readonly IRoomService _roomSerivece;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;
        private readonly IRenovationScheduleService _renovationScheduleService;
        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly IHospitalRoomService _hospitalRoomService;

        public ComplexHospitalRoomRenovationMergeController(IRoomService roomService,
            IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService,
            IRenovationScheduleService renovationScheduleService, IHospitalRoomForRenovationService hospitalRoomForRenovationService,
            IHospitalRoomService hospitalRoomService)
        {
            _roomSerivece = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _renovationScheduleService = renovationScheduleService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _hospitalRoomService = hospitalRoomService;
        }

        public List<HospitalRoom> GetRoomsForDisplay()
        {
            return _hospitalRoomService.GetRooms();
        }

        public void Merge(string room1Id, string room2Id, string renovationStartDate, string renovationFinishDate, string newRoomName, string newRoomType)
        {
            IsPossibleMerge(room1Id, room2Id, renovationStartDate, renovationFinishDate, newRoomName, newRoomType);

            int parsedRoom1Id = Convert.ToInt32(room1Id);
            int parsedRoom2Id = Convert.ToInt32(room2Id);
            HospitalRoom room1 = _hospitalRoomService.Get(parsedRoom1Id);
            HospitalRoom room2 = _hospitalRoomService.Get(parsedRoom2Id);

            DateTime parsedRenovationStartDate = Convert.ToDateTime(renovationStartDate);
            DateTime parsedRenovationFinishDate = Convert.ToDateTime(renovationFinishDate);

            Enum.TryParse(newRoomType, out RoomType parsedNewRoomType);
            HospitalRoom newRoom = new HospitalRoom(parsedNewRoomType, newRoomName);

            // SetMergeRenovation
            // -----------------------------------------
            RenovationSchedule mergeRenovation = new RenovationSchedule(
                parsedRenovationStartDate, parsedRenovationFinishDate,
                room1, room2, newRoom, RenovationType.Merge
                );
            _renovationScheduleService.ScheduleMergeRenovation(mergeRenovation, room1, room2, newRoom);
            // -----------------------------------------
        }

        public List<List<string>> GetAllMergeRenovations()
        {
            List<List<string>> renovationsForDisplay = new List<List<string>>();

            List<RenovationSchedule> renovations = _renovationScheduleService.GetRenovations();
            foreach (RenovationSchedule renovation in renovations)
            {
                if (renovation.RenovationType == RenovationType.Merge)
                {
                    HospitalRoom room1 = _hospitalRoomForRenovationService.Get(renovation.Room1ID);
                    HospitalRoom room2 = _hospitalRoomForRenovationService.Get(renovation.Room2ID);
                    HospitalRoom newRoom = _hospitalRoomUnderConstructionService.Get(renovation.MainRoomID);

                    List<string> renovationAttribute = new List<string> {
                    room1.ID.ToString(),room1.Name,room1.Type.ToString(),
                    room2.ID.ToString(),room2.Name,room2.Type.ToString(),
                    renovation.StartDate.ToString(),renovation.FinishDate.ToString(),
                    newRoom.ID.ToString(),newRoom.Name,newRoom.Type.ToString()
                };

                    renovationsForDisplay.Add(renovationAttribute);
                }
            }
            return renovationsForDisplay;
        }

        private bool IsRoomIdInputValide(string roomId)
        {
            return int.TryParse(roomId, out int _);
        }

        private bool IsHospitalRoomFound(HospitalRoom room)
        {
            return room != null;
        }

        private bool IsHospitalRoomNameInputValide(string roomName)
        {
            return roomName != "";
        }

        private bool IsDateInputValide(string date)
        {
            return DateTime.TryParse(date, out DateTime _);
        }

        private bool IsDateBeforeCurrentDate(DateTime date)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(date, now);
            return value < 0;
        }

        private bool IsFinishDateBeforeStartDate(DateTime startDate, DateTime finishDate)
        {
            int value = DateTime.Compare(finishDate, startDate);
            return value < 0;
        }

        private void IsHospitalRoomValide(string roomId)
        {
            if (!IsRoomIdInputValide(roomId))
            {
                throw new InvalideHospitalRoomIdException(roomId);
            }
            int parsedHospitalRoomId = Convert.ToInt32(roomId);

            HospitalRoom room = _hospitalRoomService.Get(parsedHospitalRoomId);

            if (!IsHospitalRoomFound(room))
            {
                throw new HospitalRoomNotFoundException(roomId);
            }
        }

        private void WhetherRoomsAreSame(string room1Id, string room2Id)
        {
            if (room1Id == room2Id)
            {
                throw new RoomsMustBeDifferenteException();
            }
        }

        private void IsPossibleRoomRenovation(HospitalRoom room)
        {
            if (_hospitalRoomService.ContainsAnyAppointment(room))
            {
                throw new HospitalRoomContainAppointmentException(room.ID.ToString());
            }

            if (_roomSerivece.ContainAnyEquipment(room))
            {
                throw new HospitalRoomContainEquipmentRearrangementException(room.ID.ToString());
            }
        }

        private void IsDateValide(string date)
        {
            if (!IsDateInputValide(date))
            {
                throw new InvalideDateException(date);
            }
            DateTime parsedDate = Convert.ToDateTime(date);

            if (IsDateBeforeCurrentDate(parsedDate))
            {
                throw new DateIsBeforeTodayException(date);
            }
        }

        private void ValidateRooms(string room1Id, string room2Id, string newRoomName)
        {
            IsHospitalRoomValide(room1Id);
            IsHospitalRoomValide(room2Id);
            WhetherRoomsAreSame(room1Id, room2Id);
            if (!IsHospitalRoomNameInputValide(newRoomName))
            {
                throw new InvalideHospitalRoomNameException();
            }
        }

        private void IsPossibleRenovation(string room1Id, string room2Id)
        {
            int parsedRoom1Id = Convert.ToInt32(room1Id);
            int parsedRoom2Id = Convert.ToInt32(room2Id);
            HospitalRoom room1 = _hospitalRoomService.Get(parsedRoom1Id);
            HospitalRoom room2 = _hospitalRoomService.Get(parsedRoom2Id);

            IsPossibleRoomRenovation(room1);
            IsPossibleRoomRenovation(room2);
        }

        private void DateValidation(string renovationStartDate, string renovationFinishDate)
        {
            IsDateValide(renovationStartDate);
            IsDateValide(renovationFinishDate);

            DateTime parsedRenovationStartDate = Convert.ToDateTime(renovationStartDate);
            DateTime parsedRenovationFinishDate = Convert.ToDateTime(renovationFinishDate);
            if (IsFinishDateBeforeStartDate(parsedRenovationStartDate, parsedRenovationFinishDate))
            {
                throw new Exception("Error, finish date is before start date");
            }
        }

        private void IsPossibleMerge(string room1Id, string room2Id, string renovationStartDate, string renovationFinishDate, string newRoomName, string newRoomType)
        {
            ValidateRooms(room1Id, room2Id, newRoomName);
            IsPossibleRenovation(room1Id, room2Id);
            DateValidation(renovationStartDate, renovationFinishDate);
        }
    }
}