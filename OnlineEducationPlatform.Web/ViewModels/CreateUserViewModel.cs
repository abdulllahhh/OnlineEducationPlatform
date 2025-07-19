﻿using System.ComponentModel.DataAnnotations;

namespace OnlineEducationPlatform.Web.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } // "Student" or "Instructor"
    }
}
