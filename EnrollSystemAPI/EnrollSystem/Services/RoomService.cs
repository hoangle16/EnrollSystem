using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class RoomService : IRoomRepository
    {
        private EnrollContext _context;
        public RoomService(EnrollContext context)
        {
            _context = context;
        }
        public Room Create(Room room)
        {
            if (_context.Rooms.Any(x => x.Name == room.Name))
                throw new AppException("Room name \"" + room.Name + "\" is already exist");

            _context.Rooms.Add(room);
            _context.SaveChanges();
            return room;
        }

        public void Delete(int roomId)
        {
            var room = _context.Rooms.Find(roomId);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Room> GetAll()
        {
            return _context.Rooms;
        }

        public Room GetById(int roomId)
        {
            return _context.Rooms.Find(roomId);
        }

        public void Update(int roomId, Room roomParams)
        {
            var room = _context.Rooms.Find(roomId);
            if (room == null)
            {
                throw new AppException("Not Found");
            }
            if (!string.IsNullOrWhiteSpace(roomParams.Name))
                room.Name = roomParams.Name;
            _context.Rooms.Update(room);
            _context.SaveChanges();
        }
    }
}
