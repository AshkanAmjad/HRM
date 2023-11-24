using ERP.Data;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.Areas.DistrictArea.Controllers
{
    [Authorize(AuthenticationSchemes = "DistrictArea")]
    [Area("DistrictArea")]
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
            string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
            string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
            string userId = User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value;
            string userCounty = User.Claims.FirstOrDefault(c => c.Type == "county").Value;
            string userDepartment = User.Claims.FirstOrDefault(c => c.Type == "department").Value;
            var documents = _context.transferDocuments.Where(d => (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString()==userDepartment && (d.userReceiverId == userId || d.roleReceiver == userRole))
            || (d.userUploaderArea == userArea && d.roleReceiver == userRole && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString() == userDepartment && d.userReceiverId == "-")
            || (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString() == userDepartment && d.roleReceiver == "-" && d.userReceiverId == "-")
            || (d.userUploaderArea == userArea && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString() == userDepartment && d.CountyOrigin.ToString() == userDepartment && d.roleReceiver == userRole)
             ).AsEnumerable().Reverse().Take(3).ToList();
            return View(documents);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

