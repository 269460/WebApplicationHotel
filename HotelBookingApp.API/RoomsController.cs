using Microsoft.AspNetCore.Mvc;
using HotelBookingApp.Domain.Interfaces; 
using HotelBookingApp.Domain.Models;
using HotelBookingApp.Domain.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using HotelBookingApp.Infrastructure.Data;

namespace HotelBookingApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom([FromBody] Room room)
        {
            if (room == null)
            {
                return BadRequest("Room data is required.");
            }

            await _roomService.CreateRoomAsync(room);
            return CreatedAtAction(nameof(GetRoomById), new { id = room.RoomId }, room);

        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room room)
        {
            if (room == null || room.RoomId != id)
            {
                return BadRequest("Room data is invalid.");
            }

            var existingRoom = await _roomService.GetRoomByIdAsync(id);
            if (existingRoom == null)
            {
                return NotFound($"Room with Id = {id} not found.");
            }

            await _roomService.UpdateRoomAsync(room);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            await _roomService.DeleteRoomAsync(id);
            return NoContent(); // Standard response for a successful DELETE request
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }
        
        [HttpGet("AvailableRooms")]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate >= endDate)
            {
                return BadRequest("Data końca pobytu musi być późniejsza niż data początku pobytu.");
            }

            var availableRooms = await _roomService.GetAvailableRoomsAsync(startDate, endDate);
            return Ok(availableRooms);
        }
    }
    
}


    