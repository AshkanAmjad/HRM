using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class DocumentUploadCountyLevel
    {
        public DocumentUploadCountyLevel()
        {

        }
        [Key]
        [Display(Name = "شناسه")]
        public int id { get; set; } // شناسه
        [Display(Name = "سطح بارگذارکننده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string area { get; set; } //سطح بارگذارکننده
        [Display(Name = "نقش بارگذارکننده")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string roleUploader { get; set; } //نقش بارگذارکننده
        [Display(Name = "شناسه کاربر")]
        public string ownerUserId { get; set; } //شناسه کاربر
        [Display(Name = "شعبه مقصد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int department { get; set; }//شعبه
        [Display(Name = "عنوان سند ارسالی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string title { get; set; }//عنوان مدرک ارسالی
        public string contentType { get; set; } //نوع محتوا
        public string fileFormat { get; set; } //فرمت فایل
        public byte[] dataBytes { get; set; } //فایل
        [Display(Name = "توضیحات")]
        public string description { get; set; } //توضیحات
        [Display(Name = "نام فایل")]
        public string fileName { get; set; } // نام فایل
        public DateTime uploadDate { get; set; }//تاریخ بارگذاری
        public bool IsDelete { get; set; } /*فعال بودن مدارک کاربر*/
        public int userId { get; set; }//شناسه کاربر
        [ForeignKey(nameof(userId))]
        public virtual CountyLevel countyLevel { get; set; }
    }
}
