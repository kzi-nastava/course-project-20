using HealthCareCenter.Core.Appointments.Models;
using HealthCareCenter.Core.Rooms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Rooms.Services
{
    public interface IHospitalRoomService
    {
        List<HospitalRoom> GetRooms();
        HospitalRoom Get(int id);
        void Add(HospitalRoom newRoom);
        void Insert(HospitalRoom room);
        bool Delete(int id);
        bool Delete(HospitalRoom room);
        bool Update(HospitalRoom room);
        void Update(int roomID, Appointment appointment);
        int GetAvailableRoomID(DateTime scheduledDate, RoomType roomType);
        void AddAppointmentToRoom(int hospitalRoomID, int appointmentID);
        bool IsCurrentlyRenovating(HospitalRoom room);
        bool ContainsAnyAppointment(HospitalRoom room);
        List<HospitalRoom> GetRoomsOfType(AppointmentType type);
        bool IsOccupied(int id, DateTime time);
        void RemoveUnavailableRooms(List<HospitalRoom> availableRooms, Appointment appointment);
        List<HospitalRoomForDisplay> GetRooms(bool checkup);
    }
}
