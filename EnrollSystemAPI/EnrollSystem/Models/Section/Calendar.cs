using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Section
{
    public class CalendarModel
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string RoomName { get; set; }
        public List<Event> Events { get; set; }

    }
    public class Event
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
