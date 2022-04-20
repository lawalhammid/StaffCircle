using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    public class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
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
        //[Required]
        public byte[] PasswordSalt { get; set; }
        //[Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public string Status { get; set; } = "Active";
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [Required]
        public virtual Country Country { get; set; }
        [ForeignKey("Country")]
        public int UserCountryId { get; set; } = 1;
    }
}