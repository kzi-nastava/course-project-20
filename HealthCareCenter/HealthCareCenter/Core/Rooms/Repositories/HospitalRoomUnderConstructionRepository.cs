using HealthCareCenter.Core.Rooms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Repositories
{
    public class HospitalRoomUnderConstructionRepository : AHospitalRoomUnderConstructionRepository
    {
        private const string fileName = "hospitalRoomsUnderConstruction.json";

        public HospitalRoomUnderConstructionRepository()
        {
            Rooms = Load();
        }

        public override int GetLargestRoomId()
        {
            try
            {
                List<HospitalRoom> rooms = Rooms;
                rooms.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (rooms.Count == 0)
                {
                    return 0;
                }

                return rooms[rooms.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override List<HospitalRoom> Load()
        {
            try
            {
                List<HospitalRoom> rooms = new List<HospitalRoom>();
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = Constants.DateFormat
                };

                string JSONTextHospitalRooms = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"..\..\..\data\" + fileName);
                rooms = (List<HospitalRoom>)JsonConvert.DeserializeObject<IEnumerable<HospitalRoom>>(JSONTextHospitalRooms, settings);
                return rooms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override bool Save()
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();

                using (StreamWriter sw = new StreamWriter(@"..\..\..\data\" + fileName))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Rooms);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}