using EnrollSystem.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EnrollSystem.Models.User
{
    public class UserRegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool Gender { get; set; } // 1 male, 0 female
        [Required]
        [MaxLength(16)]
        public string IdNumber { get; set; } //CMND
        [MaxLength(16)]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public string Role { get; set; }
    }
}
