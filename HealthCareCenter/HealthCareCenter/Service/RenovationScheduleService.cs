﻿using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    internal class RenovationScheduleService
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

        public static int GetLargestRenovationId()
        {
            try
            {
                List<RenovationSchedule> renovations = RenovationScheduleRepository.Renovations;
                renovations.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (renovations.Count == 0)
                {
                    return 0;
                }

                return renovations[renovations.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
    }
}