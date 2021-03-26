using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Interfaces
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetAll();
        Room GetById(int roomId);
        Room Create(Room room);
        void Update(int roomId, Room roomParams);
        void Delete(int roomId);
    }
}
