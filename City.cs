using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UserAuthentication.Models
{
    public class City
    {
        [Key]
        public int CityID { set; get; }


        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter a city name")]
        [StringLength(15, ErrorMessage = "The {0} name cannot exceed {1} characters. ")]
        [Display(Name = "City Name")]
        public string City_Name { set; get; }
    }
}