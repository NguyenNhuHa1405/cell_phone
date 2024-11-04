using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CellPhone.Areas.User.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Phải có {0}")]
        [EmailAddress(ErrorMessage = "Phải là {0}")]
        public string Email { get; set; }
    }
}