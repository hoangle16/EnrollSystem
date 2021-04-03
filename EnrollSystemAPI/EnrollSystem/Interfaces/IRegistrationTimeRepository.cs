using EnrollSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Interfaces
{
    public interface IRegistrationTimeRepository
    {
        RegistrationTime GetRegistrationTime();
        void SetTime(DateTime start, DateTime end);
        void CloseRegistration();
    }
}
