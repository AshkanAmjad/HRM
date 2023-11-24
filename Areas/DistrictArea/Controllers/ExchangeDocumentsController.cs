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

namespace ERP.Areas.DistrictArea.Controllers
{
    [Area("DistrictArea")]
    [Authorize(AuthenticationSchemes = "DistrictArea")]
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
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value != "مدیریت") && (User.Claims.FirstOrDefault(c => c.Type == "role").Value != "معاونت فناوری اطلاعات")))
            {
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewData["Departments"] = _ManagementService.GetDepartments();
                ViewData["Counties"] = _ManagementService.GetCounties();
                ViewBag.ErrorFalseTitleValue = ErrorFalseTitleValue;
                ViewBag.UploadDocument = UploadDocument;
                ViewBag.ErrorUploadDocument = ErrorUploadDocument;
                ViewBag.ErrorFalseReceiver = ErrorFalseReceiver;
                ViewBag.ErrorFalseReceiverId = ErrorFalseReceiverId;
                ViewBag.ErrorNotExistReceiverIdOnArea = ErrorNotExistReceiverIdOnArea;
                ViewBag.ErrorNotExistRole = ErrorNotExistRole;
                return View();
            }
            return View("NotAccessDistrict");
        }
        //نمایش صفحه فرم های تبادل مدارک
        public IActionResult DisplayTransferDocumentsForms(bool UploadDocument = false, bool ErrorUploadDocument = false,
            bool ErrorFalseTitleValue = false, bool ErrorNotExistReceiverIdOnArea = false, bool ErrorNotExistRole = false,
            bool ErrorFalseReceiver = false, bool ErrorFalseReceiverId = false, bool ErrorFalseRoleReceiver = false, bool ErrorFalseReceiverDepartment = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
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
                ViewBag.ErrorFalseRoleReceiver = ErrorFalseRoleReceiver;
                ViewBag.ErrorFalseReceiverDepartment = ErrorFalseReceiverDepartment;
                ViewBag.ErrorNotExistRole = ErrorNotExistRole;
                return View();
            }
            return View("NotAccessDistrict");
        }
        #endregion

        #region Excahabge
        //ارسال مدارک به استان
        [HttpPost]
        public IActionResult TransferDocumentsProvince(IFormFile document, UploadViewModel content, List<int> SelectedRoles)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value) == "مدیریت" || (User.Claims.FirstOrDefault(c => c.Type == "role").Value) == "معاونت فناوری اطلاعات"))
            {
                if ((content.userReceiverId == null && SelectedRoles.Count == 0) || (content.userReceiverId != null && SelectedRoles.Count != 0))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                }
                if (content.userReceiverId != null && !_ManagementService.IsExistNationalCodeProvince(content.userReceiverId))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistReceiverIdOnArea=true");
                }
                if (SelectedRoles.Count != 0)
                {
                    if (!_ManagementService.IsExistSpecifiedRoleProvince(SelectedRoles))
                    {
                        return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                    }
                }
                if (content.userReceiverId != null && SelectedRoles.Count != 0)
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                }
                if (content.userReceiverId != null)
                {
                    if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                    {
                        return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverId=true");
                    }
                }
                if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseTitleValue=true");
                }

                content.IsAllowed = false;
                bool resultPc = _ManagementService.UploadProvincePC(document, content);
                bool resultDB = _ManagementService.UploadProvinceDB(document, content, SelectedRoles);
                if (resultPc == true && resultDB == true && (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?UploadDocument=true");
                }
                else
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorUploadDocument=true");
                }
            }
            return View("NotAccessDistrict");
        }
        //ارسال مدارک به شهرستان
        [HttpPost]
        public IActionResult TransferDocumentsCounty(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> SelectedRoles)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value) == "مدیریت" || (User.Claims.FirstOrDefault(c => c.Type == "role").Value) == "معاونت فناوری اطلاعات"))
            {
                int countyDestination = 0;
                foreach (var item in CountyDestination)
                {
                    countyDestination = item;
                }

                if ((content.userReceiverId == null && SelectedRoles.Count == 0) || (content.userReceiverId != null && SelectedRoles.Count != 0))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                }
                if (content.userReceiverId != null && !_ManagementService.IsExistNationalCodeWithDepartrmentCounty(content.userReceiverId, CountyDestination))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistReceiverIdOnArea=true");
                }
                if (SelectedRoles.Count != 0)
                {
                    if (!_ManagementService.IsExistSpecifiedRoleCounty(SelectedRoles, CountyDestination))
                    {
                        return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                    }
                }
                if (content.userReceiverId != null && SelectedRoles.Count != 0)
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                }
                if (CountyDestination.Count == 0)
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseData=true");
                }
                if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseTitleValue=true");
                }
                content.IsAllowed = true;
                bool resultPc = _ManagementService.UploadCountyPC(document, content); ;
                bool resultDB = _ManagementService.UploadCountyDB(document, content, CountyDestination, SelectedRoles);
                if (resultPc == true && resultDB == true && (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?UploadDocument=true");
                }
                else
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorUploadDocument=true");
                }
            }
            return View("NotAccessDistrict");
        }
        //ارسال مدارک به بخش
        [HttpPost]
        public IActionResult TransferDocumentsDistrict(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> DistrictDestination, List<int> SelectedRoles)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش")
            {
                int countyDestination = 0;
                int districtDestination = 0;
                foreach (var item in CountyDestination)
                {
                    countyDestination = item;
                }
                foreach (var item in DistrictDestination)
                {
                    districtDestination = item;
                }
                if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    if (content.userReceiverId != null)
                    {
                        if (!_ManagementService.IsExistNationalCodeWithCountyAndDepartrmentDistrict(content.userReceiverId, CountyDestination, DistrictDestination))
                        {
                            return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistReceiverIdOnArea=true");
                        }
                        if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                        {
                            return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverId=true");
                        }
                    }
                    if (content.userReceiverId == null && SelectedRoles.Count == 0 && CountyDestination.Count != 0 && DistrictDestination.Count != 0)
                    {
                        if (countyDestination.ToString() != User.Claims.FirstOrDefault(c => c.Type == "county").Value && districtDestination.ToString() != User.Claims.FirstOrDefault(c => c.Type == "department").Value)
                        {
                            return Redirect("DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverDepartment=true");
                        }
                    }
                    if (content.userReceiverId != null && SelectedRoles.Count != 0)
                    {
                        return Redirect("DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                    }
                    if (CountyDestination.Count == 0 || DistrictDestination.Count == 0)
                    {
                        return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseData=true");
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        if (!_ManagementService.IsExistSpecifiedRoleDistrict(SelectedRoles, CountyDestination, DistrictDestination))
                        {
                            return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                        }
                    }
                    if (_ManagementService.GetManagernationalCodeDistrict(countyDestination,districtDestination) == content.userUploaderId)
                    {
                        int selectedRoles = 0;
                        foreach (var item in SelectedRoles)
                        {
                            selectedRoles = item;
                        }
                        if (selectedRoles == 1 && countyDestination.ToString() == User.Claims.FirstOrDefault(c => c.Type == "county").Value &&
                            districtDestination.ToString()==User.Claims.FirstOrDefault(c=>c.Type=="department").Value)
                        {
                            return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseRoleReceiver=true");
                        }
                    }
                    if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                    {
                        return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseTitleValue=true");
                    }
                }
                else if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value) != "مدیریت" && (User.Claims.FirstOrDefault(c => c.Type == "role").Value) != "معاونت فناوری اطلاعات")
                {
                    if ((content.userReceiverId == null && SelectedRoles.Count == 0) || (content.userReceiverId != null && SelectedRoles.Count != 0))
                    {
                        return Redirect("/DistrictArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseReceiver=true");
                    }
                    if (content.userReceiverId != null)
                    {
                        if (!_ManagementService.IsExistNationalCodeWithCountyAndDepartrmentDistrict(content.userReceiverId, CountyDestination, DistrictDestination))
                        {
                            return Redirect("/DistrictArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorNotExistReceiverIdOnArea=true");
                        }
                        if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                        {
                            return Redirect("/DistrictArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseReceiverId=true");
                        }
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        if (!_ManagementService.IsExistSpecifiedRoleDistrict(SelectedRoles, CountyDestination, DistrictDestination))
                        {
                            return Redirect("/DistrictArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorNotExistRole=true");
                        }
                    }
                    if (content.userReceiverId != null && SelectedRoles.Count != 0)
                    {
                        return Redirect("DistrictArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseReceiver=true");
                    }
                    if (CountyDestination.Count == 0 || DistrictDestination.Count == 0)
                    {
                        return Redirect("/DistrictArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseData=true");
                    }
                    if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                    {
                        return Redirect("/DistrictArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseTitleValue=true");
                    }
                }
                content.IsAllowed = true;
                bool resultPc = _ManagementService.UploadDistrictPC(document, content);
                bool resultDB = _ManagementService.UploadDistrictDB(document, content, CountyDestination, DistrictDestination, SelectedRoles);
                if (resultPc == true && resultDB == true && (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?UploadDocument=true");
                }
                else if (resultPc == false && resultDB == false && (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorUploadDocument=true");
                };

                if (resultPc == true && resultDB == true && (User.Claims.FirstOrDefault(c => c.Type == "role").Value != "مدیریت" && User.Claims.FirstOrDefault(c => c.Type == "role").Value != "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?UploadDocument=true");
                }
                else
                {
                    return Redirect("/DistrictArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorUploadDocument=true");
                }
            }
            return View("NotAccessDistrict");
        }
        #endregion

        #region DisplayExcahabge
        //نمایش مبادلات کارمندان
        public IActionResult DisplayMyExcahange()
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش")
            {
                string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
                string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
                string userId = User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value;
                string userCounty = User.Claims.FirstOrDefault(c => c.Type == "county").Value;
                string userDepartment = User.Claims.FirstOrDefault(c => c.Type == "department").Value;
                var documents = _context.transferDocuments.Where(d => (d.userUploaderId == userId && (d.userUploaderArea == userArea || d.userReceiverArea == userArea))
                || (d.userUploaderArea == userArea && d.roleReceiver == userRole && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString() == userDepartment && d.userReceiverId == "-")
                || (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString() == userDepartment && d.userReceiverId == "-" && d.roleReceiver == "-")
                || (d.userUploaderArea == userArea && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString() == userDepartment && d.CountyOrigin.ToString() == userDepartment && d.roleReceiver == userRole)
                || (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString() == userDepartment && (d.userReceiverId == userId || d.roleReceiver == userRole)))
                    .AsEnumerable().Reverse().ToList();
                return View(documents);
            }
            return View("NotAccessDistrict");
        }
        //نمایش مبادلات درون سطحی بخش - بخش
        public IActionResult DisplayExcahangeWithinLevel()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
                string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
                string userCounty = User.Claims.FirstOrDefault(c => c.Type == "county").Value;
                string userDepartment = User.Claims.FirstOrDefault(c => c.Type == "department").Value;
                var documents = _context.transferDocuments.Where(d => (d.userUploaderArea == userArea && d.userReceiverArea == userArea && (d.CountyDestination.ToString() == userCounty || d.CountyOrigin.ToString() == userCounty)
                && (d.DistrictDestination.ToString() == userDepartment || d.DistrictOrigin.ToString() == userDepartment))).AsEnumerable().Reverse().ToList();
                return View(documents);
            }
            return View("NotAccessDistrict");
        }
        //نمایش مبادلات فرا سطحی
        public IActionResult DisplayExcahangeSuperLevel()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
                string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
                string userCounty = User.Claims.FirstOrDefault(c => c.Type == "county").Value;
                string userDepartment = User.Claims.FirstOrDefault(c => c.Type == "department").Value;
                var documents = _context.transferDocuments.Where(d => (d.userUploaderArea == userArea && d.userReceiverArea == "استان" && d.CountyOrigin.ToString() == userCounty && d.DistrictOrigin.ToString() == userDepartment)
                || (d.userUploaderArea == "استان" && d.userReceiverArea == userArea && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString()==userDepartment)
                || (d.userUploaderArea == userArea && d.userReceiverArea == "شهرستان" && d.CountyDestination.ToString() == userCounty)
                || (d.userUploaderArea == "شهرستان" && d.userReceiverArea == userArea && d.CountyDestination.ToString() == userCounty && d.DistrictDestination.ToString() == userDepartment)).AsEnumerable().Reverse().ToList();
                return View(documents);
            }
            return View("NotAccessDistrict");
        }
        #endregion

        #region ShowDescription
        //نمایش توضیحات در صفحه تبادلات کارمندان
        public IActionResult ShowUserDescriptionWithinLevel(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش"))
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessDistrict");
        }
        //نمایش توضیحات در صفحه تبادلات درون سطحی
        public IActionResult ShowDescriptionWithinLevel(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessDistrict");
        }
        //نمایش توضیحات در صفحه تبادلات فرا سطحی
        public IActionResult ShowDescriptionSuperLevel(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "بخش") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessDistrict");
        }
        #endregion

        #region DownloadExchangeDocumentsFromDB
        //تابع دریافت مدارک تبادل شده
        public IActionResult DownloadDocumentFromDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value) == "بخش")
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
            return View("NotAccessDistrict");
        }
        //تابع دریافت مدارک پاک شده تبادل شده 
        public IActionResult DownloadArchuveExchangeDocumentFromDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value) == "بخش")
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
            return View("NotAccessDistrict");
        }
        #endregion
    }
}
