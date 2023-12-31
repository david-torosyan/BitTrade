﻿using BitTrade.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BitTrade.Common.Models
{
    public class RegisterModel
    {

        //[Display(Name = "ID")]
        //public int ID { get; set; }

        [Required(ErrorMessage = "Please enter FirstName")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter LastName")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail id is not valid")]
        public string Email { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Please Select the gender")]
        public BaseGenderEnum Gender { get; set; } = BaseGenderEnum.Unknown;

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must have at least {2} characters", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "Password must contain: Minimum 8 characters, at least 1 UpperCase Alphabet, and 1 LowerCase Alphabet")]
        public string Password { get; set; }


        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Please enter confirm password")]
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        [DataType(DataType.Password)]
        public string Confirmpwd { get; set; }

        public BaseRoleEnum Role { get; set; } = BaseRoleEnum.user;
        public bool IsActive { get; set; } = true;
        public string ImageURL { get; set; }
        public string EmailErrorMessage { get; set; }
    }
}
