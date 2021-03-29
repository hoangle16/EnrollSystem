using EnrollSystem.Entities;
using EnrollSystem.Models.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Interfaces
{
    public interface ITeacherRepository
    {
        IEnumerable<Teacher> GetAll();
        Teacher GetById(int teacherId);
        IEnumerable<Teacher> GetListTeacherFree(ScheduleModel schedule);
    }
}
