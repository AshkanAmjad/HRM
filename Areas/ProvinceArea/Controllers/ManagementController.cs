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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERP.Areas.ProvinceArea.Controllers
{
    [Area("ProvinceArea")]
    [Authorize(AuthenticationSchemes = "ProvinceArea")]

    public class ManagementController : Controller
    {
        //پیاده سازی توابع مدیریت پرسنل
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
        //نمایش پایگاه داده کارکنان در سطح استان
        public IActionResult DisplayProvincedb(bool EditProfile = false, bool DeleteUser = false, bool ErrorDeleteUser = false, bool ErrorIsExistArchive = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var employees = _context.ProvinceLevel.AsEnumerable().Reverse().ToList();
                ViewBag.EditProfile = EditProfile;
                ViewBag.DeleteUser = DeleteUser;
                ViewBag.ErrorDeleteUser = ErrorDeleteUser;
                ViewBag.ErrorIsExistArchive = ErrorIsExistArchive;
                return View(employees);
            }
            return View("NotAccessProvince");
        }
        //نمایش پایگاه داده کارکنان در سطح شهرستان
        public IActionResult DisplayCountydb(bool EditProfile = false, bool DeleteUser = false, bool ErrorDeleteUser = false, bool ErrorIsExistArchive = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var employees = _context.CountyLevels.AsEnumerable().Reverse().ToList();
                ViewBag.EditProfile = EditProfile;
                ViewBag.DeleteUser = DeleteUser;
                ViewBag.ErrorDeleteUser = ErrorDeleteUser;
                ViewBag.ErrorIsExistArchive = ErrorIsExistArchive;
                return View(employees);
            }
            return View("NotAccessProvince");
        }
        //نمایش پایگاه داده کارکنان در سطح بخش
        public IActionResult DisplayDistrictdb(bool EditProfile = false, bool DeleteUser = false, bool ErrorDeleteUser = false, bool ErrorIsExistArchive = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var employees = _context.DistrictLevels.AsEnumerable().Reverse().ToList();
                ViewBag.EditProfile = EditProfile;
                ViewBag.DeleteUser = DeleteUser;
                ViewBag.ErrorDeleteUser = ErrorDeleteUser;
                ViewBag.ErrorIsExistArchive = ErrorIsExistArchive;
                return View(employees);
            }
            return View("NotAccessProvince");
        }

        //نمایش پایگاه داده کارکنان سه سطح 
        public IActionResult DisplayEmployeesdb()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var employees = _context.Employees.AsEnumerable().Reverse().ToList();
                return View(employees);
            }
            return View("NotAccessProvince");
        }

        //نمایش پایگاه داده مدارک در سطح استان
        public IActionResult DisplayDocumentsProvincedb()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var employees = _context.documentUploadProvinceLevels.AsEnumerable().Reverse().ToList();
                return View(employees);
            }
            return View("NotAccessProvince");
        }
        //نمایش پایگاه داده مدارک در سطح شهرستان
        public IActionResult DisplayDocumentsCountydb()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var employees = _context.documentUploadCountyLevels.AsEnumerable().Reverse().ToList();
                return View(employees);
            }
            return View("NotAccessProvince");
        }
        //نمایش پایگاه داده مدارک در سطح بخش
        public IActionResult DisplayDocumentsDistrictdb()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var employees = _context.documentUploadDistrictLevels.AsEnumerable().Reverse().ToList();
                return View(employees);
            }
            return View("NotAccessProvince");
        }
        //نمایش پایگاه داده نقش ها
        public IActionResult DisplayRolesdb(bool DeleteRole = false, bool ActivationDeletedRole = false, bool SuccessAddNewRole = false,
            string newRoleTitle = "", bool ErrorAddNewRole = false, bool ErrorEmptyNewRole = false, bool ErrorFalseRoleValue = false,bool ErrorMax=false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var roles = _context.Roles.IgnoreQueryFilters().ToList();
                ViewBag.DeleteRole = DeleteRole;
                ViewBag.ActivationDeletedRole = ActivationDeletedRole;
                ViewBag.SuccessAddNewRole = SuccessAddNewRole;
                ViewBag.newRoleTitle = newRoleTitle;
                ViewBag.ErrorAddNewRole = ErrorAddNewRole;
                ViewBag.ErrorEmptyNewRole = ErrorEmptyNewRole;
                ViewBag.ErrorFalseRoleValue = ErrorFalseRoleValue;
                ViewBag.ErrorMax = ErrorMax;
                return View(roles);
            }
            return View("NotAccessProvince");
        }
        #endregion

        #region Register
        //ثبت نام در سطح استان
        public IActionResult RegisterationProvince(bool SuccessRegister = false, string IdP = "", bool ErrorUploadPhoto = false, bool ErrorData = false, bool ErrorIsExistManager = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewData["Genders"] = _ManagementService.GetGenders();
                ViewData["Employments"] = _ManagementService.GetEmployments();
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.SuccessRegister = SuccessRegister;
                ViewBag.ErrorData = ErrorData;
                ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
                ViewBag.ErrorIsExistManager = ErrorIsExistManager;
                ViewBag.IdP = IdP;
                return View();
            }
            return View("NotAccessProvince");
        }

        [HttpPost]
        public IActionResult RegisterationProvince(List<int> SelectedRoles, List<int> SelectedEmployment, List<int> SelectedMarital, List<int> SelectedDepartment, List<int> SelectedGenders, RegisterViewModel register)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (!ModelState.IsValid || SelectedRoles.Count == 0 || SelectedEmployment.Count == 0 || SelectedMarital.Count == 0 || SelectedGenders.Count == 0)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewBag.ErrorData = true;
                    return View(register);
                }

                if (register.avatar == null)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewBag.ErrorData = true;
                    return View(register);
                }

                if (register.avatar.ContentType != "image/png")
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewBag.ErrorUploadPhoto = true;
                    return View(register);
                }

                int role = 0;
                foreach (var item in SelectedRoles)
                {
                    role = item;
                }

                if (role == 1 && _ManagementService.IsExistManagerProvince() == true)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewBag.ErrorIsExistManager = true;
                    return View(register);
                }

                //چک کردن تکراری نبودن کدملی
                if (_ManagementService.IsExistDeletedNationalCodeProvince(register.nationalCode))
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ModelState.AddModelError("nationalCode", "نام کاربری قبلا در سطح استان ثبت نام شده است");
                    return View();
                }
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
                if (_ManagementService.IsExistNationalCodeDistrict(register.nationalCode))
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ModelState.AddModelError("nationalCode", "نام کاربری قبلا در سطح بخش ثبت نام شده است");
                    return View();
                }


                ProvinceLevel user = new ProvinceLevel()
                {
                    nationalCode = register.nationalCode
                };
                string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
                _ManagementService.AddUserAndRolesProvince(SelectedRoles, SelectedGenders, SelectedEmployment, SelectedMarital, register, roleUploader);
                bool resultPC = _ManagementService.UploadAvatarProvincePC(register.avatar, register);
                bool resultDB = _ManagementService.UploadAvatarProvinceDB(register.avatar, register, roleUploader);

                return RedirectToAction("RegisterationProvince", new { SuccessRegister = true, IdP = register.nationalCode.ToString() });
            }
            return View("NotAccessProvince");
        }

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
            return View("NotAccessProvince");
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
            return View("NotAccessProvince");
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
            return View("NotAccessProvince");
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
            return View("NotAccessProvince");
        }
        #endregion

        #region EditUser
        //ویرایش در سطح استان
        public IActionResult EditUserProvince(string id, bool ErrorUploadPhoto = false, bool ErrorData = false, bool ErrorIsExistManager = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {

                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewData["Genders"] = _ManagementService.GetGenders();
                ViewData["Employments"] = _ManagementService.GetEmployments();
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
                ViewBag.ErrorIsExistManager = ErrorIsExistManager;
                ViewBag.ErrorData = ErrorData;
                return View(_ManagementService.GetUserForShowInEditProvince(id));
            }
            return View("NotAccessProvince");
        }
        [HttpPost]
        public IActionResult EditUserProvince(List<int> SelectedRoles, List<int> SelectedEmployment, List<int> SelectedMarital, List<int> SelectedGenders, EditViewModel edit)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (!ModelState.IsValid)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewBag.ErrorData = true;
                    return View(_ManagementService.GetUserForShowInEditProvince(edit.nationalCode));
                }

                if (edit.avatar != null && edit.avatar.ContentType != "image/png")
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewBag.ErrorUploadPhoto = true;
                    return View(_ManagementService.GetUserForShowInEditProvince(edit.nationalCode));
                }

                int selectedRole = 0;
                foreach (int item in SelectedRoles)
                {
                    selectedRole = item;
                }
                if (_ManagementService.IsExistManagerProvince() == true)
                {
                    if (selectedRole == 1 && _ManagementService.GetManagernationalCodeProvince() != edit.nationalCode)
                    {
                        ViewData["Roles"] = _ManagementService.GetRoles();
                        ViewData["Genders"] = _ManagementService.GetGenders();
                        ViewData["Employments"] = _ManagementService.GetEmployments();
                        ViewData["Maritals"] = _ManagementService.GetMaritals();
                        ViewBag.ErrorIsExistManager = true;
                        return View(_ManagementService.GetUserForShowInEditProvince(edit.nationalCode));
                    }
                }
                string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
                bool resultPC = _ManagementService.UploadEditAvatarProvincePC(edit.avatar, edit);
                bool resultDB = _ManagementService.UploadEditAvatarProvinceDB(edit.avatar, edit, selectedRole, roleUploader);
                _ManagementService.EditUserAndRolesProvince(selectedRole, SelectedEmployment, SelectedMarital, SelectedGenders, edit, roleUploader);
                if (roleUploader != _ManagementService.GetRoleWithnationalCodeProvince(User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value.ToString()))
                {
                    return Redirect("/PLogout");
                }
                return Redirect("/ProvinceArea/Management/DisplayProvincedb?EditProfile=true");
            }
            return View("NotAccessProvince");
        }
        //ویرایش در سطح شهرستان
        public IActionResult EditUserCounty(string id, bool ErrorUploadPhoto = false, bool ErrorData = false, bool ErrorIsExistManager = false, bool ErrorRequiredPhoto = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                ViewData["Roles"] = _ManagementService.GetRoles();
                ViewData["Genders"] = _ManagementService.GetGenders();
                ViewData["Employments"] = _ManagementService.GetEmployments();
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewData["Departments"] = _ManagementService.GetDepartments();
                ViewBag.ErrorData = ErrorData;
                ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
                ViewBag.ErrorIsExistManager = ErrorIsExistManager;
                ViewBag.ErrorRequiredPhoto = ErrorRequiredPhoto;
                return View(_ManagementService.GetUserForShowInEditCounty(id));
            }
            return View("NotAccessProvince");
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

                if (edit.avatar == null && selectedRole == 1 && _ManagementService.IsExistDeletedNationalCodeProvince(edit.nationalCode) == false)
                {
                    ViewData["Roles"] = _ManagementService.GetRoles();
                    ViewData["Genders"] = _ManagementService.GetGenders();
                    ViewData["Employments"] = _ManagementService.GetEmployments();
                    ViewData["Maritals"] = _ManagementService.GetMaritals();
                    ViewData["Departments"] = _ManagementService.GetDepartments();
                    ViewBag.ErrorRequiredPhoto = true;
                    return View(_ManagementService.GetUserForShowInEditCounty(edit.nationalCode));
                }

                string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
                bool resultPC = _ManagementService.UploadEditAvatarCountyPC(edit.avatar, edit);
                bool resultDB = _ManagementService.UploadEditAvatarCountyDB(edit.avatar, edit, selectedDepartment, selectedRole, roleUploader);
                _ManagementService.EditUserAndRolesCounty(selectedRole, SelectedEmployment, SelectedMarital, selectedDepartment, SelectedGenders, edit, roleUploader);
                return Redirect("/ProvinceArea/Management/DisplayCountydb?EditProfile=true");
            }
            return View("NotAccessProvince");
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
                ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
                ViewBag.ErrorRequiredPhoto = ErrorRequiredPhoto;
                return View(_ManagementService.GetUserForShowInEditDistrict(id));
            }
            return View("NotAccessProvince");
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
                return Redirect("/ProvinceArea/Management/DisplayDistrictdb?EditProfile=true");
            }
            return View("NotAccessProvince");
        }
        #endregion

        #region Delete
        // حذف کاربر در سطح استان
        public IActionResult ListDeleteUsersProvince(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value == id)
                {
                    return Redirect("/ProvinceArea/Management/DisplayProvincedb?ErrorDeleteUser=true");
                }
                if (_ManagementService.IsExistDeletedUserOnArchiveProvince(id))
                {
                    return Redirect("/ProvinceArea/Management/DisplayProvincedb?ErrorIsExistArchive=true");
                }
                return View(_ManagementService.GetUserInformationBynationalCodeProvince(id));
            }
            return View("NotAccessProvince");
        }

        [HttpPost]
        public IActionResult ListDeleteUsersProvince(InformationViewModel delete)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (!ModelState.IsValid)
                {
                    return View(delete);
                }
                string nationalCode = delete.nationalCode;
                _ManagementService.DeleteUserProvince(nationalCode);
                _ManagementService.DeleteUserP(nationalCode);
                _ManagementService.DeleteAvatarPicOnProvincePC(nationalCode);
                _ManagementService.DeleteUserDocumentsProvinceDB(nationalCode);
                return Redirect("/ProvinceArea/Management/DisplayProvincedb?DeleteUser=true");
            }
            return View("NotAccessProvince");
        }
        // حذقف کاربر در سطح شهرستان
        public IActionResult ListDeleteUsersCounty(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_context.CountyLevels.SingleOrDefault(u => u.nationalCode == id).role == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value == id)
                {
                    return Redirect("/ProvinceArea/Management/DisplayCountydb?ErrorDeleteUser=true");
                }
                if(_ManagementService.IsExistNationalCodeCounty(id)  &&  _ManagementService.IsExistNationalCodeProvince(id))
                {
                    return Redirect("/ProvinceArea/Management/DisplayCountydb?ErrorDeleteUser=true");
                }
                if (_ManagementService.IsExistDeletedUserOnArchiveCounty(id))
                {
                    return Redirect("/ProvinceArea/Management/DisplayCountydb?ErrorIsExistArchive=true");
                }
                return View(_ManagementService.GetUserInformationBynationalCodeCounty(id));
            }
            return View("NotAccessProvince");
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
                return Redirect("/ProvinceArea/Management/DisplayCountydb?DeleteUser=true");
            }
            return View("NotAccessProvince");
        }
        // حذقف کاربر در سطح بخش
        public IActionResult ListDeleteUsersDistrict(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_context.DistrictLevels.SingleOrDefault(u => u.nationalCode == id).role == "مدیریت" || User.Claims.FirstOrDefault(c => c.Type == "nationalCode").Value == id)
                {
                    return Redirect("/ProvinceArea/Management/DisplayDistrictdb?ErrorDeleteUser=true");
                }
                if (_ManagementService.IsExistNationalCodeCounty(id) && _ManagementService.IsExistNationalCodeDistrict(id))
                {
                    return Redirect("/ProvinceArea/Management/DisplayDistrictdb?ErrorDeleteUser=true");
                }
                if (_ManagementService.IsExistDeletedUserOnArchiveCounty(id))
                {
                    return Redirect("/ProvinceArea/Management/DisplayDistrictdb?ErrorIsExistArchive=true");
                }
                return View(_ManagementService.GetUserInformationBynationalCodeDistrict(id));
            }
            return View("NotAccessProvince");
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
                return Redirect("/ProvinceArea/Management/DisplayDistrictdb?DeleteUser=true");
            }
            return View("NotAccessProvince");
        }
        #endregion

        #region Archive
        //مشاهده جدول کارکنان حذف شده در سطح استان
        public IActionResult ArchiveProvince(bool ActiveUser = false, bool ErrorActiveUser = false, bool ErrorIsExsistId = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.ProvinceLevel.IgnoreQueryFilters().Where(u => u.IsDelete).AsEnumerable().Reverse().ToList();
                ViewBag.ActiveUser = ActiveUser;
                ViewBag.ErrorIsExsistId = ErrorIsExsistId;
                ViewBag.ErrorActiveUser = ErrorActiveUser;
                return View(deleted);
            }
            return View("NotAccessProvince");
        }
        //مشاهده جدول کارکنان حذف شده در سطح شهرستان
        public IActionResult ArchiveCounty(bool ActiveUser = false, bool ErrorActiveUser = false, bool ErrorIsExsistId = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.CountyLevels.IgnoreQueryFilters().Where(u => u.IsDelete).AsEnumerable().Reverse().ToList();
                ViewBag.ActiveUser = ActiveUser;
                ViewBag.ErrorIsExsistId = ErrorIsExsistId;
                ViewBag.ErrorActiveUser = ErrorActiveUser;
                return View(deleted);
            }
            return View("NotAccessProvince");
        }
        //مشاهده جدول کارکنان حذف شده در سطح بخش
        public IActionResult ArchiveDistrict(bool ActiveUser = false, bool ErrorActiveUser = false, bool ErrorIsExsistId = false)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.DistrictLevels.IgnoreQueryFilters().Where(u => u.IsDelete).AsEnumerable().Reverse().ToList();
                ViewBag.ActiveUser = ActiveUser;
                ViewBag.ErrorIsExsistId = ErrorIsExsistId;
                ViewBag.ErrorActiveUser = ErrorActiveUser;
                return View(deleted);
            }
            return View("NotAccessProvince");
        }
        //مشاهده جدول مدارک کارکنان حذف شده در سطح استان
        public IActionResult ArchiveDocumentsProvince()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.documentUploadProvinceLevels.IgnoreQueryFilters().Where(u => u.IsDelete).AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessProvince");
        }
        //مشاهده جدول مدارک کارکنان حذف شده در سطح شهرستان
        public IActionResult ArchiveDocumentsCounty()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.documentUploadCountyLevels.IgnoreQueryFilters().Where(u => u.IsDelete).AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessProvince");
        }
        //مشاهده جدول مدارک کارکنان حذف شده در سطح بخش
        public IActionResult ArchiveDocumentsDistrict()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.documentUploadDistrictLevels.IgnoreQueryFilters().Where(u => u.IsDelete).AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessProvince");
        }
        //مشاهده جدول مدارک تبادل شده کاربر حذف شده در سطح استان
        public IActionResult ArchiveExchangeDocumentsProvince()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            { 
                var deleted = _context.transferDocuments.IgnoreQueryFilters().Where(u => u.IsDelete && u.userUploaderArea == "استان").AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessProvince");
        }
        //مشاهده جدول مدارک تبادل شده کاربر حذف شده در سطح شهرستان
        public IActionResult ArchiveExchangeDocumentsCounty()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.transferDocuments.IgnoreQueryFilters().Where(u => u.IsDelete && u.userUploaderArea == "شهرستان").AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessProvince");
        }
        //مشاهده جدول مدارک تبادل شده کاربر حذف شده در سطح بخش
        public IActionResult ArchiveExchangeDocumentsDistrict()
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var deleted = _context.transferDocuments.IgnoreQueryFilters().Where(u => u.IsDelete && u.userUploaderArea == "بخش").AsEnumerable().Reverse().ToList();
                return View(deleted);
            }
            return View("NotAccessProvince");
        }
        #endregion

        #region DownloadArchiveDocuments
        //دریافت مدارک حذف شده در سطح استان
        public IActionResult DownloadDeletedDocumentsFromProvinceDb(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistDeletedUserDocumentOnProvinceDB(id) == true)
                {
                    var item = _ManagementService.GetDeletedDocumentFromProvinceDB(id);
                    byte[] bytes = item.dataBytes;
                    string contenetType = item.contentType;
                    string fileName = item.fileName;
                    return File(bytes, contenetType, fileName);
                }
                return Content("Not Found");
            }
            return View("NotAccessProvince");
        }
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
            return View("NotAccessProvince");
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
            return View("NotAccessProvince");
        }
        #endregion

        #region ShowActivationMessageUserDeleted
        //نمایش پیام فعال کردن کاربر حذف شده در سطح استان  
        public IActionResult ShowActivationMessageProvince(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistNationalCodeProvince(id) == true || _ManagementService.IsExistNationalCodeCounty(id) == true)
                {
                    return Redirect("/ProvinceArea/Management/ArchiveProvince?ErrorIsExsistId=true");
                }
                string role = _ManagementService.GetDeletedRoleWithnationalCodeProvince(id);

                if (role == "مدیریت" && _ManagementService.IsExistManagerProvince() == true)
                {
                    return Redirect("/ProvinceArea/Management/ArchiveProvince?ErrorActiveUser=true");
                }

                if (_ManagementService.IsExistDeletedNationalCodeCounty(id) && _ManagementService.IsExistDeletedNationalCodeProvince(id))
                {
                    if (_ManagementService.GetDeletedRoleWithnationalCodeCounty(id) == "مدیریت"
                        && _ManagementService.IsExistManagerCounty(_ManagementService.GetDeletedDepartmentIdWithnationalCodeCounty(id)))
                    {
                        return Redirect("/ProvinceArea/Management/ArchiveProvince?ErrorActiveUser=true");
                    }
                }

                return View(_ManagementService.GetUserDeletedBynationalCodeProvinceLevel(id));
            }
            return View("NotAccessProvince");
        }
        //نمایش پیام فعال کردن کاربر حذف شده در سطح شهرستان  
        public IActionResult ShowActivationMessageCounty(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistNationalCodeCounty(id) == true || _ManagementService.IsExistNationalCodeDistrict(id) == true)
                {
                    return Redirect("/ProvinceArea/Management/ArchiveCounty?ErrorIsExsistId=true");
                }
                string role = _ManagementService.GetDeletedRoleWithnationalCodeCounty(id);
                int departmentCounty = _ManagementService.GetDeletedDepartmentIdWithnationalCodeCounty(id);
                if (role == "مدیریت" && _ManagementService.IsExistManagerCounty(departmentCounty) == true)
                {
                    return Redirect("/ProvinceArea/Management/ArchiveCounty?ErrorActiveUser=true");
                }

                if (_ManagementService.IsExistDeletedNationalCodeDistrict(id) && _ManagementService.IsExistDeletedNationalCodeCounty(id))
                {
                    if (_ManagementService.GetDeletedRoleWithnationalCodeDistrict(id) == "مدیریت"
                    && _ManagementService.IsExistManagerDistrict(departmentCounty, _ManagementService.GetDeletedDepartmentIdWithnationalCodeDistrict(id)))
                    {
                        return Redirect("/ProvinceArea/Management/ArchiveCounty?ErrorActiveUser=true");
                    }
                }
                if (_ManagementService.IsExistDeletedNationalCodeCounty(id) == true && _ManagementService.IsExistDeletedNationalCodeProvince(id) == true)
                {
                    return Redirect("/ProvinceArea/Management/ArchiveCounty?ErrorActiveUser=true");
                }
                return View(_ManagementService.GetUserDeletedBynationalCodeCountyLevel(id));
            }
            return View("NotAccessProvince");
        }
        //نمایش پیام فعال کردن کاربر حذف شده در سطح بخش  
        public IActionResult ShowActivationMessageDistrict(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistNationalCodeDistrict(id) == true)
                {
                    return Redirect("/ProvinceArea/Management/ArchiveDistrict?ErrorIsExsistId=true");
                }
                string role = _ManagementService.GetDeletedRoleWithnationalCodeDistrict(id);
                int departmentDistrict = _ManagementService.GetDeletedDepartmentIdWithnationalCodeDistrict(id);
                int countyDistrict = _ManagementService.GetDeletedCountyIdWithnationalCodeDistrict(id);
                if (role == "مدیریت" && _ManagementService.IsExistManagerDistrict(countyDistrict, departmentDistrict) == true)
                {
                    return Redirect("/ProvinceArea/Management/ArchiveDistrict?ErrorActiveUser=true");
                }
                if (_ManagementService.IsExistDeletedNationalCodeDistrict(id) == true && _ManagementService.IsExistDeletedNationalCodeCounty(id) == true)
                {
                    return Redirect("/ProvinceArea/Management/ArchiveDistrict?ErrorActiveUser=true");
                }
                return View(_ManagementService.GetUserDeletedBynationalCodeDistrictLevel(id));
            }
            return View("NotAccessProvince");
        }
        #endregion

        #region ActivationUserDeleted
        //فعال سازی کاربر پاک شده در سطح استان
        public IActionResult ActivationUserDeletedProvince(string id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                string nationalCode = id;
                _ManagementService.ActivationDeletedUserProvince(nationalCode);
                _ManagementService.ActivationDeletedUserP(nationalCode);
                _ManagementService.ActivationDeletedUserDocumentsProvince(nationalCode);
                _ManagementService.DownloadProfileAvatarPicAfterActiveProvince(nationalCode);
                return Redirect("/ProvinceArea/Management/ArchiveProvince?ActiveUser=true");
            }
            return View("NotAccessProvince");
        }
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
                return Redirect("/ProvinceArea/Management/ArchiveCounty?ActiveUser=true");
            }
            return View("NotAccessProvince");
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
                return Redirect("/ProvinceArea/Management/ArchiveDistrict?ActiveUser=true");
            }
            return View("NotAccessProvince");
        }
        #endregion

        #region MyDocument
        //مدارک من
        public IActionResult MyDocuments()
        {
            string myNationalCode = User.Claims.FirstOrDefault(u => u.Type == "nationalCode").Value.ToString();
            var myDocuments = _context.documentUploadProvinceLevels.Where(d => d.ownerUserId == myNationalCode).ToList();
            return View(myDocuments);
        }

        //دریافت مدارک به وسیله شناسه
        public IActionResult DownloadMyDocuments(int id)
        {
            if (_ManagementService.IsExistUserDocumentOnProvinceDB(id))
            {
                var item = _ManagementService.GetDocumentFromProvinceDB(id);
                byte[] bytes = item.dataBytes;
                string contenetType = item.contentType;
                string fileName = item.fileName;
                return File(bytes, contenetType, fileName);
            }
            return Content("Not Found");
        }
        #endregion

        #region ManageRoles
        //نمایش مشخصات نقش انتخابی برای حذف
        public IActionResult ListDeleteRole(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                var role = _ManagementService.GetInformationRoleWithId(id);
                return View(role);
            }
            return View("NotAccessProvince");
        }

        //حذف نقش
        [HttpPost]
        public IActionResult DeleteRole(int roleId)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                bool result = _ManagementService.DeleteRoleWithroleId(roleId);
                return Redirect("/ProvinceArea/Management/DisplayRolesdb?DeleteRole=true");
            }
            return View("NotAccessProvince");
        }

        //نمایش مشخصات نقش پاک شده انتخابی برای فعال شدن مجدد
        public IActionResult ShowActivationMessageRole(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_context.Roles.ToList().Count() > 9)
                {
                    return Redirect("/ProvinceArea/Management/DisplayRolesdb?ErrorMax=true");
                }
                var deletedRole = _ManagementService.GetInformationDeletedRoleWithRoleId(id);
                return View(deletedRole);
            }
            return View("NotAccessProvince");
        }

        //فعال کردن نقش
        public IActionResult ActivationDeletedRole(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                bool result = _ManagementService.ActivationDeletedRoleWithroleId(id);
                return Redirect("/ProvinceArea/Management/DisplayRolesdb?ActivationDeletedRole=true");
            }
            return View("NotAccessProvince");
        }

        //اضافه کردن نقش جدید
        [HttpPost]
        public IActionResult AddNewRole(string newRole)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (newRole == null)
                {
                    return Redirect("/ProvinceArea/Management/DisplayRolesdb?ErrorEmptyNewRole=true");

                }
                if (!Regex.IsMatch(newRole, @"^[\u0600-\u06FF]+( [\u0600-\u06FF]+)*$"))
                {
                    return Redirect("/ProvinceArea/Management/DisplayRolesdb?ErrorFalseRoleValue=true");
                }
                if (_context.Roles.IgnoreQueryFilters().Any(r => r.RoleTitle == "معاونت " + newRole))
                {
                    return Redirect("/ProvinceArea/Management/DisplayRolesdb?ErrorAddNewRole=true");
                }
                if (_context.Roles.IgnoreQueryFilters().Any(r => r.RoleTitle == "مدیریت") && newRole == "مدیریت")
                {
                    return Redirect("/ProvinceArea/Management/DisplayRolesdb?ErrorAddNewRole=true");
                }
                if (_context.Roles.ToList().Count() > 9)
                {
                    return Redirect("/ProvinceArea/Management/DisplayRolesdb?ErrorMax=true");
                }
                string result = _ManagementService.AddNewRoleWithRroleTitle(newRole);
                return RedirectToAction("DisplayRolesdb", new { SuccessAddNewRole = true, newRoleTitle = result.ToString() });
            }
            return View("NotAccessProvince");
        }
        #endregion

        #region EditUserProfile
        //ویرایش حساب کاربری
        public IActionResult EditProfile(string id, bool ErrorUploadPhoto = false, bool ErrorData = false)
        {
            ViewData["Maritals"] = _ManagementService.GetMaritals();
            ViewBag.ErrorData = ErrorData;
            ViewBag.ErrorUploadPhoto = ErrorUploadPhoto;
            return View(_ManagementService.GetUserForShowInEditProfileProvince(id));
        }
        [HttpPost]
        public IActionResult EditProfile(List<int> SelectedMarital, EditProfileViewModel edit)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorData = true;
                return View(_ManagementService.GetUserForShowInEditProfileProvince(edit.nationalCode));
            }

            if (edit.avatar != null && edit.avatar.ContentType != "image/png")
            {
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorUploadPhoto = true;
                return View(_ManagementService.GetUserForShowInEditProfileProvince(edit.nationalCode));
            }

            if (SelectedMarital.Count == 0)
            {
                ViewData["Maritals"] = _ManagementService.GetMaritals();
                ViewBag.ErrorData = true;
                return View(_ManagementService.GetUserForShowInEditProfileProvince(edit.nationalCode));
            }
            string roleUploader = User.Claims.FirstOrDefault(c => c.Type == "role").Value.ToString();
            bool resultPC = _ManagementService.UploadEditProfileAvatarProvincePC(edit.avatar, edit);
            bool resultDB = _ManagementService.UploadEditProfileAvatarProvinceDB(edit.avatar, edit, roleUploader);
            _ManagementService.EditUserProfileProvince(SelectedMarital, edit, roleUploader);
            return Redirect("/ProvinceArea/Management/Profile?EditProfile=true");
        }
        #endregion

        #region Profile
        //حساب کاربری
        public IActionResult Profile(bool EditProfile = false)
        {
            ViewBag.EditProfile = EditProfile;
            return View(_ManagementService.GetProfileUserInformationBynationalCodeProvince(User.Claims.FirstOrDefault(u => u.Type == "nationalCode").Value));
        }
        #endregion

        #region DownloadDocuments
        //دریافت مدارک از پایگاه داده سطح استان
        public IActionResult DownloadDocumentsFromProvinceDB(int id)
        {
            if ((User.Claims.FirstOrDefault(c => c.Type == "role").Value == "مدیریت") || (User.Claims.FirstOrDefault(c => c.Type == "role").Value == "معاونت فناوری اطلاعات"))
            {
                if (_ManagementService.IsExistUserDocumentOnProvinceDB(id))
                {
                    var item = _ManagementService.GetDocumentFromProvinceDB(id);
                    byte[] bytes = item.dataBytes;
                    string contenetType = item.contentType;
                    string fileName = item.fileName;
                    return File(bytes, contenetType, fileName);
                }
                return Content("Not Found");
            }
            return View("NotAccessProvince");
        }
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
            return View("NotAccessProvince");
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
            return View("NotAccessProvince");
        }
        #endregion

    }
}
