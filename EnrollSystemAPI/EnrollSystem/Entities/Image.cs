using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public virtual User User { get; set; }
        public virtual TrainingImage TrainingImage { get; set; }
        public virtual EnrollImage EnrollImage { get; set; }
    }
}
