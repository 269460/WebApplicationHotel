namespace HotelBookingApp.Domain.Services;

using HotelBookingApp.Domain.Models;
using HotelBookingApp.Domain.Interfaces;  // Dla IUserRepository, IRoomRepository, IBookingRepository
using System.Collections.Generic;
using System.Threading.Tasks;
public interface IBookingService
{
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<bool> CancelBookingAsync(int bookingId);
    Task<IEnumerable<Booking>> GetAllBookingsForUserAsync(int userId);
}

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        await _bookingRepository.AddBookingAsync(booking);
        return booking;
    }

    public async Task<bool> CancelBookingAsync(int bookingId)
    {
        await _bookingRepository.DeleteBookingAsync(bookingId);
        return true;
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsForUserAsync(int userId)
    {
        return await _bookingRepository.GetAllBookingsAsync();
    }
}
