using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class LoginViewModel
    {
        [Key]
        public int userId { get; set; } /*شناسه*/
        [Display(Name ="نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string nationalCode { get; set; }
        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string userPass { get; set; }
        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
