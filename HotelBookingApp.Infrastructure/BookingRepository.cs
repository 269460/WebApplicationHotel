using HotelBookingApp.Domain.Interfaces;
using HotelBookingApp.Domain.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace HotelBookingApp.Infrastructure.Data
{
    public class BookingRepository : IBookingRepository
    {
        private readonly string _masterConnectionString;
        private readonly string _slaveConnectionString;

        public BookingRepository(IConfiguration configuration)
        {
            _masterConnectionString = configuration.GetConnectionString("MasterConnection");
            _slaveConnectionString = configuration.GetConnectionString("SlaveConnection");
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            using (var connection = new MySqlConnection(_slaveConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM Reservations WHERE BookingId = @BookingId", connection))
                {
                    command.Parameters.AddWithValue("@BookingId", bookingId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Booking
                            {
                                BookingId = reader.GetInt32("BookingId"),
                                RoomId = reader.GetInt32("RoomId"),
                                UserId = reader.GetInt32("UserId"),
                                // Dodaj inne pola w zależności od struktury tabeli
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            var bookings = new List<Booking>();

            using (var connection = new MySqlConnection(_slaveConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM Reservations", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            bookings.Add(new Booking
                            {
                                BookingId = reader.GetInt32("BookingId"),
                                RoomId = reader.GetInt32("RoomId"),
                                UserId = reader.GetInt32("UserId"),
                                CheckInDate = reader.GetDateTime("CheckInDate"),
                                CheckOutDate = reader.GetDateTime("CheckOutDate"),
                            });
                        }
                    }
                }
            }

            return bookings;
        }


        public async Task AddBookingAsync(Booking booking)
        {
            using (var connection = new MySqlConnection(_masterConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(
                    "INSERT INTO Reservations (RoomId, UserId, CheckInDate, CheckOutDate) VALUES (@RoomId, @UserId, @CheckInDate, @CheckOutDate)", connection))
                {
                    command.Parameters.AddWithValue("@RoomId", booking.RoomId);
                    command.Parameters.AddWithValue("@UserId", booking.UserId);
                    command.Parameters.AddWithValue("@CheckInDate", booking.CheckInDate);
                    command.Parameters.AddWithValue("@CheckOutDate", booking.CheckOutDate);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            using (var connection = new MySqlConnection(_masterConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(
                    "UPDATE Reservations SET RoomId = @RoomId, UserId = @UserId WHERE BookingId = @BookingId", connection))
                {
                    command.Parameters.AddWithValue("@RoomId", booking.RoomId);
                    command.Parameters.AddWithValue("@UserId", booking.UserId);
                    command.Parameters.AddWithValue("@BookingId", booking.BookingId);
                    // Dodaj inne parametry w zależności od struktury tabeli

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            using (var connection = new MySqlConnection(_masterConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("DELETE FROM Reservations WHERE BookingId = @BookingId", connection))
                {
                    command.Parameters.AddWithValue("@BookingId", bookingId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
