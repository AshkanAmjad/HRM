using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class RegisterViewModel
    {
        [Key]
        public int userId { get; set; } /*شناسه*/

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام کاربری")]
        [RegularExpression("[0-9]{10}", ErrorMessage = "لطفا {0} را به درستی وارد کنید")]
        public string nationalCode { get; set; } /*نام کاربری */

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "رمز عبور")]
        public string userPass { get; set; } /*رمز عبور کاربر*/

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تکرار رمز عبور")]
        [Compare("userPass", ErrorMessage = "عدم تطابق با رمز عبور")]
        public string ReuserPass { get; set; } /*رمز عبور کاربر*/

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام ")]
        [RegularExpression(@"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$", ErrorMessage = "لطفا {0} را به درستی وارد کنید")]
        public string fName { get; set; } /* نام پرسنل*/

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام خانوادگی ")]
        [RegularExpression(@"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$", ErrorMessage = "لطفا {0} را به درستی وارد کنید")]
        public string lName { get; set; } /* نام خانوادگی پرسنل*/

        [Display(Name = "تصویر پرسنل ")]
        [Required(ErrorMessage = "لطفا تصویر مناسبی را انتخاب کنید")]
        public IFormFile avatar { get; set; } /* تصویر پرسنل*/

        public int department { get; set; }//شعبه
        public string title { get; set; }//عنوان تص.یر پرسنل ارسالی
        public string fileFormat { get; set; } //فرمت فایل
        public byte[] dataBytes { get; set; } //فایل
        public string description { get; set; } //توضیحات
        public string fileName { get; set; } // نام فایل
        public DateTime uploadDate { get; set; }//تاریخ بارگذاری

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [RegularExpression("[0-9]{4}/(0[1-9]|1[012])/(0[1-9]|1[0-9]|2[0-9]|3[01])", ErrorMessage = "ورودی باید به فرمت سال(4 رقم) / ماه(2 رقم) / روز(2 رقم) باشد")]
        [Display(Name = "تاریخ تولد ")]
        public string dateOfBirth { get; set; } /*تاریخ تولد*/

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تحیلات")]
        [RegularExpression(@"^(زیر دیپلم|دیپلم|فوق دیپلم|کارشناسی|کارشناسی ارشد|دکترا)$", ErrorMessage = "فقط مدارک (زیر دیپلم - دیپلم - فوق دیپلم - کارشناسی - کارشناسی ارشد - دکترا) مورد قبول است")]

        public string education { get; set; } /* تحصیلات*/

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "شماره تماس ")]
        public long tel { get; set; } /* شماره تماس پرسنل*/

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نشانی محل سکونت")]
        [RegularExpression(@"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$", ErrorMessage = "لطفا {0} را به درستی وارد کنید")]
        public string address { get; set; } /* نشانی محل سکونت پرسنل*/

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "سابقه کارکرد")]
        public string workingStatus { get; set; } /* وضعیت کارکرد پرسنل*/


    }
}
