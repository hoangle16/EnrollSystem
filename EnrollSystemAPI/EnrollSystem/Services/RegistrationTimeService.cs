using EnrollSystem.Data;
using EnrollSystem.Entities;
using EnrollSystem.Helpers;
using EnrollSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Services
{
    public class RegistrationTimeService : IRegistrationTimeRepository
    {
        private EnrollContext _context;
        public RegistrationTimeService(EnrollContext context)
        {
            _context = context;
        }
        public void CloseRegistration()
        {
            var regis = _context.RegistrationTimes.FirstOrDefault();
            if (regis != null)
            {
                regis.EndDateTime = DateTime.Now;
                regis.StartDateTime = regis.EndDateTime;
            }
            _context.SaveChanges();
        }

        public void SetTime(DateTime start, DateTime end)
        {
            if (DateTime.Compare(start, end) >= 0)
            {
                throw new AppException("StartDateTime and EndDateTime error!!!");
            }
            var regis = _context.RegistrationTimes.FirstOrDefault();
            if (regis != null)
            {
                regis.StartDateTime = start;
                regis.EndDateTime = end;
                _context.RegistrationTimes.Update(regis);
            }
            else 
            {
                RegistrationTime _regis = new RegistrationTime();
                _regis.StartDateTime = start;
                _regis.EndDateTime = end;
                _context.RegistrationTimes.Add(_regis);
            }
            _context.SaveChanges();
        }
        public RegistrationTime GetRegistrationTime()
        {
            return _context.RegistrationTimes.FirstOrDefault();
        }
    }
}
