using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class Employment
    {
        [Key]
        [Display(Name = "وضعیت استخدام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int EmploymentId { get; set; } //شناسه اسنخدام

        [Required]
        public string EmploymentTitle { get; set; } // عنوان استخدام
    }
}
