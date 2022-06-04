using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;

namespace HealthCareCenter.Secretary.Controllers
{
    public class DistributeDynamicEquipmentController
    {
        public void Transfer(string quantityString, string[] equipmentAndAmount, Room roomWithEquipment, Room roomWithShortage, Room storage)
        {
            if (roomWithEquipment.ID == roomWithShortage.ID)
            {
                throw new Exception("You must select different rooms.");
            }

            if (!int.TryParse(quantityString, out int quantity) || quantity <= 0)
            {
                throw new Exception("Quantity must be a positive number.");
            }

            string selectedEquipment = equipmentAndAmount[0];
            int selectedEquipmentAmount = int.Parse(equipmentAndAmount[1]);

            if (quantity > selectedEquipmentAmount)
            {
                throw new Exception("You cannot enter a quantity bigger than the amount of equipment available in the room.");
            }

            DynamicEquipmentService.Transfer(quantity, selectedEquipment, roomWithEquipment, roomWithShortage, storage);
        }

        public Room GetRoom(int roomID)
        {
            return HospitalRoomService.Get(roomID);
        }
    }
}
