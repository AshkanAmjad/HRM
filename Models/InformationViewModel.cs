using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class InformationViewModel
    {
        [Key]
        public int userId { get; set; } /*شناسه*/
        public string nationalCode { get; set; } /*نام کاربری */
        public string area { get; set; } /*سطح دسترسی*/
        public int county { get; set; } /*شعبه شهرستان*/
        public int department { get; set; } /* شعبه*/
        public string role { get; set; } /*نقش پرسنل*/
        public string fName { get; set; } /* نام پرسنل*/
        public string lName { get; set; } /* نام خانوادگی پرسنل*/
        public string employmentStatus { get; set; } /* وضعیت استخدامی پرسنل*/
        public string workingStatus { get; set; } /* وضعیت کارکرد پرسنل*/
        public DateTime RegisterDate { get; set; } /*تاریخ ثبت نام */

    }

    public class ProfileInformationViewModel
    {
        [Key]
        public int userId { get; set; } /*شناسه*/
        [Required]
        public string nationalCode { get; set; } /*نام کاربری */
        [Required]
        public string userPass { get; set; } /*رمزعبور کاربر*/
        [Display(Name = "تکرار رمز عبور")]
        [Compare("userPass", ErrorMessage = "عدم تطابق با رمز عبور")]
        public string ReuserPass { get; set; } /*رمز عبور کاربر*/
        public string fName { get; set; } /* نام پرسنل*/
        public string lName { get; set; } /* نام خانوادگی پرسنل*/
        public string education { get; set; } /*تحصیلات*/
        public int department { get; set; } /*شعبه*/
        public string role { get; set; }/*نقش سازمانی*/
        public int county { get; set; }/*شعبه شهرستان*/
        public string employmentStatus { get; set; }/*وضعیت استخدامی پرسنل*/
        public long tel { get; set; } /* شماره تماس پرسنل*/
        public string address { get; set; } /* آدرس محل سکونت پرسنل*/
        [Required]
        public string maritalStatus { get; set; } /* وضعیت تاهل پرسنل*/
    }
    
}
