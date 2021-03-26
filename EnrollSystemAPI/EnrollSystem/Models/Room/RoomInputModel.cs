using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.Room
{
    public class RoomInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}
