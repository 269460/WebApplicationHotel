using HotelBookingApp.Domain.Interfaces;
using HotelBookingApp.Domain.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Infrastructure.Data
{
    public class RoomRepository : IRoomRepository
    {
        private readonly string _masterConnectionString;
        private readonly string _slaveConnectionString;

        public RoomRepository(IConfiguration configuration)
        {
            _masterConnectionString = configuration.GetConnectionString("MasterConnection");
            _slaveConnectionString = configuration.GetConnectionString("SlaveConnection");
        }

        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            Room room = null;
            var query = "SELECT RoomId, RoomNumber, Capacity FROM Rooms WHERE RoomId = @RoomId";

            using (var connection = new MySqlConnection(_slaveConnectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomId", roomId);
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        room = new Room
                        {
                            RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                            Number = reader.GetString(reader.GetOrdinal("RoomNumber")),
                            Capacity = reader.GetInt32(reader.GetOrdinal("Capacity"))
                        };
                    }
                }
            }

            return room;
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync()
        {
            var rooms = new List<Room>();
            var query = "SELECT RoomId, RoomNumber, Capacity FROM Rooms";

            using (var connection = new MySqlConnection(_slaveConnectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var room = new Room
                        {
                            RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                            Number = reader.GetString(reader.GetOrdinal("RoomNumber")),
                            Capacity = reader.GetInt32(reader.GetOrdinal("Capacity"))
                        };
                        rooms.Add(room);
                    }
                }
            }

            return rooms;
        }

        public async Task AddRoomAsync(Room room)
        {
            var query = "INSERT INTO Rooms (RoomNumber, Capacity) VALUES (@RoomNumber, @Capacity)";

            using (var connection = new MySqlConnection(_masterConnectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomNumber", room.Number);
                command.Parameters.AddWithValue("@Capacity", room.Capacity);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateRoomAsync(Room room)
        {
            var query = "UPDATE Rooms SET RoomNumber = @RoomNumber, Capacity = @Capacity WHERE RoomId = @RoomId";

            using (var connection = new MySqlConnection(_masterConnectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomNumber", room.Number);
                command.Parameters.AddWithValue("@Capacity", room.Capacity);
                command.Parameters.AddWithValue("@RoomId", room.RoomId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteRoomAsync(int roomId)
        {
            var query = "DELETE FROM Rooms WHERE RoomId = @RoomId";

            using (var connection = new MySqlConnection(_masterConnectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomId", roomId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Room>> GetReservedRoomsAsync(DateTime startDate, DateTime endDate)
        {
            var reservedRooms = new List<Room>();
            var query = @"
            SELECT r.RoomId, r.RoomNumber, r.Capacity 
            FROM Rooms r
            INNER JOIN Reservations res ON r.RoomId = res.RoomId
            WHERE res.StartDate < @EndDate AND res.EndDate > @StartDate";

            using (var connection = new MySqlConnection(_slaveConnectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StartDate", startDate);
                command.Parameters.AddWithValue("@EndDate", endDate);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var room = new Room
                        {
                            RoomId = reader.GetInt32(reader.GetOrdinal("RoomId")),
                            Number = reader.GetString(reader.GetOrdinal("RoomNumber")),
                            Capacity = reader.GetInt32(reader.GetOrdinal("Capacity"))
                        };
                        reservedRooms.Add(room);
                    }
                }
            }

            return reservedRooms;
        }
    }
}
