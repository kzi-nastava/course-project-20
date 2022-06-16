using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Repositories
{
    public class RoomRepository
    {
        private AHospitalRoomUnderConstructionRepository _hospitalRoomUnderConstructionRepository;
        private AHospitalRoomForRenovationRepository _hospitalRoomForRenovationRepository;

        public RoomRepository(AHospitalRoomUnderConstructionRepository hospitalRoomUnderConstructionRepository, AHospitalRoomForRenovationRepository hospitalRoomForRenovationRepository)
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