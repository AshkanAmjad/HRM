using ERP.Data;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ERP.Areas.DistrictArea.Controllers
{
    [Area("DistrictArea")]
    [Authorize(AuthenticationSchemes = "DistrictArea")]
    public class ManagementController : Controller
    {
        #region IOC&DataBase 
        //اتصال به پایگاه داده
        private ERPContext _context;
        //اتصال به IOC
        private IManagementService _ManagementService;
        public ManagementController(ERPContext context, IManagementService managementService)
        {
            _context = context;
            _ManagementService = managementService;
        }
        #endregion

        #region DispalyDb
        //نمایش پایگاه داده کارکنان در سطح بخش
        public IActionResult DisplayDistrictdb(bool EditProfile = false, bool DeleteUser = false, bool ErrorDeleteUser = false, bool ErrorEditProfile = false, bool ErrorIsExistArchive = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userDepartment = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                string userCounty = User.Claims.FirstOrDefault(u => u.Type == "county").Value;
                var employees = _context.DistrictLevels.Where(u=>u.county.ToString()==userCounty && u.department.ToString()==userDepartment).AsEnumerable().Reverse().ToList();
                ViewBag.EditProfile = EditProfile;
                ViewBag.DeleteUser = DeleteUser;
                ViewBag.ErrorDeleteUser = ErrorDeleteUser;
                ViewBag.ErrorEditProfile = ErrorEditProfile;
                ViewBag.ErrorIsExistArchive = ErrorIsExistArchive;
                return View(employees);
            }
            return View("NotAccessDistrict");
        }
        //نمایش پایگاه داده مدارک در سطح بخش
        public IActionResult DisplayDocumentsDistrictdb()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userDepartment = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                string userCounty = User.Claims.FirstOrDefault(u => u.Type == "county").Value;
                var employees = _context.documentUploadDistrictLevels.Where(u=>u.county.ToString()==userCounty && u.department.ToString()==userDepartment).AsEnumerable().Reverse().ToList();
                return View(employees);
            }
            return View("NotAccessDistrict");
        }
        #endregion

        #region MyDocument
        //مدارک من
        public IActionResult MyDocuments()
        {
            string myNationalCode = User.Claims.FirstOrDefault(u => u.Type == "nationalCode").Value.ToString();
            var myDocuments = _context.documentUploadDistrictLevels.Where(d => d.ownerUserId == myNationalCode).ToList();
            return View(myDocuments);
        }

        //دریافت مدارک به وسیله شناسه
        public IActionResult DownloadMyDocuments(int id)
        {
            if (_ManagementService.IsExistUserDocumentOnDistrictDB(id))
            {
                var item = _ManagementService.GetDocumentFromDistrictDB(id);
                byte[] bytes = item.dataBytes;
                string contenetType = item.contentType;
                string fileName = item.fileName;
                return File(bytes, contenetType, fileName);
            }
            return Content("Not Found");
        }
        #endregion

        #region Profile
        //حساب کاربری
        public IActionResult Profile(bool EditProfile = false)
        {
            ViewBag.EditProfile = EditProfile;
            return View(_ManagementService.GetProfileUserInformationBynationalCodeDistrict(User.Claims.FirstOrDefault(u => u.Type == "nationalCode").Value));
        }
        #endregion

        #region EditUserProfile
        //ویرایش حساب کاربری
        public IActionResult EditProfile(string id, bool ErrorUploadPhoto = false, bool ErrorData = false)
        {
            ViewData["Maritals"] = _ManagementService.GetMaritals();
            ViewBag.ErrorData = ErrorData;
            ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
            return View(_ManagementService.GetUserForShowInEditProfileDistrict(id));
        }
        [HttpPost]
        public IActionResult EditProfile(List<int> SelectedMarital, EditProfileViewModel edit)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorData = true;
                return View(_ManagementService.GetUserForShowInEditProfileDistrict(edit.nationalCode));
            }

            if (edit.avatar != null && edit.avatar.ContentType != "image/png")
            {
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorUploadPhoto = true;
                return View(_ManagementService.GetUserForShowInEditProfileDistrict(edit.nationalCode));
            }

            if (SelectedMarital.Count == 0)
            {
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorData = true;
                return View(_ManagementService.GetUserForShowInEditProfileDistrict(edit.nationalCode));
            }
            string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
            bool resultPC = _ManagementService.UploadEditProfileAvatarDistrictPC(edit.avatar, edit);
            bool resultDB = _ManagementService.UploadEditProfileAvatarDistrictDB(edit.avatar, edit,roleUploader);
            _ManagementService.EditUserProfileDistrict(SelectedMarital, edit,roleUploader);
            return Redirect("/DistrictArea/Management/Profile?EditProfile=true");
        }
        #endregion

        #region DownloadDocuments
        //دریافت مدارک از پایگاه داده سطح بخش
        public IActionResult DownloadDocumentsFromDistrictDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistDeletedUserDocumentOnDistrictDB(id))
                {
                    var item = _ManagementService.GetDocumentFromDistrictDB(id);
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
