using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int AvatarId { get; set; }
        public virtual Image Avatar { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; } // 1 male, 0 female
        public string IdNumber { get; set; } //CMND
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Student Student { get; set; }
    }
}
