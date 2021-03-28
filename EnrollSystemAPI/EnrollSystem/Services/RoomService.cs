using AutoMapper;
using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using EnrollSystem.Models.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class RoomService : IRoomRepository
    {
        private EnrollContext _context;
        private IMapper _mapper;
        public RoomService(EnrollContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            if (_context.Rooms.Any(x => x.Name == roomParams.Name))
                throw new AppException("Room name \"" + roomParams.Name + "\" is already exist");
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

        public IEnumerable<Room> GetRoomsNotInUse(ScheduleModel schedule)
        {
            //get rooms
            var rooms = _context.Rooms.ToList();
            /*check room has conflict*/
            string[] dayOfWeek = schedule.Schedule.Split(",");
            //get list section overlap day and time 
            var dbSection = _context.Sections.Where(e => schedule.StartDay.Date <= e.EndDay.Date && e.StartDay.Date <= schedule.EndDay.Date)
                //.Where(e => CheckSchedule.ContainsAny(e.Schedule, dayOfWeek))
                .Where(e => schedule.StartTime <= e.EndTime && e.StartTime <= schedule.EndTime);
            //over schedule
            List<Section> newSection = new List<Section>();
            foreach(var day in dayOfWeek)
            {
                var overlapSectionList = dbSection.Where(e => e.Schedule.Contains(day)).ToList();
                newSection.AddRange(overlapSectionList);
            }
            var _roomsInSection = _mapper.Map<IList<Room>>(newSection);
            //remove duplicate
            _roomsInSection = _roomsInSection.GroupBy(e => e.Id).Select(x => x.First()).ToList();
            foreach(var overlapRoom in _roomsInSection)
            {
                rooms.RemoveAll(e => e.Id == overlapRoom.Id);
            }
            return rooms;
        }
    }
}
