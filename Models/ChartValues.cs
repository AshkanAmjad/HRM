using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Models
{
    public class ChartValues
    {
        public string chartTitle { get; set; } //عنوان مقادیر نمودار 
        public int chartValue { get; set; } //مقدار نمودار
    }
}
