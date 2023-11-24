using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class Marital
    {
        [Key]
        [Display(Name = "وضعیت تاهل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int maritalId { get; set; } //شناسه وضعیت تاهل

        [Required]
        public string maritalTitle { get; set; } // عنوان وضعیت تاهل
    }
}
