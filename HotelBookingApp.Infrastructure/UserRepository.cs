using HotelBookingApp.Domain.Interfaces;
using HotelBookingApp.Domain.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly string _masterConnectionString;
        private readonly string _slaveConnectionString;

        public UserRepository(IConfiguration configuration)
        {
            _masterConnectionString = configuration.GetConnectionString("MasterConnection");
            _slaveConnectionString = configuration.GetConnectionString("SlaveConnection");
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            using (var connection = new MySqlConnection(_slaveConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM Users WHERE UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                // Dodaj inne pola w zależności od struktury tabeli
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>();

            using (var connection = new MySqlConnection(_slaveConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM Users", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                // Dodaj inne pola w zależności od struktury tabeli
                            });
                        }
                    }
                }
            }

            return users;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using (var connection = new MySqlConnection(_slaveConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("SELECT * FROM Users WHERE Username = @Username", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                // Dodaj inne pola w zależności od struktury tabeli
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task AddUserAsync(User user)
        {
            using (var connection = new MySqlConnection(_masterConnectionString))
            {
                await connection.OpenAsync();
                using (var command =
                       new MySqlCommand(
                           "INSERT INTO Users (Username, /* Inne kolumny */) VALUES (@Username, /* Inne wartości */)",
                           connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    // Dodaj inne parametry w zależności od struktury tabeli

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            using (var connection = new MySqlConnection(_masterConnectionString))
            {
                await connection.OpenAsync();
                using (var command =
                       new MySqlCommand(
                           "UPDATE Users SET Username = @Username, /* Inne kolumny */ WHERE UserId = @UserId",
                           connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    // Dodaj inne parametry w zależności od struktury tabeli

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            using (var connection = new MySqlConnection(_masterConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand("DELETE FROM Users WHERE UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
