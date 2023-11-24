using ERP.Convertors;
using ERP.Data;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Areas.ProvinceArea.Controllers
{
    [Area("ProvinceArea")]
    [Authorize(AuthenticationSchemes = "ProvinceArea")]
    public class ChartController : Controller
    {
        #region IOC & DataBase 
        //اتصال به پایگاه داده
        private ERPContext _context;
        //اتصال به IOC
        private IManagementService _ManagementService;
        public ChartController(ERPContext context, IManagementService managementService)
        {
            _context = context;
            _ManagementService = managementService;
        }
        #endregion

        #region DisplayCharts
        //نمایش نمودارهای سطح استان
        public IActionResult DisplayChartsProvince()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                ////////////////////////////////////
                //محاسبه تعداد پرسنل مشغول در هر معاونت
                var role = _ManagementService.RoleCount("استان", 0, 1);
                ViewData["roleCount"] = role;
                ////////////////////////////////////
                //محاسبه تعداد کل کارمندان استان
                var countEmployees = _ManagementService.CountEmployeesPerDepartment("استان", 0, 1);
                ViewData["countEmployees"] = countEmployees[0];
                ViewData["countDeletedEmployess"] = countEmployees[1];
                ////////////////////////////////////
                //محاسبه تعداد کارمندان از نظر تفکیک جنسیت
                var gender = _ManagementService.GenderCount("استان", 0, 1);
                ViewData["genderCount"] = gender;
                ////////////////////////////////////
                //محاسبه تعداد کارمندان از نظر تفکیک وضعیت استخدامی
                var employment = _ManagementService.EmploymentCount("استان", 0, 1);
                ViewData["employmentCount"] = employment;
                ////////////////////////////////////
                // محاسبه تعداد کارمندان بر اساس نوع مدرک تحصیلی اخذ شده
                var education = _ManagementService.EducationDegreeCount("استان", 0, 1);
                ViewData["educationCount"] = education;
                ////////////////////////////////////
                //محاسبه بیشترین و کمترین سابقه در کارمندان 
                DateTime maxRegisterDate = _context.Employees.Where(u => u.area == "استان" && u.country == 0 && u.department == 1).Select(u => u.RegisterDate).ToList().Min();
                var maxRegisterDateName = _context.Employees.Where(u => u.area == "استان" && u.country == 0 && u.department == 1 && u.RegisterDate == maxRegisterDate).ToList();
                DateTime minRegisterDate = _context.Employees.Where(u => u.area == "استان" && u.country == 0 && u.department == 1).Select(u => u.RegisterDate).ToList().Max();
                var minRegisterDateName = _context.Employees.Where(u => u.area == "استان" && u.country == 0 && u.department == 1 && u.RegisterDate == minRegisterDate).ToList();
                ViewData["maxRegisterDateDays"] = maxRegisterDate.DailyWorkHistory();
                ViewData["maxRegisterDateName"] = maxRegisterDateName;
                ViewData["maxRegisterDateHours"] = maxRegisterDate.HourlyWorkHistor();
                ViewData["minRegisterDateDays"] = minRegisterDate.DailyWorkHistory();
                ViewData["minRegisterDateName"] = minRegisterDateName;
                ViewData["minRegisterDateHours"] = minRegisterDate.HourlyWorkHistor();
                ////////////////////////////////////
                ////////////////////////////////////
                return View();
            }
            return View("NotAccessProvince");
        }
        //نمایش نمودارهای سطح شهرستان
        public IActionResult DisplayChartsCounty(bool ErrorFalseData = false, bool ErrorNoMember = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                ViewBag.ErrorFalseData = ErrorFalseData;
                ViewBag.ErrorNoMember = ErrorNoMember;
                return View();
            }
            return View("NotAccessProvince");
        }
        [HttpPost]
        public IActionResult DisplayChartsCounty(List<int> filterCounty)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (filterCounty.Count == 0)
                {
                    return Redirect("/ProvinceArea/Chart/DisplayChartsCounty?ErrorFalseData=true");
                }
                int county = 0;
                int show = 0;
                for (int i = 0; i < filterCounty.Count; i++)
                {
                    if (filterCounty.Count != 0)
                    {
                        county = filterCounty[i];
                        show = 1;
                        break;
                    }
                }
                if (!_context.Employees.Any(u => u.area == "شهرستان" && u.country == 0 && u.department == county))
                {
                    return Redirect("/ProvinceArea/Chart/DisplayChartsCounty?ErrorNoMember=true");
                }
                ViewData["show"] = show;
                ViewData["county"] = county;
                ////////////////////////////////////
                //محاسبه تعداد پرسنل مشغول در هر معاونت
                var role = _ManagementService.RoleCount("شهرستان", 0, county);
                ViewData["roleCount"] = role;
                ////////////////////////////////////
                //محاسبه تعداد کل کارمندان استان
                var countEmployees = _ManagementService.CountEmployeesPerDepartment("شهرستان", 0, county);
                ViewData["countEmployees"] = countEmployees[0];
                ViewData["countDeletedEmployess"] = countEmployees[1];
                ////////////////////////////////////
                //محاسبه تعداد کارمندان از نظر تفکیک جنسیت
                var gender = _ManagementService.GenderCount("شهرستان", 0, county);
                ViewData["genderCount"] = gender;
                ////////////////////////////////////
                //محاسبه تعداد کارمندان از نظر تفکیک وضعیت استخدامی
                var employment = _ManagementService.EmploymentCount("شهرستان", 0, county);
                ViewData["employmentCount"] = employment;
                ////////////////////////////////////
                // محاسبه تعداد کارمندان بر اساس نوع مدرک تحصیلی اخذ شده
                var education = _ManagementService.EducationDegreeCount("شهرستان", 0, county);
                ViewData["educationCount"] = education;
                ////////////////////////////////////
                //محاسبه بیشترین و کمترین سابقه در کارمندان 
                if (_context.Employees.Any(u => u.area == "شهرستان" && u.country == 0 && u.department == county))
                {
                    DateTime maxRegisterDate = _context.Employees.Where(u => u.area == "شهرستان" && u.country == 0 && u.department == county).Select(u => u.RegisterDate).ToList().Min();
                    var maxRegisterDateName = _context.Employees.Where(u => u.area == "شهرستان" && u.country == 0 && u.department == county && u.RegisterDate == maxRegisterDate).ToList();
                    DateTime minRegisterDate = _context.Employees.Where(u => u.area == "شهرستان" && u.country == 0 && u.department == county).Select(u => u.RegisterDate).ToList().Max();
                    var minRegisterDateName = _context.Employees.Where(u => u.area == "شهرستان" && u.country == 0 && u.department == county && u.RegisterDate == minRegisterDate).ToList();
                    ViewData["maxRegisterDateDays"] = maxRegisterDate.DailyWorkHistory();
                    ViewData["maxRegisterDateName"] = maxRegisterDateName;
                    ViewData["maxRegisterDateHours"] = maxRegisterDate.HourlyWorkHistor();
                    ViewData["minRegisterDateDays"] = minRegisterDate.DailyWorkHistory();
                    ViewData["minRegisterDateName"] = minRegisterDateName;
                    ViewData["minRegisterDateHours"] = minRegisterDate.HourlyWorkHistor();
                }
                ////////////////////////////////////
                ////////////////////////////////////

                return View();
            }
            return View("NotAccessProvince");
        }
        //نمایش نمودارهای سطح بخش
        public IActionResult DisplayChartsDistrict(bool ErrorFalseData = false, bool ErrorNoMember = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                ViewBag.ErrorFalseData = ErrorFalseData;
                ViewBag.ErrorNoMember = ErrorNoMember;
                return View();
            }
            return View("NotAccessProvince");
        }
        [HttpPost]
        public IActionResult DisplayChartsDistrict(List<int> filterCounty, List<int> filterDistrict)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (filterCounty.Count == 0 && filterDistrict.Count == 0)
                {
                    return Redirect("/ProvinceArea/Chart/DisplayChartsDistrict?ErrorFalseData=true");
                }
                int county = 0;
                int district = 0;
                int show = 0;
                for (int i = 0; i < filterCounty.Count; i++)
                {
                    if (filterCounty.Count != 0 && filterDistrict.Count != 0)
                    {
                        county = filterCounty[i];
                        district = filterDistrict[i];
                        show = 1;
                        break;
                    }
                }
                if (!_context.Employees.Any(u => u.area == "بخش" & u.country == county && u.department == district))
                {
                    return Redirect("/ProvinceArea/Chart/DisplayChartsDistrict?ErrorNoMember=true");
                }
                ViewData["show"] = show;
                ViewData["county"] = county;
                ViewData["district"] = district;
                ////////////////////////////////////
                //محاسبه تعداد پرسنل مشغول در هر معاونت
                var role = _ManagementService.RoleCount("بخش", county, district);
                ViewData["roleCount"] = role;
                ////////////////////////////////////
                //محاسبه تعداد کل کارمندان استان
                var countEmployees = _ManagementService.CountEmployeesPerDepartment("بخش", county, district);
                ViewData["countEmployees"] = countEmployees[0];
                ViewData["countDeletedEmployess"] = countEmployees[1];
                ////////////////////////////////////
                //محاسبه تعداد کارمندان از نظر تفکیک جنسیت
                var gender = _ManagementService.GenderCount("بخش", county, district);
                ViewData["genderCount"] = gender;
                ////////////////////////////////////
                //محاسبه تعداد کارمندان از نظر تفکیک وضعیت استخدامی
                var employment = _ManagementService.EmploymentCount("بخش", county, district);
                ViewData["employmentCount"] = employment;
                ////////////////////////////////////
                // محاسبه تعداد کارمندان بر اساس نوع مدرک تحصیلی اخذ شده
                var education = _ManagementService.EducationDegreeCount("بخش", county, district);
                ViewData["educationCount"] = education;
                ////////////////////////////////////
                //محاسبه بیشترین و کمترین سابقه در کارمندان 
                if (_context.Employees.Any(u => u.area == "بخش" && u.country == county && u.department == district))
                {
                    DateTime maxRegisterDate = _context.Employees.Where(u => u.area == "بخش" && u.country == county && u.department == district).Select(u => u.RegisterDate).ToList().Min();
                    var maxRegisterDateName = _context.Employees.Where(u => u.area == "بخش" && u.country == county && u.department == district && u.RegisterDate == maxRegisterDate).ToList();
                    DateTime minRegisterDate = _context.Employees.Where(u => u.area == "بخش" && u.country == county && u.department == district).Select(u => u.RegisterDate).ToList().Max();
                    var minRegisterDateName = _context.Employees.Where(u => u.area == "بخش" && u.country == county && u.department == district && u.RegisterDate == minRegisterDate).ToList();
                    ViewData["maxRegisterDateDays"] = maxRegisterDate.DailyWorkHistory();
                    ViewData["maxRegisterDateName"] = maxRegisterDateName;
                    ViewData["maxRegisterDateHours"] = maxRegisterDate.HourlyWorkHistor();
                    ViewData["minRegisterDateDays"] = minRegisterDate.DailyWorkHistory();
                    ViewData["minRegisterDateName"] = minRegisterDateName;
                    ViewData["minRegisterDateHours"] = minRegisterDate.HourlyWorkHistor();
                }
                ////////////////////////////////////
                ////////////////////////////////////

                return View();
            }
            return View("NotAccessProvince");
        }
        #endregion
    }
}
