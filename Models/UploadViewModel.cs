using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class UploadViewModel
    {
        [Key]
        [Required]
        [Display(Name = "شناسه")]
        public int id { get; set; } //شناسه
        [Required]
        [Display(Name = "سطح بارگذاری کننده")]
        public string userUploaderArea { get; set; }
        [Required]
        [Display(Name = "نام کاربری بارگذاری کننده")]
        public string userUploaderId { get; set; } //نام کاربری بارگذاری کننده
        [Required]
        [Display(Name = "معاونت کارمند بارگذاری کننده")]
        public string roleUploader { get; set; }//معاونت کارمند بارگذاری کننده
        [Required]
        [Display(Name = "سطح دریافت کننده")]
        public string userReceiverArea { get; set; }
        [Display(Name = "نام کاربری کارمند دریافت کننده")]
        public string userReceiverId { get; set; } //نام کاربری کارمند دریافت کننده
        [Display(Name = "معاونت کارمند دریافت کننده")]
        public string roleReceiver { get; set; } // معاونت کارمند دریافت کننده
        [Required]
        [Display(Name = "شعبه استان مبدا")]
        public int ProvinceOrigin { get; set; }  //شعبه استان مبدا
        [Required]
        [Display(Name = "شعبه شهرستان مبدا")]
        public int CountyOrigin { get; set; }  //شعبه شهرستان مبدا
        [Required]
        [Display(Name = "شعبه بخش مبدا")]
        public int DistrictOrigin { get; set; } //بخش مبدا
        [Required]
        [Display(Name = "شعبه استان مقصد")]
        public int ProvinceDestination { get; set; } //شعبه استان مقصد
        [Required]
        [Display(Name = "شعبه شهرستان مقصد")]
        public int CountyDestination { get; set; }  //شعبه شهرستان مقصد
        [Required]
        [Display(Name = "شعبه بخش مقصد")]
        public int DistrictDestination { get; set; } //شعبه بخش مقصد
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنوان سند")]
        public string title { get; set; } //عنوان سند ارسالی
        [Display(Name = "نوع محتوا")]
        public string contentType { get; set; } //نوع محتوای سند ارسالی
        [Display(Name = "فرمت فایل")]
        public string fileFormat { get; set; } //فرمت فایل
        [Display(Name = "انتخاب سند")]
        [Required(ErrorMessage = "لطفا فایل مورد نظر را انتخاب کنید")]
        public IFormFile document { get; set; } //بارگذاری
        [Display(Name = "فایل")]
        public byte[] dataBytes { get; set; } //فایل
        [Display(Name = "توضیحات")]
        public string description { get; set; } //توضیحات
        [Display(Name = "نام فایل")]
        public string fileName { get; set; } // نام فایل
        public DateTime uploadDate { get; set; }//تاریخ بارگذاری
        public bool IsDelete { get; set; } //فعال بودن انتقال 
        public bool IsAllowed { get; set; } //فعال بودن انتقال به استا
    }
}
