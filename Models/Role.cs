using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class Role
    {
        [Key]
        [Display(Name ="نقش پرسنل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int RoleId { get; set; } //شناسه نقش

        [Required]
        public string RoleTitle { get; set; } // عنوان نقش
        public bool IsDelete { get; set; } // فعال بودن نقش
    }
}
