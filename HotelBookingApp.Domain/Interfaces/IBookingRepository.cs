//Interfejs IBookingRepository będzie odpowiedzialny za zarządzanie danymi dotyczącymi rezerwacji. Oto przykładowe metody, które mogą być zdefiniowane w tym interfejsie:
using HotelBookingApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task DeleteBookingAsync(int bookingId);
    }
}