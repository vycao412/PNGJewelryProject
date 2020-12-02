using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PNG.Models
{
    public class Account
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage="Email is not valid")]
        public string Email { get; set;}

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

        public int RoleID { get; set; }
        public int StatusID { get; set; }

        public Account()
        {

        }

        public Account(string email, string name, int roleId, int statusId)
        {
            UserName = name;
            Email = email;
            RoleID = roleId;
            StatusID = statusId;
        }
    }
}