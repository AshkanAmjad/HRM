using ERP.Data;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP.Areas.ProvinceArea.Controllers
{
    [Area("ProvinceArea")]
    [Authorize(AuthenticationSchemes = "ProvinceArea")]

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
            if (userRole == "مدیریت" || userRole == "معاونت فناوری اطلاعات")
            {
                var documents = _context.transferDocuments.Where(d => (d.userReceiverArea == userArea && (d.userReceiverId == userId && d.roleReceiver == userRole && d.IsAllowed))
                || (d.userUploaderArea == userArea && d.roleReceiver == userRole && d.userReceiverId == "-")
                || (d.userReceiverArea == userArea && d.roleReceiver== userRole && d.userReceiverId == "-" && d.IsAllowed)
                || (d.userReceiverArea == userArea && d.roleReceiver == "-" && d.userReceiverId == "-")
                || (d.userReceiverArea == userArea && d.userUploaderArea == "بخش" && d.roleReceiver == userRole && d.userReceiverId == "-" & d.IsAllowed))
                 .AsEnumerable().Reverse().Take(3).ToList();
                return View(documents);
            }
            else
            {
                var documents = _context.transferDocuments.Where(d => (d.userReceiverArea == userArea && (d.userReceiverId == userId && d.roleReceiver == userRole && d.IsAllowed))
                || (d.userUploaderArea == userArea && d.roleReceiver == userRole && d.userReceiverId == "-")
                || (d.userReceiverArea == userArea && d.roleReceiver == userRole)
                || (d.userReceiverArea == userArea && d.roleReceiver == "-" && d.userReceiverId == "-" && d.userUploaderArea == userArea))
                 .AsEnumerable().Reverse().Take(3).ToList();
                return View(documents);
            }
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

