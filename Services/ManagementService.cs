using ERP.Convertors;
using ERP.Data;
using ERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP.Services
{
    public class ManagementService : IManagementService
    {
        //پیاده سازی تمامی توابع سامانه در سه سطح استان - شهرستان - بخش
        #region DataBase
        private ERPContext _context;
        public ManagementService(ERPContext context)
        {
            _context = context;
        }
        #endregion

        #region Unique

        public bool IsExistNationalCodeProvince(string nationalCode)
        {
            return _context.ProvinceLevel.Any(u => u.nationalCode == nationalCode);
        }

        public bool IsExistNationalCodeCounty(string nationalCode)
        {
            return _context.CountyLevels.Any(u => u.nationalCode == nationalCode);
        }
        public bool IsExistNationalCodeWithDepartrmentCounty(string nationalCode, List<int> departmentList)
        {
            int department = 0;
            foreach (var item in departmentList)
            {
                department = item;
            }
            return _context.CountyLevels.Any(u => u.nationalCode == nationalCode && u.department == department);
        }
        public bool IsExistNationalCodeDistrict(string nationalCode)
        {
            return _context.DistrictLevels.Any(u => u.nationalCode == nationalCode);
        }
        public bool IsExistNationalCodeWithCountyAndDepartrmentDistrict(string nationalCode, List<int> countyList, List<int> departmentList)
        {
            int county = 0;
            int department = 0;
            foreach (var item in countyList)
            {
                county = item;
            }
            foreach (var item in departmentList)
            {
                department = item;
            }
            return _context.DistrictLevels.Any(u => u.nationalCode == nationalCode && u.county == county && u.department == department);
        }

        public bool IsExistDeletedNationalCodeProvince(string nationalCode)
        {
            return _context.ProvinceLevel.IgnoreQueryFilters().Any(u => u.nationalCode == nationalCode);
        }

        public bool IsExistDeletedNationalCodeCounty(string nationalCode)
        {
            return _context.CountyLevels.IgnoreQueryFilters().Any(u => u.nationalCode == nationalCode);
        }

        public bool IsExistDeletedNationalCodeDistrict(string nationalCode)
        {
            return _context.DistrictLevels.IgnoreQueryFilters().Any(u => u.nationalCode == nationalCode);
        }

        #endregion

        #region Register

        public string AddCounty(CountyLevel employee)
        {
            _context.CountyLevels.Add(employee);
            _context.SaveChanges();
            return employee.nationalCode;
        }

        public string AddDistrict(DistrictLevel employee)
        {
            _context.DistrictLevels.Add(employee);
            _context.SaveChanges();
            return employee.nationalCode;
        }

        public string AddProvince(ProvinceLevel employee)
        {
            _context.ProvinceLevel.Add(employee);
            _context.SaveChanges();
            return employee.nationalCode;
        }

        public string AddEmployees(Employees employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee.nationalCode;
        }

        #endregion

        #region Login
        public ProvinceLevel PLoginUser(LoginViewModel login)
        {
            string userName = login.nationalCode;
            string password = login.userPass;
            return _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == userName && u.userPass == password);

        }
        public ProvinceLevel PLoginUserDeleted(LoginViewModel login)
        {
            string userName = login.nationalCode;
            string password = login.userPass;
            return _context.ProvinceLevel.IgnoreQueryFilters().Where(u => u.IsDelete).SingleOrDefault(u => u.nationalCode == userName && u.userPass == password);

        }

        public CountyLevel CLoginUser(LoginViewModel login)
        {
            string userName = login.nationalCode;
            string password = login.userPass;
            return _context.CountyLevels.SingleOrDefault(u => u.nationalCode == userName && u.userPass == password);

        }

        public CountyLevel CLoginUserDeleted(LoginViewModel login)
        {
            string userName = login.nationalCode;
            string password = login.userPass;
            return _context.CountyLevels.IgnoreQueryFilters().Where(u => u.IsDelete).SingleOrDefault(u => u.nationalCode == userName && u.userPass == password);

        }

        public DistrictLevel DLoginUser(LoginViewModel login)
        {
            string userName = login.nationalCode;
            string password = login.userPass;
            return _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == userName && u.userPass == password);

        }

        public DistrictLevel DLoginUserDeleted(LoginViewModel login)
        {
            string userName = login.nationalCode;
            string password = login.userPass;
            return _context.DistrictLevels.IgnoreQueryFilters().Where(u => u.IsDelete).SingleOrDefault(u => u.nationalCode == userName && u.userPass == password);

        }

        public Employees ELoginUser(LoginViewModel login, string area)
        {
            string userName = login.nationalCode;
            string password = login.userPass;
            return _context.Employees.SingleOrDefault(u => u.nationalCode == userName && u.userPass == password && u.area == area);
        }



        #endregion

        #region GetUserForgotPasswprd
        public ProvinceLevel GetUserForgotPassProvince(ForgotPasswordViewModel forgot)
        {
            string userName = forgot.nationalCode;
            return _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == userName);
        }
        public ProvinceLevel GetUserForgotPassDeletedProvince(ForgotPasswordViewModel forgot)
        {
            string userName = forgot.nationalCode;
            return _context.ProvinceLevel.IgnoreQueryFilters().Where(u => u.IsDelete).SingleOrDefault(u => u.nationalCode == userName);
        }
        public CountyLevel GetUserForgotPassCounty(ForgotPasswordViewModel forgot)
        {
            string userName = forgot.nationalCode;
            return _context.CountyLevels.SingleOrDefault(u => u.nationalCode == userName);
        }
        public CountyLevel GetUserForgotPassDeletedCounty(ForgotPasswordViewModel forgot)
        {
            string userName = forgot.nationalCode;
            return _context.CountyLevels.IgnoreQueryFilters().Where(u => u.IsDelete).SingleOrDefault(u => u.nationalCode == userName);
        }
        public DistrictLevel GetUserForgotPassDistrict(ForgotPasswordViewModel forgot)
        {
            string userName = forgot.nationalCode;
            return _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == userName);
        }
        public DistrictLevel GetUserForgotPassDeletedDistrict(ForgotPasswordViewModel forgot)
        {
            string userName = forgot.nationalCode;
            return _context.DistrictLevels.IgnoreQueryFilters().Where(u => u.IsDelete).SingleOrDefault(u => u.nationalCode == userName);
        }
        #endregion

        #region ResetPassword
        public bool ResetPassswordProvince(ResetPassword reset)
        {
            var nationalCode = reset.nationalCode;
            ProvinceLevel user = GetUserBynationalCodeProvinceLevel(reset.nationalCode);
            user.userPass = reset.userPass;
            Employees employee = GetUserBynationalCodeP(reset.nationalCode);
            employee.userPass = reset.userPass;
            _context.ProvinceLevel.Update(user);
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return true;
        }
        public bool ResetPassswordCounty(ResetPassword reset)
        {
            var nationalCode = reset.nationalCode;
            CountyLevel user = GetUserBynationalCodeCountyLevel(reset.nationalCode);
            user.userPass = reset.userPass;
            Employees employee = GetUserBynationalCodeC(reset.nationalCode);
            employee.userPass = reset.userPass;
            _context.CountyLevels.Update(user);
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return true;
        }
        public bool ResetPassswordDistrict(ResetPassword reset)
        {
            var nationalCode = reset.nationalCode;
            DistrictLevel user = GetUserBynationalCodeDistrictLevel(reset.nationalCode);
            user.userPass = reset.userPass;
            Employees employee = GetUserBynationalCodeD(reset.nationalCode);
            employee.userPass = reset.userPass;
            _context.DistrictLevels.Update(user);
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return true;
        }

        #endregion

        #region GetRoleTitleWithRoleId 
        public string GetRoleTitleWithRoleId(int roleId)
        {
            return _context.Roles.SingleOrDefault(r => r.RoleId == roleId).RoleTitle.ToString();
        }
        public string GetGenderTitleWithGenderId(int genderId)
        {
            return _context.Genders.SingleOrDefault(g => g.genderId == genderId).genderTitle.ToString();
        }
        public string GetMaritalTitleWithMaritalId(int maritalId)
        {
            return _context.Maritals.SingleOrDefault(m => m.maritalId == maritalId).maritalTitle.ToString();
        }
        public string GetEmployentTitleWithEmployentId(int employentId)
        {
            return _context.Employments.SingleOrDefault(e => e.EmploymentId == employentId).EmploymentTitle.ToString();
        }
        #endregion

        #region GetRoleIdWithRoleTitle
        public int GetRoleIdWithRoleTitleProvince(string nationalCode)
        {
            string roleTitle = _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == nationalCode).role.ToString();
            return _context.Roles.SingleOrDefault(r => r.RoleTitle == roleTitle).RoleId;
        }
        public int GetRoleIdWithRoleTitleCounty(string nationalCode)
        {
            string roleTitle = _context.CountyLevels.SingleOrDefault(u => u.nationalCode == nationalCode).role.ToString();
            return _context.Roles.SingleOrDefault(r => r.RoleTitle == roleTitle).RoleId;
        }
        public int GetRoleIdWithRoleTitleDistrict(string nationalCode)
        {
            string roleTitle = _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode).role.ToString();
            return _context.Roles.SingleOrDefault(r => r.RoleTitle == roleTitle).RoleId;
        }
        #endregion

        #region GetDepartmentIdWithnationalCode
        public int GetDepartmentIdWithnationalCodeProvince(string nationalCode)
        {
            return _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == nationalCode).department;
        }
        public int GetDepartmentIdWithnationalCodeCounty(string nationalCode)
        {
            return _context.CountyLevels.SingleOrDefault(u => u.nationalCode == nationalCode).department;
        }
        public int GetDepartmentIdWithnationalCodeDistrict(string nationalCode)
        {
            return _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode).department;
        }
        public int GetDeletedDepartmentIdWithnationalCodeProvince(string nationalCode)
        {
            return _context.ProvinceLevel.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).department;
        }
        public int GetDeletedDepartmentIdWithnationalCodeCounty(string nationalCode)
        {
            return _context.CountyLevels.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).department;
        }
        public int GetDeletedDepartmentIdWithnationalCodeDistrict(string nationalCode)
        {
            return _context.DistrictLevels.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).department;
        }
        #endregion

        #region GetGenderIdWithGenderTitle
        public int GetGenderIdWithGenderTitleProvince(string nationalCode)
        {
            string genderTitle = _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == nationalCode).gender.ToString();
            return _context.Genders.SingleOrDefault(g => g.genderTitle == genderTitle).genderId;
        }
        public int GetGenderIdWithGenderTitleCounty(string nationalCode)
        {
            string genderTitle = _context.CountyLevels.SingleOrDefault(u => u.nationalCode == nationalCode).gender.ToString();
            return _context.Genders.SingleOrDefault(g => g.genderTitle == genderTitle).genderId;
        }
        public int GetGenderIdWithGenderTitleDistrict(string nationalCode)
        {
            string genderTitle = _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode).gender.ToString();
            return _context.Genders.SingleOrDefault(g => g.genderTitle == genderTitle).genderId;
        }
        #endregion

        #region GetMaritalIdWithMaritalTitle
        public int GetMaritalIdWithMaritalTitleProvince(string nationalCode)
        {
            string maritalTitle = _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == nationalCode).maritalStatus.ToString();
            return _context.Maritals.SingleOrDefault(m => m.maritalTitle == maritalTitle).maritalId;
        }
        public int GetMaritalIdWithMaritalTitleCounty(string nationalCode)
        {
            string maritalTitle = _context.CountyLevels.SingleOrDefault(u => u.nationalCode == nationalCode).maritalStatus.ToString();
            return _context.Maritals.SingleOrDefault(m => m.maritalTitle == maritalTitle).maritalId;
        }
        public int GetMaritalIdWithMaritalTitleDistrict(string nationalCode)
        {
            string maritalTitle = _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode).maritalStatus.ToString();
            return _context.Maritals.SingleOrDefault(m => m.maritalTitle == maritalTitle).maritalId;
        }
        #endregion

        #region GetEmploymentIdWithEmploymentTitle
        public int GetEmploymentIdWithEmploymentTitleProvince(string nationalCode)
        {
            string emplymentTitle = _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == nationalCode).employmentStatus.ToString();
            return _context.Employments.SingleOrDefault(e => e.EmploymentTitle == emplymentTitle).EmploymentId;
        }
        public int GetEmploymentIdWithEmploymentTitleCounty(string nationalCode)
        {
            string emplymentTitle = _context.CountyLevels.SingleOrDefault(u => u.nationalCode == nationalCode).employmentStatus.ToString();
            return _context.Employments.SingleOrDefault(e => e.EmploymentTitle == emplymentTitle).EmploymentId;
        }
        public int GetEmploymentIdWithEmploymentTitleDistrict(string nationalCode)
        {
            string emplymentTitle = _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode).employmentStatus.ToString();
            return _context.Employments.SingleOrDefault(e => e.EmploymentTitle == emplymentTitle).EmploymentId;
        }
        #endregion

        #region GetUserByUserName
        public Employees GetUserByUserName(int userId)
        {
            return _context.Employees.SingleOrDefault(u => u.userId == userId);
        }
        public ProvinceLevel GetUserByUserNameProvinceLevel(int userId)
        {
            return _context.ProvinceLevel.SingleOrDefault(u => u.userId == userId);
        }
        public CountyLevel GetUserByUserNameCountyLevel(int userId)
        {
            return _context.CountyLevels.SingleOrDefault(u => u.userId == userId);
        }
        public DistrictLevel GetUserByUserNameDistrictLevel(int userId)
        {
            return _context.DistrictLevels.SingleOrDefault(u => u.userId == userId);
        }
        #endregion

        #region GetUserBynationalCode
        public Employees GetUserBynationalCode(string nationalCode)
        {
            return _context.Employees.SingleOrDefault(u => u.nationalCode == nationalCode);
        }
        public Employees GetUserBynationalCodeP(string nationalCode)
        {
            return _context.Employees.SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "استان");
        }
        public Employees GetUserBynationalCodeC(string nationalCode)
        {
            return _context.Employees.SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "شهرستان");
        }
        public Employees GetUserBynationalCodeD(string nationalCode)
        {
            return _context.Employees.SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "بخش");
        }

        public ProvinceLevel GetUserBynationalCodeProvinceLevel(string nationalCode)
        {
            return _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == nationalCode);
        }
        public CountyLevel GetUserBynationalCodeCountyLevel(string nationalCode)
        {
            return _context.CountyLevels.SingleOrDefault(u => u.nationalCode == nationalCode);
        }
        public DistrictLevel GetUserBynationalCodeDistrictLevel(string nationalCode)
        {
            return _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode);
        }
        #endregion

        #region Register&Roles
        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public bool AddUserAndRolesProvince(List<int> roleId, List<int> genderId, List<int> employmentId, List<int> maritalId, RegisterViewModel register, string roleUploader)
        {
            int role = 0;
            int gender = 0;
            int employment = 0;
            int marital = 0;
            foreach (int item in roleId)
            {
                role = item;

            };
            foreach (int item in genderId)
            {
                gender = item;
            };
            foreach (int item in employmentId)
            {
                employment = item;

            };
            foreach (int item in maritalId)
            {
                marital = item;
            };
            ProvinceLevel employee = new ProvinceLevel()
            {
                userId = register.userId,
                nationalCode = register.nationalCode,
                userPass = register.userPass,
                role = GetRoleTitleWithRoleId(role),
                department = 1,
                fName = register.fName,
                lName = register.lName,
                tel = register.tel,
                gender = GetGenderTitleWithGenderId(gender),
                address = register.address,
                maritalStatus = GetMaritalTitleWithMaritalId(marital),
                employmentStatus = GetEmployentTitleWithEmployentId(employment),
                workingStatus = register.workingStatus,
                education = register.education,
                dateOfBirth = register.dateOfBirth,
                RegisterDate = DateTime.Now

            };
            Employees employees = new Employees()
            {
                userId = register.userId,
                nationalCode = register.nationalCode,
                userPass = register.userPass,
                role = GetRoleTitleWithRoleId(role),
                area = "استان",
                department = 1,
                fName = register.fName,
                lName = register.lName,
                tel = register.tel,
                gender = GetGenderTitleWithGenderId(gender),
                address = register.address,
                maritalStatus = GetMaritalTitleWithMaritalId(marital),
                employmentStatus = GetEmployentTitleWithEmployentId(employment),
                workingStatus = register.workingStatus,
                education = register.education,
                dateOfBirth = register.dateOfBirth,
                country = 0,
                RegisterDate = DateTime.Now

            };
            AddProvince(employee);
            AddEmployees(employees);
            _context.SaveChanges();
            return true;
        }

        public bool AddUserAndRolesCounty(List<int> roleId, List<int> genderId, List<int> departmentId, List<int> employmentId, List<int> maritalId, RegisterViewModel register, string roleUploader)
        {
            int role = 0;
            int gender = 0;
            int department = 0;
            int employment = 0;
            int marital = 0;
            foreach (int item in roleId)
            {
                role = item;

            };
            foreach (int item in genderId)
            {
                gender = item;
            };
            foreach (int item in departmentId)
            {
                department = item;
            };
            foreach (int item in employmentId)
            {
                employment = item;

            };
            foreach (int item in maritalId)
            {
                marital = item;
            };

            if (role == 1)
            {
                int managerCountyLevelRole = 7;
                ProvinceLevel managerCountylevel = new ProvinceLevel()
                {
                    userId = register.userId,
                    nationalCode = register.nationalCode,
                    userPass = register.userPass,
                    role = GetRoleTitleWithRoleId(managerCountyLevelRole),
                    department = 1,
                    fName = register.fName,
                    lName = register.lName,
                    tel = register.tel,
                    gender = GetGenderTitleWithGenderId(gender),
                    address = register.address,
                    maritalStatus = GetMaritalTitleWithMaritalId(marital),
                    employmentStatus = GetEmployentTitleWithEmployentId(employment),
                    workingStatus = register.workingStatus,
                    education = register.education,
                    dateOfBirth = register.dateOfBirth,
                    RegisterDate = DateTime.Now
                };
                Employees managerCountylevelEmployees = new Employees()
                {
                    userId = register.userId,
                    nationalCode = register.nationalCode,
                    userPass = register.userPass,
                    role = GetRoleTitleWithRoleId(managerCountyLevelRole),
                    area = "استان",
                    department = 1,
                    fName = register.fName,
                    lName = register.lName,
                    tel = register.tel,
                    gender = GetGenderTitleWithGenderId(gender),
                    address = register.address,
                    maritalStatus = GetMaritalTitleWithMaritalId(marital),
                    employmentStatus = GetEmployentTitleWithEmployentId(employment),
                    workingStatus = register.workingStatus,
                    education = register.education,
                    dateOfBirth = register.dateOfBirth,
                    RegisterDate = DateTime.Now

                };
                AddProvince(managerCountylevel);
                AddEmployees(managerCountylevelEmployees);
                bool resultPC = UploadAvatarProvincePC(register.avatar, register);
                bool resultDB = UploadAvatarProvinceDB(register.avatar, register, roleUploader);
            };

            CountyLevel employee = new CountyLevel()
            {
                userId = register.userId,
                nationalCode = register.nationalCode,
                userPass = register.userPass,
                role = GetRoleTitleWithRoleId(role),
                department = department,
                fName = register.fName,
                lName = register.lName,
                tel = register.tel,
                gender = GetGenderTitleWithGenderId(gender),
                address = register.address,
                maritalStatus = GetMaritalTitleWithMaritalId(marital),
                employmentStatus = GetEmployentTitleWithEmployentId(employment),
                workingStatus = register.workingStatus,
                education = register.education,
                dateOfBirth = register.dateOfBirth,
                RegisterDate = DateTime.Now

            };
            Employees employees = new Employees()
            {
                userId = register.userId,
                nationalCode = register.nationalCode,
                userPass = register.userPass,
                role = GetRoleTitleWithRoleId(role),
                area = "شهرستان",
                department = department,
                fName = register.fName,
                lName = register.lName,
                tel = register.tel,
                gender = GetGenderTitleWithGenderId(gender),
                address = register.address,
                maritalStatus = GetMaritalTitleWithMaritalId(marital),
                employmentStatus = GetEmployentTitleWithEmployentId(employment),
                workingStatus = register.workingStatus,
                education = register.education,
                dateOfBirth = register.dateOfBirth,
                country = 0,
                RegisterDate = DateTime.Now

            };
            AddCounty(employee);
            AddEmployees(employees);
            _context.SaveChanges();
            return true;
        }

        public bool AddUserAndRolesDistrict(List<int> roleId, List<int> genderId, List<int> departmentId, List<int> employmentId, List<int> maritalId, List<int> countyId, RegisterViewModel register, string roleUploader)
        {
            int role = 0;
            int gender = 0;
            int department = 0;
            int employment = 0;
            int marital = 0;
            int county = 0;
            foreach (int item in roleId)
            {
                role = item;

            };
            foreach (int item in genderId)
            {
                gender = item;
            };
            foreach (int item in departmentId)
            {
                department = item;
            };
            foreach (int item in employmentId)
            {
                employment = item;

            };
            foreach (int item in maritalId)
            {
                marital = item;
            };
            foreach (int item in countyId)
            {
                county = item;
            };
            if (role == 1)
            {
                int managerCountyLevelRole = 7;
                CountyLevel managerDistrictlevel = new CountyLevel()
                {
                    userId = register.userId,
                    nationalCode = register.nationalCode,
                    userPass = register.userPass,
                    role = GetRoleTitleWithRoleId(managerCountyLevelRole),
                    department = county,
                    fName = register.fName,
                    lName = register.lName,
                    tel = register.tel,
                    gender = GetGenderTitleWithGenderId(gender),
                    address = register.address,
                    maritalStatus = GetMaritalTitleWithMaritalId(marital),
                    employmentStatus = GetEmployentTitleWithEmployentId(employment),
                    workingStatus = register.workingStatus,
                    education = register.education,
                    dateOfBirth = register.dateOfBirth,
                    RegisterDate = DateTime.Now
                };
                Employees managerDistrictlevelEmployees = new Employees()
                {
                    userId = register.userId,
                    nationalCode = register.nationalCode,
                    userPass = register.userPass,
                    role = GetRoleTitleWithRoleId(managerCountyLevelRole),
                    area = "شهرستان",
                    department = county,
                    fName = register.fName,
                    lName = register.lName,
                    tel = register.tel,
                    gender = GetGenderTitleWithGenderId(gender),
                    address = register.address,
                    maritalStatus = GetMaritalTitleWithMaritalId(marital),
                    employmentStatus = GetEmployentTitleWithEmployentId(employment),
                    workingStatus = register.workingStatus,
                    education = register.education,
                    dateOfBirth = register.dateOfBirth,
                    RegisterDate = DateTime.Now
                };
                AddCounty(managerDistrictlevel);
                AddEmployees(managerDistrictlevelEmployees);
                bool resultPC = UploadAvatarCountyPC(register.avatar, register);
                bool resultDB = UploadAvatarCountyDB(register.avatar, register, countyId, roleUploader);
            }

            DistrictLevel employee = new DistrictLevel()
            {
                userId = register.userId,
                nationalCode = register.nationalCode,
                userPass = register.userPass,
                role = GetRoleTitleWithRoleId(role),
                department = department,
                fName = register.fName,
                lName = register.lName,
                tel = register.tel,
                gender = GetGenderTitleWithGenderId(gender),
                county = county,
                address = register.address,
                maritalStatus = GetMaritalTitleWithMaritalId(marital),
                employmentStatus = GetEmployentTitleWithEmployentId(employment),
                workingStatus = register.workingStatus,
                education = register.education,
                dateOfBirth = register.dateOfBirth,
                RegisterDate = DateTime.Now

            };
            Employees employees = new Employees()
            {
                userId = register.userId,
                nationalCode = register.nationalCode,
                userPass = register.userPass,
                role = GetRoleTitleWithRoleId(role),
                area = "بخش",
                department = department,
                fName = register.fName,
                lName = register.lName,
                tel = register.tel,
                gender = GetGenderTitleWithGenderId(gender),
                address = register.address,
                maritalStatus = GetMaritalTitleWithMaritalId(marital),
                employmentStatus = GetEmployentTitleWithEmployentId(employment),
                workingStatus = register.workingStatus,
                education = register.education,
                dateOfBirth = register.dateOfBirth,
                country = county,
                RegisterDate = DateTime.Now
            };
            AddDistrict(employee);
            AddEmployees(employees);
            _context.SaveChanges();
            return true;
        }


        #endregion

        #region Genders
        public List<Gender> GetGenders()
        {
            return _context.Genders.ToList();
        }
        #endregion

        #region Employments
        public List<Employment> GetEmployments()
        {
            return _context.Employments.ToList();
        }
        #endregion

        #region Martitals
        public List<Marital> GetMaritals()
        {
            return _context.Maritals.ToList();
        }
        #endregion

        #region Departments
        public List<Department> GetDepartments()
        {
            return _context.Departments.ToList();
        }
        #endregion

        #region Counties
        public List<County> GetCounties()
        {
            return _context.Counties.ToList();
        }
        #endregion

        #region Edit
        public EditViewModel GetUserForShowInEditProvince(string nationalCode)
        {
            return _context.ProvinceLevel.Where(u => u.nationalCode == nationalCode)
                .Select(u => new EditViewModel()
                {
                    userId = u.userId,
                    nationalCode = u.nationalCode,
                    fName = u.fName,
                    lName = u.lName,
                    userGender = GetGenderIdWithGenderTitleProvince(nationalCode),
                    tel = u.tel,
                    department = u.department,
                    maritalStatus = GetMaritalIdWithMaritalTitleProvince(nationalCode),
                    employmentStatus = GetEmploymentIdWithEmploymentTitleProvince(nationalCode),
                    address = u.address,
                    workingStatus = u.workingStatus,
                    education = u.education,
                    dateOfBirth = u.dateOfBirth,
                    userRole = GetRoleIdWithRoleTitleProvince(nationalCode)
                }).Single();
        }

        public bool EditUserAndRolesProvince(int roleId, List<int> employmentId, List<int> maritalId, List<int> genderId, EditViewModel edit, string roleUploader)
        {
            int role = roleId;
            int gender = 0;
            int employment = 0;
            int marital = 0;
            foreach (int item in genderId)
            {
                gender = item;
            };
            foreach (int item in employmentId)
            {
                employment = item;

            };
            foreach (int item in maritalId)
            {
                marital = item;
            };

            if (IsExistNationalCodeProvince(edit.nationalCode) && IsExistNationalCodeCounty(edit.nationalCode))
            {
                CountyLevel managerCountylevel = GetUserBynationalCodeCountyLevel(edit.nationalCode);
                managerCountylevel.nationalCode = edit.nationalCode;
                managerCountylevel.fName = edit.fName;
                managerCountylevel.lName = edit.lName;
                managerCountylevel.gender = GetGenderTitleWithGenderId(gender);
                managerCountylevel.employmentStatus = GetEmployentTitleWithEmployentId(employment);
                managerCountylevel.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerCountylevel.role = GetRoleTitleWithRoleId(1);
                managerCountylevel.dateOfBirth = edit.dateOfBirth;
                managerCountylevel.tel = edit.tel;
                managerCountylevel.department = GetDepartmentIdWithnationalCodeCounty(edit.nationalCode);
                managerCountylevel.education = edit.education;
                managerCountylevel.workingStatus = edit.workingStatus;
                managerCountylevel.address = edit.address;
                Employees managerCountylevelEmployees = GetUserBynationalCodeC(edit.nationalCode);
                managerCountylevelEmployees.nationalCode = edit.nationalCode;
                managerCountylevelEmployees.fName = edit.fName;
                managerCountylevelEmployees.lName = edit.lName;
                managerCountylevelEmployees.gender = GetGenderTitleWithGenderId(gender);
                managerCountylevelEmployees.employmentStatus = GetEmployentTitleWithEmployentId(employment);
                managerCountylevelEmployees.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerCountylevelEmployees.role = GetRoleTitleWithRoleId(1);
                managerCountylevelEmployees.dateOfBirth = edit.dateOfBirth;
                managerCountylevelEmployees.tel = edit.tel;
                managerCountylevelEmployees.education = edit.education;
                managerCountylevelEmployees.workingStatus = edit.workingStatus;
                managerCountylevelEmployees.address = edit.address;
                managerCountylevelEmployees.department = GetDepartmentIdWithnationalCodeCounty(edit.nationalCode);
                managerCountylevelEmployees.country = 0;
                managerCountylevelEmployees.area = "شهرستان";
                if (!string.IsNullOrEmpty(edit.userPass))
                {
                    managerCountylevel.userPass = edit.userPass;
                    managerCountylevelEmployees.userPass = edit.userPass;
                }
                int departmentId = GetDepartmentIdWithnationalCodeCounty(edit.nationalCode);
                bool resultPC = UploadEditAvatarCountyPC(edit.avatar, edit);
                bool resultDB = UploadEditAvatarCountyDB(edit.avatar, edit, departmentId, role, roleUploader);
                _context.CountyLevels.Update(managerCountylevel);
                _context.Employees.Update(managerCountylevelEmployees);
            }

            ProvinceLevel user = GetUserBynationalCodeProvinceLevel(edit.nationalCode);
            user.nationalCode = edit.nationalCode;
            user.fName = edit.fName;
            user.lName = edit.lName;
            user.gender = GetGenderTitleWithGenderId(gender);
            user.employmentStatus = GetEmployentTitleWithEmployentId(employment);
            user.department = 1;
            user.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            user.role = GetRoleTitleWithRoleId(role);
            user.dateOfBirth = edit.dateOfBirth;
            user.tel = edit.tel;
            user.education = edit.education;
            user.workingStatus = edit.workingStatus;
            user.address = edit.address;
            Employees employee = GetUserBynationalCodeP(edit.nationalCode);
            employee.area = "استان";
            employee.nationalCode = edit.nationalCode;
            employee.fName = edit.fName;
            employee.lName = edit.lName;
            employee.gender = GetGenderTitleWithGenderId(gender);
            employee.employmentStatus = GetEmployentTitleWithEmployentId(employment);
            employee.department = 1;
            employee.education = edit.education;
            employee.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            employee.role = GetRoleTitleWithRoleId(role);
            employee.dateOfBirth = edit.dateOfBirth;
            employee.tel = edit.tel;
            employee.workingStatus = edit.workingStatus;
            employee.address = edit.address;
            employee.country = 0;
            if (!string.IsNullOrEmpty(edit.userPass))
            {
                user.userPass = edit.userPass;
                employee.userPass = edit.userPass;
            }
            _context.ProvinceLevel.Update(user);
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return true;
        }
        public EditViewModel GetUserForShowInEditCounty(string nationalCode)
        {
            return _context.CountyLevels.Where(u => u.nationalCode == nationalCode)
                .Select(u => new EditViewModel()
                {
                    userId = u.userId,
                    nationalCode = u.nationalCode,
                    fName = u.fName,
                    lName = u.lName,
                    userGender = GetGenderIdWithGenderTitleCounty(nationalCode),
                    tel = u.tel,
                    department = u.department,
                    maritalStatus = GetMaritalIdWithMaritalTitleCounty(nationalCode),
                    employmentStatus = GetEmploymentIdWithEmploymentTitleCounty(nationalCode),
                    address = u.address,
                    education = u.education,
                    dateOfBirth = u.dateOfBirth,
                    workingStatus = u.workingStatus,
                    userRole = GetRoleIdWithRoleTitleCounty(nationalCode)
                }).Single();
        }

        public bool EditUserAndRolesCounty(int roleId, List<int> employmentId, List<int> maritalId, int departmentId, List<int> genderId, EditViewModel edit, string roleUploader)
        {
            int role = roleId;
            int department = departmentId;
            int employment = 0;
            int gender = 0;
            int marital = 0;
            foreach (int item in genderId)
            {
                gender = item;
            };
            foreach (int item in employmentId)
            {
                employment = item;

            };
            foreach (int item in maritalId)
            {
                marital = item;
            };

            if (role == 1 && !IsExistNationalCodeProvince(edit.nationalCode))
            {
                ProvinceLevel employeeProvince = new ProvinceLevel()
                {
                    nationalCode = edit.nationalCode,
                    userPass = _context.CountyLevels.SingleOrDefault(u => u.nationalCode == edit.nationalCode).userPass,
                    role = GetRoleTitleWithRoleId(7),
                    department = 1,
                    fName = edit.fName,
                    lName = edit.lName,
                    tel = edit.tel,
                    gender = GetGenderTitleWithGenderId(gender),
                    address = edit.address,
                    maritalStatus = GetMaritalTitleWithMaritalId(marital),
                    employmentStatus = GetEmployentTitleWithEmployentId(employment),
                    workingStatus = edit.workingStatus,
                    education = edit.education,
                    dateOfBirth = edit.dateOfBirth,
                    RegisterDate = DateTime.Now
                };

                Employees employees = new Employees()
                {
                    nationalCode = edit.nationalCode,
                    userPass = _context.CountyLevels.SingleOrDefault(u => u.nationalCode == edit.nationalCode).userPass,
                    role = GetRoleTitleWithRoleId(7),
                    area = "استان",
                    department = 1,
                    fName = edit.fName,
                    lName = edit.lName,
                    tel = edit.tel,
                    gender = GetGenderTitleWithGenderId(gender),
                    address = edit.address,
                    maritalStatus = GetMaritalTitleWithMaritalId(marital),
                    employmentStatus = GetEmployentTitleWithEmployentId(employment),
                    workingStatus = edit.workingStatus,
                    education = edit.education,
                    dateOfBirth = edit.dateOfBirth,
                    country = 0,
                    RegisterDate = DateTime.Now
                };
                AddProvince(employeeProvince);
                AddEmployees(employees);
                bool resultPC_1 = UploadEditAvatarProvincePC(edit.avatar, edit);
                bool resultPC_2 = UploadEditAvatarCountyPC(edit.avatar, edit);
                bool resultDB = UploadEditAvatarProvinceDB(edit.avatar, edit, 7, roleUploader);
            }

            if (IsExistNationalCodeProvince(edit.nationalCode) && IsExistNationalCodeCounty(edit.nationalCode))
            {
                ProvinceLevel managerCountylevel = GetUserBynationalCodeProvinceLevel(edit.nationalCode);
                managerCountylevel.nationalCode = edit.nationalCode;
                managerCountylevel.fName = edit.fName;
                managerCountylevel.lName = edit.lName;
                managerCountylevel.gender = GetGenderTitleWithGenderId(gender);
                managerCountylevel.employmentStatus = GetEmployentTitleWithEmployentId(employment);
                managerCountylevel.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerCountylevel.role = GetRoleTitleWithRoleId(7);
                managerCountylevel.dateOfBirth = edit.dateOfBirth;
                managerCountylevel.tel = edit.tel;
                managerCountylevel.education = edit.education;
                managerCountylevel.workingStatus = edit.workingStatus;
                managerCountylevel.address = edit.address;
                managerCountylevel.department = 1;
                Employees managerCountylevelEmployees = GetUserBynationalCodeP(edit.nationalCode);
                managerCountylevelEmployees.nationalCode = edit.nationalCode;
                managerCountylevelEmployees.fName = edit.fName;
                managerCountylevelEmployees.lName = edit.lName;
                managerCountylevelEmployees.gender = GetGenderTitleWithGenderId(gender);
                managerCountylevelEmployees.employmentStatus = GetEmployentTitleWithEmployentId(employment);
                managerCountylevelEmployees.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerCountylevelEmployees.role = GetRoleTitleWithRoleId(7);
                managerCountylevelEmployees.dateOfBirth = edit.dateOfBirth;
                managerCountylevelEmployees.tel = edit.tel;
                managerCountylevelEmployees.education = edit.education;
                managerCountylevelEmployees.workingStatus = edit.workingStatus;
                managerCountylevelEmployees.address = edit.address;
                managerCountylevelEmployees.department = 1;
                managerCountylevelEmployees.country = 0;
                managerCountylevelEmployees.area = "استان";
                if (!string.IsNullOrEmpty(edit.userPass))
                {
                    managerCountylevel.userPass = edit.userPass;
                    managerCountylevelEmployees.userPass = edit.userPass;
                }
                bool resultPC_1 = UploadEditAvatarProvincePC(edit.avatar, edit);
                bool resultPc_2 = UploadEditAvatarCountyPC(edit.avatar, edit);
                bool resultDB = UploadEditAvatarProvinceDB(edit.avatar, edit, 7, roleUploader);
                _context.ProvinceLevel.Update(managerCountylevel);
                _context.Employees.Update(managerCountylevelEmployees);
            }

            if (IsExistNationalCodeDistrict(edit.nationalCode) == true && IsExistNationalCodeCounty(edit.nationalCode) == true)
            {
                DistrictLevel managerDistrictlevel = GetUserBynationalCodeDistrictLevel(edit.nationalCode);
                managerDistrictlevel.nationalCode = edit.nationalCode;
                managerDistrictlevel.fName = edit.fName;
                managerDistrictlevel.lName = edit.lName;
                managerDistrictlevel.gender = GetGenderTitleWithGenderId(gender);
                managerDistrictlevel.employmentStatus = GetEmployentTitleWithEmployentId(employment);
                managerDistrictlevel.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerDistrictlevel.role = GetRoleTitleWithRoleId(1);
                managerDistrictlevel.dateOfBirth = edit.dateOfBirth;
                managerDistrictlevel.tel = edit.tel;
                managerDistrictlevel.education = edit.education;
                managerDistrictlevel.county = department;
                managerDistrictlevel.workingStatus = edit.workingStatus;
                managerDistrictlevel.department = GetDepartmentIdWithnationalCodeDistrict(edit.nationalCode);
                managerDistrictlevel.address = edit.address;
                Employees managerDistrictlevelEmployees = GetUserBynationalCodeD(edit.nationalCode);
                managerDistrictlevelEmployees.nationalCode = edit.nationalCode;
                managerDistrictlevelEmployees.fName = edit.fName;
                managerDistrictlevelEmployees.lName = edit.lName;
                managerDistrictlevelEmployees.gender = GetGenderTitleWithGenderId(gender);
                managerDistrictlevelEmployees.employmentStatus = GetEmployentTitleWithEmployentId(employment);
                managerDistrictlevelEmployees.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerDistrictlevelEmployees.role = GetRoleTitleWithRoleId(1);
                managerDistrictlevelEmployees.dateOfBirth = edit.dateOfBirth;
                managerDistrictlevelEmployees.tel = edit.tel;
                managerDistrictlevelEmployees.education = edit.education;
                managerDistrictlevelEmployees.workingStatus = edit.workingStatus;
                managerDistrictlevelEmployees.address = edit.address;
                managerDistrictlevelEmployees.department = GetDepartmentIdWithnationalCodeDistrict(edit.nationalCode);
                managerDistrictlevelEmployees.country = department;
                managerDistrictlevelEmployees.area = "بخش";
                if (!string.IsNullOrEmpty(edit.userPass))
                {
                    managerDistrictlevel.userPass = edit.userPass;
                    managerDistrictlevelEmployees.userPass = edit.userPass;
                }
                int departmenIdDisrict = GetDepartmentIdWithnationalCodeDistrict(edit.nationalCode);
                int countyIdDistrict = GetCountyIdWithnationalCodeDistrict(edit.nationalCode);
                bool resultPC = UploadEditAvatarDistrictPC(edit.avatar, edit);
                bool resultDB = UploadEditAvatarDistrictDB(edit.avatar, edit, department, countyIdDistrict, roleUploader);
            }

            CountyLevel user = GetUserBynationalCodeCountyLevel(edit.nationalCode);
            user.nationalCode = edit.nationalCode;
            user.fName = edit.fName;
            user.lName = edit.lName;
            user.gender = GetGenderTitleWithGenderId(gender);
            user.employmentStatus = GetEmployentTitleWithEmployentId(employment);
            user.department = department;
            user.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            user.role = GetRoleTitleWithRoleId(role);
            user.dateOfBirth = edit.dateOfBirth;
            user.tel = edit.tel;
            user.education = edit.education;
            user.workingStatus = edit.workingStatus;
            user.address = edit.address;
            Employees employee = GetUserBynationalCodeC(edit.nationalCode);
            employee.area = "شهرستان";
            employee.nationalCode = edit.nationalCode;
            employee.fName = edit.fName;
            employee.lName = edit.lName;
            employee.gender = GetGenderTitleWithGenderId(gender);
            employee.employmentStatus = GetEmployentTitleWithEmployentId(employment);
            employee.department = department;
            employee.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            employee.role = GetRoleTitleWithRoleId(role);
            employee.dateOfBirth = edit.dateOfBirth;
            employee.tel = edit.tel;
            employee.education = edit.education;
            employee.workingStatus = edit.workingStatus;
            employee.address = edit.address;
            employee.country = 0;
            if (!string.IsNullOrEmpty(edit.userPass))
            {
                user.userPass = edit.userPass;
                employee.userPass = edit.userPass;
            }
            _context.CountyLevels.Update(user);
            _context.Employees.Update(employee);

            _context.SaveChanges();
            return true;
        }
        public EditViewModel GetUserForShowInEditDistrict(string nationalCode)
        {
            return _context.DistrictLevels.Where(u => u.nationalCode == nationalCode)
                .Select(u => new EditViewModel()
                {
                    userId = u.userId,
                    nationalCode = u.nationalCode,
                    fName = u.fName,
                    lName = u.lName,
                    county = u.county,
                    userGender = GetGenderIdWithGenderTitleDistrict(nationalCode),
                    tel = u.tel,
                    department = u.department,
                    maritalStatus = GetMaritalIdWithMaritalTitleDistrict(nationalCode),
                    employmentStatus = GetEmploymentIdWithEmploymentTitleDistrict(nationalCode),
                    address = u.address,
                    education = u.education,
                    dateOfBirth = u.dateOfBirth,
                    workingStatus = u.workingStatus,
                    userRole = GetRoleIdWithRoleTitleDistrict(nationalCode)
                }).Single();
        }

        public bool EditUserAndRolesDistrict(int roleId, List<int> employmentId, List<int> maritalId, int departmentId, List<int> genderId, int countyId, EditViewModel edit, string roleUploader)
        {
            int role = roleId;
            int gender = 0;
            int department = departmentId;
            int employment = 0;
            int marital = 0;
            int county = countyId;
            foreach (int item in genderId)
            {
                gender = item;
            };
            foreach (int item in employmentId)
            {
                employment = item;

            };
            foreach (int item in maritalId)
            {
                marital = item;
            };
            if (role == 1 && !IsExistNationalCodeCounty(edit.nationalCode))
            {
                CountyLevel employeeProvince = new CountyLevel()
                {
                    nationalCode = edit.nationalCode,
                    userPass = _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == edit.nationalCode).userPass,
                    role = GetRoleTitleWithRoleId(7),
                    department = county,
                    fName = edit.fName,
                    lName = edit.lName,
                    tel = edit.tel,
                    gender = GetGenderTitleWithGenderId(gender),
                    address = edit.address,
                    maritalStatus = GetMaritalTitleWithMaritalId(marital),
                    employmentStatus = GetEmployentTitleWithEmployentId(employment),
                    workingStatus = edit.workingStatus,
                    education = edit.education,
                    dateOfBirth = edit.dateOfBirth,
                    RegisterDate = DateTime.Now
                };

                Employees employees = new Employees()
                {
                    nationalCode = edit.nationalCode,
                    userPass = _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == edit.nationalCode).userPass,
                    role = GetRoleTitleWithRoleId(7),
                    area = "شهرستان",
                    department = county,
                    fName = edit.fName,
                    lName = edit.lName,
                    tel = edit.tel,
                    gender = GetGenderTitleWithGenderId(gender),
                    address = edit.address,
                    maritalStatus = GetMaritalTitleWithMaritalId(marital),
                    employmentStatus = GetEmployentTitleWithEmployentId(employment),
                    workingStatus = edit.workingStatus,
                    education = edit.education,
                    dateOfBirth = edit.dateOfBirth,
                    country = 0,
                    RegisterDate = DateTime.Now
                };
                AddCounty(employeeProvince);
                AddEmployees(employees);
                bool resultPC = UploadEditAvatarCountyPC(edit.avatar, edit);
                bool resultDB = UploadEditAvatarCountyDB(edit.avatar, edit, department, role, roleUploader);

            }

            if (IsExistNationalCodeDistrict(edit.nationalCode) == true && IsExistNationalCodeCounty(edit.nationalCode) == true)
            {
                CountyLevel managerDistrictlevel = GetUserBynationalCodeCountyLevel(edit.nationalCode);
                managerDistrictlevel.nationalCode = edit.nationalCode;
                managerDistrictlevel.fName = edit.fName;
                managerDistrictlevel.lName = edit.lName;
                managerDistrictlevel.gender = GetGenderTitleWithGenderId(gender);
                managerDistrictlevel.employmentStatus = GetEmployentTitleWithEmployentId(employment);
                managerDistrictlevel.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerDistrictlevel.role = GetRoleTitleWithRoleId(7);
                managerDistrictlevel.dateOfBirth = edit.dateOfBirth;
                managerDistrictlevel.tel = edit.tel;
                managerDistrictlevel.education = edit.education;
                managerDistrictlevel.workingStatus = edit.workingStatus;
                managerDistrictlevel.department = department;
                managerDistrictlevel.address = edit.address;
                managerDistrictlevel.department = county;
                Employees managerDistrictlevelEmployees = GetUserBynationalCodeC(edit.nationalCode);
                managerDistrictlevelEmployees.nationalCode = edit.nationalCode;
                managerDistrictlevelEmployees.fName = edit.fName;
                managerDistrictlevelEmployees.lName = edit.lName;
                managerDistrictlevelEmployees.gender = GetGenderTitleWithGenderId(gender);
                managerDistrictlevelEmployees.employmentStatus = GetEmployentTitleWithEmployentId(employment);
                managerDistrictlevelEmployees.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerDistrictlevelEmployees.role = GetRoleTitleWithRoleId(7);
                managerDistrictlevelEmployees.dateOfBirth = edit.dateOfBirth;
                managerDistrictlevelEmployees.tel = edit.tel;
                managerDistrictlevelEmployees.education = edit.education;
                managerDistrictlevelEmployees.workingStatus = edit.workingStatus;
                managerDistrictlevelEmployees.address = edit.address;
                managerDistrictlevelEmployees.country = 0;
                managerDistrictlevelEmployees.department = county;
                managerDistrictlevelEmployees.area = "شهرستان";
                if (!string.IsNullOrEmpty(edit.userPass))
                {
                    managerDistrictlevel.userPass = edit.userPass;
                    managerDistrictlevelEmployees.userPass = edit.userPass;
                }
                int departmenIdDisrict = GetDepartmentIdWithnationalCodeDistrict(edit.nationalCode);
                int countyIdDistrict = GetCountyIdWithnationalCodeDistrict(edit.nationalCode);
                bool resultPC_1 = UploadEditAvatarCountyPC(edit.avatar, edit);
                bool reultPc_2 = UploadEditAvatarDistrictPC(edit.avatar, edit);
                bool resultDB = UploadEditAvatarCountyDB(edit.avatar, edit, county, role, roleUploader);
            }

            DistrictLevel user = GetUserBynationalCodeDistrictLevel(edit.nationalCode);
            user.nationalCode = edit.nationalCode;
            user.fName = edit.fName;
            user.lName = edit.lName;
            user.gender = GetGenderTitleWithGenderId(gender);
            user.employmentStatus = GetEmployentTitleWithEmployentId(employment);
            user.department = department;
            user.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            user.county = county;
            user.role = GetRoleTitleWithRoleId(role);
            user.dateOfBirth = edit.dateOfBirth;
            user.tel = edit.tel;
            user.education = edit.education;
            user.workingStatus = edit.workingStatus;
            user.address = edit.address;
            Employees employee = GetUserBynationalCodeD(edit.nationalCode);
            employee.area = "بخش";
            employee.nationalCode = edit.nationalCode;
            employee.fName = edit.fName;
            employee.lName = edit.lName;
            employee.gender = GetGenderTitleWithGenderId(gender);
            employee.employmentStatus = GetEmployentTitleWithEmployentId(employment);
            employee.department = department;
            employee.dateOfBirth = edit.dateOfBirth;
            employee.education = edit.education;
            employee.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            employee.role = GetRoleTitleWithRoleId(role);
            employee.tel = edit.tel;
            employee.department = department;
            employee.workingStatus = edit.workingStatus;
            employee.address = edit.address;
            employee.country = county;
            if (!string.IsNullOrEmpty(edit.userPass))
            {
                user.userPass = edit.userPass;
                employee.userPass = edit.userPass;
            }
            _context.DistrictLevels.Update(user);
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return true;
        }
        #endregion

        #region GetUserInformation
        public InformationViewModel GetUserInformationByuserIdProvince(int userId)
        {
            var user = GetUserByUserNameProvinceLevel(userId);
            InformationViewModel information = new InformationViewModel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public InformationViewModel GetUserInformationByuserIdCounty(int userId)
        {
            var user = GetUserByUserNameCountyLevel(userId);
            InformationViewModel information = new InformationViewModel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public InformationViewModel GetUserInformationByuserIdDistrict(int userId)
        {
            var user = GetUserByUserNameDistrictLevel(userId);
            InformationViewModel information = new InformationViewModel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public InformationViewModel GetUserInformationByuserId(int userId)
        {
            var user = GetUserByUserName(userId);
            InformationViewModel information = new InformationViewModel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public InformationViewModel GetUserInformationBynationalCodeProvince(string nationalCode)
        {
            var user = GetUserBynationalCodeProvinceLevel(nationalCode);
            InformationViewModel information = new InformationViewModel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public InformationViewModel GetUserInformationBynationalCodeCounty(string nationalCode)
        {
            var user = GetUserBynationalCodeCountyLevel(nationalCode);
            InformationViewModel information = new InformationViewModel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public InformationViewModel GetUserInformationBynationalCodeDistrict(string nationalCode)
        {
            var user = GetUserBynationalCodeDistrictLevel(nationalCode);
            InformationViewModel information = new InformationViewModel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.county = user.county;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public InformationViewModel GetUserInformationBynationalCode(string nationalCode)
        {
            var user = GetUserBynationalCode(nationalCode);
            InformationViewModel information = new InformationViewModel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }
        #endregion

        #region GetProfileUserInformation
        public ProfileInformationViewModel GetProfileUserInformationBynationalCodeProvince(string nationalCode)
        {
            return _context.ProvinceLevel.Where(u => u.nationalCode == nationalCode)
                .Select(u => new ProfileInformationViewModel()
                {
                    nationalCode = u.nationalCode,
                    userPass = u.userPass,
                    fName = u.fName,
                    lName = u.lName,
                    department = u.department,
                    role = u.role,
                    education = u.education,
                    tel = u.tel,
                    employmentStatus = u.employmentStatus,
                    maritalStatus = u.maritalStatus,
                    address = u.address
                }).Single();
        }
        public ProfileInformationViewModel GetProfileUserInformationBynationalCodeCounty(string nationalCode)
        {
            return _context.CountyLevels.Where(u => u.nationalCode == nationalCode)
                .Select(u => new ProfileInformationViewModel()
                {
                    nationalCode = u.nationalCode,
                    userPass = u.userPass,
                    fName = u.fName,
                    lName = u.lName,
                    department = u.department,
                    role = u.role,
                    education = u.education,
                    tel = u.tel,
                    employmentStatus = u.employmentStatus,
                    maritalStatus = u.maritalStatus,
                    address = u.address
                }).Single();
        }
        public ProfileInformationViewModel GetProfileUserInformationBynationalCodeDistrict(string nationalCode)
        {
            return _context.DistrictLevels.Where(u => u.nationalCode == nationalCode)
                .Select(u => new ProfileInformationViewModel()
                {
                    nationalCode = u.nationalCode,
                    userPass = u.userPass,
                    fName = u.fName,
                    lName = u.lName,
                    department = u.department,
                    role = u.role,
                    education = u.education,
                    tel = u.tel,
                    county = u.county,
                    employmentStatus = u.employmentStatus,
                    maritalStatus = u.maritalStatus,
                    address = u.address
                }).Single();
        }

        #endregion

        #region GetRegisterDateWithnationalCode
        public DateTime GetDeletedRegisterDateWithnationalCodeProvince(string nationalCode)
        {
            return _context.ProvinceLevel.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).RegisterDate;
        }
        public DateTime GetDeletedRegisterDateWithnationalCodeCounty(string nationalCode)
        {
            return _context.CountyLevels.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).RegisterDate;
        }
        public DateTime GetDeletedRegisterDateWithnationalCodeDitrict(string nationalCode)
        {
            return _context.DistrictLevels.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).RegisterDate;
        }
        public DateTime GetDeletedRegisterDateWithnationalCodeP(string nationalCode)
        {
            return _context.Employees.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "استان").RegisterDate;
        }
        public DateTime GetDeletedRegisterDateWithnationalCodeC(string nationalCode)
        {
            return _context.Employees.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "شهرستان").RegisterDate;
        }
        public DateTime GetDeletedRegisterDateWithnationalCodeD(string nationalCode)
        {
            return _context.Employees.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "بخش").RegisterDate;
        }
        public DateTime GetRegisterDateWithnationalCodeProvince(string nationalCode)
        {
            return _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == nationalCode).RegisterDate;
        }
        public DateTime GetRegisterDateWithnationalCodeCounty(string nationalCode)
        {
            return _context.CountyLevels.SingleOrDefault(u => u.nationalCode == nationalCode).RegisterDate;
        }
        public DateTime GetRegisterDateWithnationalCodeDitrict(string nationalCode)
        {
            return _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode).RegisterDate;
        }
        public DateTime GetRegisterDateWithnationalCodeP(string nationalCode)
        {
            return _context.Employees.SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "استان").RegisterDate;
        }
        public DateTime GetRegisterDateWithnationalCodeC(string nationalCode)
        {
            return _context.Employees.SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "شهرستان").RegisterDate;
        }
        public DateTime GetRegisterDateWithnationalCodeD(string nationalCode)
        {
            return _context.Employees.SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "بخش").RegisterDate;
        }
        #endregion

        #region GetCountyIdWithnationalCode
        public int GetCountyIdWithnationalCodeDistrict(string nationalCode)
        {
            return _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode).county;
        }
        public int GetDeletedCountyIdWithnationalCodeDistrict(string nationalCode)
        {
            return _context.DistrictLevels.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).county;
        }
        #endregion

        #region EditUserProfile
        public EditProfileViewModel GetUserForShowInEditProfileProvince(String nationalCode)
        {
            return _context.ProvinceLevel.Where(u => u.nationalCode == nationalCode)
            .Select(u => new EditProfileViewModel()
            {
                userId = u.userId,
                nationalCode = u.nationalCode,
                tel = u.tel,
                maritalStatus = GetMaritalIdWithMaritalTitleProvince(nationalCode),
                address = u.address,
                education = u.education,
            }).Single();
        }
        public bool EditUserProfileProvince(List<int> maritalId, EditProfileViewModel edit, string roleUploader)
        {
            int marital = 0;
            foreach (int item in maritalId)
            {
                marital = item;
            };
            if (IsExistNationalCodeProvince(edit.nationalCode) == true && IsExistNationalCodeCounty(edit.nationalCode) == true)
            {
                CountyLevel managerCountylevel = GetUserBynationalCodeCountyLevel(edit.nationalCode);
                managerCountylevel.nationalCode = edit.nationalCode;
                managerCountylevel.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerCountylevel.role = GetRoleTitleWithRoleId(1);
                managerCountylevel.tel = edit.tel;
                managerCountylevel.education = edit.education;
                managerCountylevel.address = edit.address;
                managerCountylevel.department = GetDepartmentIdWithnationalCodeCounty(edit.nationalCode);
                Employees managerCountylevelEmployees = GetUserBynationalCodeC(edit.nationalCode);
                managerCountylevelEmployees.nationalCode = edit.nationalCode;
                managerCountylevelEmployees.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerCountylevelEmployees.role = GetRoleTitleWithRoleId(1);
                managerCountylevelEmployees.tel = edit.tel;
                managerCountylevelEmployees.education = edit.education;
                managerCountylevelEmployees.address = edit.address;
                managerCountylevelEmployees.country = GetDepartmentIdWithnationalCodeCounty(edit.nationalCode);
                managerCountylevelEmployees.area = "شهرستان";
                int departmentId = GetDepartmentIdWithnationalCodeCounty(edit.nationalCode);
                bool resultPC = UploadEditProfileAvatarCountyPC(edit.avatar, edit);
                bool resultDB = UploadEditProfileAvatarCountyDB(edit.avatar, edit, roleUploader);
                _context.CountyLevels.Update(managerCountylevel);
                _context.Employees.Update(managerCountylevelEmployees);
            }
            ProvinceLevel user = GetUserBynationalCodeProvinceLevel(edit.nationalCode);
            user.nationalCode = edit.nationalCode;
            user.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            user.tel = edit.tel;
            user.education = edit.education;
            user.address = edit.address;
            Employees employee = GetUserBynationalCodeP(edit.nationalCode);
            employee.nationalCode = edit.nationalCode;
            employee.education = edit.education;
            employee.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            employee.tel = edit.tel;
            employee.address = edit.address;
            if (!string.IsNullOrEmpty(edit.userPass))
            {
                user.userPass = edit.userPass;
                employee.userPass = edit.userPass;
            }
            _context.ProvinceLevel.Update(user);
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return true;
        }
        public EditProfileViewModel GetUserForShowInEditProfileCounty(String nationalCode)
        {
            return _context.CountyLevels.Where(u => u.nationalCode == nationalCode)
            .Select(u => new EditProfileViewModel()
            {
                userId = u.userId,
                nationalCode = u.nationalCode,
                tel = u.tel,
                maritalStatus = GetMaritalIdWithMaritalTitleCounty(nationalCode),
                address = u.address,
                education = u.education,
            }).Single();
        }
        public bool EditUserProfileCounty(List<int> maritalId, EditProfileViewModel edit, string roleUploader)
        {
            int marital = 0;
            foreach (int item in maritalId)
            {
                marital = item;
            };
            if (IsExistNationalCodeProvince(edit.nationalCode) == true && IsExistNationalCodeCounty(edit.nationalCode) == true)
            {
                ProvinceLevel managerCountylevel = GetUserBynationalCodeProvinceLevel(edit.nationalCode);
                managerCountylevel.nationalCode = edit.nationalCode;
                managerCountylevel.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerCountylevel.tel = edit.tel;
                managerCountylevel.education = edit.education;
                managerCountylevel.address = edit.address;
                Employees managerCountylevelEmployees = GetUserBynationalCodeP(edit.nationalCode);
                managerCountylevelEmployees.nationalCode = edit.nationalCode;
                managerCountylevelEmployees.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerCountylevelEmployees.tel = edit.tel;
                managerCountylevelEmployees.education = edit.education;
                managerCountylevelEmployees.address = edit.address;
                managerCountylevelEmployees.area = "استان";
                int departmentId = GetDepartmentIdWithnationalCodeCounty(edit.nationalCode);
                bool resultPC_1 = UploadEditProfileAvatarProvincePC(edit.avatar, edit);
                bool resultPc_2 = UploadEditProfileAvatarCountyPC(edit.avatar, edit);
                bool resultDB = UploadEditProfileAvatarProvinceDB(edit.avatar, edit, roleUploader);
                _context.ProvinceLevel.Update(managerCountylevel);
                _context.Employees.Update(managerCountylevelEmployees);
            }
            if (IsExistNationalCodeDistrict(edit.nationalCode) == true && IsExistNationalCodeCounty(edit.nationalCode) == true)
            {
                DistrictLevel managerDistrictlevel = GetUserBynationalCodeDistrictLevel(edit.nationalCode);
                managerDistrictlevel.nationalCode = edit.nationalCode;
                managerDistrictlevel.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerDistrictlevel.tel = edit.tel;
                managerDistrictlevel.education = edit.education;
                managerDistrictlevel.address = edit.address;
                Employees managerDistrictlevelEmployees = GetUserBynationalCodeD(edit.nationalCode);
                managerDistrictlevelEmployees.nationalCode = edit.nationalCode;
                managerDistrictlevelEmployees.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerDistrictlevelEmployees.tel = edit.tel;
                managerDistrictlevelEmployees.education = edit.education;
                managerDistrictlevelEmployees.address = edit.address;
                managerDistrictlevelEmployees.area = "بخش";
                bool resultPC = UploadEditProfileAvatarDistrictPC(edit.avatar, edit);
                bool resultDB = UploadEditProfileAvatarDistrictDB(edit.avatar, edit, roleUploader);
            }
            CountyLevel user = GetUserBynationalCodeCountyLevel(edit.nationalCode);
            user.nationalCode = edit.nationalCode;
            user.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            user.tel = edit.tel;
            user.education = edit.education;
            user.address = edit.address;
            Employees employee = GetUserBynationalCodeC(edit.nationalCode);
            employee.nationalCode = edit.nationalCode;
            employee.education = edit.education;
            employee.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            employee.tel = edit.tel;
            employee.address = edit.address;
            if (!string.IsNullOrEmpty(edit.userPass))
            {
                user.userPass = edit.userPass;
                employee.userPass = edit.userPass;
            }
            _context.CountyLevels.Update(user);
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return true;
        }
        public EditProfileViewModel GetUserForShowInEditProfileDistrict(String nationalCode)
        {
            return _context.DistrictLevels.Where(u => u.nationalCode == nationalCode)
            .Select(u => new EditProfileViewModel()
            {
                userId = u.userId,
                nationalCode = u.nationalCode,
                tel = u.tel,
                maritalStatus = GetMaritalIdWithMaritalTitleDistrict(nationalCode),
                address = u.address,
                education = u.education,
            }).Single();
        }
        public bool EditUserProfileDistrict(List<int> maritalId, EditProfileViewModel edit, string roleUploader)
        {
            int marital = 0;
            foreach (int item in maritalId)
            {
                marital = item;
            };
            if (IsExistNationalCodeDistrict(edit.nationalCode) == true && IsExistNationalCodeCounty(edit.nationalCode) == true)
            {
                CountyLevel managerDistrictlevel = GetUserBynationalCodeCountyLevel(edit.nationalCode);
                managerDistrictlevel.nationalCode = edit.nationalCode;
                managerDistrictlevel.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerDistrictlevel.tel = edit.tel;
                managerDistrictlevel.education = edit.education;
                managerDistrictlevel.address = edit.address;
                Employees managerDistrictlevelEmployees = GetUserBynationalCodeC(edit.nationalCode);
                managerDistrictlevelEmployees.nationalCode = edit.nationalCode;
                managerDistrictlevelEmployees.maritalStatus = GetMaritalTitleWithMaritalId(marital);
                managerDistrictlevelEmployees.tel = edit.tel;
                managerDistrictlevelEmployees.education = edit.education;
                managerDistrictlevelEmployees.address = edit.address;
                managerDistrictlevelEmployees.area = "شهرستان";
                bool resultPC_1 = UploadEditProfileAvatarCountyPC(edit.avatar, edit);
                bool resultPC_2 = UploadEditProfileAvatarDistrictPC(edit.avatar, edit);
                bool resultDB = UploadEditProfileAvatarCountyDB(edit.avatar, edit, roleUploader);
            }
            DistrictLevel user = GetUserBynationalCodeDistrictLevel(edit.nationalCode);
            user.nationalCode = edit.nationalCode;
            user.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            user.tel = edit.tel;
            user.education = edit.education;
            user.address = edit.address;
            Employees employee = GetUserBynationalCodeD(edit.nationalCode);
            employee.nationalCode = edit.nationalCode;
            employee.education = edit.education;
            employee.maritalStatus = GetMaritalTitleWithMaritalId(marital);
            employee.tel = edit.tel;
            employee.address = edit.address;
            employee.area = "بخش";
            if (!string.IsNullOrEmpty(edit.userPass))
            {
                user.userPass = edit.userPass;
                employee.userPass = edit.userPass;
            }
            _context.DistrictLevels.Update(user);
            _context.Employees.Update(employee);
            _context.SaveChanges();
            return true;
        }
        #endregion

        #region ShowInPageDelete
        public InformationViewModel ListDeleteUsersProvince(string nationalCode)
        {
            return GetUserInformationBynationalCodeProvince(nationalCode);
        }
        public InformationViewModel ListDeleteUsersCounty(string nationalCode)
        {
            return GetUserInformationBynationalCodeCounty(nationalCode);
        }
        public InformationViewModel ListDeleteUsersDistrict(string nationalCode)
        {
            return GetUserInformationBynationalCodeDistrict(nationalCode);
        }
        #endregion

        #region UpdateUser
        public void UpdateUserProvince(ProvinceLevel user)
        {
            _context.ProvinceLevel.Update(user);
            _context.SaveChanges();
        }
        public void UpdateUserCounty(CountyLevel user)
        {
            _context.CountyLevels.Update(user);

            _context.SaveChanges();
        }
        public void UpdateUserDistrict(DistrictLevel user)
        {
            _context.DistrictLevels.Update(user);
            _context.SaveChanges();
        }
        public void UpdateUser(Employees user)
        {
            _context.Employees.Update(user);
            _context.SaveChanges();
        }
        #endregion

        #region DeleteUser
        public bool DeleteUserProvince(string nationalCode)
        {
            if (IsExistNationalCodeCounty(nationalCode) == true && IsExistNationalCodeProvince(nationalCode) == true)
            {
                CountyLevel userC = GetUserBynationalCodeCountyLevel(nationalCode);
                userC.IsDelete = true;
                userC.workingStatus = GetDeletedRegisterDateWithnationalCodeCounty(nationalCode).DailyWorkHistory();
                UpdateUserCounty(userC);
            }
            ProvinceLevel user = GetUserBynationalCodeProvinceLevel(nationalCode);
            user.workingStatus = GetRegisterDateWithnationalCodeProvince(nationalCode).DailyWorkHistory();
            user.IsDelete = true;
            UpdateUserProvince(user);
            return true;
        }
        public bool DeleteUserCounty(string nationalCode)
        {
            if (IsExistNationalCodeCounty(nationalCode) == true && IsExistNationalCodeDistrict(nationalCode) == true)
            {
                DistrictLevel userD = GetUserBynationalCodeDistrictLevel(nationalCode);
                userD.IsDelete = true;
                userD.workingStatus = GetDeletedRegisterDateWithnationalCodeDitrict(nationalCode).DailyWorkHistory();
                UpdateUserDistrict(userD);
            }
            CountyLevel user = GetUserBynationalCodeCountyLevel(nationalCode);
            user.workingStatus = GetRegisterDateWithnationalCodeCounty(nationalCode).DailyWorkHistory();
            user.IsDelete = true;
            UpdateUserCounty(user);
            return true;
        }
        public bool DeleteUserDistrict(string nationalCode)
        {
            DistrictLevel user = GetUserBynationalCodeDistrictLevel(nationalCode);
            user.workingStatus = GetRegisterDateWithnationalCodeDitrict(nationalCode).DailyWorkHistory();
            user.IsDelete = true;
            UpdateUserDistrict(user);
            return true;
        }

        public bool DeleteUserP(string nationalCode)
        {
            if (IsExistDeletedNationalCodeCounty(nationalCode) == true && IsExistDeletedNationalCodeProvince(nationalCode) == true)
            {
                Employees userC = GetUserBynationalCodeC(nationalCode);
                userC.workingStatus = GetDeletedRegisterDateWithnationalCodeC(nationalCode).DailyWorkHistory();
                userC.IsDelete = true;
                UpdateUser(userC);
            }
            Employees user = GetUserBynationalCodeP(nationalCode);
            user.workingStatus = GetDeletedRegisterDateWithnationalCodeProvince(nationalCode).DailyWorkHistory();
            user.IsDelete = true;
            UpdateUser(user);
            return true;
        }
        public bool DeleteUserC(string nationalCode)
        {
            if (IsExistDeletedNationalCodeCounty(nationalCode) == true && IsExistDeletedNationalCodeDistrict(nationalCode) == true)
            {
                Employees userD = GetUserBynationalCodeD(nationalCode);
                userD.workingStatus = GetDeletedRegisterDateWithnationalCodeD(nationalCode).DailyWorkHistory();
                userD.IsDelete = true;
                UpdateUser(userD);
            }
            Employees user = GetUserBynationalCodeC(nationalCode);
            user.workingStatus = GetDeletedRegisterDateWithnationalCodeCounty(nationalCode).DailyWorkHistory();
            user.IsDelete = true;
            UpdateUser(user);
            return true;
        }
        public bool DeleteUserD(string nationalCode)
        {
            Employees user = GetUserBynationalCodeD(nationalCode);
            user.workingStatus = GetDeletedRegisterDateWithnationalCodeDitrict(nationalCode).DailyWorkHistory();
            user.IsDelete = true;
            UpdateUser(user);
            return true;
        }
        #endregion

        #region DeleteUserDocumentsDB
        public bool DeleteUserDocumentsProvinceDB(string nationalCode)
        {
            if (IsExistUserDocumentsOnDocumentProvinceDB(nationalCode) == true && IsExistUserDocumentsOnDocumentCountyDB(nationalCode) == true)
            {
                _context.documentUploadCountyLevels.Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = true);
                if (IsExistExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "شهرستان"))
                {
                    _context.transferDocuments.Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "شهرستان").ToList().ForEach(d => d.IsDelete = true);
                }
            }
            _context.documentUploadProvinceLevels.Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = true);
            if (IsExistExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "استان"))
            {
                _context.transferDocuments.Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "استان").ToList().ForEach(d => d.IsDelete = true);
            }
            _context.SaveChanges();
            return true;
        }
        public bool DeleteUserDocumentsCountyDB(string nationalCode)
        {
            if (IsExistUserDocumentsOnDocumentCountyDB(nationalCode) == true && IsExistUserDocumentsOnDocumentDistrictDB(nationalCode) == true)
            {
                _context.documentUploadDistrictLevels.Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = true);
                if (IsExistExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "بخش"))
                {
                    _context.transferDocuments.Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "بخش").ToList().ForEach(d => d.IsDelete = true);
                }
            }
            _context.documentUploadCountyLevels.Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = true);
            if (IsExistExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "شهرستان"))
            {
                _context.transferDocuments.Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "شهرستان").ToList().ForEach(d => d.IsDelete = true);
            }
            _context.SaveChanges();
            return true;
        }
        public bool DeleteUserDocumentsDistrictDB(string nationalCode)
        {
            _context.documentUploadDistrictLevels.Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = true);
            if (IsExistExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "بخش"))
            {
                _context.transferDocuments.Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "بخش").ToList().ForEach(d => d.IsDelete = true);
            }
            _context.SaveChanges();
            return true;
        }
        #endregion

        #region IsExistUserDocumentsOnDocumentDB
        public bool IsExistUserDocumentsOnDocumentProvinceDB(string nationalCode)
        {
            return _context.documentUploadProvinceLevels.Any(d => d.ownerUserId == nationalCode);
        }
        public bool IsExistUserDocumentsOnDocumentCountyDB(string nationalCode)
        {
            return _context.documentUploadCountyLevels.Any(d => d.ownerUserId == nationalCode);
        }
        public bool IsExistUserDocumentsOnDocumentDistrictDB(string nationalCode)
        {
            return _context.documentUploadDistrictLevels.Any(d => d.ownerUserId == nationalCode);

        }
        #endregion

        #region ActivationUserDocumentsDeleted
        public bool ActivationDeletedUserDocumentsProvince(string nationalCode)
        {
            if (IsExistDeletedNationalCodeProvince(nationalCode) == true && IsExistDeletedNationalCodeCounty(nationalCode) == true)
            {
                _context.documentUploadCountyLevels.IgnoreQueryFilters().Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = false);
                if (IsExistDeletedExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "شهرستان"))
                {
                    _context.transferDocuments.IgnoreQueryFilters().Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "شهرستان").ToList().ForEach(d => d.IsDelete = false);

                }
            }
            _context.documentUploadProvinceLevels.IgnoreQueryFilters().Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = false);
            if (IsExistDeletedExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "استان"))
            {
                _context.transferDocuments.IgnoreQueryFilters().Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "استان").ToList().ForEach(d => d.IsDelete = false);
            }
            _context.SaveChanges();
            return true;
        }
        public bool ActivationDeletedUserDocumentsCounty(string nationalCode)
        {
            if (IsExistDeletedNationalCodeCounty(nationalCode) == true && IsExistDeletedNationalCodeDistrict(nationalCode) == true)
            {
                _context.documentUploadDistrictLevels.IgnoreQueryFilters().Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = false);
                if (IsExistDeletedExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "بخش"))
                {
                    _context.transferDocuments.IgnoreQueryFilters().Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "بخش").ToList().ForEach(d => d.IsDelete = false);
                }
            }
            _context.documentUploadCountyLevels.IgnoreQueryFilters().Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = false);
            if (IsExistDeletedExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "شهرستان"))
            {
                _context.transferDocuments.IgnoreQueryFilters().Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "شهرستان").ToList().ForEach(d => d.IsDelete = false);
            }
            _context.SaveChanges();
            return true;
        }
        public bool ActivationDeletedUserDocumentsDistrict(string nationalCode)
        {
            _context.documentUploadDistrictLevels.IgnoreQueryFilters().Where(d => d.ownerUserId == nationalCode).ToList().ForEach(d => d.IsDelete = false);
            if (IsExistDeletedExchangeDocumentOnDBWithnationalCodeAndareaUploader(nationalCode, "بخش"))
            {
                _context.transferDocuments.IgnoreQueryFilters().Where(d => d.userUploaderId == nationalCode && d.userUploaderArea == "بخش").ToList().ForEach(d => d.IsDelete = false);
            }
            _context.SaveChanges();
            return true;
        }
        #endregion

        #region DeleteAvatrPicOnEditDB
        public bool DeleteAvatrPicOnEditProvinceLevel(string nationalCode, string title)
        {

            int id = _context.documentUploadProvinceLevels.Where(d => d.ownerUserId == nationalCode).SingleOrDefault(d => d.title == title).id;
            _context.documentUploadProvinceLevels.Remove(_context.documentUploadProvinceLevels.Find(id));
            return true;
        }
        public bool DeleteAvatrPicOnEditCountyLevel(string nationalCode, string title)
        {
            int id = _context.documentUploadCountyLevels.Where(d => d.ownerUserId == nationalCode).SingleOrDefault(d => d.title == title).id;
            _context.documentUploadCountyLevels.Remove(_context.documentUploadCountyLevels.Find(id));
            return true;
        }
        public bool DeleteAvatrPicOnEditDistrictLevel(string nationalCode, string title)
        {
            int id = _context.documentUploadDistrictLevels.Where(d => d.ownerUserId == nationalCode).SingleOrDefault(d => d.title == title).id;
            _context.documentUploadDistrictLevels.Remove(_context.documentUploadDistrictLevels.Find(id));
            return true;
        }

        #endregion

        #region DeleteAvatarPicOnPC
        public bool DeleteAvatarPicOnProvincePC(string nationalCode)
        {
            if (IsExistUserDocumentsOnDocumentCountyDB(nationalCode) == true && IsExistUserDocumentsOnDocumentProvinceDB(nationalCode) == true)
            {
                string deletePathCounty = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/CountyArea", nationalCode + ".png");
                if (File.Exists(deletePathCounty))
                {
                    File.Delete(deletePathCounty);
                }
            }
            string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/ProvinceArea", nationalCode + ".png");
            if (File.Exists(deletePath))
            {
                File.Delete(deletePath);
            }
            return true;
        }
        public bool DeleteAvatarPicOnCountyPC(string nationalCode)
        {
            if (IsExistUserDocumentsOnDocumentCountyDB(nationalCode) == true && IsExistUserDocumentsOnDocumentDistrictDB(nationalCode) == true)
            {
                string deletePathDistrict = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/DistrictArea", nationalCode + ".png");
                if (File.Exists(deletePathDistrict))
                {
                    File.Delete(deletePathDistrict);
                }
            }
            string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/CountyArea", nationalCode + ".png");
            if (File.Exists(deletePath))
            {
                File.Delete(deletePath);
            }
            return true;
        }
        public bool DeleteAvatarPicOnDistrictPC(string nationalCode)
        {
            string deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/DistrictArea", nationalCode + ".png");
            if (File.Exists(deletePath))
            {
                File.Delete(deletePath);
            }
            return true;
        }
        #endregion

        #region GetUserDeletedBynationalCode
        public Employees GetUserDeletedBynationalCode(string nationalCode)
        {
            return _context.Employees.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode);
        }

        public Employees GetUserDeletedBynationalCodeP(string nationalCode)
        {
            return _context.Employees.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "استان");
        }

        public Employees GetUserDeletedBynationalCodeC(string nationalCode)
        {
            return _context.Employees.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "شهرستان");
        }
        public Employees GetUserDeletedBynationalCodeD(string nationalCode)
        {
            return _context.Employees.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode && u.area == "بخش");
        }

        public ProvinceLevel GetUserDeletedBynationalCodeProvinceLevel(string nationalCode)
        {
            return _context.ProvinceLevel.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode);
        }

        public CountyLevel GetUserDeletedBynationalCodeCountyLevel(string nationalCode)
        {
            return _context.CountyLevels.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode);
        }

        public DistrictLevel GetUserDeletedBynationalCodeDistrictLevel(string nationalCode)
        {
            return _context.DistrictLevels.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode);
        }

        #endregion

        #region GetUserDeletedInformation
        public ProvinceLevel GetUserDeletedInformationBynationalCodeProvince(string nationalCode)
        {
            var user = GetUserDeletedBynationalCodeProvinceLevel(nationalCode);
            ProvinceLevel information = new ProvinceLevel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public CountyLevel GetUserDeletedInformationBynationalCodeCounty(string nationalCode)
        {
            var user = GetUserDeletedBynationalCodeCountyLevel(nationalCode);
            CountyLevel information = new CountyLevel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public DistrictLevel GetUserDeletedInformationBynationalCodeDistrict(string nationalCode)
        {
            var user = GetUserDeletedBynationalCodeDistrictLevel(nationalCode);
            DistrictLevel information = new DistrictLevel();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }

        public Employees GetUserDeletedInformationBynationalCode(string nationalCode)
        {
            var user = GetUserDeletedBynationalCode(nationalCode);
            Employees information = new Employees();
            information.userId = user.userId;
            information.fName = user.fName;
            information.lName = user.lName;
            information.department = user.department;
            information.employmentStatus = user.employmentStatus;
            information.nationalCode = user.nationalCode;
            information.RegisterDate = user.RegisterDate;
            information.role = user.role;
            information.workingStatus = user.workingStatus;
            return information;
        }
        #endregion

        #region ActivationUserDeleted
        public bool ActivationDeletedUserProvince(string nationalCode)
        {
            if (IsExistDeletedNationalCodeCounty(nationalCode) == true && IsExistDeletedNationalCodeProvince(nationalCode))
            {
                CountyLevel userC = GetUserDeletedBynationalCodeCountyLevel(nationalCode);
                userC.IsDelete = false;
                userC.workingStatus = "فعال";
                UpdateUserCounty(userC);
            }
            ProvinceLevel user = GetUserDeletedBynationalCodeProvinceLevel(nationalCode);
            user.IsDelete = false;
            user.workingStatus = "فعال";
            UpdateUserProvince(user);
            return true;
        }

        public bool ActivationDeletedUserCounty(string nationalCode)
        {
            if (IsExistDeletedNationalCodeCounty(nationalCode) == true && IsExistDeletedNationalCodeDistrict(nationalCode))
            {
                DistrictLevel userD = GetUserDeletedBynationalCodeDistrictLevel(nationalCode);
                userD.IsDelete = false;
                userD.workingStatus = "فعال";
                UpdateUserDistrict(userD);
            }

            CountyLevel user = GetUserDeletedBynationalCodeCountyLevel(nationalCode);
            user.IsDelete = false;
            user.workingStatus = "فعال";
            UpdateUserCounty(user);
            return true;
        }

        public bool ActivationDeletedUserDistrict(string nationalCode)
        {
            DistrictLevel user = GetUserDeletedBynationalCodeDistrictLevel(nationalCode);
            user.IsDelete = false;
            user.workingStatus = "فعال";
            UpdateUserDistrict(user);
            return true;
        }

        public bool ActivationDeletedUser(string nationalCode)
        {
            Employees user = GetUserDeletedBynationalCode(nationalCode);
            user.IsDelete = false;
            user.workingStatus = "فعال";
            UpdateUser(user);
            return true;
        }
        public bool ActivationDeletedUserP(string nationalCode)
        {
            if (IsExistDeletedNationalCodeProvince(nationalCode) == true && IsExistDeletedNationalCodeCounty(nationalCode) == true)
            {
                Employees userC = GetUserDeletedBynationalCodeC(nationalCode);
                userC.IsDelete = false;
                userC.workingStatus = "فعال";
                UpdateUser(userC);
            }
            Employees user = GetUserDeletedBynationalCodeP(nationalCode);
            user.IsDelete = false;
            user.workingStatus = "فعال";
            UpdateUser(user);
            return true;
        }
        public bool ActivationDeletedUserC(string nationalCode)
        {
            if (IsExistDeletedNationalCodeCounty(nationalCode) == true && IsExistDeletedNationalCodeDistrict(nationalCode) == true)
            {
                Employees userD = GetUserDeletedBynationalCodeD(nationalCode);
                userD.IsDelete = false;
                userD.workingStatus = "فعال";
                UpdateUser(userD);
            }
            Employees user = GetUserDeletedBynationalCodeC(nationalCode);
            user.IsDelete = false;
            user.workingStatus = "فعال";
            UpdateUser(user);
            return true;
        }
        public bool ActivationDeletedUserD(string nationalCode)
        {
            Employees user = GetUserDeletedBynationalCodeD(nationalCode);
            user.IsDelete = false;
            user.workingStatus = "فعال";
            UpdateUser(user);
            return true;
        }

        #endregion

        #region UploadAvatarPC
        public bool UploadAvatarProvincePC(IFormFile file, RegisterViewModel document)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/ProvinceArea", document.nationalCode + Path.GetExtension(file.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return true;
                }
            }
            return false;
        }
        public bool UploadAvatarCountyPC(IFormFile file, RegisterViewModel document)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/CountyArea", document.nationalCode + Path.GetExtension(file.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return true;
                }
            }
            return false;
        }
        public bool UploadAvatarDistrictPC(IFormFile file, RegisterViewModel document)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/DistrictArea", document.nationalCode + Path.GetExtension(file.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region UploadAvatarDB
        public bool UploadAvatarProvinceDB(IFormFile file, RegisterViewModel document, string roleUploader)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    int department = 1;
                    document.uploadDate = DateTime.Now;
                    document.fileName = "no-document";
                    document.description = "ثبت تصویر پرسنل";
                    var docName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(docName);
                    var newFileName = String.Concat(document.nationalCode, fileExtension);
                    var objfiles = new DocumentUploadProvinceLevel()
                    {
                        id = 0,
                        fileName = newFileName,
                        uploadDate = DateTime.Now,
                        contentType = document.avatar.ContentType,
                        title = "تصویر پرسنل",
                        fileFormat = fileExtension,
                        ownerUserId = document.nationalCode,
                        description = document.description,
                        userId = GetIdWithNationalCodeForForeignKeyProvince(document.nationalCode),
                        area = "استان",
                        roleUploader = roleUploader,
                        department = department,
                        IsDelete = false
                    };
                    using (var target = new MemoryStream())
                    {
                        file.CopyTo(target);
                        objfiles.dataBytes = target.ToArray();
                    }
                    _context.documentUploadProvinceLevels.Add(objfiles);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool UploadAvatarCountyDB(IFormFile file, RegisterViewModel document, List<int> departmentId, string roleUploader)
        {

            if (file != null)
            {
                if (file.Length > 0)
                {

                    int department = 0;
                    foreach (int item in departmentId)
                    {
                        department = item;
                    };
                    document.uploadDate = DateTime.Now;
                    document.fileName = "no-document";
                    document.description = "ثبت تصویر پرسنل";
                    var docName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(docName);
                    var newFileName = String.Concat(document.nationalCode, fileExtension);
                    var objfiles = new DocumentUploadCountyLevel()
                    {
                        id = 0,
                        fileName = newFileName,
                        uploadDate = DateTime.Now,
                        title = "تصویر پرسنل",
                        contentType = document.avatar.ContentType,
                        fileFormat = fileExtension,
                        ownerUserId = document.nationalCode,
                        userId = GetIdWithNationalCodeForForeignKeyCounty(document.nationalCode),
                        area = "شهرستان",
                        roleUploader = roleUploader,
                        description = document.description,
                        department = department,
                        IsDelete = false
                    };
                    using (var target = new MemoryStream())
                    {
                        file.CopyTo(target);
                        objfiles.dataBytes = target.ToArray();
                    }
                    _context.documentUploadCountyLevels.Add(objfiles);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool UploadAvatarDistrictDB(IFormFile file, RegisterViewModel document, List<int> departmentId, List<int> selectedCounty, string roleUploader)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    int department = 0;
                    int county = 0;
                    foreach (int item in departmentId)
                    {
                        department = item;
                    };
                    foreach (int item in selectedCounty)
                    {
                        county = item;
                    };
                    document.uploadDate = DateTime.Now;
                    document.fileName = "no-document";
                    document.description = "ثبت تصویر پرسنل";
                    var docName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(docName);
                    var newFileName = String.Concat(document.nationalCode, fileExtension);
                    var objfiles = new DocumentUploadDistrictLevel()
                    {
                        id = 0,
                        fileName = newFileName,
                        uploadDate = DateTime.Now,
                        title = "تصویر پرسنل",
                        contentType = document.avatar.ContentType,
                        fileFormat = fileExtension,
                        ownerUserId = document.nationalCode,
                        userId = GetIdWithNationalCodeForForeignKeyDistrict(document.nationalCode),
                        description = document.description,
                        department = department,
                        area = "بخش",
                        roleUploader = roleUploader,
                        county = county,
                        IsDelete = false
                    };
                    using (var target = new MemoryStream())
                    {
                        file.CopyTo(target);
                        objfiles.dataBytes = target.ToArray();
                    }
                    _context.documentUploadDistrictLevels.Add(objfiles);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region UploadEditAvatarPC
        public bool UploadEditAvatarProvincePC(IFormFile file, EditViewModel document)
        {
            if (document.avatar != null)
            {
                if (file.Length > 0)
                {
                    bool result = DeleteAvatarPicOnProvincePC(document.nationalCode);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/ProvinceArea", document.nationalCode + Path.GetExtension(file.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return true;
                }

            }
            return false;
        }
        public bool UploadEditAvatarCountyPC(IFormFile file, EditViewModel document)
        {
            if (document.avatar != null)
            {
                if (file.Length > 0)
                {
                    bool result = DeleteAvatarPicOnCountyPC(document.nationalCode);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/CountyArea", document.nationalCode + Path.GetExtension(file.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return true;
                }

            }
            return false;
        }
        public bool UploadEditAvatarDistrictPC(IFormFile file, EditViewModel document)
        {
            if (document.avatar != null)
            {
                if (file.Length > 0)
                {
                    bool result = DeleteAvatarPicOnDistrictPC(document.nationalCode);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/DistrictArea", document.nationalCode + Path.GetExtension(file.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return true;
                }

            }
            return false;
        }
        #endregion

        #region UploadEditAvatarDB
        public bool UploadEditAvatarProvinceDB(IFormFile file, EditViewModel document, int selectedRoles, string roleUploader)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    document.title = "تصویر پرسنل";
                    bool isExist = false;
                    if (selectedRoles == 1 || (selectedRoles == 7 && !IsExistManagerAvatarProvineDB(document.nationalCode)))
                    {
                        isExist = IsExistManagerAvatarProvineDB(document.nationalCode);
                        if (isExist == false)
                        {
                            if (file != null)
                            {
                                if (file.Length > 0)
                                {
                                    int department = 1;
                                    document.uploadDate = DateTime.Now;
                                    document.fileName = "no-document";
                                    document.description = "ثبت تصویر پرسنل";
                                    var docName = Path.GetFileName(file.FileName);
                                    var fileExtension = Path.GetExtension(docName);
                                    var newFileName = String.Concat(document.nationalCode, fileExtension);
                                    var objfiles = new DocumentUploadProvinceLevel()
                                    {
                                        id = 0,
                                        fileName = newFileName,
                                        uploadDate = DateTime.Now,
                                        title = document.title,
                                        contentType = document.avatar.ContentType,
                                        fileFormat = fileExtension,
                                        ownerUserId = document.nationalCode,
                                        description = document.description,
                                        roleUploader = roleUploader,
                                        userId = GetIdWithNationalCodeForForeignKeyProvince(document.nationalCode),
                                        area = "استان",
                                        department = department,
                                        IsDelete = false
                                    };
                                    using (var target = new MemoryStream())
                                    {
                                        file.CopyTo(target);
                                        objfiles.dataBytes = target.ToArray();
                                    }
                                    _context.documentUploadProvinceLevels.Add(objfiles);
                                    _context.SaveChanges();
                                    return true;
                                }
                            }
                        }

                    }
                    bool deleteResult = DeleteAvatrPicOnEditProvinceLevel(document.nationalCode, document.title);
                    if (deleteResult == true)
                    {
                        int department = 1;
                        document.uploadDate = DateTime.Now;
                        document.fileName = "no-document";
                        document.description = "تغییر تصویر پرسنل";
                        var docName = Path.GetFileName(file.FileName);
                        var fileExtension = Path.GetExtension(docName);
                        var newFileName = String.Concat(document.nationalCode, fileExtension);
                        var objfiles = new DocumentUploadProvinceLevel()
                        {
                            id = 0,
                            fileName = newFileName,
                            uploadDate = DateTime.Now,
                            title = document.title,
                            contentType = document.avatar.ContentType,
                            fileFormat = fileExtension,
                            ownerUserId = document.nationalCode,
                            description = document.description,
                            roleUploader = roleUploader,
                            userId = GetIdWithNationalCodeForForeignKeyProvince(document.nationalCode),
                            area = "استان",
                            department = department,
                            IsDelete = false
                        };
                        using (var target = new MemoryStream())
                        {
                            file.CopyTo(target);
                            objfiles.dataBytes = target.ToArray();
                        }
                        _context.documentUploadProvinceLevels.Add(objfiles);
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool UploadEditAvatarCountyDB(IFormFile file, EditViewModel document, int departmentId, int selectedRoles, string roleUploader)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    bool isExist = false;
                    document.title = "تصویر پرسنل";
                    if (!IsExistManagerAvatarCountyDB(document.nationalCode) && IsExistNationalCodeDistrict(document.nationalCode))
                    {
                        if ((selectedRoles == 1))
                        {
                            isExist = IsExistManagerAvatarCountyDB(document.nationalCode);
                            if (isExist == false)
                            {
                                if (file != null)
                                {
                                    if (file.Length > 0)
                                    {
                                        int department = document.department;
                                        document.uploadDate = DateTime.Now;
                                        document.fileName = "no-document";
                                        document.description = "ثبت تصویر پرسنل";
                                        var docName = Path.GetFileName(file.FileName);
                                        var fileExtension = Path.GetExtension(docName);
                                        var newFileName = String.Concat(document.nationalCode, fileExtension);
                                        var objfiles = new DocumentUploadCountyLevel()
                                        {
                                            id = 0,
                                            fileName = newFileName,
                                            uploadDate = DateTime.Now,
                                            title = document.title,
                                            contentType = document.avatar.ContentType,
                                            fileFormat = fileExtension,
                                            ownerUserId = document.nationalCode,
                                            description = document.description,
                                            roleUploader = roleUploader,
                                            userId = GetIdWithNationalCodeForForeignKeyCounty(document.nationalCode),
                                            area = "شهرستان",
                                            department = document.county,
                                            IsDelete = false
                                        };
                                        using (var target = new MemoryStream())
                                        {
                                            file.CopyTo(target);
                                            objfiles.dataBytes = target.ToArray();
                                        }
                                        _context.documentUploadCountyLevels.Add(objfiles);
                                        _context.SaveChanges();
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                    bool deleteResult = DeleteAvatrPicOnEditCountyLevel(document.nationalCode, document.title);
                    if (deleteResult == true)
                    {
                        document.uploadDate = DateTime.Now;
                        document.fileName = "no-document";
                        document.description = "تغییر تصویر پرسنل";
                        var docName = Path.GetFileName(file.FileName);
                        var fileExtension = Path.GetExtension(docName);
                        var newFileName = String.Concat(document.nationalCode, fileExtension);
                        var objfiles = new DocumentUploadCountyLevel()
                        {
                            id = 0,
                            fileName = newFileName,
                            uploadDate = DateTime.Now,
                            title = document.title,
                            contentType = document.avatar.ContentType,
                            fileFormat = fileExtension,
                            ownerUserId = document.nationalCode,
                            roleUploader = roleUploader,
                            userId = GetIdWithNationalCodeForForeignKeyCounty(document.nationalCode),
                            area = "شهرستان",
                            description = document.description,
                            department = departmentId,
                            IsDelete = false
                        };
                        using (var target = new MemoryStream())
                        {
                            file.CopyTo(target);
                            objfiles.dataBytes = target.ToArray();
                        }
                        _context.documentUploadCountyLevels.Add(objfiles);
                        _context.SaveChanges();
                        return true;
                    }


                }
            }
            return false;
        }

        public bool UploadEditAvatarDistrictDB(IFormFile file, EditViewModel document, int departmentId, int selectedCounty, string roleUploader)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    document.title = "تصویر پرسنل";
                    bool deleteResult = DeleteAvatrPicOnEditDistrictLevel(document.nationalCode, document.title);
                    if (deleteResult == true)
                    {
                        document.uploadDate = DateTime.Now;
                        document.fileName = "no-document";
                        document.description = "تغییر تصویر پرسنل";
                        var docName = Path.GetFileName(file.FileName);
                        var fileExtension = Path.GetExtension(docName);
                        var newFileName = String.Concat(document.nationalCode, fileExtension);
                        var objfiles = new DocumentUploadDistrictLevel()
                        {
                            id = 0,
                            fileName = newFileName,
                            uploadDate = DateTime.Now,
                            contentType = document.avatar.ContentType,
                            title = document.title,
                            fileFormat = fileExtension,
                            ownerUserId = document.nationalCode,
                            description = document.description,
                            roleUploader = roleUploader,
                            userId = GetIdWithNationalCodeForForeignKeyDistrict(document.nationalCode),
                            area = "بخش",
                            department = departmentId,
                            county = selectedCounty,
                            IsDelete = false
                        };
                        using (var target = new MemoryStream())
                        {
                            file.CopyTo(target);
                            objfiles.dataBytes = target.ToArray();
                        }
                        _context.documentUploadDistrictLevels.Add(objfiles);
                        _context.SaveChanges();
                        return true;
                    }


                }
            }
            return false;
        }

        #endregion

        #region UploadEditProfileAvatarPC
        public bool UploadEditProfileAvatarProvincePC(IFormFile file, EditProfileViewModel document)
        {
            if (document.avatar != null)
            {
                if (file.Length > 0)
                {
                    bool result = DeleteAvatarPicOnProvincePC(document.nationalCode);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/ProvinceArea", document.nationalCode + Path.GetExtension(file.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return true;
                }
            }
            return false;
        }
        public bool UploadEditProfileAvatarCountyPC(IFormFile file, EditProfileViewModel document)
        {
            if (document.avatar != null)
            {
                if (file.Length > 0)
                {
                    bool result = DeleteAvatarPicOnCountyPC(document.nationalCode);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/CountyArea", document.nationalCode + Path.GetExtension(file.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return true;
                }
            }
            return false;
        }
        public bool UploadEditProfileAvatarDistrictPC(IFormFile file, EditProfileViewModel document)
        {
            if (document.avatar != null)
            {
                if (file.Length > 0)
                {
                    bool result = DeleteAvatarPicOnDistrictPC(document.nationalCode);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserAvatar/DistrictArea", document.nationalCode + Path.GetExtension(file.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region UploadEditProfileAvatarDB
        public bool UploadEditProfileAvatarProvinceDB(IFormFile file, EditProfileViewModel document, string roleUploader)
        {

            if (file != null)
            {
                if (file.Length > 0)
                {
                    int role = GetRoleIdWithRoleTitleProvince(document.nationalCode);
                    int departmentId = GetDepartmentIdWithnationalCodeProvince(document.nationalCode);
                    document.title = "تصویر پرسنل";
                    bool isExist = false;
                    if (role == 1)
                    {
                        isExist = IsExistManagerAvatarProvineDB(document.nationalCode);
                        if (isExist == false)
                        {
                            if (file != null)
                            {
                                if (file.Length > 0)
                                {
                                    int department = departmentId;
                                    document.uploadDate = DateTime.Now;
                                    document.fileName = "no-document";
                                    document.description = "ثبت تصویر پرسنل";
                                    var docName = Path.GetFileName(file.FileName);
                                    var fileExtension = Path.GetExtension(docName);
                                    var newFileName = String.Concat(document.nationalCode, fileExtension);
                                    var objfiles = new DocumentUploadProvinceLevel()
                                    {
                                        id = 0,
                                        fileName = newFileName,
                                        uploadDate = DateTime.Now,
                                        title = document.title,
                                        contentType = document.avatar.ContentType,
                                        fileFormat = fileExtension,
                                        roleUploader = roleUploader,
                                        userId = GetIdWithNationalCodeForForeignKeyProvince(document.nationalCode),
                                        area = "استان",
                                        ownerUserId = document.nationalCode,
                                        description = document.description,
                                        department = departmentId,
                                        IsDelete = false
                                    };
                                    using (var target = new MemoryStream())
                                    {
                                        file.CopyTo(target);
                                        objfiles.dataBytes = target.ToArray();
                                    }
                                    _context.documentUploadProvinceLevels.Add(objfiles);
                                    _context.SaveChanges();
                                    return true;
                                }
                            }
                        }

                    }
                    bool deleteResult = DeleteAvatrPicOnEditProvinceLevel(document.nationalCode, document.title);
                    if (deleteResult == true)
                    {
                        document.uploadDate = DateTime.Now;
                        document.fileName = "no-document";
                        document.description = "تغییر تصویر پرسنل در پروفایل";
                        var docName = Path.GetFileName(file.FileName);
                        var fileExtension = Path.GetExtension(docName);
                        var newFileName = String.Concat(document.nationalCode, fileExtension);
                        var objfiles = new DocumentUploadProvinceLevel()
                        {
                            id = 0,
                            fileName = newFileName,
                            uploadDate = DateTime.Now,
                            title = document.title,
                            contentType = document.avatar.ContentType,
                            fileFormat = fileExtension,
                            userId = GetIdWithNationalCodeForForeignKeyProvince(document.nationalCode),
                            area = "استان",
                            roleUploader = roleUploader,
                            ownerUserId = document.nationalCode,
                            description = document.description,
                            department = departmentId,
                            IsDelete = false
                        };
                        using (var target = new MemoryStream())
                        {
                            file.CopyTo(target);
                            objfiles.dataBytes = target.ToArray();
                        }
                        _context.documentUploadProvinceLevels.Add(objfiles);
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        public bool UploadEditProfileAvatarCountyDB(IFormFile file, EditProfileViewModel document, string roleUploader)
        {

            if (file != null)
            {
                if (file.Length > 0)
                {
                    int departmentId = GetDepartmentIdWithnationalCodeCounty(document.nationalCode);
                    document.title = "تصویر پرسنل";
                    bool deleteResult = DeleteAvatrPicOnEditCountyLevel(document.nationalCode, document.title);
                    if (deleteResult == true)
                    {
                        document.uploadDate = DateTime.Now;
                        document.fileName = "no-document";
                        document.description = "تغییر تصویر پرسنل در پروفایل";
                        var docName = Path.GetFileName(file.FileName);
                        var fileExtension = Path.GetExtension(docName);
                        var newFileName = String.Concat(document.nationalCode, fileExtension);
                        var objfiles = new DocumentUploadCountyLevel()
                        {
                            id = 0,
                            fileName = newFileName,
                            uploadDate = DateTime.Now,
                            title = document.title,
                            contentType = document.avatar.ContentType,
                            fileFormat = fileExtension,
                            ownerUserId = document.nationalCode,
                            roleUploader = roleUploader,
                            userId = GetIdWithNationalCodeForForeignKeyCounty(document.nationalCode),
                            area = "شهرستان",
                            description = document.description,
                            department = departmentId,
                            IsDelete = false
                        };
                        using (var target = new MemoryStream())
                        {
                            file.CopyTo(target);
                            objfiles.dataBytes = target.ToArray();
                        }
                        _context.documentUploadCountyLevels.Add(objfiles);
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        public bool UploadEditProfileAvatarDistrictDB(IFormFile file, EditProfileViewModel document, string roleUploader)
        {

            if (file != null)
            {
                if (file.Length > 0)
                {
                    int departmentId = GetDepartmentIdWithnationalCodeDistrict(document.nationalCode);
                    int countyId = GetCountyIdWithnationalCodeDistrict(document.nationalCode);
                    document.title = "تصویر پرسنل";
                    bool deleteResult = DeleteAvatrPicOnEditDistrictLevel(document.nationalCode, document.title);
                    if (deleteResult == true)
                    {
                        document.uploadDate = DateTime.Now;
                        document.fileName = "no-document";
                        document.description = "تغییر تصویر پرسنل در پروفایل";
                        var docName = Path.GetFileName(file.FileName);
                        var fileExtension = Path.GetExtension(docName);
                        var newFileName = String.Concat(document.nationalCode, fileExtension);
                        var objfiles = new DocumentUploadDistrictLevel()
                        {
                            id = 0,
                            fileName = newFileName,
                            uploadDate = DateTime.Now,
                            title = document.title,
                            contentType = document.avatar.ContentType,
                            fileFormat = fileExtension,
                            ownerUserId = document.nationalCode,
                            userId = GetIdWithNationalCodeForForeignKeyDistrict(document.nationalCode),
                            area = "بخش",
                            roleUploader = roleUploader,
                            description = document.description,
                            department = departmentId,
                            county = countyId,
                            IsDelete = false
                        };
                        using (var target = new MemoryStream())
                        {
                            file.CopyTo(target);
                            objfiles.dataBytes = target.ToArray();
                        }
                        _context.documentUploadDistrictLevels.Add(objfiles);
                        _context.SaveChanges();
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region DownloadProfileAvatarPicAfterActive
        public async Task<bool> DownloadProfileAvatarPicAfterActiveProvince(string nationalCode)
        {

            if (IsexistProfileAvatarPicOnDBWithnationalCodeProvince(nationalCode) == true)
            {
                if (IsExistNationalCodeProvince(nationalCode) == true && IsExistNationalCodeCounty(nationalCode) == true)
                {
                    DownloadProfileAvatarPicAfterActiveCounty(nationalCode);
                }
                byte[] bytes;
                var item = GetProfileAvatarPicFromDBWithnationalCodeProvince(nationalCode);
                if (item != null)
                {
                    var fileName = item.fileName;
                    bytes = item.dataBytes;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UserAvatar\ProvinceArea", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileStream.WriteAsync(bytes);
                    }
                    return true;
                }

            }
            return false;
        }
        public async Task<bool> DownloadProfileAvatarPicAfterActiveCounty(string nationalCode)
        {

            if (IsexistProfileAvatarPicOnDBWithnationalCodeCounty(nationalCode) == true)
            {
                if (IsExistNationalCodeCounty(nationalCode) == true && IsExistNationalCodeDistrict(nationalCode) == true)
                {
                    DownloadProfileAvatarPicAfterActiveDistrict(nationalCode);
                }
                byte[] bytes;
                var item = GetProfileAvatarPicFromDBWithnationalCodeCounty(nationalCode);
                if (item != null)
                {
                    var fileName = item.fileName;
                    bytes = item.dataBytes;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UserAvatar\CountyArea", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileStream.WriteAsync(bytes);
                    }
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> DownloadProfileAvatarPicAfterActiveDistrict(string nationalCode)
        {
            if (IsexistProfileAvatarPicOnDBWithnationalCodeDistrict(nationalCode) == true)
            {
                byte[] bytes;
                var item = GetProfileAvatarPicFromDBWithnationalCodeDistrict(nationalCode);
                if (item != null)
                {
                    var fileName = item.fileName;
                    bytes = item.dataBytes;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\UserAvatar\DistrictArea", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileStream.WriteAsync(bytes);
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region IsexistProfileAvatarPicOnDBWithnationalCode
        public bool IsexistProfileAvatarPicOnDBWithnationalCodeProvince(string nationalCode)
        {
            return _context.documentUploadProvinceLevels.Any(d => d.ownerUserId == nationalCode && d.title == "تصویر پرسنل");
        }
        public bool IsexistProfileAvatarPicOnDBWithnationalCodeCounty(string nationalCode)
        {
            return _context.documentUploadCountyLevels.Any(d => d.ownerUserId == nationalCode && d.title == "تصویر پرسنل");
        }
        public bool IsexistProfileAvatarPicOnDBWithnationalCodeDistrict(string nationalCode)
        {
            return _context.documentUploadDistrictLevels.Any(d => d.ownerUserId == nationalCode && d.title == "تصویر پرسنل");
        }
        #endregion

        #region GetProfileAvatarPicFromDBWithnationalCode
        public DocumentUploadProvinceLevel GetProfileAvatarPicFromDBWithnationalCodeProvince(string nationalCode)
        {
            return _context.documentUploadProvinceLevels.Where(d => d.ownerUserId == nationalCode).SingleOrDefault(d => d.title == "تصویر پرسنل");
        }
        public DocumentUploadCountyLevel GetProfileAvatarPicFromDBWithnationalCodeCounty(string nationalCode)
        {
            return _context.documentUploadCountyLevels.Where(d => d.ownerUserId == nationalCode).SingleOrDefault(d => d.title == "تصویر پرسنل");
        }
        public DocumentUploadDistrictLevel GetProfileAvatarPicFromDBWithnationalCodeDistrict(string nationalCode)
        {
            return _context.documentUploadDistrictLevels.Where(d => d.ownerUserId == nationalCode).SingleOrDefault(d => d.title == "تصویر پرسنل");
        }
        #endregion

        #region IsExsistManagerAvatarOnDB
        public bool IsExistManagerAvatarProvineDB(string nationalCode)
        {
            return _context.documentUploadProvinceLevels.Any(u => u.ownerUserId == nationalCode && u.title == "تصویر پرسنل");
        }
        public bool IsExistManagerAvatarCountyDB(string nationalCode)
        {
            return _context.documentUploadCountyLevels.Any(u => u.ownerUserId == nationalCode && u.title == "تصویر پرسنل");
        }
        #endregion

        #region IsExistUserDocumentOnDBWithDocumentId
        public bool IsExistUserDocumentOnProvinceDB(int id)
        {
            return _context.documentUploadProvinceLevels.Any(d => d.id == id);
        }
        public bool IsExistUserDocumentOnCountyDB(int id)
        {
            return _context.documentUploadCountyLevels.Any(d => d.id == id);
        }
        public bool IsExistUserDocumentOnDistrictDB(int id)
        {
            return _context.documentUploadDistrictLevels.Any(d => d.id == id);
        }
        public bool IsExistExchangeDocumentOnDB(int id)
        {
            return _context.transferDocuments.Any(d => d.id == id);
        }
        public bool IsExistExchangeDocumentOnDBWithnationalCodeAndareaUploader(string userUploaderId, string userUploaderArea)
        {
            return _context.transferDocuments.Any(d => d.userUploaderId == userUploaderId && d.userUploaderArea == userUploaderArea);
        }

        #endregion

        #region IsExistDeletedUserDocumentOnDBWithDocumentId
        public bool IsExistDeletedUserDocumentOnProvinceDB(int id)
        {
            return _context.documentUploadProvinceLevels.IgnoreQueryFilters().Any(d => d.id == id);
        }
        public bool IsExistDeletedUserDocumentOnCountyDB(int id)
        {
            return _context.documentUploadCountyLevels.IgnoreQueryFilters().Any(d => d.id == id);
        }
        public bool IsExistDeletedUserDocumentOnDistrictDB(int id)
        {
            return _context.documentUploadDistrictLevels.IgnoreQueryFilters().Any(d => d.id == id);
        }
        public bool IsExistDeletedExchangeDocumentOnDBWithnationalCodeAndareaUploader(string userUploaderId, string userUploaderArea)
        {
            return _context.transferDocuments.IgnoreQueryFilters().Any(d => d.userUploaderId == userUploaderId && d.userUploaderArea == userUploaderArea);

        }
        public bool IsExistDeletedExchangeDocumentOnDB(int id)
        {
            return _context.transferDocuments.IgnoreQueryFilters().Any(d => d.id == id);
        }

        #endregion

        #region GetDocumentFromDBWithDocumentId
        public DocumentUploadProvinceLevel GetDocumentFromProvinceDB(int id)
        {
            return _context.documentUploadProvinceLevels.SingleOrDefault(d => d.id == id);
        }
        public DocumentUploadCountyLevel GetDocumentFromCountyDB(int id)
        {
            return _context.documentUploadCountyLevels.SingleOrDefault(d => d.id == id);

        }
        public DocumentUploadDistrictLevel GetDocumentFromDistrictDB(int id)
        {
            return _context.documentUploadDistrictLevels.SingleOrDefault(d => d.id == id);
        }
        public TransferDocumentsBetweenLevels GetExchangeDocumentFromDB(int id)
        {
            return _context.transferDocuments.SingleOrDefault(d => d.id == id);
        }

        #endregion

        #region DownloadDeletedDocumentFromDBWithDocumenId
        public DocumentUploadProvinceLevel GetDeletedDocumentFromProvinceDB(int id)
        {
            return _context.documentUploadProvinceLevels.IgnoreQueryFilters().SingleOrDefault(d => d.id == id);
        }
        public DocumentUploadCountyLevel GetDeletedDocumentFromCountyDB(int id)
        {
            return _context.documentUploadCountyLevels.IgnoreQueryFilters().SingleOrDefault(d => d.id == id);
        }
        public DocumentUploadDistrictLevel GetDeletedDocumentFromDistrictDB(int id)
        {
            return _context.documentUploadDistrictLevels.IgnoreQueryFilters().SingleOrDefault(d => d.id == id);
        }
        public TransferDocumentsBetweenLevels GetDeletedExchangeDocumentFromDB(int id)
        {
            return _context.transferDocuments.IgnoreQueryFilters().SingleOrDefault(d => d.id == id);
        }
        #endregion

        #region IsExistManager
        public bool IsExistManagerProvince()
        {
            return _context.ProvinceLevel.Any(u => u.role == "مدیریت");
        }
        public bool IsExistManagerCounty(int department)
        {
            return _context.CountyLevels.Any(u => u.department == department && u.role == "مدیریت");

        }
        public bool IsExistManagerDistrict(int county, int department)
        {
            return _context.DistrictLevels.Any(u => u.county == county && u.department == department && u.role == "مدیریت");
        }
        #endregion

        #region GetRoleWithnationalCode
        public string GetRoleWithnationalCodeProvince(string nationalCode)
        {
            return _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == nationalCode).role;
        }
        public string GetRoleWithnationalCodeCounty(string nationalCode)
        {
            return _context.CountyLevels.SingleOrDefault(u => u.nationalCode == nationalCode).role;
        }
        public string GetRoleWithnationalCodeDistrict(string nationalCode)
        {
            return _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode).role;
        }
        public string GetDeletedRoleWithnationalCodeProvince(string nationalCode)
        {
            return _context.ProvinceLevel.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).role;
        }
        public string GetDeletedRoleWithnationalCodeCounty(string nationalCode)
        {
            return _context.CountyLevels.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).role;
        }
        public string GetDeletedRoleWithnationalCodeDistrict(string nationalCode)
        {
            return _context.DistrictLevels.IgnoreQueryFilters().SingleOrDefault(u => u.nationalCode == nationalCode).role;
        }
        #endregion

        #region GetManagernationalCode
        //استخراج کد ملی مدیریت در سطح استان
        public string GetManagernationalCodeProvince()
        {
            return _context.ProvinceLevel.SingleOrDefault(u => u.role == "مدیریت").nationalCode;
        }
        //استخراج کد ملی مدیریت در سطح شهرستان
        public string GetManagernationalCodeCounty(int department)
        {
            return _context.CountyLevels.SingleOrDefault(u => u.role == "مدیریت" && u.department == department).nationalCode;
        }
        //استخراج کد ملی مدیریت در سطح بخش
        public string GetManagernationalCodeDistrict(int county, int department)
        {
            return _context.DistrictLevels.SingleOrDefault(u => u.role == "مدیریت" && u.county == county && u.department == department).nationalCode;
        }
        #endregion

        #region IsExistDeletedUserOnArchive
        public bool IsExistDeletedUserOnArchiveProvince(string nationalCode)
        {
            return _context.ProvinceLevel.IgnoreQueryFilters().Any(u => u.nationalCode == nationalCode && u.IsDelete == true);
        }

        public bool IsExistDeletedUserOnArchiveCounty(string nationalCode)
        {
            return _context.CountyLevels.IgnoreQueryFilters().Any(u => u.nationalCode == nationalCode && u.IsDelete == true);
        }
        public bool IsExistDeletedUserOnArchiveDistrict(string nationalCode)
        {
            return _context.DistrictLevels.IgnoreQueryFilters().Any(u => u.nationalCode == nationalCode && u.IsDelete == true);
        }
        #endregion

        #region ManageRoles
        public bool DeleteRoleWithroleId(int roleId)
        {
            var role = GetInformationRoleWithId(roleId);
            role.IsDelete = true;
            _context.SaveChanges();
            return true;
        }
        public bool ActivationDeletedRoleWithroleId(int roleId)
        {
            var deletedRole = GetInformationDeletedRoleWithRoleId(roleId);
            deletedRole.IsDelete = false;
            _context.SaveChanges();
            return true;
        }
        public string AddNewRoleWithRroleTitle(string roleTitle)
        {
            Role newRole = new Role()
            {
                RoleTitle = "معاونت " + roleTitle
            };
            _context.Roles.Add(newRole);
            _context.SaveChanges();
            return newRole.RoleTitle;
        }

        #endregion

        #region GetInformationRoleWithId
        public Role GetInformationRoleWithId(int roleId)
        {
            return _context.Roles.SingleOrDefault(r => r.RoleId == roleId);
        }
        public Role GetInformationDeletedRoleWithRoleId(int roleId)
        {
            return _context.Roles.IgnoreQueryFilters().SingleOrDefault(r => r.RoleId == roleId);

        }
        #endregion

        #region UploadDocumentPC
        public bool UploadProvincePC(IFormFile document, UploadViewModel content)
        {

            if (document != null)
            {
                if (document.Length > 0)
                {
                    content.uploadDate = DateTime.Now;
                    content.fileName = "no-document";
                    content.fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(document.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "DocumentProvince", content.userUploaderArea + "-" + content.userUploaderId + "-" + content.userReceiverArea
                        + "-" + content.userReceiverId + "-" + content.title + "-" + content.uploadDate.ToShamsiFileUpload() + Path.GetExtension(document.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.CopyTo(stream);
                    }
                    return true;
                }
            }
            return false;
        }
        public bool UploadCountyPC(IFormFile document, UploadViewModel content)
        {

            if (document != null && content.IsAllowed == true)
            {
                if (document.Length > 0)
                {
                    content.uploadDate = DateTime.Now;
                    content.fileName = "no-document";
                    content.fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(document.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "DocumentCounty", content.userUploaderArea + "-" + content.userUploaderId + "-" + content.userReceiverArea + "-" +
                        content.userReceiverId + "-" + content.title + "-" + content.uploadDate.ToShamsiFileUpload() + Path.GetExtension(document.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.CopyTo(stream);
                    }
                    return true;
                }
            }
            return false;
        }
        public bool UploadDistrictPC(IFormFile document, UploadViewModel content)
        {

            if (document != null && content.IsAllowed == true)
            {
                if (document.Length > 0)
                {
                    content.uploadDate = DateTime.Now;
                    content.fileName = "no-document";
                    content.fileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(document.FileName);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "DocumentDistrict", content.userUploaderArea + "-" + content.userUploaderId + "-" + content.userReceiverArea + "-" + content.userReceiverId
                        + "-" + content.title + "-" + content.uploadDate.ToShamsiFileUpload() + Path.GetExtension(document.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        document.CopyTo(stream);
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region UploadDocumentDB
        public bool UploadProvinceDB(IFormFile document, UploadViewModel content, List<int> SelectedRoles)
        {
            if (document != null)
            {
                if (document.Length > 0)
                {
                    content.fileName = "-";
                    int selectedRoles = 0;
                    if (content.description == null)
                    {
                        content.description = "بدون توضیح";
                    }
                    if (content.userReceiverId == null)
                    {
                        content.roleReceiver = "-";
                        content.userReceiverId = "-";
                    }
                    else if (content.userReceiverId != null)
                    {
                        if (content.userReceiverArea == "استان")
                        {
                            content.roleReceiver = GetDeletedRoleWithnationalCodeProvince(content.userReceiverId);
                        }
                        else if (content.userReceiverArea == "شهرستان")
                        {
                            content.roleReceiver = GetDeletedRoleWithnationalCodeCounty(content.userReceiverId);
                        }
                        else if (content.userReceiverArea == "بخش")
                        {
                            content.roleReceiver = GetDeletedRoleWithnationalCodeDistrict(content.userReceiverId);
                        }
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        foreach (var item in SelectedRoles)
                        {
                            selectedRoles = item;
                        }
                        content.roleReceiver = GetRoleTitleWithRoleId(selectedRoles);
                    }

                    Random random = new Random();
                    var docName = Path.GetFileName(document.FileName);
                    var fileExtension = Path.GetExtension(docName);
                    var newFileName = String.Concat(random.Next(1000, 9999), fileExtension);
                    var objfiles = new TransferDocumentsBetweenLevels()
                    {
                        fileName = newFileName,
                        uploadDate = DateTime.Now,
                        ProvinceOrigin = content.ProvinceOrigin,
                        CountyOrigin = content.CountyOrigin,
                        DistrictOrigin = content.DistrictOrigin,
                        ProvinceDestination = content.ProvinceDestination,
                        CountyDestination = content.CountyDestination,
                        DistrictDestination = content.DistrictDestination,
                        userReceiverId = content.userReceiverId,
                        userUploaderId = content.userUploaderId,
                        userReceiverArea = content.userReceiverArea,
                        userUploaderArea = content.userUploaderArea,
                        IsAllowed = content.IsAllowed,
                        title = content.title,
                        contentType = document.ContentType,
                        fileFormat = fileExtension,
                        roleUploader = content.roleUploader,
                        roleReceiver = content.roleReceiver,
                        description = content.description,
                        IsDelete = false
                    };
                    using (var target = new MemoryStream())
                    {
                        document.CopyTo(target);
                        objfiles.dataBytes = target.ToArray();
                    }
                    _context.transferDocuments.Add(objfiles);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool UploadCountyDB(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> SelectedRoles)
        {

            if (document != null && content.IsAllowed)
            {
                if (document.Length > 0)
                {
                    int countyDestination = 0;
                    int selectedRoles = 0;

                    foreach (int item in CountyDestination)
                    {
                        countyDestination = item;
                    };
                    content.fileName = "-";
                    if (content.description == null)
                    {
                        content.description = "بدون توضیح";
                    }
                    if (content.userReceiverId == null)
                    {
                        content.roleReceiver = "-";
                        content.userReceiverId = "-";
                    }
                    else if (content.userReceiverId != null)
                    {
                        if (content.userReceiverArea == "استان")
                        {
                            content.roleReceiver = GetDeletedRoleWithnationalCodeProvince(content.userReceiverId);
                        }
                        else if (content.userReceiverArea == "شهرستان")
                        {
                            content.roleReceiver = GetDeletedRoleWithnationalCodeCounty(content.userReceiverId);
                        }
                        else if (content.userReceiverArea == "بخش")
                        {
                            content.roleReceiver = GetDeletedRoleWithnationalCodeDistrict(content.userReceiverId);
                        }
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        foreach (var item in SelectedRoles)
                        {
                            selectedRoles = item;
                        }
                        content.roleReceiver = GetRoleTitleWithRoleId(selectedRoles);
                    }

                    Random random = new Random();
                    var docName = Path.GetFileName(document.FileName);
                    var fileExtension = Path.GetExtension(docName);
                    var newFileName = String.Concat(random.Next(1000, 9999), fileExtension);
                    var objfiles = new TransferDocumentsBetweenLevels()
                    {
                        fileName = newFileName,
                        uploadDate = DateTime.Now,
                        ProvinceOrigin = content.ProvinceOrigin,
                        CountyOrigin = content.CountyOrigin,
                        DistrictOrigin = content.DistrictOrigin,
                        ProvinceDestination = content.ProvinceDestination,
                        CountyDestination = countyDestination,
                        DistrictDestination = content.DistrictDestination,
                        userReceiverId = content.userReceiverId,
                        userUploaderId = content.userUploaderId,
                        userReceiverArea = content.userReceiverArea,
                        userUploaderArea = content.userUploaderArea,
                        IsAllowed = content.IsAllowed,
                        title = content.title,
                        contentType = document.ContentType,
                        fileFormat = fileExtension,
                        roleUploader = content.roleUploader,
                        roleReceiver = content.roleReceiver,
                        description = content.description,
                        IsDelete = false
                    };
                    using (var target = new MemoryStream())
                    {
                        document.CopyTo(target);
                        objfiles.dataBytes = target.ToArray();
                    }
                    _context.transferDocuments.Add(objfiles);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public bool UploadDistrictDB(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> DistrictDestination, List<int> SelectedRoles)
        {

            if (document != null && content.IsAllowed)
            {
                if (document.Length > 0)
                {
                    int countyDestination = 0;
                    int districtDestination = 0;
                    int selectedRoles = 0;

                    foreach (int item in CountyDestination)
                    {
                        countyDestination = item;
                    };
                    foreach (int item in DistrictDestination)
                    {
                        districtDestination = item;
                    };
                    content.fileName = "-";
                    if (content.description == null)
                    {
                        content.description = "بدون توضیح";
                    }
                    if (content.userReceiverId == null)
                    {
                        content.roleReceiver = "-";
                        content.userReceiverId = "-";
                    }
                    else if (content.userReceiverId != null)
                    {
                        if (content.userReceiverArea == "استان")
                        {
                            content.roleReceiver = GetDeletedRoleWithnationalCodeProvince(content.userReceiverId);
                        }
                        else if (content.userReceiverArea == "شهرستان")
                        {
                            content.roleReceiver = GetDeletedRoleWithnationalCodeCounty(content.userReceiverId);
                        }
                        else if (content.userReceiverArea == "بخش")
                        {
                            content.roleReceiver = GetDeletedRoleWithnationalCodeDistrict(content.userReceiverId);
                        }
                    }
                    if (SelectedRoles.Count != 0)
                    {
                        foreach (var item in SelectedRoles)
                        {
                            selectedRoles = item;
                        }
                        content.roleReceiver = GetRoleTitleWithRoleId(selectedRoles);
                    }

                    Random random = new Random();
                    var docName = Path.GetFileName(document.FileName);
                    var fileExtension = Path.GetExtension(docName);
                    var newFileName = String.Concat(random.Next(1000, 9999), fileExtension);
                    var objfiles = new TransferDocumentsBetweenLevels()
                    {
                        fileName = newFileName,
                        uploadDate = DateTime.Now,
                        ProvinceOrigin = content.ProvinceOrigin,
                        CountyOrigin = content.CountyOrigin,
                        DistrictOrigin = content.DistrictOrigin,
                        ProvinceDestination = content.ProvinceDestination,
                        CountyDestination = countyDestination,
                        DistrictDestination = districtDestination,
                        userReceiverId = content.userReceiverId,
                        userUploaderId = content.userUploaderId,
                        userReceiverArea = content.userReceiverArea,
                        userUploaderArea = content.userUploaderArea,
                        IsAllowed = content.IsAllowed,
                        title = content.title,
                        contentType = document.ContentType,
                        fileFormat = fileExtension,
                        roleUploader = content.roleUploader,
                        roleReceiver = content.roleReceiver,
                        description = content.description,
                        IsDelete = false
                    };
                    using (var target = new MemoryStream())
                    {
                        document.CopyTo(target);
                        objfiles.dataBytes = target.ToArray();
                    }
                    _context.transferDocuments.Add(objfiles);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region ShowDescription
        public TransferDocumentsBetweenLevels getTransferDocumentWithId(int id)
        {
            return _context.transferDocuments.SingleOrDefault(d => d.id == id);
        }
        public TransferDocumentsBetweenLevels getDeletedTransferDocumentWithId(int id)
        {
            return _context.transferDocuments.IgnoreQueryFilters().SingleOrDefault(d => d.id == id);
        }
        #endregion

        #region IsExistSpecifiedRole
        public bool IsExistSpecifiedRoleProvince(List<int> RoleId)
        {
            int role = 0;
            foreach (var item in RoleId)
            {
                role = item;
            }
            return _context.ProvinceLevel.Any(r => r.role == GetRoleTitleWithRoleId(role));
        }
        public bool IsExistSpecifiedRoleCounty(List<int> RoleId, List<int> CountyId)
        {
            int role = 0;
            int county = 0;
            foreach (var item in RoleId)
            {
                role = item;
            }
            foreach (var item in CountyId)
            {
                county = item;
            }
            return _context.CountyLevels.Where(r => r.department == county).Any(r => r.role == GetRoleTitleWithRoleId(role));
        }
        public bool IsExistSpecifiedRoleDistrict(List<int> RoleId, List<int> CountyId, List<int> DistrictId)
        {
            int role = 0;
            int county = 0;
            int district = 0;
            foreach (var item in RoleId)
            {
                role = item;
            }
            foreach (var item in CountyId)
            {
                county = item;
            }
            foreach (var item in DistrictId)
            {
                district = item;
            }
            return _context.DistrictLevels.Where(r => r.county == county && r.department == district).Any(r => r.role == GetRoleTitleWithRoleId(role));
        }
        #endregion

        #region IsExistMember
        public bool IsExistMemberCounty(List<int> CountyId)
        {
            int department = 0;
            foreach(var item in CountyId)
            {
                department = item; 
            };  
            return _context.CountyLevels.Any(u=>u.department == department);
        }
        public bool IsExistMemberDistrict(List<int> CountyId, List<int> DistrictId)
        {
            int county = 0;
            int department = 0;
            foreach (var item in CountyId)
            {
                county = item;
            };
            foreach (var item in DistrictId)
            {
                department = item;
            }
            return _context.DistrictLevels.Any(u => u.county==county && u.department == department);
        }
        #endregion

        #region Chart
        public List<int> CountEmployeesPerDepartment(string area, int county, int department)
        {
            var employees = new List<int>();
            int activeUsers = _context.Employees.Where(u => u.area == area && u.country == county && u.department == department).ToList().Count;
            int deletedUsers = _context.Employees.IgnoreQueryFilters().Where(u => u.IsDelete && u.area == area && u.country == county && u.department == department).ToList().Count;
            employees.Add(activeUsers);
            employees.Add(deletedUsers);
            return employees;
        }
        public List<ChartValues> RoleCount(string area, int county, int department)
        {
            var data = _context.Employees.Where(c => c.area == area && c.country == county && c.department == department).ToList();
            var roleTitles = _context.Roles.Select(r => r.RoleTitle).ToList();
            var role = new List<ChartValues>();
            for (int i = 0; i < roleTitles.Count; i++)
            {
                int count = data.Where(r => r.role == roleTitles[i]).ToList().Count;
                role.Add(new ChartValues { chartTitle = roleTitles[i], chartValue = count });
            }
            return role;
        }
        public List<ChartValues> GenderCount(string area, int county, int department)
        {
            var genderTitles = _context.Genders.Select(u => u.genderTitle).ToList();
            var employees = _context.Employees.Where(u => u.area == area && u.country == county && u.department == department).ToList();
            var gender = new List<ChartValues>();
            for (int i = 0; i < genderTitles.Count; i++)
            {
                int count = employees.Where(g => g.gender == genderTitles[i]).ToList().Count;
                gender.Add(new ChartValues { chartTitle = genderTitles[i], chartValue = count });
            }
            return gender;
        }
        public List<ChartValues> EmploymentCount(string area, int county, int department)
        {
            var employmentTitles = _context.Employments.Select(e => e.EmploymentTitle).ToList();
            var employees = _context.Employees.Where(u => u.area == area && u.country == county && u.department == department).ToList();
            var employment = new List<ChartValues>();
            for (int i = 0; i < employmentTitles.Count; i++)
            {
                int count = employees.Where(e => e.employmentStatus == employmentTitles[i]).ToList().Count;
                employment.Add(new ChartValues { chartTitle = employmentTitles[i], chartValue = count });
            }
            return employment;
        }
        public List<ChartValues> EducationDegreeCount(string area, int county, int department)
        {
            var educationTitle = new List<string> { "زیر دیپلم", "دیپلم", "فوق دیپلم", "کارشناسی", "کارشناسی ارشد", "دکترا" };
            var education = new List<ChartValues>();
            for (int i = 0; i < educationTitle.Count; i++)
            {
                int count = _context.Employees.Where(u => u.area == area && u.country == county && u.department == department && u.education == educationTitle[i]).ToList().Count;
                education.Add(new ChartValues { chartTitle = educationTitle[i], chartValue = count });

            }
            return education;
        }
        #endregion

        #region GetIdWithNationalCodeForForeignKey
        public int GetIdWithNationalCodeForForeignKeyProvince(string nationalCode)
        {
            return _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == nationalCode).userId;
        }
        public int GetIdWithNationalCodeForForeignKeyCounty(string nationalCode)
        {
            return _context.CountyLevels.SingleOrDefault(u => u.nationalCode == nationalCode).userId;
        }
        public int GetIdWithNationalCodeForForeignKeyDistrict(string nationalCode)
        {
            return _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == nationalCode).userId;
        }
        #endregion
    }
}