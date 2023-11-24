using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class Employees
    {
        public Employees()
        {

        }

        [Key]
        public int userId { get; set; } /*شناسه*/
        [Required]
        public string nationalCode { get; set; } /*نام کاربری */
        [Required]
        public string userPass { get; set; } /*رمزعبور کاربر*/
        public string area { get; set; } /*سطح دسترسی*/
        public int country { get; set; }/*شهرستان*/
        public int department { get; set; } /* شعبه*/
        public string role { get; set; } /*نقش پرسنل*/
        public string fName { get; set; } /* نام پرسنل*/
        public string lName { get; set; } /* نام خانوادگی پرسنل*/
        public string gender { get; set; } /* جنسیت پرسنل*/
        public string education { get; set; } /*تحصیلات*/
        public string dateOfBirth { get; set; } /*تاریخ تولد*/
        public long tel { get; set; } /* شماره تماس پرسنل*/
        public string address { get; set; } /* آدرس محل سکونت پرسنل*/
        public string maritalStatus { get; set; } /* وضعیت تاهل پرسنل*/
        public string employmentStatus { get; set; } /* وضعیت استخدامی پرسنل*/
        public string workingStatus { get; set; } /* وضعیت کارکرد پرسنل*/
        public bool IsDelete { get; set; } /*فعال بودن کاربر*/
        public DateTime RegisterDate { get; set; } /*تاریخ ثبت نام */
        public virtual ICollection<ProvinceLevel> ProvinceLevels { get; set; }
        public virtual ICollection<CountyLevel> CountyLevels { get; set; }

        public virtual ICollection <DistrictLevel> DistrictLevels { get; set; }
    }
}

