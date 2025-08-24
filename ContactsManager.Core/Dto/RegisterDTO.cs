using ContactsManager.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.Dto
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "PersonName can't be empty")]

        public string PersonName { get; set; }

        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "Email value should be valid")]
        [Remote(action: "IsEmailAlreadyRegister", controller: "Account", ErrorMessage = "Email already in use")]
        public string Email { get; set; }


        [Required(ErrorMessage = "PhoneNumber can't be empty")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "PhoneNumber should contain only numbers")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Password can't be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword can't be empty")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and ConfirmPassword do not match")]
        public string ConfirmPassword { get; set; }
        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
