using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class County
    {

        [Key]
        [Display(Name = "شعبه شهرستان بالا سطح")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int countyId { get; set; } //شناسه شهرستان 

        [Required]
        public string countyTitle { get; set; } //عنوان شهرستان
    }
}
