using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    internal class RoomRepository
    {
        /// <summary>
        /// Get every equipment amount like dictionary, where key is name of equipment and value amount.
        /// </summary>
        /// <returns>equipments amount</returns>
        public static Dictionary<string, int> GetEquipmentsAmount()
        {
            List<HospitalRoom> rooms = HospitalRoomRepository.GetRooms();
            Room storage = StorageRepository.GetStorage();

            List<Room> hospitalPremises = new List<Room>();

            hospitalPremises.Add(storage);
            foreach (HospitalRoom room in rooms)
                hospitalPremises.Add(room);

            Dictionary<string, int> equipmentsAmount = new Dictionary<string, int>();
            foreach (Room room in hospitalPremises)
            {
                foreach (KeyValuePair<string, int> entry in room.EquipmentIDsAmounts)
                {
                    if (equipmentsAmount.ContainsKey(entry.Key))
                        equipmentsAmount[entry.Key] = equipmentsAmount[entry.Key] + entry.Value;
                    else
                        equipmentsAmount[entry.Key] = entry.Value;
                }
            }
            return equipmentsAmount;
        }
    }
}