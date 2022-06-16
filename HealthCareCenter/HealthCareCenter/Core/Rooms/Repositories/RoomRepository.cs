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
        private readonly BaseHospitalRoomRepository _hospitalRoomRepository;

        public RoomRepository(
            BaseHospitalRoomUnderConstructionRepository hospitalRoomUnderConstructionRepository,
            BaseHospitalRoomForRenovationRepository hospitalRoomForRenovationRepository,
            BaseHospitalRoomRepository hospitalRoomRepository)
        {
            _hospitalRoomUnderConstructionRepository = hospitalRoomUnderConstructionRepository;
            _hospitalRoomForRenovationRepository = hospitalRoomForRenovationRepository;
            _hospitalRoomRepository = hospitalRoomRepository;
        }

        public List<int> GetLargestIDs()
        {
            List<int> largestIDs = new List<int> { _hospitalRoomRepository.GetLargestID(), _hospitalRoomForRenovationRepository.GetLargestRoomId(), _hospitalRoomUnderConstructionRepository.GetLargestRoomId() };
            return largestIDs;
        }
    }
}