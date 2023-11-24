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

namespace ERP.Areas.CountyArea.Controllers
{
    [Area("CountyArea")]
    [Authorize(AuthenticationSchemes = "CountyArea")]
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
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value != "مدیریت") && (User.Claims.FirstOrDefault(c => c.Type == "role").Value != "معاونت فناوری اطلاعات")))
            {
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewData["Departments"] = _ManagementService.GetDepartments();
                ViewBag.ErrorFalseTitleValue = ErrorFalseTitleValue;
                ViewBag.UploadDocument = UploadDocument;
                ViewBag.ErrorUploadDocument = ErrorUploadDocument;
                ViewBag.ErrorFalseReceiver = ErrorFalseReceiver;
                ViewBag.ErrorFalseReceiverId = ErrorFalseReceiverId;
                ViewBag.ErrorNotExistReceiverIdOnArea = ErrorNotExistReceiverIdOnArea;
                ViewBag.ErrorNotExistRole = ErrorNotExistRole;
                return View();
            }
            return View("NotAccessCounty");
        }
        //نمایش صفحه فرم های تبادل مدارک
        public IActionResult DisplayTransferDocumentsForms(bool UploadDocument = false, bool ErrorUploadDocument = false,
            bool ErrorFalseTitleValue = false, bool ErrorNotExistReceiverIdOnArea = false, bool ErrorNotExistRole = false,
            bool ErrorFalseReceiver = false, bool ErrorFalseReceiverId = false, bool ErrorFalseRoleReceiver = false, bool ErrorFalseReceiverDepartment = false, bool ErrorNotExistMember = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                ViewData["Departments"] = _ManagementService.GetDepartments();
                ViewData["Counties"] = _ManagementService.GetCounties();
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewBag.ErrorFalseTitleValue = ErrorFalseTitleValue;
                ViewBag.UploadDocument = UploadDocument;
                ViewBag.ErrorUploadDocument = ErrorUploadDocument;
                ViewBag.ErrorNotExistReceiverIdOnArea = ErrorNotExistReceiverIdOnArea;
                ViewBag.ErrorFalseReceiver = ErrorFalseReceiver;
                ViewBag.ErrorNotExistMember = ErrorNotExistMember;
                ViewBag.ErrorFalseRoleReceiver = ErrorFalseRoleReceiver;
                ViewBag.ErrorFalseReceiverDepartment = ErrorFalseReceiverDepartment;
                ViewBag.ErrorNotExistRole = ErrorNotExistRole;
                return View();
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region Excahabge
        //ارسال مدارک به استان
        [HttpPost]
        public IActionResult TransferDocumentsProvince(IFormFile document, UploadViewModel content, List<int> SelectedRoles)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value) == "مدیریت" || (User.Claims.FirstOrDefault(c => c.Type == "role").Value) == "معاونت فناوری اطلاعات"))
            {
                if ((content.userReceiverId == null && SelectedRoles.Count == 0) || (content.userReceiverId != null && SelectedRoles.Count != 0))
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                }
                if (content.userReceiverId != null && !_ManagementService.IsExistNationalCodeProvince(content.userReceiverId))
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistReceiverIdOnArea=true");
                }
                if (SelectedRoles.Count != 0)
                {
                    if (!_ManagementService.IsExistSpecifiedRoleProvince(SelectedRoles))
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                    }
                }
                if (content.userReceiverId != null && SelectedRoles.Count != 0)
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                }
                if (content.userReceiverId != null)
                {
                    if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverId=true");
                    }
                }
                if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseTitleValue=true");
                }

                content.IsAllowed = true;
                bool resultPc = _ManagementService.UploadProvincePC(document, content);
                bool resultDB = _ManagementService.UploadProvinceDB(document, content, SelectedRoles);
                if (resultPc == true && resultDB == true && (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?UploadDocument=true");
                }
                else
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorUploadDocument=true");
                }
            }
            return View("NotAccessCounty");
        }
        //ارسال مدارک به شهرستان
        [HttpPost]
        public IActionResult TransferDocumentsCounty(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> SelectedRoles)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان")
            {
                int countyDestination = 0;
                foreach (var item in CountyDestination)
                {
                    countyDestination = item;
                }
                if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    if (content.userReceiverId != null && !_ManagementService.IsExistNationalCodeWithDepartrmentCounty(content.userReceiverId, CountyDestination))
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistReceiverIdOnArea=true");
                    }
                    if (content.userReceiverId != null && SelectedRoles.Count != 0)
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        if (!_ManagementService.IsExistSpecifiedRoleCounty(SelectedRoles, CountyDestination))
                        {
                            return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                        }
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        if (!_ManagementService.IsExistSpecifiedRoleCounty(SelectedRoles, CountyDestination))
                        {
                            return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                        }
                    }
                    if (CountyDestination.Count != 0 && content.userReceiverId == null && SelectedRoles.Count == 0)
                    {
                        if (countyDestination.ToString() != User.Claims.FirstOrDefault(c => c.Type == "department").Value)
                        {
                            return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverDepartment=true");
                        }
                    }
                    if (CountyDestination.Count == 0)
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseData=true");
                    }
                    if (content.userReceiverId != null)
                    {
                        if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                        {
                            return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverId=true");
                        }
                    }
                    if (_ManagementService.GetManagernationalCodeCounty(countyDestination) == content.userUploaderId)
                    {
                        int selectedRoles = 0;
                        foreach (var item in SelectedRoles)
                        {
                            selectedRoles = item;
                        }
                        if (selectedRoles == 1 && countyDestination.ToString() == User.Claims.FirstOrDefault(c => c.Type == "department").Value)
                        {
                            return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseRoleReceiver=true");
                        }
                    }
                    if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseTitleValue=true");
                    }
                }
                else if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value) != "مدیریت" && (User.Claims.FirstOrDefault(c => c.Type == "role").Value) != "معاونت فناوری اطلاعات")
                {
                    if (content.userReceiverId != null && !_ManagementService.IsExistNationalCodeWithDepartrmentCounty(content.userReceiverId, CountyDestination))
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorNotExistReceiverIdOnArea=true");
                    }
                    if ((content.userReceiverId == null && SelectedRoles.Count == 0) || (content.userReceiverId != null && SelectedRoles.Count != 0))
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseReceiver=true");
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        if (!_ManagementService.IsExistSpecifiedRoleCounty(SelectedRoles, CountyDestination))
                        {
                            return Redirect("/CountyArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorNotExistRole=true");
                        }
                    }
                    if (content.userReceiverId != null)
                    {
                        if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                        {
                            return Redirect("/CountyArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseReceiverId=true");
                        }
                    }
                    if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorFalseTitleValue=true");
                    }
                }
                content.IsAllowed = true;
                bool resultPc = _ManagementService.UploadCountyPC(document, content); ;
                bool resultDB = _ManagementService.UploadCountyDB(document, content, CountyDestination, SelectedRoles);
                if (resultPc == true && resultDB == true && (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?UploadDocument=true");
                }
                else if (resultPc == false && resultDB == false && (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorUploadDocument=true");
                };

                if (resultPc == true && resultDB == true && (User.Claims.FirstOrDefault(c => c.Type == "role").Value != "مدیریت" && User.Claims.FirstOrDefault(c => c.Type == "role").Value != "معاونت فناوری اطلاعات"))
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?UploadDocument=true");
                }
                else
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayUserTransferDocumentsForm?ErrorUploadDocument=true");
                }

            }
            return View("NotAccessCounty");
        }
        //ارسال مدارک به بخش
        [HttpPost]
        public IActionResult TransferDocumentsDistrict(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> DistrictDestination, List<int> SelectedRoles)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                if (content.userReceiverId != null)
                {
                    if (!_ManagementService.IsExistNationalCodeWithCountyAndDepartrmentDistrict(content.userReceiverId, CountyDestination, DistrictDestination))
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistReceiverIdOnArea=true");
                    }
                    if (content.userReceiverId == User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value)
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiverId=true");
                    }
                }
                if (SelectedRoles.Count != 0)
                {
                    if (!_ManagementService.IsExistSpecifiedRoleDistrict(SelectedRoles, CountyDestination, DistrictDestination))
                    {
                        return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistRole=true");
                    }
                }
                if (content.userReceiverId != null && SelectedRoles.Count != 0)
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseReceiver=true");
                }
                if (CountyDestination.Count == 0 || DistrictDestination.Count == 0)
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseData=true");
                }
                if (!_ManagementService.IsExistMemberDistrict(CountyDestination, DistrictDestination))
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorNotExistMember=true");
                }
                if (!Regex.IsMatch(content.title, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorFalseTitleValue=true");
                }
                content.IsAllowed = true;
                bool resultPc = _ManagementService.UploadDistrictPC(document, content);
                bool resultDB = _ManagementService.UploadDistrictDB(document, content, CountyDestination, DistrictDestination, SelectedRoles);
                if (resultPc == true && resultDB == true)
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?UploadDocument=true");
                }
                else
                {
                    return Redirect("/CountyArea/ExchangeDocuments/DisplayTransferDocumentsForms?ErrorUploadDocument=true");
                }
            }
            return View("NotAccessCounty");
        }

        #endregion

        #region DisplayExcahabge
        //نمایش مبادلات کارمندان
        public IActionResult DisplayMyExcahange()
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان")
            {
                string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
                string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
                string userId = User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value;
                string userDepartment = User.Claims.FirstOrDefault(c => c.Type == "department").Value;
                var documents = _context.transferDocuments.Where(d => (d.userUploaderId == userId && (d.userUploaderArea == userArea || d.userReceiverArea == userArea))
               || (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userDepartment && d.userReceiverId == "-" && d.roleReceiver == "-")
               || (d.userUploaderArea == userArea && d.roleReceiver == userRole && d.CountyDestination.ToString() == userDepartment && d.userReceiverId == "-")
               || (d.userUploaderArea == userArea && d.CountyDestination.ToString() == userDepartment && d.CountyOrigin.ToString() == userDepartment && d.roleReceiver == userRole)
               || (d.userReceiverArea == userArea && d.CountyDestination.ToString() == userDepartment && (d.userReceiverId == userId || d.roleReceiver == userRole)))
               .AsEnumerable().Reverse().ToList();

                return View(documents);
            }
            return View("NotAccessCounty");
        }
        //نمایش مبادلات درون سطحی شهرستان - شهرستان
        public IActionResult DisplayExcahangeWithinLevel()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
                string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
                string userDepartment = User.Claims.FirstOrDefault(c => c.Type == "department").Value;
                var documents = _context.transferDocuments.Where(d => (d.userUploaderArea == userArea && d.userReceiverArea == userArea && (d.CountyDestination.ToString() == userDepartment || d.CountyOrigin.ToString() == userDepartment)))
                    .AsEnumerable().Reverse().ToList();
                return View(documents);
            }
            return View("NotAccessCounty");
        }
        //نمایش مبادلات فرا سطحی
        public IActionResult DisplayExcahangeSuperLevel()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                string userArea = User.Claims.FirstOrDefault(c => c.Type == "area").Value;
                string userRole = User.Claims.FirstOrDefault(c => c.Type == "role").Value;
                string userDepartment = User.Claims.FirstOrDefault(c => c.Type == "department").Value;
                var documents = _context.transferDocuments.Where(d => (d.userUploaderArea == userArea && d.userReceiverArea == "استان" && d.CountyDestination.ToString() == userDepartment)
                || (d.userUploaderArea == "استان" && d.userReceiverArea == userArea && d.CountyDestination.ToString() == userDepartment)
                || (d.userUploaderArea == userArea && d.userReceiverArea == "بخش" && d.CountyDestination.ToString() == userDepartment)
                || (d.userUploaderArea == "بخش" && d.userReceiverId == userArea && d.CountyDestination.ToString() == userDepartment)
                || (d.userUploaderArea == "بخش" && d.userReceiverArea == "استان" && d.CountyDestination.ToString() == userDepartment)
                || (d.userUploaderArea=="بخش" && d.userReceiverArea==userArea && d.CountyDestination.ToString()==userDepartment && d.CountyOrigin.ToString()==userDepartment))
                    .AsEnumerable().Reverse().ToList();
                return View(documents);
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region ShowDescription
        //نمایش توضیحات در صفحه تبادلات کارمندان
        public IActionResult ShowUserDescriptionWithinLevel(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان"))
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessCounty");
        }
        //نمایش توضیحات در صفحه تبادلات درون سطحی
        public IActionResult ShowDescriptionWithinLevel(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessCounty");
        }
        //نمایش توضیحات در صفحه تبادلات فرا سطحی
        public IActionResult ShowDescriptionSuperLevel(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessCounty");
        }
        //نمایش توضیحات مدرک حذف شده سطح شهرستان
        public IActionResult ShowDeletedDescriptionCounty(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getDeletedTransferDocumentWithId(id));
            }
            return View("NotAccessCounty");
        }
        //نمایش توضیحات مدرک حذف شده سطح بخش
        public IActionResult ShowDeletedDescriptionDistrict(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getDeletedTransferDocumentWithId(id));
            }
            return View("NotAccessCounty");
        }
        //نمایش توضیحات تبادل در صفحه اجازه دسترسی
        public IActionResult ShowPermisssionPublishDescriptionSuperLevel(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                return View(_ManagementService.getTransferDocumentWithId(id));
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region DownloadExchangeDocumentsFromDB
        //تابع دریافت مدارک تبادل شده
        public IActionResult DownloadDocumentFromDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value) == "شهرستان")
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
            return View("NotAccessCounty");
        }
        //تابع دریافت مدارک پاک شده تبادل شده 
        public IActionResult DownloadArchuveExchangeDocumentFromDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value) == "شهرستان")
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
            return View("NotAccessCounty");
        }
        #endregion

        #region Permission
        //نمایش مشخصات اجازه انتشار فایل ارسالی از بخش در استان
        public IActionResult DisplayPermissionPublishProvince(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                var document = _ManagementService.GetExchangeDocumentFromDB(id);
                return View(document);
            }
            return View("NotAccessCounty");
        }
        //اجازه انتشار فایل ارسالی از بخش در استان
        public IActionResult PermissionPublishProvince(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "area").Value == "شهرستان") && ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات")))
            {
                TransferDocumentsBetweenLevels document = _ManagementService.getTransferDocumentWithId(id);
                document.IsAllowed = true;
                _context.transferDocuments.Update(document);
                _context.SaveChanges();
                return RedirectToAction("DisplayExcahangeSuperLevel");
            }
            return View("NotAccessCounty");
        }
        #endregion
    }
}
