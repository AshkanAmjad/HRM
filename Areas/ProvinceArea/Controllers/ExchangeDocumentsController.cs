using ERP.Data;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ERP.Areas.ProvinceArea.Controllers
{
    [Area("ProvinceArea")]
    [Authorize(AuthenticationSchemes = "ProvinceArea")]
    public class ExchangeDocumentsController : Controller
    {
        #region IOC & DataBase 
        //اتصال به پایگاه داده
        private ERPContext _context;
        //اتصال به IOC
        private IManagementService _ManagementService;
        public ExchangeDocumentsController(ERPContext context, IManagementService managementService)
        {
            _context = context;
            _ManagementService = managementService;
        }
        #endregion

        #region DisplayTransferDocumentsForms
        //نمایش صفحه فرم تبادل مدارک کارمندان
        public IActionResult DisplayUserTransferDocumentsForm(bool UploadDocument = false, bool ErrorUploadDocument = false,
            bool ErrorFalseTitleValue = false, bool ErrorNotExistReceiverIdOnArea = false,
            bool ErrorFalseReceiver = false, bool ErrorFalseReceiverId = false, bool ErrorNotExistRole = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value != "مدیریت") && (User.Claims.FirstOrDefault(c => c.Type == "role").Value != "معاونت فناوری اطلاعات")))
            {
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewBag.ErrorFalseTitleValue = ErrorFalseTitleValue;
                ViewBag.UploadDocument = UploadDocument;
                ViewBag.ErrorUploadDocument = ErrorUploadDocument;
                ViewBag.ErrorFalseReceiver = ErrorFalseReceiver;
                ViewBag.ErrorFalseReceiverId = ErrorFalseReceiverId;
                ViewBag.ErrorNotExistReceiverIdOnArea = ErrorNotExistReceiverIdOnArea;
                ViewBag.ErrorNotExistRole = ErrorNotExistRole;
                return View();
            }
            return View("NotAccessProvince");
        }
        //نمایش صفحه فرم های تبادل مدارک
        public IActionResult DisplayTransferDocumentsForms(bool UploadDocument = false, bool ErrorUploadDocument = false,
            bool ErrorFalseTitleValue = false, bool ErrorNotExistReceiverIdOnArea = false,
            bool ErrorFalseReceiver = false, bool ErrorFalseReceiverId = false, bool ErrorFalseRoleReceiver = false, bool ErrorNotExistRole = false, bool ErrorNotExistMember = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                ViewData["Departments"] = _ManagementService.GetDepartments();
                ViewData["Counties"] = _ManagementService.GetCounties();
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewBag.ErrorFalseTitleValue = ErrorFalseTitleValue;
                ViewBag.UploadDocument = UploadDocument;
                ViewBag.ErrorUploadDocument = ErrorUploadDocument;
                ViewBag.ErrorNotExistReceiverIdOnArea = ErrorNotExistReceiverIdOnArea;
                ViewBag.ErrorFalseReceiver = ErrorFalseReceiver;
                ViewBag.ErrorFalseReceiverId = ErrorFalseReceiverId;
                ViewBag.ErrorNotExistMember = ErrorNotExistMember;
                ViewBag.ErrorFalseRoleReceiver = ErrorFalseRoleReceiver;
                ViewBag.ErrorNotExistRole = ErrorNotExistRole;
                return View();
            }
            return View("NotAccessProvince");
        }
        #endregion

        #region Excahabge
        //ارسال مدارک به استان
        [HttpPost]
        public IActionResult TransferDocumentsProvince(IFormFile document, UploadViewModel content, List<int> SelectedRoles)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان"))
            {
                if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value) == "مدیریت" || (User.Claims.FirstOrDefault(c => c.Type == "role").Value) == "معاونت فناوری اطلاعات")
                {
                    int selectedRoles = 0;
                    foreach (var item in SelectedRoles)
                    {
                        selectedRoles = item;
                    }
                    if (content.userReceiverId != null && !_ManagementService.IsExistNationalCodeProvince(content.userReceiverId))
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistReceiverIdOnArea=true");
                    }
                    if (content.userReceiverId != null && SelectedRoles.Count != 0)
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        if (!_ManagementService.IsExistSpecifiedRoleProvince(SelectedRoles))
                        {
                            return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                        }
                    }
                    if (content.userReceiverId != null)
                    {
                        if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                        {
                            return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverId=true");
                        }
                    }
                    if (_ManagementService.GetManagernationalCodeProvince() == content.userUploaderId)
                    {
                        if (selectedRoles == 1)
                        {
                            return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseRoleReceiver=true");
                        }
                    }
                    if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseTitleValue=true");
                    }
                }
                else if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value) != "مدیریت" && (User.Claims.FirstOrDefault(c => c.Type == "role").Value) != "معاونت فناوری اطلاعات")
                {
                    if (content.userReceiverId != null && !_ManagementService.IsExistNationalCodeProvince(content.userReceiverId))
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorNotExistReceiverIdOnArea=true");
                    }
                    if ((content.userReceiverId == null && SelectedRoles.Count == 0) || (content.userReceiverId != null && SelectedRoles.Count != 0))
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseReceiver=true");
                    }
                    if (content.userReceiverId != null)
                    {
                        if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                        {
                            return Redirect("/ProvinceArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseReceiverId=true");
                        }
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        if (!_ManagementService.IsExistSpecifiedRoleProvince(SelectedRoles))
                        {
                            return Redirect("/ProvinceArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorNotExistRole=true");
                        }
                    }
                    if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseTitleValue=true");
                    }
                }
                content.IsAllowed = true;
                bool resultPc = _ManagementService.UploadProvincePC(document, content);
                bool resultDB = _ManagementService.UploadProvinceDB(document, content, SelectedRoles);
                if (resultPc == true && resultDB == true && (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?UploadDocument=true");
                }
                else if (resultPc == false && resultDB == false && (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorUploadDocument=true");
                };

                if (resultPc == true && resultDB == true && (User.Claims.FirstOrDefault(c => c.Type == "role").Value != "مدیریت" && User.Claims.FirstOrDefault(c => c.Type == "role").Value != "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?UploadDocument=true");
                }
                else
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorUploadDocument=true");
                }
            }
            return View("NotAccessProvince");
        }
        //ارسال مدارک به شهرستان
        [HttpPost]
        public IActionResult TransferDocumentsCounty(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> SelectedRoles)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                if (content.userReceiverId != null)
                {
                    if (!_ManagementService.IsExistNationalCodeWithDepartrmentCounty(content.userReceiverId, CountyDestination))
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistReceiverIdOnArea=true");
                    }
                    if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverId=true");
                    }
                }
                if (SelectedRoles.Count != 0)
                {
                    if (!_ManagementService.IsExistSpecifiedRoleCounty(SelectedRoles, CountyDestination))
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                    }
                }
                if (CountyDestination.Count == 0)
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseData=true");
                }
                if (!_ManagementService.IsExistMemberCounty(CountyDestination))
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistMember=true");
                }
                if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseTitleValue=true");
                }
                content.IsAllowed = true;
                bool resultPc = _ManagementService.UploadCountyPC(document, content); ;
                bool resultDB = _ManagementService.UploadCountyDB(document, content, CountyDestination, SelectedRoles);
                if (resultPc == true && resultDB == true)
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?UploadDocument=true");
                }
                else
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorUploadDocument=true");
                }
            }
            return View("NotAccessProvince");
        }
        //ارسال مدارک به بخش
        [HttpPost]
        public IActionResult TransferDocumentsDistrict(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> DistrictDestination, List<int> SelectedRoles)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                if (content.userReceiverId != null)
                {
                    if (!_ManagementService.IsExistNationalCodeWithCountyAndDepartrmentDistrict(content.userReceiverId, CountyDestination, DistrictDestination))
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistReceiverIdOnArea=true");
                    }
                    if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverId=true");
                    }
                }
                if (SelectedRoles.Count != 0)
                {
                    if (!_ManagementService.IsExistSpecifiedRoleDistrict(SelectedRoles, CountyDestination, DistrictDestination))
                    {
                        return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                    }
                }
                if (content.userReceiverId != null && SelectedRoles.Count != 0)
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                }
                if (CountyDestination.Count == 0 || DistrictDestination.Count == 0)
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseData=true");
                }
                if (!_ManagementService.IsExistMemberDistrict(CountyDestination,DistrictDestination))
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistMember=true");
                }
                if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseTitleValue=true");
                }
                content.IsAllowed = true;
                bool resultPc = _ManagementService.UploadDistrictPC(document, content);
                bool resultDB = _ManagementService.UploadDistrictDB(document, content, CountyDestination, DistrictDestination, SelectedRoles);
                if (resultPc == true && resultDB == true)
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?UploadDocument=true");
                }
                else
                {
                    return Redirect("/ProvinceArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorUploadDocument=true");
                }
            }
            return View("NotAccessProvince");
        }

        #endregion

        #region DisplayExcahabge
        //نمایش مبادلات کارمندان
        public IActionResult DisplayMyExcahange()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان"))
            {
                string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
                string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
                string userId = User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value;
                var documents = _context.transferDocuments.Where(d => (d.userUploaderId == userId && (d.userUploaderArea == userArea || d.userReceiverArea == userArea) && d.IsAllowed)
                || (d.userUploaderArea == userArea && d.roleReceiver == userRole && d.userReceiverId == "-" && d.IsAllowed)
                || (d.userReceiverArea == userArea && d.roleReceiver == userRole && d.userReceiverId == "-" && d.IsAllowed)
                || (d.userReceiverArea == userArea && d.userReceiverId == "-" && d.roleReceiver == "-")
                || (d.userReceiverArea == userArea && (d.userReceiverId == userId && d.roleReceiver == userRole) && d.IsAllowed)
                || (d.userReceiverArea == userArea && d.userUploaderArea == "بخش" && d.roleReceiver == userRole && d.userReceiverId == "-" & d.IsAllowed))
                .AsEnumerable().Reverse().ToList();
                return View(documents);
            }
            return View("NotAccessProvince");
        }
        //نمایش مبادلات درون سطحی استان - استان
        public IActionResult DisplayExcahangeWithinLevel()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
                string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
                var documents = _context.transferDocuments.Where(d => (d.userUploaderArea == userArea && d.userReceiverArea == userArea)).AsEnumerable().Reverse().ToList();
                return View(documents);
            }
            return View("NotAccessProvince");
        }
        //نمایش مبادلات فرا سطحی
        public IActionResult DisplayExcahangeSuperLevel()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;

                string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
                var documents = _context.transferDocuments.Where(d => (d.userUploaderArea == userArea && d.userReceiverArea == "شهرستان")
                || (d.userUploaderArea == "شهرستان" && d.userReceiverArea == userArea)
                || (d.userUploaderArea == userArea && d.userReceiverArea == "بخش")
                || (d.userUploaderArea == "بخش" && d.userReceiverArea == userArea && d.IsAllowed)).AsEnumerable().Reverse().ToList();
                return View(documents);
            }
            return View("NotAccessProvince");
        }
        #endregion

        #region ShowDescription
        //نمایش توضیحات در صفحه تبادلات کارمندان
        public IActionResult ShowUserDescriptionWithinLevel(int id)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان")
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessProvince");
        }
        //نمایش توضیحات در صفحه تبادلات درون سطحی
        public IActionResult ShowDescriptionWithinLevel(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessProvince");
        }
        //نمایش توضیحات در صفحه تبادلات فرا سطحی
        public IActionResult ShowDescriptionSuperLevel(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessProvince");
        }
        //نمایش توضیحات مدرک حذف شده سطح استان
        public IActionResult ShowDeletedDescriptionProvince(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getDeletedTransferDocumentWithId(id));
            }
            return View("NotAccessProvince");
        }
        //نمایش توضیحات مدرک حذف شده سطح شهرستان
        public IActionResult ShowDeletedDescriptionCounty(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getDeletedTransferDocumentWithId(id));
            }
            return View("NotAccessProvince");
        }
        //نمایش توضیحات مدرک حذف شده سطح بخش
        public IActionResult ShowDeletedDescriptionDistrict(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "استان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getDeletedTransferDocumentWithId(id));
            }
            return View("NotAccessProvince");
        }

        #endregion

        #region DownloadExchangeDocumentsFromDB
        //تابع دریافت مدارک تبادل شده
        public IActionResult DownloadDocumentFromDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value) == "استان")
            {
                if (_ManagementService.IsExistExchangeDocumentOnDB(id))
                {
                    var item = _ManagementService.GetExchangeDocumentFromDB(id);
                    byte[] bytes = item.dataBytes;
                    string contenetType = item.contentType;
                    string fileName = item.fileName;
                    return File(bytes, contenetType, fileName);
                }
                return Content("Not Found");
            }
            return View("NotAccessProvince");
        }
        //تابع دریافت مدارک پاک شده تبادل شده 
        public IActionResult DownloadArchuveExchangeDocumentFromDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value) == "استان")
            {
                if (_ManagementService.IsExistDeletedExchangeDocumentOnDB(id))
                {
                    var item = _ManagementService.GetDeletedExchangeDocumentFromDB(id);
                    byte[] bytes = item.dataBytes;
                    string contenetType = item.contentType;
                    string fileName = item.fileName;
                    return File(bytes, contenetType, fileName);
                }
                return Content("Not Found");
            }
            return View("NotAccessProvince");
        }
        #endregion
    }


}
