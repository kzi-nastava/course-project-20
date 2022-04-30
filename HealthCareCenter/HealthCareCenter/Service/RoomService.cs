using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    internal class RoomService
    {
        /// <summary>
        /// Get every equipment amount like dictionary, where key is name of equipment and value amount.
        /// </summary>
        /// <returns>equipments amount</returns>
        public static Dictionary<string, int> GetEquipmentsAmount()
        {
            List<HospitalRoom> rooms = HospitalRoomService.GetRooms();
            Room storage = StorageRepository.GetStorage();

            List<Room> hospitalPremises = new List<Room>();

            hospitalPremises.Add(storage);
            foreach (HospitalRoom room in rooms)
                hospitalPremises.Add(room);

            Dictionary<string, int> equipmentsAmount = new Dictionary<string, int>();
            foreach (Room room in hospitalPremises)
            {
                foreach (KeyValuePair<string, int> entry in room.EquipmentAmounts)
                {
                    if (equipmentsAmount.ContainsKey(entry.Key))
                        equipmentsAmount[entry.Key] = equipmentsAmount[entry.Key] + entry.Value;
                    else
                        equipmentsAmount[entry.Key] = entry.Value;
                }
            }
            return equipmentsAmount;
        }

        /// <summary>
        /// Update room by type, if is storage thand update storage or if is hospital room than update hospital room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public static bool UpdateRoom(Room room)
        {
            try
            {
                if (room.IsStorage())
                    StorageRepository.SaveStorage(room);
                else
                    HospitalRoomService.UpdateRoom((HospitalRoom)room);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}