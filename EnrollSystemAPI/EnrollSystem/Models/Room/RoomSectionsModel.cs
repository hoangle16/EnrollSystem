using EnrollSystem.Models.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Room
{
    public class RoomSectionsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<SectionModel> SectionList { get; set; }
    }
}
