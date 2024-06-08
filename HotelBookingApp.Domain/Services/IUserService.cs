namespace HotelBookingApp.Domain.Services;
using HotelBookingApp.Domain.Models;
using HotelBookingApp.Domain.Interfaces;  // Dla IUserRepository, IRoomRepository, IBookingRepository

public interface IUserService
{
    Task<User> AuthenticateUserAsync(string username, string password);
    Task<User> RegisterUserAsync(User user);
    Task UpdateUserAsync(User user);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> AuthenticateUserAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);
        if (user != null && VerifyPasswordHash(password, user.PasswordHash))
        {
            return user;
        }
        return null;
    }

    public async Task<User> RegisterUserAsync(User user)
    {
        await _userRepository.AddUserAsync(user);
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateUserAsync(user);
    }

    private bool VerifyPasswordHash(string password, string storedHash)
    {
        // Implementacja weryfikacji hasła
        return true;
    }
}
