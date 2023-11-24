using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class ForgotPasswordViewModel
    {
        [Key]
        [Display(Name ="نام کاربری")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(10, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string nationalCode { get; set; }
    }
    public class VerificationCode
    {
        [Required]
        public string nationalCode { get; set; }/*کد ملی کاربر*/

        [Required]
        public string randomNumber { get; set; }/*کد احراز هویت*/

        [Display(Name = "کد تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(5, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string verificationCode { get; set; }/*کد احراز هویت ارسالی*/
    }

    public class ResetPasswordByEmail
    {
        [Required]
        public string nationalCode { get; set; }/*کد ملی کاربر*/
        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
        public string email { get; set; } /*ایمیل کاربر*/
    }

    public class ResetPassword
    {
        [Required]
        public string nationalCode { get; set; }/*کد ملی کاربر*/

        [Display(Name = "رمز عبور جدید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string userPass { get; set; }/*رمز عبور کاربر*/

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تکرار رمز عبور")]
        [Compare("userPass", ErrorMessage = "عدم تطابق با رمز عبور جدید")]
        public string ReuserPass { get; set; } /* تکرار رمز عبور کاربر */
    }
}
