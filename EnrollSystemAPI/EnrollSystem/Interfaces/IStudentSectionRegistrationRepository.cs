using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Interfaces
{
    public interface IStudentSectionRegistrationRepository
    {
        IEnumerable<StudentSectionRegistration> GetListBySectionId(int sectionId);
        IEnumerable<StudentSectionRegistration> GetListByStudentId(int studentId);
        StudentSectionRegistration Registration(int studentId, int sectionId);
        IEnumerable<Section> GetSectionForRegistration(int studentId);
        void Approval(int sectionId);
        void DeleteRegister(int studentId, int sectionId);
        //helper
        int GetUserIdByStudentId(int studentId);
    }
}
