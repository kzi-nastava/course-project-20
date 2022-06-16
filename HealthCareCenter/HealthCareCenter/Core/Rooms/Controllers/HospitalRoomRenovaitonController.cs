using System;
using System.Collections.Generic;
using HealthCareCenter.Core.Rooms.Services;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Equipment.Services;
using HealthCareCenter.Core.Rooms.Repositories;
using HealthCareCenter.Core.Equipment.Repositories;

namespace HealthCareCenter.Core.Rooms.Controllers
{
    public class HospitalRoomRenovaitonController
    {
        private readonly IRoomService _roomService;
        private readonly IRenovationScheduleService _renovationScheduleService;

        public HospitalRoomRenovaitonController(IRoomService roomService, IRenovationScheduleService renovationScheduleService)
        {
            _roomService = roomService;
            _renovationScheduleService = renovationScheduleService;
        }

        public void ScheduleRenovation(string hospitalRoomForRenovationId, string startDate, string finishDate)
        {
            IsPossibleToScheduleRenovtion(hospitalRoomForRenovationId, startDate, finishDate);

            int parsedHospitalRoomForRenovationId = Convert.ToInt32(hospitalRoomForRenovationId);
            HospitalRoom roomForRenovation = HospitalRoomService.Get(parsedHospitalRoomForRenovationId);

            DateTime parsedStartDate = Convert.ToDateTime(startDate);
            DateTime parsedFinishDate = Convert.ToDateTime(finishDate);

            RenovationSchedule renovation = new RenovationSchedule(parsedStartDate, parsedFinishDate, roomForRenovation);
            _renovationScheduleService.ScheduleSimpleRenovation(renovation, roomForRenovation);
        }

        public List<HospitalRoom> GetRoomsForDisplay()
        {
            return HospitalRoomService.GetRooms();
        }

        public List<RenovationSchedule> GetRenovationsForDisplay()
        {
            return _renovationScheduleService.GetRenovations();
        }

        private bool IsHospitalRoomIdInputValide(string roomId)
        {
            return int.TryParse(roomId, out _);
        }

        private bool IsHospitalRoomFound(HospitalRoom room)
        {
            return room != null;
        }

        private bool IsDateInputValide(string date)
        {
            return DateTime.TryParse(date, out DateTime _);
        }

        private bool IsDateBeforeCurrentTime(DateTime date)
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

        private void IsDateValide(string startDate, string finishDate)
        {
            if (!IsDateInputValide(startDate)) { throw new InvalideDateException(startDate); }
            DateTime parsedStartDate = Convert.ToDateTime(startDate);

            if (!IsDateInputValide(finishDate)) { throw new InvalideDateException(finishDate); }
            DateTime parsedFinishDate = Convert.ToDateTime(finishDate);

            if (IsDateBeforeCurrentTime(parsedStartDate)) { throw new DateIsBeforeTodayException(parsedStartDate.ToString()); }

            if (IsDateBeforeCurrentTime(parsedFinishDate)) { throw new DateIsBeforeTodayException(parsedFinishDate.ToString()); }

            if (IsFinishDateBeforeStartDate(parsedStartDate, parsedFinishDate)) { throw new Exception($"Finish date={parsedFinishDate} is before start date={parsedStartDate}!"); }
        }

        private void IsHospitalRoomValide(string roomId)
        {
            if (!IsHospitalRoomIdInputValide(roomId)) { throw new InvalideHospitalRoomIdException(roomId); }

            int parsedHospitalRoomId = Convert.ToInt32(roomId);

            HospitalRoom roomForRenovation = HospitalRoomService.Get(parsedHospitalRoomId);

            if (!IsHospitalRoomFound(roomForRenovation)) { throw new HospitalRoomNotFoundException(roomId); }
        }

        private void IsPossibleRenovation(HospitalRoom roomForRenovation)
        {
            if (HospitalRoomService.ContainsAnyAppointment(roomForRenovation)) { throw new HospitalRoomContainAppointmentException(roomForRenovation.ID.ToString()); }
            if (_roomService.ContainsAnyRearrangement(
                roomForRenovation,
                new EquipmentRearrangementService(
                    new RoomService(
                        new StorageRepository(),
                        new EquipmentService(
                            new EquipmentRepository()),
                        new HospitalRoomUnderConstructionService(
                            new HospitalRoomUnderConstructionRepository()),
                        new HospitalRoomForRenovationService(
                            new HospitalRoomForRenovationRepository())),
                    new EquipmentService(
                        new EquipmentRepository()),
                    new HospitalRoomUnderConstructionService(
                        new HospitalRoomUnderConstructionRepository())))) { throw new HospitalRoomContainEquipmentRearrangementException(roomForRenovation.ID.ToString()); }
        }

        private void IsPossibleToScheduleRenovtion(string hospitalRoomForRenovationId, string startDate, string finishDate)
        {
            IsHospitalRoomValide(hospitalRoomForRenovationId);
            IsDateValide(startDate, finishDate);

            int parsedHospitalRoomForRenovationId = Convert.ToInt32(hospitalRoomForRenovationId);
            HospitalRoom roomForRenovation = HospitalRoomService.Get(parsedHospitalRoomForRenovationId);
            IsPossibleRenovation(roomForRenovation);
        }
    }
}