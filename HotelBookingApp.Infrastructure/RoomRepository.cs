using System;
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
        private readonly string _connectionString;

        public RoomRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            Room room = null;
            var query = "SELECT RoomId, RoomNumber, Capacity FROM Rooms WHERE RoomId = @RoomId";

            using (var connection = new MySqlConnection(_connectionString))
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
                            Number = reader.GetInt32(reader.GetOrdinal("RoomNumber")),
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
            var query = "SELECT RoomId, RoomNumber, Description, Capacity, Price FROM Rooms";

            using (var connection = new MySqlConnection(_connectionString))
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
                            Number = reader.GetInt32(reader.GetOrdinal("RoomNumber")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                            Price = reader.GetFloat(reader.GetOrdinal("Price")),
                        };
                        rooms.Add(room);
                    }
                }
            }

            return rooms;
        }

        public async Task<IEnumerable<Room>> GetReservedRoomsAsync(DateTime startDate, DateTime endDate)
        {
            var reservedRooms = new List<Room>();
            var query = @"
            SELECT r.RoomId, r.RoomNumber, r.Capacity 
            FROM Rooms r
            INNER JOIN Reservations res ON r.RoomId = res.RoomId
            WHERE res.StartDate < @EndDate AND res.EndDate > @StartDate";

            using (var connection = new MySqlConnection(_connectionString))
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
                            Number = reader.GetInt32(reader.GetOrdinal("RoomNumber")),
                            Capacity = reader.GetInt32(reader.GetOrdinal("Capacity"))
                        };
                        reservedRooms.Add(room);
                    }
                }
            }

            return reservedRooms;
        }

        public async Task AddRoomAsync(Room room)
        {
            var query = @"
        INSERT INTO Rooms (RoomNumber, Description, Capacity, Price) 
        VALUES (@RoomNumber, @Description, @Capacity, @Price)";
            
            Console.WriteLine("AddRoomAsync function called.");

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomNumber", room.Number);
                command.Parameters.AddWithValue("@Description", room.Description);
                command.Parameters.AddWithValue("@Capacity", room.Capacity);
                command.Parameters.AddWithValue("@Price", room.Price); // Dodajemy parametr dla pola 'Price'
                // command.Parameters.AddWithValue("@IsAvailable", room.IsAvailable);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }


        public async Task UpdateRoomAsync(Room room)
        {
            var query = "UPDATE Rooms SET RoomNumber = @RoomNumber, Capacity = @Capacity WHERE RoomId = @RoomId";

            using (var connection = new MySqlConnection(_connectionString))
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

            using (var connection = new MySqlConnection(_connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomId", roomId);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}