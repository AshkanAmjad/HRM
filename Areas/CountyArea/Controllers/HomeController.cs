using ERP.Data;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.Areas.CountyArea.Controllers
{
    [Authorize(AuthenticationSchemes = "CountyArea")]
    [Area("CountyArea")]
    public class HomeController : Controller
    {

        #region DataBase
        //ارتباط با پایگاه داده
        private ERPContext _context;
        private IManagementService _ManagementService;

        public HomeController(ERPContext context, IManagementService managementService)
        {
            _context = context;
            _ManagementService = managementService;

        }

        #endregion

        #region Index
        //تابع انتفال به صفحه اصلی
        public IActionResult Index()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value;
            string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
            string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
            string userDepartment = User.Claims.FirstOrDefault(c => c.Type == "department").Value;
            if (userRole == "مدیریت" || userRole == "معاونت فناوری اطلاعات")
            {
                var documents = _context.transferDocuments.Where(d => (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userDepartment && (d.userReceiverId == userId || d.roleReceiver == userRole))
                || (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userDepartment && d.roleReceiver == "-" && d.userReceiverId == "-")
                || (d.userUploaderArea == userArea && d.roleReceiver == userRole && d.CountyDestination.ToString() == userDepartment && d.userReceiverId == "-")
                || (d.userUploaderArea == userArea && d.CountyDestination.ToString() == userDepartment && d.CountyOrigin.ToString() == userDepartment && d.roleReceiver == userRole)
                || (d.userUploaderArea == "بخش" && d.userReceiverArea == "استان" && d.CountyOrigin.ToString() == userDepartment)
                 ).AsEnumerable().Reverse().Take(3).ToList();
                return View(documents);
            }
            else
            {
                var documents = _context.transferDocuments.Where(d => (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userDepartment && (d.userReceiverId == userId || d.roleReceiver == userRole))
                || (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userDepartment && d.roleReceiver == "-" && d.userReceiverId == "-")
                || (d.userUploaderArea == userArea && d.roleReceiver == userRole && d.CountyDestination.ToString() == userDepartment && d.userReceiverId == "-")
                || (d.userReceiverArea == userArea && d.roleReceiver == userRole)
                || (d.userUploaderArea == userArea && d.CountyDestination.ToString() == userDepartment && d.CountyOrigin.ToString() == userDepartment && d.roleReceiver == userRole)
                ).AsEnumerable().Reverse().Take(3).ToList();
                return View(documents);
            }
        }
        #endregion

    }
}
