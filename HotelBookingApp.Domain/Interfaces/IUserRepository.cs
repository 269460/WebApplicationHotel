// #Interfejs IUserRepository będzie odpowiedzialny za zarządzanie danymi użytkowników. Tutaj również możemy zdefiniować metody CRUD oraz inne metody specyficzne dla zarządzania użytkownikami, jak np. wyszukiwanie po nazwie użytkownika:

namespace HotelBookingApp.Domain.Interfaces;
using HotelBookingApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int userId);
}