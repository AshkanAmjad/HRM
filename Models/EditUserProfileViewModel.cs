using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{

    public class EditProfileViewModel
    {
        [Key]
        public int userId { get; set; } /*شناسه*/
        [Required]
        public string nationalCode { get; set; } /*نام کاربری */
        public string userPass { get; set; } /*رمزعبور کاربر*/
        [Display(Name = "تکرار رمز عبور")]
        [Compare("userPass", ErrorMessage = "عدم تطابق با رمز عبور")]
        public string ReuserPass { get; set; } /*رمز عبور کاربر*/
        [Display(Name = "تصویر پرسنل ")]
        public IFormFile avatar { get; set; } /* تصویر پرسنل*/
        public string title { get; set; }//عنوان تص.یر پرسنل ارسالی
        public string fileFormat { get; set; } //فرمت فایل
        public byte[] dataBytes { get; set; } //فایل
        public string description { get; set; } //توضیحات
        public string fileName { get; set; } // نام فایل
        public DateTime uploadDate { get; set; }//تاریخ بارگذاری

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تسهیلات ")]
        [RegularExpression(@"^(زیر دیپلم|دیپلم|فوق دیپلم|کارشناسی|کارشناسی ارشد|دکترا)$", ErrorMessage = "فقط مدارک (زیر دیپلم - دیپلم - فوق دیپلم - کارشناسی - کارشناسی ارشد - دکترا) مورد قبول است")]

        public string education { get; set; } /*تحصیلات*/
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = " معاونت")]
        public long tel { get; set; } /* شماره تماس پرسنل*/
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نشانی محل سکونت")]
        [RegularExpression(@"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$", ErrorMessage = "لطفا {0} را به درستی وارد کنید")]
        public string address { get; set; } /* نشانی محل سکونت پرسنل*/
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "وضعیت تاهل")]
        public int maritalStatus { get; set; } /* وضعیت تاهل پرسنل*/
    }

}
