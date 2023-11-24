using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Controllers
{
    public class HomeController : Controller
    {
        //تابع تعیین سطح دسترسی
        public IActionResult Index()
        {
            return View();
        }
    }
}
