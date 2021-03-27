using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Interfaces
{
    public interface ISectionRepository
    {
        Section Create(Section section);
        IEnumerable<Section> GetAll();
        Section GetById(int sectionId);
        void Update(int sectionId, Section sectionParams);
        void Delete(int sectionId);
    }
}
