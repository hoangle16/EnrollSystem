using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Interfaces
{
    public interface ICourseRepository
    {
        Course Create(Course course);
        IEnumerable<Course> GetAll();
        Course GetById(int courseId);
        void Update(int courseId, Course courseParams);
        void Delete(int courseId);
    }
}
