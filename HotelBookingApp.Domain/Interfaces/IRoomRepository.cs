namespace HotelBookingApp.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using HotelBookingApp.Domain.Models;

    public interface IRoomRepository
    {
        Task<Room> GetRoomByIdAsync(int roomId);
        Task<IEnumerable<Room>> GetAllRoomsAsync();
        Task AddRoomAsync(Room room);
        Task UpdateRoomAsync(Room room);
        Task DeleteRoomAsync(int roomId);
        Task<IEnumerable<Room>> GetReservedRoomsAsync(DateTime startDate, DateTime endDate);
    }
}