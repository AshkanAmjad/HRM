using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class Gender
    {
        [Key]
        [Display(Name = "جنسیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int genderId { get; set; } //شناسه جنسیت

        [Required]
        public string genderTitle { get; set; } //عنوان جنسیت
    }
}
