using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class Department
    {
        [Key]
        [Display(Name = "شعبه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int departmentId { get; set; } //شناسه شعبه

        [Required]
        public string departmentTitle { get; set; } // عنوان شعبه
    }
}
