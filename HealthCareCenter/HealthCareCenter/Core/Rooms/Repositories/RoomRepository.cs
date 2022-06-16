using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Repositories
{
    public class RoomRepository
    {
        private BaseHospitalRoomUnderConstructionRepository _hospitalRoomUnderConstructionRepository;
        private BaseHospitalRoomForRenovationRepository _hospitalRoomForRenovationRepository;

        public RoomRepository(BaseHospitalRoomUnderConstructionRepository hospitalRoomUnderConstructionRepository, BaseHospitalRoomForRenovationRepository hospitalRoomForRenovationRepository)
        {
            _hospitalRoomUnderConstructionRepository = hospitalRoomUnderConstructionRepository;
            _hospitalRoomForRenovationRepository = hospitalRoomForRenovationRepository;
        }

        public List<int> GetLargestIDs()
        {
            List<int> largestIDs = new List<int> { HospitalRoomRepository.GetLargestRoomId(), _hospitalRoomForRenovationRepository.GetLargestRoomId(), _hospitalRoomUnderConstructionRepository.GetLargestRoomId() };
            return largestIDs;
        }
    }
}