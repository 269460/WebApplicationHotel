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
        private readonly string _masterConnectionString;
        private readonly string _slaveConnectionString;

        public RoomRepository(IConfiguration configuration)
        {
            // _masterConnectionString = configuration.GetConnectionString("MasterConnection");
            // _slaveConnectionString = configuration.GetConnectionString("SlaveConnection");
            _masterConnectionString = configuration.GetConnectionString("DefaultConnection");
            _slaveConnectionString = configuration.GetConnectionString("DefaultConnection");
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
    SELECT r.RoomId, r.RoomNumber, r.Price, r.Description, r.Capacity 
    FROM Rooms r
    INNER JOIN Bookings b ON r.RoomId = b.RoomId
    WHERE b.CheckInDate < @EndDate AND b.CheckOutDate > @StartDate";

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
                            Number = reader.GetInt32(reader.GetOrdinal("RoomNumber")),  // Zmieniono na RoomNumber
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Capacity = reader.GetInt32(reader.GetOrdinal("Capacity")),
                            Price = reader.GetFloat(reader.GetOrdinal("Price")) // Zmieniono typ na GetDecimal
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

            using (var connection = new MySqlConnection(_masterConnectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoomNumber", room.Number);
                command.Parameters.AddWithValue("@Description", room.Description);
                command.Parameters.AddWithValue("@Capacity", room.Capacity);
                command.Parameters.AddWithValue("@Price", room.Price); // Dodajemy parametr dla pola 'Price'

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateRoomAsync(Room room)
        {
            var query = @"
                UPDATE Rooms 
                SET Number = @Number, Description = @Description, Capacity = @Capacity, Price = @Price, IsAvailable = @IsAvailable
                WHERE RoomId = @RoomId";

            using (var connection = new MySqlConnection(_masterConnectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Number", room.Number);  // Updated here
                command.Parameters.AddWithValue("@Description", room.Description);
                command.Parameters.AddWithValue("@Capacity", room.Capacity);
                command.Parameters.AddWithValue("@Price", room.Price);
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
    }
}
