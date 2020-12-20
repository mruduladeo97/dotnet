using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserAuthentication.Models
{
    public class UserData
    {
        [Key]
        [DataType(DataType.Text, ErrorMessage = "The username inserted is invalid")]
        [StringLength(10, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        [MinLength(3, ErrorMessage = "The {0} value cannot be less than {1} characters.")]
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please Enter Username")]
        public string UserName { set; get; }

        [DataType(DataType.Password, ErrorMessage = "The password inserted is invalid")]
        [StringLength(16, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        [MinLength(6, ErrorMessage = "The {0} value cannot be less than {1} characters.")]
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { set; get; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password", ErrorMessage = "Confirm Password does not match")]
        [Display(Name = "Confirm Password")]

        public string ConfirmPassword { get; set; }



        [DataType(DataType.Text, ErrorMessage = "The Full name inserted is invalid")]
        [StringLength(15, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        [MinLength(3, ErrorMessage = "The {0} value cannot be less than {1} characters.")]
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please Enter Full Name")]
        public string Full_Name { set; get; }

        [DataType(DataType.EmailAddress, ErrorMessage = "The Email inserted is invalid")]
        [StringLength(50, ErrorMessage = "The {0} value cannot exceed {1} characters. ")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Please Enter Email")]
        public string EmailId { set; get; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [MinLength(10, ErrorMessage = "The {0} value cannot be less than {1} characters.")]
        [RegularExpression(@"\d{10}", ErrorMessage = "Not a valid phone number")]
        public string Phone { set; get; }

        [Required(ErrorMessage = "Please Select City")]
        [Display(Name = "City")]
        public int CityID { set; get; }

        public IEnumerable<SelectListItem> CitiesList { set; get; }

        [Display(Name = "Remember Me")]
        public bool isActive { get; set; }

    }
}