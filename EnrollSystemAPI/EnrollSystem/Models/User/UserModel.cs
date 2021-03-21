using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; } 
        public string IdNumber { get; set; } 
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public int AdminId { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
    }
}
