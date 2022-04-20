using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace api.DTOs
{
    public class CreateUsersDTO
    {
        [Required(ErrorMessage = "Please enter your full name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please enter email address")]
        [StringLength(200, MinimumLength = 9)]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(13, MinimumLength = 10)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}