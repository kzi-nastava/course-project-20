using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Equipment.Repositories;
using HealthCareCenter.Core.Appointments.Repository;

namespace HealthCareCenter.Core.Rooms.Controllers
{
    public class ComplexRoomRenovationSplitController
    {
        private readonly IRoomService _roomService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;
        private readonly IRenovationScheduleService _renovationScheduleService;
        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly IHospitalRoomService _hospitalRoomService;

        public ComplexRoomRenovationSplitController(
            IRoomService roomService,
            IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService,
            IRenovationScheduleService renovationScheduleService,
            IHospitalRoomForRenovationService hospitalRoomForRenovationService,
            IHospitalRoomService hospitalRoomService)
        {
            _roomService = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;            _renovationScheduleService = renovationScheduleService;            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;            _hospitalRoomService = hospitalRoomService;        }

        public HospitalRoom GenerateNewRoom1(string room1Type, string room1Name)
        {
            Enum.TryParse(room1Type, out RoomType parsedRoom1Type);
            HospitalRoom newRoom1 = new HospitalRoom(parsedRoom1Type, room1Name);
            return newRoom1;
        }

        public HospitalRoom GenerateNewRoom2(string room2Type, string room2Name)
        {
            Enum.TryParse(room2Type, out RoomType parsedRoom2Type);
            HospitalRoom newRoom2 = new HospitalRoom(parsedRoom2Type, room2Name);
            newRoom2.ID += 1;
            return newRoom2;
        }

        public HospitalRoom GetSplitRoom(string splitRoomId)
        {
            int parsedSplitRoomId = Convert.ToInt32(splitRoomId);
            HospitalRoom splitRoom = _hospitalRoomService.Get(parsedSplitRoomId);
            return splitRoom;
        }

        public List<HospitalRoom> GetRoomsForDisplay()
        {
            return _hospitalRoomService.GetRooms();
        }

        public List<List<string>> GetSplitRenovationsForDisplay()
        {
            List<List<string>> renovationForDisplay = new List<List<string>>();
            List<RenovationSchedule> renovations = _renovationScheduleService.GetRenovations();
            foreach (RenovationSchedule renovation in renovations)
            {
                if (renovation.RenovationType == RenovationType.Split)
                {
                    HospitalRoom room1 = _hospitalRoomUnderConstructionService.Get(renovation.Room1ID);
                    HospitalRoom room2 = _hospitalRoomUnderConstructionService.Get(renovation.Room2ID);
                    HospitalRoom splitRoom = _hospitalRoomForRenovationService.Get(renovation.MainRoomID);

                    List<string> renovationAttributes = new List<string> {
                    room1.ID.ToString(),room1.Name,room1.Type.ToString(),
                    room2.ID.ToString(),room2.Name,room2.Type.ToString(),
                    renovation.StartDate.ToString(),renovation.FinishDate.ToString(),
                    splitRoom.ID.ToString(),splitRoom.Name,splitRoom.Type.ToString()
                };

                    renovationForDisplay.Add(renovationAttributes);
                }
            }

            return renovationForDisplay;
        }

        public void Split(string splitRoomId, string room1Name, string room2Name, string room1Type, string room2Type, string startDate, string finishDate)
        {
            Enum.TryParse(room1Type, out RoomType parsedRoom1Type);
            Enum.TryParse(room2Type, out RoomType parsedRoom2Type);

            IsSplittingPossible(splitRoomId, room1Name, room2Name, startDate, finishDate);

            int parsedSplitRoomId = Convert.ToInt32(splitRoomId);
            HospitalRoom splitRoom = _hospitalRoomService.Get(parsedSplitRoomId);

            HospitalRoom newRoom1 = new HospitalRoom(parsedRoom1Type, room1Name);
            HospitalRoom newRoom2 = new HospitalRoom(parsedRoom2Type, room2Name);
            newRoom2.ID += 1;

            DateTime parsedStartDate = Convert.ToDateTime(startDate);
            DateTime parsedFinishDate = Convert.ToDateTime(finishDate);

            // Set split renovation
            // ------------------------------
            RenovationSchedule splitRenovation = new RenovationSchedule(
                parsedStartDate, parsedFinishDate,
                newRoom1, newRoom2, splitRoom,
                RenovationType.Split);

            _renovationScheduleService.ScheduleSplitRenovation(splitRenovation, newRoom1, newRoom2, splitRoom);
            // ------------------------------
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

        private bool IsDateInputBeforeCurrentTime(DateTime date)
        {
            DateTime now = DateTime.Now;
            int value = DateTime.Compare(date, now);
            return value < 0;
        }

        private bool IsEndDateBeforeStartDate(DateTime startDate, DateTime endDate)
        {
            int value = DateTime.Compare(endDate, startDate);
            return value < 0;
        }

        private void IsDateValide(string startDate, string finishDate)
        {
            if (!IsDateInputValide(startDate))
            {
                throw new InvalideDateException(startDate);
            }

            if (!IsDateInputValide(finishDate))
            {
                throw new InvalideDateException(finishDate);
            }

            DateTime parsedStartDate = Convert.ToDateTime(startDate);
            DateTime parsedFinishDate = Convert.ToDateTime(finishDate);
            if (IsDateInputBeforeCurrentTime(parsedStartDate))
            {
                throw new DateIsBeforeTodayException(startDate);
            }

            if (IsDateInputBeforeCurrentTime(parsedFinishDate))
            {
                throw new DateIsBeforeTodayException(finishDate);
            }

            if (IsEndDateBeforeStartDate(parsedStartDate, parsedFinishDate))
            {
                throw new Exception("Error, finish date is before start date");
            }
        }

        private void IsSplitRoomValide(string splitRoomId)
        {
            if (!IsRoomIdInputValide(splitRoomId))
            {
                throw new InvalideHospitalRoomIdException(splitRoomId);
            }

            int parsedSplitRoomId = Convert.ToInt32(splitRoomId);
            HospitalRoom splitRoom = _hospitalRoomService.Get(parsedSplitRoomId);

            if (!IsHospitalRoomFound(splitRoom))
            {
                throw new HospitalRoomNotFoundException(splitRoomId);
            }
        }

        private void IsPossibleRenovation(HospitalRoom splitRoom)
        {
            if (_hospitalRoomService.ContainsAnyAppointment(splitRoom))
            {
                throw new HospitalRoomContainAppointmentException(splitRoom.ID.ToString());
            }

            if (_roomService.ContainsAnyRearrangement(
                splitRoom, 
                new EquipmentRearrangementService(
                    new RoomService(
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository()),
                        new HospitalRoomService(
                            new AppointmentRepository(),
                            new HospitalRoomForRenovationService(
                                new HospitalRoomForRenovationRepository()),
                            new HospitalRoomRepository())),
                    new EquipmentService(
                        new EquipmentRepository()),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository()))))
            {
                throw new HospitalRoomContainEquipmentRearrangementException(splitRoom.ID.ToString());
            }
        }

        private void IsSplittingPossible(string splitRoomId, string room1Name, string room2Name, string startDate, string finishDate)
        {
            IsSplitRoomValide(splitRoomId);

            int parsedSplitRoomId = Convert.ToInt32(splitRoomId);
            HospitalRoom splitRoom = _hospitalRoomService.Get(parsedSplitRoomId);
            IsPossibleRenovation(splitRoom);
            if (!IsHospitalRoomNameInputValide(room1Name) || !IsHospitalRoomNameInputValide(room2Name))
            {
                throw new InvalideHospitalRoomNameException();
            }
            IsDateValide(startDate, finishDate);
        }
    }
}