using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Rooms.Models;
using HealthCareCenter.Core.Rooms.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Services
{
    public class RenovationScheduleService
    {
        public static List<RenovationSchedule> GetRenovations()
        {
            return RenovationScheduleRepository.Renovations;
        }

        public static RenovationSchedule Get(int id)
        {
            try
            {
                foreach (RenovationSchedule renovation in RenovationScheduleRepository.Renovations)
                {
                    if (renovation.ID == id)
                    {
                        return renovation;
                    }
                }

                throw new RenovationScheduleNotFound();
            }
            catch (RenovationScheduleNotFound ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static RenovationSchedule Get(HospitalRoom room)
        {
            try
            {
                List<RenovationSchedule> renovations = GetRenovations();
                foreach (RenovationSchedule renovation in renovations)
                {
                    if (room.ID == renovation.Room1ID)
                        return renovation;
                    else if (room.ID == renovation.Room2ID)
                        return renovation;
                }
                throw new RenovationScheduleNotFound();
            }
            catch (RenovationScheduleNotFound ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Add(RenovationSchedule newRenovation)
        {
            RenovationScheduleRepository.Renovations.Add(newRenovation);
            RenovationScheduleRepository.Save();
        }

        public static bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < RenovationScheduleRepository.Renovations.Count; i++)
                {
                    if (id == RenovationScheduleRepository.Renovations[i].ID)
                    {
                        RenovationScheduleRepository.Renovations.RemoveAt(i);
                        RenovationScheduleRepository.Save();
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

        public static bool Delete(RenovationSchedule renovation)
        {
            try
            {
                for (int i = 0; i < RenovationScheduleRepository.Renovations.Count; i++)
                {
                    if (renovation.ID == RenovationScheduleRepository.Renovations[i].ID)
                    {
                        RenovationScheduleRepository.Renovations.RemoveAt(i);
                        RenovationScheduleRepository.Save();
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

        public static void ScheduleSimpleRenovation(RenovationSchedule renovationSchedule, HospitalRoom roomForRenovation)
        {
            Add(renovationSchedule);
            HospitalRoomService.Delete(roomForRenovation);
            HospitalRoomForRenovationService.Add(roomForRenovation);
        }

        public static void ScheduleMergeRenovation(RenovationSchedule renovationSchedule, HospitalRoom room1, HospitalRoom room2, HospitalRoom newRoom)
        {
            HospitalRoomUnderConstructionService.Add(newRoom);

            HospitalRoomService.Delete(room1);
            HospitalRoomService.Delete(room2);

            HospitalRoomForRenovationService.Add(room1);
            HospitalRoomForRenovationService.Add(room2);

            Add(renovationSchedule);
        }

        public static void ScheduleSplitRenovation(RenovationSchedule renovationSchedule, HospitalRoom newRoom1, HospitalRoom newRoom2, HospitalRoom splitRoom)
        {
            HospitalRoomForRenovationService.Add(splitRoom);
            HospitalRoomService.Delete(splitRoom);

            HospitalRoomUnderConstructionService.Add(newRoom1);
            HospitalRoomUnderConstructionService.Add(newRoom2);

            Add(renovationSchedule);
        }

        private static bool IsDateBeforeToday(DateTime date)
        {
            int value = DateTime.Compare(date, DateTime.Now);
            return value < 0;
        }

        private static void FinishSimpleRenovation(RenovationSchedule renovationSchedule)
        {
            HospitalRoom renovatedRoom = HospitalRoomForRenovationService.Get(renovationSchedule.MainRoomID);
            HospitalRoomService.Insert(renovatedRoom);
            HospitalRoomForRenovationService.Delete(renovatedRoom);
            Delete(renovationSchedule);
        }

        private static void FinishMergeRenovation(RenovationSchedule renovationSchedule)
        {
            HospitalRoom newRoom = HospitalRoomUnderConstructionService.Get(renovationSchedule.MainRoomID);
            HospitalRoom room1 = HospitalRoomForRenovationService.Get(renovationSchedule.Room1ID);
            HospitalRoom room2 = HospitalRoomForRenovationService.Get(renovationSchedule.Room2ID);
            HospitalRoomService.Insert(newRoom);
            // -----
            RoomService.TransferAllEquipment(room1, newRoom);
            RoomService.TransferAllEquipment(room2, newRoom);
            // -----
            HospitalRoomForRenovationService.Delete(room1);
            HospitalRoomForRenovationService.Delete(room2);
            HospitalRoomUnderConstructionService.Delete(newRoom.ID);
            Delete(renovationSchedule);
        }

        private static void FinishSplitRenovation(RenovationSchedule renovationSchedule)
        {
            HospitalRoom mainRoom = HospitalRoomForRenovationService.Get(renovationSchedule.MainRoomID);
            HospitalRoom room1 = HospitalRoomUnderConstructionService.Get(renovationSchedule.Room1ID);
            HospitalRoom room2 = HospitalRoomUnderConstructionService.Get(renovationSchedule.Room2ID);

            HospitalRoomUnderConstructionService.Delete(room1.ID);
            HospitalRoomUnderConstructionService.Delete(room2.ID);
            HospitalRoomForRenovationService.Delete(mainRoom.ID);

            HospitalRoomService.Insert(room1);
            HospitalRoomService.Insert(room2);

            Delete(renovationSchedule);
        }

        public static void FinishRenovation(RenovationSchedule renovationSchedule)
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
    }
}