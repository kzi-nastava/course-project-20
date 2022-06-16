using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Services
{
    public class RenovationScheduleService : IRenovationScheduleService
    {
        private readonly IRoomService _roomService;
        private readonly IHospitalRoomUnderConstructionService _hospitalRoomUnderConstructionService;
        private readonly IHospitalRoomForRenovationService _hospitalRoomForRenovationService;
        private readonly ARenovationScheduleRepository _renovationScheduleRepository;

        public RenovationScheduleService(IRoomService roomService, IHospitalRoomUnderConstructionService hospitalRoomUnderConstructionService, IHospitalRoomForRenovationService hospitalRoomForRenovationService, ARenovationScheduleRepository renovationScheduleRepository)
        {
            _roomService = roomService;
            _hospitalRoomUnderConstructionService = hospitalRoomUnderConstructionService;
            _hospitalRoomForRenovationService = hospitalRoomForRenovationService;
            _renovationScheduleRepository = renovationScheduleRepository;
        }

        public List<RenovationSchedule> GetRenovations()
        {
            return _renovationScheduleRepository.Renovations;
        }

        public void Add(RenovationSchedule newRenovation)
        {
            _renovationScheduleRepository.Renovations.Add(newRenovation);
            _renovationScheduleRepository.Save();
        }

        public bool Delete(RenovationSchedule renovation)
        {
            try
            {
                for (int i = 0; i < _renovationScheduleRepository.Renovations.Count; i++)
                {
                    if (renovation.ID == _renovationScheduleRepository.Renovations[i].ID)
                    {
                        _renovationScheduleRepository.Renovations.RemoveAt(i);
                        _renovationScheduleRepository.Save();
                        return true;
                    }
                }
                throw new RenovationScheduleNotFound();
            }
            catch (RenovationScheduleNotFound ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ScheduleSimpleRenovation(RenovationSchedule renovationSchedule, HospitalRoom roomForRenovation)
        {
            Add(renovationSchedule);
            HospitalRoomService.Delete(roomForRenovation);
            _hospitalRoomForRenovationService.Add(roomForRenovation);
        }

        public void ScheduleMergeRenovation(RenovationSchedule renovationSchedule, HospitalRoom room1, HospitalRoom room2, HospitalRoom newRoom)
        {
            _hospitalRoomUnderConstructionService.Add(newRoom);

            HospitalRoomService.Delete(room1);
            HospitalRoomService.Delete(room2);

            _hospitalRoomForRenovationService.Add(room1);
            _hospitalRoomForRenovationService.Add(room2);

            Add(renovationSchedule);
        }

        public void ScheduleSplitRenovation(RenovationSchedule renovationSchedule, HospitalRoom newRoom1, HospitalRoom newRoom2, HospitalRoom splitRoom)
        {
            _hospitalRoomForRenovationService.Add(splitRoom);
            HospitalRoomService.Delete(splitRoom);

            _hospitalRoomUnderConstructionService.Add(newRoom1);
            _hospitalRoomUnderConstructionService.Add(newRoom2);

            Add(renovationSchedule);
        }

        public void FinishRenovation(RenovationSchedule renovationSchedule)
        {
            if (IsDateBeforeToday(renovationSchedule.FinishDate))
            {
                if (renovationSchedule.RenovationType == RenovationType.Simple)
                {
                    FinishSimpleRenovation(renovationSchedule);
                }
                else if (renovationSchedule.RenovationType == RenovationType.Merge)
                {
                    FinishMergeRenovation(renovationSchedule);
                }
                else if (renovationSchedule.RenovationType == RenovationType.Split)
                {
                    FinishSplitRenovation(renovationSchedule);
                }
            }
        }

        private bool IsDateBeforeToday(DateTime date)
        {
            int value = DateTime.Compare(date, DateTime.Now);
            return value < 0;
        }

        private void FinishSimpleRenovation(RenovationSchedule renovationSchedule)
        {
            HospitalRoom renovatedRoom = _hospitalRoomForRenovationService.Get(renovationSchedule.MainRoomID);
            HospitalRoomService.Insert(renovatedRoom);
            _hospitalRoomForRenovationService.Delete(renovatedRoom);
            Delete(renovationSchedule);
        }

        private void FinishMergeRenovation(RenovationSchedule renovationSchedule)
        {
            HospitalRoom newRoom = _hospitalRoomUnderConstructionService.Get(renovationSchedule.MainRoomID);
            HospitalRoom room1 = _hospitalRoomForRenovationService.Get(renovationSchedule.Room1ID);
            HospitalRoom room2 = _hospitalRoomForRenovationService.Get(renovationSchedule.Room2ID);
            HospitalRoomService.Insert(newRoom);
            // -----

            _roomService.TransferAllEquipment(room1, newRoom);
            _roomService.TransferAllEquipment(room2, newRoom);
            // -----
            _hospitalRoomForRenovationService.Delete(room1);
            _hospitalRoomForRenovationService.Delete(room2);
            _hospitalRoomUnderConstructionService.Delete(newRoom.ID);
            Delete(renovationSchedule);
        }

        private void FinishSplitRenovation(RenovationSchedule renovationSchedule)
        {
            HospitalRoom mainRoom = _hospitalRoomForRenovationService.Get(renovationSchedule.MainRoomID);
            HospitalRoom room1 = _hospitalRoomUnderConstructionService.Get(renovationSchedule.Room1ID);
            HospitalRoom room2 = _hospitalRoomUnderConstructionService.Get(renovationSchedule.Room2ID);

            _hospitalRoomUnderConstructionService.Delete(room1.ID);
            _hospitalRoomUnderConstructionService.Delete(room2.ID);
            _hospitalRoomForRenovationService.Delete(mainRoom.ID);

            HospitalRoomService.Insert(room1);
            HospitalRoomService.Insert(room2);

            Delete(renovationSchedule);
        }
    }
}