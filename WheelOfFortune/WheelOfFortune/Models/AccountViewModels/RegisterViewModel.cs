using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace WheelOfFortune.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "FirstName")]
        public string Firstname { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string Lastname { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; }

        [Required]
        [Display(Name = "ActualPhoto")]
        //public string Photo { get; set; }
        //[FileExtensions(Extensions = "png")]
        public IFormFile ActualPhoto { get; set; }

        [Required]
        [Display(Name = "Birthdate")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        //[Range(typeof(DateTime),"01/01/1980","01/01/2017")]
        public DateTime Birthdate { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
