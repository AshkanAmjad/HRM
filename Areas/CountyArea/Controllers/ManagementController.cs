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

namespace ERP.Areas.CountyArea.Controllers
{
    [Area("CountyArea")]
    [Authorize(AuthenticationSchemes = "CountyArea")]
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
        //نمایش پایگاه داده کارکنان در سطح شهرستان
        public IActionResult DisplayCountydb(bool EditProfile = false, bool DeleteUser = false, bool ErrorDeleteUser = false, bool ErrorEditProfile = false, bool ErrorIsExistArchive = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userDepartment = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                var employees = _context.CountyLevels.Where(u => u.department.ToString() == userDepartment).AsEnumerable().Reverse().ToList();
                ViewBag.EditProfile = EditProfile;
                ViewBag.DeleteUser = DeleteUser;
                ViewBag.ErrorDeleteUser = ErrorDeleteUser;
                ViewBag.ErrorEditProfile = ErrorEditProfile;
                ViewBag.ErrorIsExistArchive = ErrorIsExistArchive;
                return View(employees);
            }
            return View("NotAccessCounty");
        }
        //نمایش پایگاه داده کارکنان در سطح بخش
        public IActionResult DisplayDistrictdb(bool EditProfile = false, bool DeleteUser = false, bool ErrorDeleteUser = false, bool ErrorIsExistArchive = false, bool ErrorRequiredPhoto = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userCounty = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                var employees = _context.DistrictLevels.Where(u => u.county.ToString() == userCounty).AsEnumerable().Reverse().ToList();
                ViewBag.EditProfile = EditProfile;
                ViewBag.DeleteUser = DeleteUser;
                ViewBag.ErrorDeleteUser = ErrorDeleteUser;
                ViewBag.ErrorIsExistArchive = ErrorIsExistArchive;
                ViewBag.ErrorRequiredPhoto = ErrorRequiredPhoto;
                return View(employees);
            }
            return View("NotAccessCounty");
        }
        //نمایش پایگاه داده مدارک در سطح شهرستان
        public IActionResult DisplayDocumentsCountydb()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userDepartment = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                var employees = _context.documentUploadCountyLevels.Where(u => u.department.ToString() == userDepartment).AsEnumerable().Reverse().ToList();
                return View(employees);
            }
            return View("NotAccessCounty");
        }
        //نمایش پایگاه داده مدارک در سطح بخش
        public IActionResult DisplayDocumentsDistrictdb()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userCounty = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                var employees = _context.documentUploadDistrictLevels.Where(u => u.county.ToString() == userCounty).AsEnumerable().Reverse().ToList();
                return View(employees);
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region Register
        //ثبت نام در سطح شهرستان
        public IActionResult RegisterationCounty(bool SuccessRegister = false, string IdC = "", bool ErrorUploadPhoto = false, bool ErrorData = false, bool ErrorIsExistManager = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewData["Genders"] = _ManagementService.GetGenders();
                ViewData["Employments"] = _ManagementService.GetEmployments();
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewData["Departments"] = _ManagementService.GetDepartments();
                ViewBag.SuccessRegister = SuccessRegister;
                ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
                ViewBag.ErrorIsExistManager = ErrorIsExistManager;
                ViewBag.ErrorData = ErrorData;
                ViewBag.IdC = IdC;
                return View();
            }
            return View("NotAccessCounty");
        }

        [HttpPost]
        public IActionResult RegisterationCounty(List<int> SelectedRoles, List<int> SelectedEmployment, List<int> SelectedMarital, List<int> SelectedDepartment, List<int> SelectedGenders, RegisterViewModel register)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (!ModelState.IsValid || SelectedRoles.Count == 0 || SelectedEmployment.Count == 0 || SelectedMarital.Count == 0 || SelectedGenders.Count == 0 || SelectedDepartment.Count == 0)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewBag.ErrorData = true;
                    return View(register);
                }

                if (register.avatar == null)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewBag.ErrorData = true;
                    return View(register);
                }

                if (register.avatar.ContentType != "image/png")
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewBag.ErrorUploadPhoto = true;
                    return View(register);
                }

                int role = 0;
                int department = 0;
                foreach (var item in SelectedRoles)
                {
                    role = item;
                }
                foreach (var item in SelectedDepartment)
                {
                    department = item;
                }

                if (role == 1 && _ManagementService.IsExistManagerCounty(department) == true)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewBag.ErrorIsExistManager = true;
                    return View(register);
                }

                //چک کردن تکراری نبودن کدملی

                if (_ManagementService.IsExistDeletedNationalCodeCounty(register.nationalCode))
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ModelState.AddModelError("nationalCode", "نام کاربری قبلا در سطح شهرستان ثبت نام شده است");
                    return View();
                }
                if (_ManagementService.IsExistNationalCodeProvince(register.nationalCode))
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ModelState.AddModelError("nationalCode", "نام کاربری قبلا در سطح استان ثبت نام شده است");
                    return View();
                }
                if (_ManagementService.IsExistNationalCodeDistrict(register.nationalCode))
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ModelState.AddModelError("nationalCode", "نام کاربری قبلا در سطح بخش ثبت نام شده است");
                    return View();
                }

                CountyLevel user = new CountyLevel()
                {
                    nationalCode = register.nationalCode
                };
                string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
                _ManagementService.AddUserAndRolesCounty(SelectedRoles, SelectedGenders, SelectedDepartment, SelectedEmployment, SelectedMarital, register, roleUploader);
                bool resultPC = _ManagementService.UploadAvatarCountyPC(register.avatar, register);
                bool resultDB = _ManagementService.UploadAvatarCountyDB(register.avatar, register, SelectedDepartment, roleUploader);
                return RedirectToAction("RegisterationCounty", new { SuccessRegister = true, IdC = register.nationalCode });
            }
            return View("NotAccessCounty");
        }

        //ثبت نام در سطح بخش
        public IActionResult RegisterationDistrict(bool SuccessRegister = false, string IdD = "", bool ErrorUploadPhoto = false, bool ErrorData = false, bool ErrorIsExistManager = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewData["Genders"] = _ManagementService.GetGenders();
                ViewData["Employments"] = _ManagementService.GetEmployments();
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewData["Departments"] = _ManagementService.GetDepartments();
                ViewData["Counties"] = _ManagementService.GetCounties();
                ViewBag.SuccessRegister = SuccessRegister;
                ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
                ViewBag.ErrorIsExistManager = ErrorIsExistManager;
                ViewBag.ErrorData = ErrorData;
                ViewBag.IdD = IdD;
                return View();
            }
            return View("NotAccessCounty");
        }

        [HttpPost]
        public IActionResult RegisterationDistrict(List<int> SelectedRoles, List<int> SelectedEmployment, List<int> SelectedMarital, List<int> SelectedDepartment, List<int> SelectedGenders, List<int> SelectedCounty, RegisterViewModel register)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {

                if (!ModelState.IsValid || SelectedCounty.Count == 0 || SelectedRoles.Count == 0 || SelectedEmployment.Count == 0 || SelectedMarital.Count == 0 || SelectedGenders.Count == 0 || SelectedDepartment.Count == 0)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ViewBag.ErrorData = true;
                    return View(register);
                }

                if (register.avatar == null)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ViewBag.ErrorData = true;
                    return View(register);
                }

                if (register.avatar.ContentType != "image/png")
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ViewBag.ErrorUploadPhoto = true;
                    return View(register);
                }

                int role = 0;
                int department = 0;
                int county = 0;
                foreach (var item in SelectedRoles)
                {
                    role = item;
                }
                foreach (var item in SelectedDepartment)
                {
                    department = item;
                }
                foreach (var item in SelectedCounty)
                {
                    county = item;
                }

                if (role == 1 && _ManagementService.IsExistManagerDistrict(county, department) == true)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ViewBag.ErrorIsExistManager = true;
                    return View(register);
                }

                //چک کردن تکراری نبودن کدملی

                if (_ManagementService.IsExistDeletedNationalCodeDistrict(register.nationalCode))
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ModelState.AddModelError("nationalCode", "نام کاربری قبلا در سطح بخش ثبت نام شده است");
                    return View();
                }
                if (_ManagementService.IsExistNationalCodeCounty(register.nationalCode))
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ModelState.AddModelError("nationalCode", "نام کاربری قبلا در سطح شهرستان ثبت نام شده است");
                    return View();
                }
                if (_ManagementService.IsExistNationalCodeProvince(register.nationalCode))
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ModelState.AddModelError("nationalCode", "نام کاربری قبلا در سطح استان ثبت نام شده است");
                    return View();
                }


                DistrictLevel user = new DistrictLevel()
                {
                    nationalCode = register.nationalCode
                };
                string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
                _ManagementService.AddUserAndRolesDistrict(SelectedRoles, SelectedGenders, SelectedDepartment, SelectedEmployment, SelectedMarital, SelectedCounty, register, roleUploader);
                bool resultPC = _ManagementService.UploadAvatarDistrictPC(register.avatar, register);
                bool resultDB = _ManagementService.UploadAvatarDistrictDB(register.avatar, register, SelectedDepartment, SelectedCounty, roleUploader);
                return RedirectToAction("RegisterationDistrict", new { SuccessRegister = true, IdD = register.nationalCode });
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region EditUser
        //ویرایش در سطح شهرستان
        public IActionResult EditUserCounty(string id, bool ErrorUploadPhoto = false, bool ErrorData = false, bool ErrorIsExistManager = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_context.CountyLevels.SingleOrDefault(u => u.nationalCode == id).role == "مدیریت")
                {
                    return Redirect("/CountyArea/Management/DisplayCountydb?ErrorEditProfile=true");

                }
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewData["Genders"] = _ManagementService.GetGenders();
                ViewData["Employments"] = _ManagementService.GetEmployments();
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewData["Departments"] = _ManagementService.GetDepartments();
                ViewBag.ErrorData = ErrorData;
                ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
                ViewBag.ErrorIsExistManager = ErrorIsExistManager;
                return View(_ManagementService.GetUserForShowInEditCounty(id));
            }
            return View("NotAccessCounty");
        }
        [HttpPost]
        public IActionResult EditUserCounty(List<int> SelectedRoles, List<int> SelectedEmployment, List<int> SelectedMarital, List<int> SelectedDepartment, List<int> SelectedGenders, EditViewModel edit)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewBag.ErrorData = true;
                    return View(_ManagementService.GetUserForShowInEditCounty(edit.nationalCode));
                }

                if (edit.avatar != null && edit.avatar.ContentType != "image/png")
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewBag.ErrorUploadPhoto = true;
                    return View(_ManagementService.GetUserForShowInEditCounty(edit.nationalCode));
                }

                int selectedRole = 0;
                int selectedDepartment = 0;

                foreach (var item in SelectedRoles)
                {
                    selectedRole = item;
                }
                foreach (int item in SelectedDepartment)
                {
                    selectedDepartment = item;
                }

                if (_ManagementService.IsExistManagerCounty(selectedDepartment) == true)
                {
                    if (selectedRole == 1 && _ManagementService.GetManagernationalCodeCounty(selectedDepartment) != edit.nationalCode)
                    {

                        ViewData["Roles"] = _ManagementService.GetRoles();
                        ViewData["Genders"] = _ManagementService.GetGenders();
                        ViewData["Employments"] = _ManagementService.GetEmployments();
                        ViewData["Maritals"] = _ManagementService.GetMaritals();
                        ViewData["Departments"] = _ManagementService.GetDepartments();
                        ViewBag.ErrorIsExistManager = true;
                        return View(_ManagementService.GetUserForShowInEditCounty(edit.nationalCode));
                    }
                }
                string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
                bool resultPC = _ManagementService.UploadEditAvatarCountyPC(edit.avatar, edit);
                bool resultDB = _ManagementService.UploadEditAvatarCountyDB(edit.avatar, edit, selectedDepartment, selectedRole, roleUploader);
                _ManagementService.EditUserAndRolesCounty(selectedRole, SelectedEmployment, SelectedMarital, selectedDepartment, SelectedGenders, edit, roleUploader);
                if (roleUploader != _ManagementService.GetRoleWithnationalCodeCounty(User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value.ToString()))
                {
                    return Redirect("/CLogout");
                }
                return Redirect("/CountyArea/Management/DisplayCountydb?EditProfile=true");
            }
            return View("NotAccessCounty");
        }
        //ویرایش در سطح بخش
        public IActionResult EditUserDistrict(string id, bool ErrorUploadPhoto = false, bool ErrorData = false, bool ErrorIsExistManager = false, bool ErrorRequiredPhoto = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {

                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewData["Genders"] = _ManagementService.GetGenders();
                ViewData["Employments"] = _ManagementService.GetEmployments();
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewData["Departments"] = _ManagementService.GetDepartments();
                ViewData["Counties"] = _ManagementService.GetCounties();
                ViewBag.ErrorData = ErrorData;
                ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
                ViewBag.ErrorIsExistManager = ErrorIsExistManager;
                ViewBag.ErrorRequiredPhoto = ErrorRequiredPhoto;
                return View(_ManagementService.GetUserForShowInEditDistrict(id));
            }
            return View("NotAccessCounty");
        }
        [HttpPost]
        public IActionResult EditUserDistrict(List<int> SelectedRoles, List<int> SelectedEmployment, List<int> SelectedMarital, List<int> SelectedDepartment, List<int> SelectedGenders, List<int> SelectedCounty, EditViewModel edit)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ViewBag.ErrorData = true;
                    return View(_ManagementService.GetUserForShowInEditDistrict(edit.nationalCode));
                }

                if (edit.avatar != null && edit.avatar.ContentType != "image/png")
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ViewBag.ErrorUploadPhoto = true;
                    return View(_ManagementService.GetUserForShowInEditDistrict(edit.nationalCode));
                }

                int selectedRole = 0;
                int selectedDepartment = 0;
                int selectedCounty = 0;

                foreach (var item in SelectedRoles)
                {
                    selectedRole = item;
                }
                foreach (int item in SelectedDepartment)
                {
                    selectedDepartment = item;
                }
                foreach (int item in SelectedCounty)
                {
                    selectedCounty = item;
                }

                if (_ManagementService.IsExistManagerDistrict(selectedCounty, selectedDepartment) == true)
                {
                    if (selectedRole == 1 && _ManagementService.GetManagernationalCodeDistrict(selectedCounty, selectedDepartment) != edit.nationalCode)
                    {
                        ViewData["Roles"] = _ManagementService.GetRoles();
                        ViewData["Genders"] = _ManagementService.GetGenders();
                        ViewData["Employments"] = _ManagementService.GetEmployments();
                        ViewData["Maritals"] = _ManagementService.GetMaritals();
                        ViewData["Departments"] = _ManagementService.GetDepartments();
                        ViewData["Counties"] = _ManagementService.GetCounties();
                        ViewBag.ErrorIsExistManager = true;
                        return View(_ManagementService.GetUserForShowInEditDistrict(edit.nationalCode));
                    }
                }
                if (edit.avatar == null && selectedRole == 1 && _ManagementService.IsExistDeletedNationalCodeCounty(edit.nationalCode) == false)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewData["Counties"] = _ManagementService.GetCounties();
                    ViewBag.ErrorRequiredPhoto = true;
                    return View(_ManagementService.GetUserForShowInEditDistrict(edit.nationalCode));
                }
                string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
                bool resultPC = _ManagementService.UploadEditAvatarDistrictPC(edit.avatar, edit);
                bool resultDB = _ManagementService.UploadEditAvatarDistrictDB(edit.avatar, edit, selectedDepartment, selectedCounty, roleUploader);
                _ManagementService.EditUserAndRolesDistrict(selectedRole, SelectedEmployment, SelectedMarital, selectedDepartment, SelectedGenders, selectedCounty, edit, roleUploader);
                return Redirect("/CountyArea/Management/DisplayDistrictdb?EditProfile=true");
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region Delete
        // حذقف کاربر در سطح شهرستان
        public IActionResult ListDeleteUsersCounty(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_context.CountyLevels.SingleOrDefault(u => u.nationalCode == id).role == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value == id)
                {
                    return Redirect("/CountyArea/Management/DisplayCountydb?ErrorDeleteUser=true");
                }
                if (_ManagementService.IsExistNationalCodeCounty(id) && _ManagementService.IsExistNationalCodeProvince(id))
                {
                    return Redirect("/CountyArea/Management/DisplayCountydb?ErrorDeleteUser=true");
                }
                if (_ManagementService.IsExistDeletedUserOnArchiveCounty(id))
                {
                    return Redirect("/CountyArea/Management/DisplayCountydb?ErrorIsExistArchive=true");
                }
                return View(_ManagementService.GetUserInformationBynationalCodeCounty(id));
            }
            return View("NotAccessCounty");
        }

        [HttpPost]
        public IActionResult ListDeleteUsersCounty(InformationViewModel delete)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (!ModelState.IsValid)
                {
                    return View(delete);
                }
                string nationalCode = delete.nationalCode;
                _ManagementService.DeleteUserCounty(nationalCode);
                _ManagementService.DeleteUserC(nationalCode);
                _ManagementService.DeleteAvatarPicOnCountyPC(nationalCode);
                _ManagementService.DeleteUserDocumentsCountyDB(nationalCode);
                return Redirect("/CountyArea/Management/DisplayCountydb?DeleteUser=true");
            }
            return View("NotAccessCounty");
        }
        // حذقف کاربر در سطح بخش
        public IActionResult ListDeleteUsersDistrict(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_context.DistrictLevels.SingleOrDefault(u => u.nationalCode == id).role == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value == id)
                {
                    return Redirect("/CountyArea/Management/DisplayDistrictdb?ErrorDeleteUser=true");
                }
                if (_ManagementService.IsExistNationalCodeCounty(id) && _ManagementService.IsExistNationalCodeDistrict(id))
                {
                    return Redirect("/CountyArea/Management/DisplayDistrictdb?ErrorDeleteUser=true");
                }
                if (_ManagementService.IsExistDeletedUserOnArchiveCounty(id))
                {
                    return Redirect("/CountyArea/Management/DisplayDistrictdb?ErrorIsExistArchive=true");
                }
                return View(_ManagementService.GetUserInformationBynationalCodeDistrict(id));
            }
            return View("NotAccessCounty");
        }

        [HttpPost]
        public IActionResult ListDeleteUsersDistrict(InformationViewModel delete)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (!ModelState.IsValid)
                {
                    return View(delete);
                }
                string nationalCode = delete.nationalCode;
                _ManagementService.DeleteUserDistrict(nationalCode);
                _ManagementService.DeleteUserD(nationalCode);
                _ManagementService.DeleteAvatarPicOnDistrictPC(nationalCode);
                _ManagementService.DeleteUserDocumentsDistrictDB(nationalCode);
                return Redirect("/CountyArea/Management/DisplayDistrictdb?DeleteUser=true");
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region Archive
        //مشاهده جدول کارکنان حذف شده در سطح شهرستان
        public IActionResult ArchiveCounty(bool ActiveUser = false, bool ErrorActiveUser = false, bool ErrorIsExsistId = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userDepartment = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                var deleted = _context.CountyLevels.IgnoreQueryFilters().Where(u => u.IsDelete && u.department.ToString() == userDepartment).AsEnumerable().Reverse().ToList();
                ViewBag.ActiveUser = ActiveUser;
                ViewBag.ErrorIsExsistId = ErrorIsExsistId;
                ViewBag.ErrorActiveUser = ErrorActiveUser;
                return View(deleted);
            }
            return View("NotAccessCounty");
        }
        //مشاهده جدول کارکنان حذف شده در سطح بخش
        public IActionResult ArchiveDistrict(bool ActiveUser = false, bool ErrorActiveUser = false, bool ErrorIsExsistId = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userCounty = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                var deleted = _context.DistrictLevels.IgnoreQueryFilters().Where(u => u.IsDelete && u.county.ToString() == userCounty).AsEnumerable().Reverse().ToList();
                ViewBag.ActiveUser = ActiveUser;
                ViewBag.ErrorIsExsistId = ErrorIsExsistId;
                ViewBag.ErrorActiveUser = ErrorActiveUser;
                return View(deleted);
            }
            return View("NotAccessCounty");
        }
        //مشاهده جدول مدارک کارکنان حذف شده در سطح شهرستان
        public IActionResult ArchiveDocumentsCounty()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userDepartment = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                var deleted = _context.documentUploadCountyLevels.IgnoreQueryFilters().Where(u => u.IsDelete && u.department.ToString() == userDepartment).AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessCounty");
        }
        //مشاهده جدول مدارک کارکنان حذف شده در سطح بخش
        public IActionResult ArchiveDocumentsDistrict()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string userCounty = User.Claims.FirstOrDefault(u => u.Type == "department").Value;
                var deleted = _context.documentUploadDistrictLevels.IgnoreQueryFilters().Where(u => u.IsDelete && u.county.ToString() == userCounty).AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessCounty");
        }
        //مشاهده جدول مدارک تبادل شده کاربر حذف شده در سطح شهرستان
        public IActionResult ArchiveExchangeDocumentsCounty()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.transferDocuments.IgnoreQueryFilters().Where(u => u.IsDelete && u.userUploaderArea == "شهرستان").AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessCounty");
        }
        //مشاهده جدول مدارک تبادل شده کاربر حذف شده در سطح بخش
        public IActionResult ArchiveExchangeDocumentsDistrict()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.transferDocuments.IgnoreQueryFilters().Where(u => u.IsDelete && u.userUploaderArea == "بخش").AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region DownloadArchiveDocuments
        //دریافت مدارک حذف شده در سطح شهرستان
        public IActionResult DownloadDeletedDocumentsFromCountyDb(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistDeletedUserDocumentOnCountyDB(id) == true)
                {
                    var item = _ManagementService.GetDeletedDocumentFromCountyDB(id);
                    byte[] bytes = item.dataBytes;
                    string contenetType = item.contentType;
                    string fileName = item.fileName;
                    return File(bytes, contenetType, fileName);
                }
                return Content("Not Found");
            }
            return View("NotAccessCounty");
        }
        //دریافت مدارک حذف شده در سطح بخش
        public IActionResult DownloadDeletedDocumentsFromDistrictDb(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistDeletedUserDocumentOnDistrictDB(id) == true)
                {
                    var item = _ManagementService.GetDeletedDocumentFromDistrictDB(id);
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

        #region ShowActivationMessageUserDeleted
        //نمایش پیام فعال کردن کاربر حذف شده در سطح شهرستان  
        public IActionResult ShowActivationMessageCounty(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistNationalCodeCounty(id) == true || _ManagementService.IsExistNationalCodeDistrict(id) == true)
                {
                    return Redirect("/CountyArea/Management/ArchiveCounty?ErrorIsExsistId=true");
                }
                string role = _ManagementService.GetDeletedRoleWithnationalCodeCounty(id);
                int departmentCounty = _ManagementService.GetDeletedDepartmentIdWithnationalCodeCounty(id);

                if (role == "مدیریت" && _ManagementService.IsExistManagerCounty(departmentCounty) == true)
                {
                    return Redirect("/CountyArea/Management/ArchiveCounty?ErrorActiveUser=true");
                }

                if (_ManagementService.IsExistDeletedNationalCodeDistrict(id) && _ManagementService.IsExistDeletedNationalCodeCounty(id))
                {
                    if (_ManagementService.GetDeletedRoleWithnationalCodeDistrict(id) == "مدیریت"
                        && _ManagementService.IsExistManagerDistrict(departmentCounty, _ManagementService.GetDeletedDepartmentIdWithnationalCodeDistrict(id)))
                    {
                        return Redirect("/CountyArea/Management/ArchiveCounty?ErrorActiveUser=true");
                    }
                }

                if (_ManagementService.IsExistDeletedNationalCodeCounty(id) == true && _ManagementService.IsExistDeletedNationalCodeProvince(id) == true)
                {
                    return Redirect("/CountyArea/Management/ArchiveCounty?ErrorActiveUser=true");
                }
                return View(_ManagementService.GetUserDeletedBynationalCodeCountyLevel(id));
            }
            return View("NotAccessCounty");
        }
        //نمایش پیام فعال کردن کاربر حذف شده در سطح بخش  
        public IActionResult ShowActivationMessageDistrict(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistNationalCodeDistrict(id) == true)
                {
                    return Redirect("/CountyArea/Management/ArchiveDistrict?ErrorIsExsistId=true");
                }
                string role = _ManagementService.GetDeletedRoleWithnationalCodeDistrict(id);
                int departmentDistrict = _ManagementService.GetDeletedDepartmentIdWithnationalCodeDistrict(id);
                int countyDistrict = _ManagementService.GetDeletedCountyIdWithnationalCodeDistrict(id);
                if (role == "مدیریت" && _ManagementService.IsExistManagerDistrict(countyDistrict, departmentDistrict) == true)
                {
                    return Redirect("/CountyArea/Management/ArchiveDistrict?ErrorActiveUser=true");
                }
                if (_ManagementService.IsExistDeletedNationalCodeDistrict(id) == true && _ManagementService.IsExistDeletedNationalCodeCounty(id) == true)
                {
                    return Redirect("/CountyArea/Management/ArchiveDistrict?ErrorActiveUser=true");
                }
                return View(_ManagementService.GetUserDeletedBynationalCodeDistrictLevel(id));
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region ActivationUserDeleted
        //فعال سازی کاربر پاک شده در سطح شهرستان
        public IActionResult ActivationUserDeletedCounty(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string nationalCode = id;
                _ManagementService.ActivationDeletedUserCounty(nationalCode);
                _ManagementService.ActivationDeletedUserC(nationalCode);
                _ManagementService.ActivationDeletedUserDocumentsCounty(nationalCode);
                _ManagementService.DownloadProfileAvatarPicAfterActiveCounty(nationalCode);
                return Redirect("/CountyArea/Management/ArchiveCounty?ActiveUser=true");
            }
            return View("NotAccessCounty");
        }
        //فعال سازی کاربر پاک شده در سطح بخش
        public IActionResult ActivationUserDeletedDistrict(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string nationalCode = id;
                _ManagementService.ActivationDeletedUserDistrict(nationalCode);
                _ManagementService.ActivationDeletedUserD(nationalCode);
                _ManagementService.ActivationDeletedUserDocumentsDistrict(nationalCode);
                _ManagementService.DownloadProfileAvatarPicAfterActiveDistrict(nationalCode);
                return Redirect("/CountyArea/Management/ArchiveDistrict?ActiveUser=true");
            }
            return View("NotAccessCounty");
        }
        #endregion

        #region MyDocument
        //مدارک من
        public IActionResult MyDocuments()
        {
            string myNationalCode = User.Claims.FirstOrDefault(u => u.Type == "nationalCode").Value.ToString();
            var myDocuments = _context.documentUploadCountyLevels.Where(d => d.ownerUserId == myNationalCode).ToList();
            return View(myDocuments);
        }

        //دریافت مدارک به وسیله شناسه
        public IActionResult DownloadMyDocuments(int id)
        {
            if (_ManagementService.IsExistUserDocumentOnCountyDB(id))
            {
                var item = _ManagementService.GetDocumentFromCountyDB(id);
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
            return View(_ManagementService.GetProfileUserInformationBynationalCodeCounty(User.Claims.FirstOrDefault(u => u.Type == "nationalCode").Value));
        }
        #endregion

        #region EditUserProfile
        //ویرایش حساب کاربری
        public IActionResult EditProfile(string id, bool ErrorUploadPhoto = false, bool ErrorData = false)
        {
            ViewData["Maritals"] = _ManagementService.GetMaritals();
            ViewBag.ErrorData = ErrorData;
            ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
            return View(_ManagementService.GetUserForShowInEditProfileCounty(id));
        }
        [HttpPost]
        public IActionResult EditProfile(List<int> SelectedMarital, EditProfileViewModel edit)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorData = true;
                return View(_ManagementService.GetUserForShowInEditProfileCounty(edit.nationalCode));
            }

            if (edit.avatar != null && edit.avatar.ContentType != "image/png")
            {
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorUploadPhoto = true;
                return View(_ManagementService.GetUserForShowInEditProfileCounty(edit.nationalCode));
            }

            if (SelectedMarital.Count == 0)
            {
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorData = true;
                return View(_ManagementService.GetUserForShowInEditProfileCounty(edit.nationalCode));
            }
            string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
            bool resultPC = _ManagementService.UploadEditProfileAvatarCountyPC(edit.avatar, edit);
            bool resultDB = _ManagementService.UploadEditProfileAvatarCountyDB(edit.avatar, edit, roleUploader);
            _ManagementService.EditUserProfileCounty(SelectedMarital, edit, roleUploader);
            return Redirect("/CountyArea/Management/Profile?EditProfile=true");
        }
        #endregion

        #region DownloadDocuments
        //دریافت مدارک از پایگاه داده سطح شهرستان
        public IActionResult DownloadDocumentsFromCountyDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistUserDocumentOnCountyDB(id))
                {
                    var item = _ManagementService.GetDocumentFromCountyDB(id);
                    byte[] bytes = item.dataBytes;
                    string contenetType = item.contentType;
                    string fileName = item.fileName;
                    return File(bytes, contenetType, fileName);
                }
                return Content("Not Found");
            }
            return View("NotAccessCounty");
        }
        //دریافت مدارک از پایگاه داده سطح بخش
        public IActionResult DownloadDocumentsFromDistrictDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
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
            return View("NotAccessCounty");
        }
        #endregion
    }
}
