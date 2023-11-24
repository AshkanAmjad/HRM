using ERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services
{
    public interface IManagementService
    {
        #region Unique
        //فراخوانی تابع تکراری نبودن کد ملی در سطح استان
        bool IsExistNationalCodeProvince(string nationalCode);
        //فراخوانی تابع تکراری نبودن کد ملی در سطح شهرستان
        bool IsExistNationalCodeCounty(string nationalCode);
        //فراخوانی تابع تکراری نبودن کد ملی با کد ملی و شعبه در سطح شهرستان
        bool IsExistNationalCodeWithDepartrmentCounty(string nationalCode, List<int> departmentList);
        //فراخوانی تابع تکراری نبودن کد ملی در سطح بخش
        bool IsExistNationalCodeDistrict(string nationalCode);
        //فراخوانی تابع تکراری نبودن کد ملی با کد ملی و شعبه و شعبه شهرستان در سطح بخش
        bool IsExistNationalCodeWithCountyAndDepartrmentDistrict(string nationalCode, List<int> countyList, List<int> departmentList);
        //فراخوانی تابع تکراری نبودن کد ملی حذف شده در سطح استان
        bool IsExistDeletedNationalCodeProvince(string nationalCode);
        //فراخوانی تابع تکراری نبودن کد ملی حذف شده در سطح شهرستان
        bool IsExistDeletedNationalCodeCounty(string nationalCode);
        //فراخوانی تابع تکراری نبودن کد ملی حذف شده در سطح بخش
        bool IsExistDeletedNationalCodeDistrict(string nationalCode);
        #endregion

        #region IsExistDeletedUserOnArchive
        //وجودیت کاربر حذف شده در باگانی در سطح استان
        bool IsExistDeletedUserOnArchiveProvince(string nationalCode);
        //وجودیت کاربر حذف شده در باگانی در سطح شهرستان
        bool IsExistDeletedUserOnArchiveCounty(string nationalCode);
        //وجودیت کاربر حذف شده در باگانی در سطح بخش
        bool IsExistDeletedUserOnArchiveDistrict(string nationalCode);
        #endregion

        #region Register

        //فراخوانی تابع اظافه کردن عضو در پایگاه داده استان
        string AddProvince(ProvinceLevel employee);

        //فراخوانی تابع اظافه کردن عضو در پایگاه داده شهرستان
        string AddCounty(CountyLevel employee);

        //فراخوانی تابع اظافه کردن عضو در پایگاه داده بخش
        string AddDistrict(DistrictLevel employee);

        //فراخوانی تابع اظافه کردن عضو در پایگاه داده پرسنل
        string AddEmployees(Employees employee);

        #endregion

        #region ForgotPassword
        //استخراج کاربر برای فراموشی رمز عبور در سطح استان
        ProvinceLevel GetUserForgotPassProvince(ForgotPasswordViewModel forgot);
        ProvinceLevel GetUserForgotPassDeletedProvince(ForgotPasswordViewModel forgot);
        //استخراج کاربر برای فراموشی رمز عبور در سطح شهرستان
        CountyLevel GetUserForgotPassCounty(ForgotPasswordViewModel nationalCode);
        CountyLevel GetUserForgotPassDeletedCounty(ForgotPasswordViewModel nationalCode);
        //استخراج کاربر برای فراموشی رمز عبور در سطح بخش
        DistrictLevel GetUserForgotPassDistrict(ForgotPasswordViewModel nationalCode);
        DistrictLevel GetUserForgotPassDeletedDistrict(ForgotPasswordViewModel nationalCode);
        #endregion

        #region ResetPassword
        //تغییر رمز عبور در سطح استان
        bool ResetPassswordProvince(ResetPassword reset);
        //تغییر رمز عبور در سطح شهرستان
        bool ResetPassswordCounty(ResetPassword reset);
        //تغییر رمز عبور در سطح بخش
        bool ResetPassswordDistrict(ResetPassword reset);
        #endregion

        #region Register&Roles
        //استخراج نقش از جدول نقش ها
        List<Role> GetRoles();
        //اضافه عضو و نقش او به پایگاه داده سطح استان
        bool AddUserAndRolesProvince(List<int> roleId, List<int> genderId, List<int> employmentId, List<int> maritalId, RegisterViewModel register, string roleUploader);
        //اضافه عضو و نقش او به پایگاه داده سطح شهرستان
        bool AddUserAndRolesCounty(List<int> roleId, List<int> genderId, List<int> departmentId, List<int> employmentId, List<int> maritalId, RegisterViewModel register, string roleUploader);
        //اضافه عضو و نقش او به پایگاه داده سطح بخش
        bool AddUserAndRolesDistrict(List<int> roleId, List<int> genderId, List<int> departmentId, List<int> employmentId, List<int> maritalId, List<int> countyId, RegisterViewModel register, string roleUploader);

        #endregion

        #region Genders
        //استخراج جنسیت از جدول جنسیت ها
        List<Gender> GetGenders();
        #endregion

        #region Employments
        //استخراج وضعیت استخدام از جدول وضعیت استخدام
        List<Employment> GetEmployments();
        #endregion

        #region Martitals
        //استخراج وضعیت تاهل از جدول وضعیت تاهل
        List<Marital> GetMaritals();
        #endregion

        #region Departments
        //استخراج شعبه ها از جدول شعبه ها
        List<Department> GetDepartments();
        #endregion

        #region Counties
        //استخراج شهرستان بخش مورد فعالیت از جدول شهرستان ها
        List<County> GetCounties();
        #endregion

        #region edit
        // استخراج مشخصات کاربر با کد ملی برای نمایش در صفحه ویرایش در سطح استان
        EditViewModel GetUserForShowInEditProvince(String nationalCode);
        //ویرایش مشخصات کاربر در سطح استان
        bool EditUserAndRolesProvince(int roleId, List<int> employmentId, List<int> maritalId, List<int> genderId, EditViewModel edit, string roleUploader);
        // استخراج مشخصات کاربر با کد ملی برای نمایش در صفحه ویرایش در سطح شهرستان
        EditViewModel GetUserForShowInEditCounty(String nationalCode);
        //ویرایش مشخصات کاربر در سطح شهرستان
        bool EditUserAndRolesCounty(int roleId, List<int> employmentId, List<int> maritalId, int departmentId, List<int> genderId, EditViewModel edit, string roleUploader);
        //استخراج مشخصات کاربر با کد ملی برای نمایش در صفحه ویرایش در سطح بخش
        EditViewModel GetUserForShowInEditDistrict(String nationalCode);
        //ویرایش مشخصات کاربر در سطح بخش
        bool EditUserAndRolesDistrict(int roleId, List<int> employmentId, List<int> maritalId, int departmentId, List<int> genderId, int countyId, EditViewModel edit, string roleUploader);
        #endregion

        #region Login

        //تابع ورود سطح استان
        ProvinceLevel PLoginUser(LoginViewModel login);
        ProvinceLevel PLoginUserDeleted(LoginViewModel login);

        //تابع ورود سطح شهرستان
        CountyLevel CLoginUser(LoginViewModel login);
        CountyLevel CLoginUserDeleted(LoginViewModel login);


        //تابع ورود سطح بخش
        DistrictLevel DLoginUser(LoginViewModel login);
        DistrictLevel DLoginUserDeleted(LoginViewModel login);

        //تابع ورود در سطح کارمندان
        Employees ELoginUser(LoginViewModel login, string area);


        #endregion

        #region GetUserByUserName
        //پیدا کردن کاربر با شناسه
        Employees GetUserByUserName(int userId);
        //پیدا کردن کاربر با شناسه در سطح استان
        ProvinceLevel GetUserByUserNameProvinceLevel(int userId);
        //پیدا کردن کاربر با شناسه در سطح شهرستان
        CountyLevel GetUserByUserNameCountyLevel(int userId);
        //پیدا کردن کاربر با شناسه در سطح بخش
        DistrictLevel GetUserByUserNameDistrictLevel(int userId);
        #endregion

        #region GetUserBynationalCode
        //استخراج اطلاعات از جدول پرسنل
        Employees GetUserBynationalCode(string nationalCode);
        //استخراج اطلاعات استان از جدول پرسنل
        Employees GetUserBynationalCodeP(string nationalCode);
        //استخراج اطلاعات شهرستان از جدول پرسنل
        Employees GetUserBynationalCodeC(string nationalCode);
        //استخراج اطلاعات بخش از جدول پرسنل
        Employees GetUserBynationalCodeD(string nationalCode);
        //استخراج اطلاعات از جدول استان
        ProvinceLevel GetUserBynationalCodeProvinceLevel(string nationalCode);
        //استخراج اطلاعات از جدول شهرستان
        CountyLevel GetUserBynationalCodeCountyLevel(string nationalCode);
        //استخراج اطلاعات از جدول بخش
        DistrictLevel GetUserBynationalCodeDistrictLevel(string nationalCode);
        #endregion

        #region GetUserDeletedBynationalCode
        //پیدا کردن کاربر حذف شده با شناسه
        Employees GetUserDeletedBynationalCode(string nationalCode);
        //پیدا کردن کاربر با شناسه در سطح استان
        ProvinceLevel GetUserDeletedBynationalCodeProvinceLevel(string nationalCode);
        //پیدا کردن کاربر با شناسه در سطح شهرستان
        CountyLevel GetUserDeletedBynationalCodeCountyLevel(string nationalCode);
        //پیدا کردن کاربر با شناسه در سطح بخش
        DistrictLevel GetUserDeletedBynationalCodeDistrictLevel(string nationalCode);
        #endregion

        #region GetUserInformation
        //استخراج اطلاعات کاربران سطح استان با شناسه کاربر
        InformationViewModel GetUserInformationByuserIdProvince(int userId);
        //استخراج اطلاعات کاربران سطح شهرستان با شناسه کاربر
        InformationViewModel GetUserInformationByuserIdCounty(int userId);
        //استخراج اطلاعات کاربران سطح بخش با شناسه کاربر
        InformationViewModel GetUserInformationByuserIdDistrict(int userId);
        //استخراج اطلاعات کاربران با شناسه کاربر
        InformationViewModel GetUserInformationByuserId(int userId);

        //استخراج اطلاعات کاربران سطوح مختلف با کدملی کاربر
        InformationViewModel GetUserInformationBynationalCodeProvince(string nationalCode);
        InformationViewModel GetUserInformationBynationalCodeCounty(string nationalCode);
        InformationViewModel GetUserInformationBynationalCodeDistrict(string nationalCode);
        InformationViewModel GetUserInformationBynationalCode(string nationalCode);
        #endregion

        #region GetProfileUserInformation
        //استراج اطلاعات کاربر در صفحه حساب کاربری سطح استان
        ProfileInformationViewModel GetProfileUserInformationBynationalCodeProvince(string nationalCode);
        //استراج اطلاعات کاربر در صفحه حساب کاربری سطح شهرستان
        ProfileInformationViewModel GetProfileUserInformationBynationalCodeCounty(string nationalCode);
        //استراج اطلاعات کاربر در صفحه حساب کاربری سطح بخش
        ProfileInformationViewModel GetProfileUserInformationBynationalCodeDistrict(string nationalCode);

        #endregion

        #region GetDepartmentIdWithnationalCode
        //استخراج شعبه با کدملی از جدول کارکنان سطح استان
        int GetDepartmentIdWithnationalCodeProvince(string nationalCode);
        //استخراج شعبه با کدملی از جدول کارکنان سطح شهرستان
        int GetDepartmentIdWithnationalCodeCounty(string nationalCode);
        //استخراج شعبه با کدملی از جدول کارکنان سطح بخش
        int GetDepartmentIdWithnationalCodeDistrict(string nationalCode);
        //استخراج شعبه کاربر حذف شده با کدملی از جدول کارکنان سطح استان
        int GetDeletedDepartmentIdWithnationalCodeProvince(string nationalCode);
        //استخراج شعبه کاربر حذف شده با کدملی از جدول کارکنان سطح شهرستان
        int GetDeletedDepartmentIdWithnationalCodeCounty(string nationalCode);
        //استخراج شعبه کاربر حذف شده با کدملی از جدول کارکنان سطح بخش
        int GetDeletedDepartmentIdWithnationalCodeDistrict(string nationalCode);

        #endregion

        #region GetCountyIdWithnationalCode
        //استخراج شعبه شهرستان با کدملی از جدول کارکنان سطح بخش
        int GetCountyIdWithnationalCodeDistrict(string nationalCode);
        //استخراج شعبه شهرستان کاربر حذف شده با کدملی از جدول کارکنان سطح بخش
        int GetDeletedCountyIdWithnationalCodeDistrict(string nationalCode);
        #endregion

        #region GetRegisterDateWithnationalCode
        //استخراج زمان ثبت نام با کدملی در سطح استان
        DateTime GetRegisterDateWithnationalCodeProvince(string nationalCode);
        //استخراج زمان ثبت نام با کدملی در سطح شهرستان
        DateTime GetRegisterDateWithnationalCodeCounty(string nationalCode);
        //استخراج زمان ثبت نام با کدملی در سطح بخش
        DateTime GetRegisterDateWithnationalCodeDitrict(string nationalCode);
        //استخراج زمان ثبت نام استان با کدملی
        DateTime GetRegisterDateWithnationalCodeP(string nationalCode);
        //استخراج زمان ثبت نام شهرستان با کدملی
        DateTime GetRegisterDateWithnationalCodeC(string nationalCode);
        //استخراج زمان ثبت نام بخش با کدملی
        DateTime GetRegisterDateWithnationalCodeD(string nationalCode);
        //استخراج زمان ثبت نام کاربر حذف شده با کدملی در سطح استان
        DateTime GetDeletedRegisterDateWithnationalCodeProvince(string nationalCode);
        //استخراج زمان ثبت نام کاربر حذف شده با کدملی در سطح شهرستان
        DateTime GetDeletedRegisterDateWithnationalCodeCounty(string nationalCode);
        //استخراج زمان ثبت نام کاربر حذف شده با کدملی در سطح بخش
        DateTime GetDeletedRegisterDateWithnationalCodeDitrict(string nationalCode);
        //استخراج زمان ثبت نام کاربر حذف شده استان با کدملی
        DateTime GetDeletedRegisterDateWithnationalCodeP(string nationalCode);
        //استخراج زمان ثبت نام کاربر حذف شده شهرستان با کدملی
        DateTime GetDeletedRegisterDateWithnationalCodeC(string nationalCode);
        //استخراج زمان ثبت نام کاربر حذف شده بخش با کدملی
        DateTime GetDeletedRegisterDateWithnationalCodeD(string nationalCode);

        #endregion

        #region EditUserProfile
        //استخراج مشخصات حساب کاربری در سطح استان
        EditProfileViewModel GetUserForShowInEditProfileProvince(String nationalCode);
        //ویرایش مشخصات حساب کاربری در سطح استان
        bool EditUserProfileProvince(List<int> maritalId, EditProfileViewModel edit, string roleUploader);
        //استخراج مشخصات حساب کاربری در سطح شهرستان
        EditProfileViewModel GetUserForShowInEditProfileCounty(String nationalCode);
        //ویرایش مشخصات حساب کاربری در سطح شهرستان
        bool EditUserProfileCounty(List<int> maritalId, EditProfileViewModel edit, string roleUploader);
        //استخراج مشخصات حساب کاربری در سطح بخش
        EditProfileViewModel GetUserForShowInEditProfileDistrict(String nationalCode);
        //ویرایش مشخصات حساب کاربری در سطح بخش
        bool EditUserProfileDistrict(List<int> maritalId, EditProfileViewModel edit, string roleUploader);
        #endregion

        #region DeleteAvatrPicOnEditDB
        // استخراج اطلاعات سند با کد ملی و عنوان از جدول اسناد و حذف آن در سطح استان
        bool DeleteAvatrPicOnEditProvinceLevel(string nationalCode, string title);
        // استخراج اطلاعات سند با کد ملی و عنوان از جدول اسناد و حذف آن در سطح شهرستان
        bool DeleteAvatrPicOnEditCountyLevel(string nationalCode, string title);
        // استخراج اطلاعات سند با کد ملی و عنوان از جدول اسناد و حذف آن در سطح بخش
        bool DeleteAvatrPicOnEditDistrictLevel(string nationalCode, string title);
        #endregion

        #region DeleteAvatarPicOnPC
        //حذف تصویر پرسنل از سرور در سطح استان
        bool DeleteAvatarPicOnProvincePC(string nationalCode);
        //حذف تصویر پرسنل از سرور در سطح شهرستان
        bool DeleteAvatarPicOnCountyPC(string nationalCode);
        //حذف تصویر پرسنل از سرور در سطح بخش
        bool DeleteAvatarPicOnDistrictPC(string nationalCode);
        #endregion

        #region ShowInPageDelete
        //نمایش مشخصات کاربر در صفحه حذف در سطح استان
        InformationViewModel ListDeleteUsersProvince(string nationalCode);
        //نمایش مشخصات کاربر در صفحه حذف در سطح شهرستان
        InformationViewModel ListDeleteUsersCounty(string nationalCode);
        //نمایش مشخصات کاربر در صفحه حذف در سطح بخش
        InformationViewModel ListDeleteUsersDistrict(string nationalCode);
        #endregion

        #region UpdateUser
        //به روز رسانی مشخصات کاربر در سطح استان
        void UpdateUserProvince(ProvinceLevel user);
        //به روز رسانی مشخصات کاربر در سطح شهرستان
        void UpdateUserCounty(CountyLevel user);
        //به روز رسانی مشخصات کاربر در سطح بخش
        void UpdateUserDistrict(DistrictLevel user);
        //به روز رسانی مشخصات کاربر در جدول پرسنل
        void UpdateUser(Employees user);
        #endregion

        #region DeleteUser
        //حذف کاربر در سطح استان
        bool DeleteUserProvince(string nationalCode);
        //حذف کاربر در سطح شهرستان
        bool DeleteUserCounty(string nationalCode);
        //حذف کاربر در سطح بخش
        bool DeleteUserDistrict(string nationalCode);
        //حذف کاربر استان در جدول پرسنل
        bool DeleteUserP(string nationalCode);
        //حذف کاربر شهرستان در جدول پرسنل
        bool DeleteUserC(string nationalCode);
        //حذف کاربر بخش در جدول پرسنل
        bool DeleteUserD(string nationalCode);

        #endregion

        #region IsExistUserDocumentsOnDocumentDB
        //وجودیت مدارک کاربر در جدول مدارک در سطح استان
        bool IsExistUserDocumentsOnDocumentProvinceDB(string nationalCode);
        //وجودیت مدارک کاربر در جدول مدارک در سطح شهرستان
        bool IsExistUserDocumentsOnDocumentCountyDB(string nationalCode);
        //وجودیت مدارک کاربر در جدول مدارک در سطح بخش
        bool IsExistUserDocumentsOnDocumentDistrictDB(string nationalCode);
        #endregion

        #region DeleteUserDocumentsDB
        //حذف مدارک کاربر با حذف کاربر در سطح استان
        bool DeleteUserDocumentsProvinceDB(string nationalCode);
        //حذف مدارک کاربر با حذف کاربر در سطح شهرستان
        bool DeleteUserDocumentsCountyDB(string nationalCode);
        //حذف مدارک کاربر با حذف کاربر در سطح بخش
        bool DeleteUserDocumentsDistrictDB(string nationalCode);
        #endregion

        #region ActivationUserDocumentsDeleted
        //فعال سازی مدارک کاربر حذف شده در سطح استان
        bool ActivationDeletedUserDocumentsProvince(string nationalCode);
        //فعال سازی مدارک کاربر حذف شده در سطح شهرستان
        bool ActivationDeletedUserDocumentsCounty(string nationalCode);
        //فعال سازی مدارک کاربر حذف شده در سطح بخش
        bool ActivationDeletedUserDocumentsDistrict(string nationalCode);
        #endregion

        #region GetUserDeletedInformation
        //استخراج اطلاعات کاربران حذف شده سطح استان با شناسه کاربر
        ProvinceLevel GetUserDeletedInformationBynationalCodeProvince(string nationalCode);
        //استخراج اطلاعات کاربران حذف شده سطح شهرستان با شناسه کاربر
        CountyLevel GetUserDeletedInformationBynationalCodeCounty(string nationalCode);
        //استخراج اطلاعات کاربران حذف شده سطح بخش با شناسه کاربر
        DistrictLevel GetUserDeletedInformationBynationalCodeDistrict(string nationalCode);
        //استخراج اطلاعات کاربران حذف شده با شناسه کاربر
        Employees GetUserDeletedInformationBynationalCode(string nationalCode);
        #endregion

        #region ActivationUserDeleted
        //فعال سازی کاربر حذف شده در سطح استان
        bool ActivationDeletedUserProvince(string nationalCode);
        //فعال سازی کاربر حذف شده در سطح شهرستان
        bool ActivationDeletedUserCounty(string nationalCode);
        //فعال سازی کاربر حذف شده در سطح بخش
        bool ActivationDeletedUserDistrict(string nationalCode);
        //فعال سازی کاربر حذف شده در جدول پرسنل
        bool ActivationDeletedUser(string nationalCode);
        //فعال سازی کاربر استان حذف شده در جدول پرسنل
        bool ActivationDeletedUserP(string nationalCode);
        //فعال سازی کاربر شهرستان حذف شده در جدول پرسنل
        bool ActivationDeletedUserC(string nationalCode);
        //فعال سازی کاربر بخش حذف شده در جدول پرسنل
        bool ActivationDeletedUserD(string nationalCode);
        #endregion

        #region GetRoleTitleWithRoleId
        //استخراج عنوان نقش با شناسه نقش
        string GetRoleTitleWithRoleId(int roleId);
        //استخراج عنوان جنسیت با شناسه جنسیت
        string GetGenderTitleWithGenderId(int genderId);
        //استخراج عنوان تاهل با شناسه تاهل
        string GetMaritalTitleWithMaritalId(int maritalId);
        //استخراج عنوان وضعیت استخدام با شناسه وضعیت استخدام
        string GetEmployentTitleWithEmployentId(int employentId);

        #endregion

        #region GetRoleIdWithRoleTitle
        //استخراج شناسه نقش با عنوان نقش در سطح استان
        int GetRoleIdWithRoleTitleProvince(string nationalCode);
        //استخراج شناسه نقش با عنوان نقش در سطح شهرستان
        int GetRoleIdWithRoleTitleCounty(string nationalCode);
        //استخراج شناسه نقش با عنوان نقش در سطح بخش
        int GetRoleIdWithRoleTitleDistrict(string nationalCode);
        #endregion

        #region GetGenderIdWithGenderTitle
        //استخراج شناسه جنسسیت با عنوان جنسسیت در سطح استان
        int GetGenderIdWithGenderTitleProvince(string nationalCode);
        //استخراج شناسه جنسسیت با عنوان جنسسیت در سطح شهرستان
        int GetGenderIdWithGenderTitleCounty(string nationalCode);
        //استخراج شناسه جنسسیت با عنوان جنسسیت در سطح بخش
        int GetGenderIdWithGenderTitleDistrict(string nationalCode);
        #endregion

        #region GetMaritalIdWithMaritalTitle
        //استخراج شناسه تاهل با عنوان تاهل در سطح استان
        int GetMaritalIdWithMaritalTitleProvince(string nationalCode);
        //استخراج شناسه تاهل با عنوان تاهل در سطح شهرستان
        int GetMaritalIdWithMaritalTitleCounty(string nationalCode);
        //استخراج شناسه تاهل با عنوان تاهل در سطح بخش
        int GetMaritalIdWithMaritalTitleDistrict(string nationalCode);
        #endregion

        #region GetEmploymentIdWithEmploymentTitle
        //استخراج شناسه تاهل با عنوان تاهل در سطح استان
        int GetEmploymentIdWithEmploymentTitleProvince(string nationalCode);
        //استخراج شناسه تاهل با عنوان تاهل در سطح شهرستان
        int GetEmploymentIdWithEmploymentTitleCounty(string nationalCode);
        //استخراج شناسه تاهل با عنوان تاهل در سطح بخش
        int GetEmploymentIdWithEmploymentTitleDistrict(string nationalCode);
        #endregion

        #region UploadAvatarPC
        //بارگذاری عکس در سرور سطح استان
        bool UploadAvatarProvincePC(IFormFile file, RegisterViewModel document);
        //بارگذاری عکس در سرور سطح شهرستان
        bool UploadAvatarCountyPC(IFormFile file, RegisterViewModel document);
        //بارگذاری عکس در سرور سطح بخش
        bool UploadAvatarDistrictPC(IFormFile file, RegisterViewModel document);
        #endregion

        #region UploadAvatarDB
        //بارگذاری عکس در پایگاه داده سطح استان
        bool UploadAvatarProvinceDB(IFormFile file, RegisterViewModel document, string roleUploader);
        //بارگذاری عکس در پایگاه داده سطح شهرستان
        bool UploadAvatarCountyDB(IFormFile file, RegisterViewModel document, List<int> departmentId, string roleUploader);
        //بارگذاری عکس در پایگاه داده سطح بخش
        bool UploadAvatarDistrictDB(IFormFile file, RegisterViewModel document, List<int> departmentId, List<int> selectedCounty, string roleUploader);
        #endregion

        #region UploadEditAvatarPC
        //بارگذاری ویرایش عکس در سرور سطح استان
        bool UploadEditAvatarProvincePC(IFormFile file, EditViewModel document);
        //بارگذاری ویرایش عکس در سرور سطح شهرستان
        bool UploadEditAvatarCountyPC(IFormFile file, EditViewModel document);
        //بارگذاری ویرایش عکس در سرور سطح بخش
        bool UploadEditAvatarDistrictPC(IFormFile file, EditViewModel document);
        #endregion

        #region UploadEditAvatarDB
        //بارگذاری ویرایش عکس در پایگاه داده سطح استان
        bool UploadEditAvatarProvinceDB(IFormFile file, EditViewModel document, int selectedRoles, string roleUploader);
        //بارگذاری ویرایش عکس در پایگاه داده سطح شهرستان
        bool UploadEditAvatarCountyDB(IFormFile file, EditViewModel document, int departmentId, int selectedRoles, string roleUploader);
        //بارگذاری ویرایش عکس در پایگاه داده سطح بخش
        bool UploadEditAvatarDistrictDB(IFormFile file, EditViewModel document, int departmentId, int selectedCounty, string roleUploader);
        #endregion

        #region UploadEditProfileAvatarPC
        //بارگذاری ویرایش عکس  پروفایل در سرور سطح استان
        bool UploadEditProfileAvatarProvincePC(IFormFile file, EditProfileViewModel document);
        //بارگذاری ویرایش عکس  پروفایل در سرور سطح شهرستان
        bool UploadEditProfileAvatarCountyPC(IFormFile file, EditProfileViewModel document);
        //بارگذاری ویرایش عکس  پروفایل در سرور سطح بخش
        bool UploadEditProfileAvatarDistrictPC(IFormFile file, EditProfileViewModel document);

        #endregion

        #region UploadEditProfileAvatarDB
        //بارگذاری ویرایش عکس پروفایل  در پایگاه داده سطح استان
        bool UploadEditProfileAvatarProvinceDB(IFormFile file, EditProfileViewModel document, string roleUploader);
        //بارگذاری ویرایش عکس در پایگاه داده سطح شهرستان
        bool UploadEditProfileAvatarCountyDB(IFormFile file, EditProfileViewModel document, string roleUploader);
        //بارگذاری ویرایش عکس در پایگاه داده سطح بخش
        bool UploadEditProfileAvatarDistrictDB(IFormFile file, EditProfileViewModel document, string roleUploader);
        #endregion

        #region DownloadProfileAvatarPicAfterActive
        //دانلود عکس پروفایل از پایگاه داده پس از فعال شدن حساب کاربری در سطح استان
        Task<bool> DownloadProfileAvatarPicAfterActiveProvince(string nationalCode);
        //دانلود عکس پروفایل از پایگاه داده پس از فعال شدن حساب کاربری در سطح استان
        Task<bool> DownloadProfileAvatarPicAfterActiveCounty(string nationalCode);
        //دانلود عکس پروفایل از پایگاه داده پس از فعال شدن حساب کاربری در سطح استان
        Task<bool> DownloadProfileAvatarPicAfterActiveDistrict(string nationalCode);
        #endregion

        #region IsexistProfileAvatarPicOnDBWithnationalCode
        //وجودیت عکس پروفایل کاربر در پایگاه داده با کد ملی در سطح استان
        bool IsexistProfileAvatarPicOnDBWithnationalCodeProvince(string nationalCode);
        //وجودیت عکس پروفایل کاربر در پایگاه داده با کد ملی در سطح شهرستان
        bool IsexistProfileAvatarPicOnDBWithnationalCodeCounty(string nationalCode);
        //وجودیت عکس پروفایل کاربر در پایگاه داده با کد ملی در سطح بخش
        bool IsexistProfileAvatarPicOnDBWithnationalCodeDistrict(string nationalCode);
        #endregion

        #region GetProfileAvatarPicFromDBWithnationalCode
        //استخراج عکس پروفایل از پایگاه داده با کد ملی در سطح استان
        DocumentUploadProvinceLevel GetProfileAvatarPicFromDBWithnationalCodeProvince(string nationalCode);
        //استخراج عکس پروفایل از پایگاه داده با کد ملی در سطح شهرستان
        DocumentUploadCountyLevel GetProfileAvatarPicFromDBWithnationalCodeCounty(string nationalCode);
        //استخراج عکس پروفایل از پایگاه داده با کد ملی در سطح بخش
        DocumentUploadDistrictLevel GetProfileAvatarPicFromDBWithnationalCodeDistrict(string nationalCode);
        #endregion

        #region IsExistManagerAvatarOnDB
        //وجودیت تصویر نقش مدیریت اولیه در پایگاه داده سطح استان
        bool IsExistManagerAvatarProvineDB(string nationalCode);
        //وجودیت تصویر نقش مدیریت بخش در پایگاه داده سطح شهرستان
        bool IsExistManagerAvatarCountyDB(string nationalCode);
        #endregion

        #region IsExistUserDocumentOnDBWithDocumentId
        //وجودیت مدرک کاربر در پایگاه داده با شناسه مدرک در سطح استان
        bool IsExistUserDocumentOnProvinceDB(int id);
        //وجودیت مدرک کاربر در پایگاه داده با شناسه مدرک در سطح شهرستان
        bool IsExistUserDocumentOnCountyDB(int id);
        //وجودیت مدرک کاربر در پایگاه داده با شناسه مدرک در سطح بخش
        bool IsExistUserDocumentOnDistrictDB(int id);
        //وجودیت مدرک تبادل شده در پایگاه داده با شناسه مدرک
        bool IsExistExchangeDocumentOnDB(int id);
        //وجودیت مدرک تبادل شده در پایگاه داده با نام کاربری و ناحیه بارگذاری کننده
        bool IsExistExchangeDocumentOnDBWithnationalCodeAndareaUploader(string userUploaderId, string userUploaderArea);
        #endregion

        #region IsExistDeletedUserDocumentOnDBWithDocumentId
        //وجودیت مدرک کاربر حذف شده در پایگاه داده با شناسه مدرک در سطح استان
        bool IsExistDeletedUserDocumentOnProvinceDB(int id);
        //وجودیت مدرک کاربر حذف شده در پایگاه داده با شناسه مدرک در سطح شهرستان
        bool IsExistDeletedUserDocumentOnCountyDB(int id);
        //وجودیت مدرک کاربر حذف شده در پایگاه داده با شناسه مدرک در سطح بخش
        bool IsExistDeletedUserDocumentOnDistrictDB(int id);
        //وجودیت مدرک تبادل شده حذف شده در پایگاه داده با نام کاربری و ناحیه بارگذاری کننده
        bool IsExistDeletedExchangeDocumentOnDBWithnationalCodeAndareaUploader(string userUploaderId, string userUploaderArea);
        //وجودیت مدرک تبادل شده حذف شده در پایگاه داده با نام کاربری و ناحیه بارگذاری کننده
        bool IsExistDeletedExchangeDocumentOnDB(int id);

        #endregion

        #region GetDocumentFromDBWithDocumentId
        //استخراج مدرک کاربر از پایگاه داده با شناسه مدرک در سطح استان
        DocumentUploadProvinceLevel GetDocumentFromProvinceDB(int id);
        //استخراج مدرک کاربر از پایگاه داده با شناسه مدرک در سطح شهرستان
        DocumentUploadCountyLevel GetDocumentFromCountyDB(int id);
        //استخراج مدرک کاربر از پایگاه داده با شناسه مدرک در سطح بخش
        DocumentUploadDistrictLevel GetDocumentFromDistrictDB(int id);
        //استخراج مدرک تبادل شده از پایگاه داده با شناسه مدرک
        TransferDocumentsBetweenLevels GetExchangeDocumentFromDB(int id);
        #endregion

        #region DownloadDeletedDocumentFromDBWithDocumenId
        //استخراج مدرک کاربر حذف شده از پایگاه داده با شناسه مدرک در سطح استان
        DocumentUploadProvinceLevel GetDeletedDocumentFromProvinceDB(int id);
        //استخراج مدرک کاربر حدف شده از پایگاه داده با شناسه مدرک در سطح شهرستان
        DocumentUploadCountyLevel GetDeletedDocumentFromCountyDB(int id);
        //استخراج مدرک کاربر حذف شده از پایگاه داده با شناسه مدرک در سطح بخش
        DocumentUploadDistrictLevel GetDeletedDocumentFromDistrictDB(int id);
        //استخراج مدرک تبادل شده حذف شده از پایگاه داده با شناسه مدرک
        TransferDocumentsBetweenLevels GetDeletedExchangeDocumentFromDB(int id);
        #endregion

        #region IsExistManager
        //یکتا بودن نقش مدیریت یک شعبه در سطح استان
        bool IsExistManagerProvince();
        //یکتا بودن نقش مدیریت یک شعبه در شهرستان
        bool IsExistManagerCounty(int department);
        //یکتا بودن نقش مدیریت یک شعبه در سطح بخش
        bool IsExistManagerDistrict(int county, int department);
        #endregion

        #region GetRoleWithnationalCode
        //استخراج نقش کاربر در سطح استان
        string GetRoleWithnationalCodeProvince(string nationalCode);
        //استخراج نقش کاربر در سطح شهرستان
        string GetRoleWithnationalCodeCounty(string nationalCode);
        //استخراج نقش کاربر در سطح بخش
        string GetRoleWithnationalCodeDistrict(string nationalCode);
        //استخراج نقش کاربر حذف شده در سطح استان
        string GetDeletedRoleWithnationalCodeProvince(string nationalCode);
        //استخراج نقش کاربر حذف شده در سطح شهرستان
        string GetDeletedRoleWithnationalCodeCounty(string nationalCode);
        //استخراج نقش کاربر حذف شده در سطح بخش
        string GetDeletedRoleWithnationalCodeDistrict(string nationalCode);
        #endregion

        #region GetManagernationalCode
        //استخراج کد ملی مدیریت در سطح استان
        string GetManagernationalCodeProvince();
        //استخراج کد ملی مدیریت در سطح شهرستان
        string GetManagernationalCodeCounty(int department);
        //استخراج کد ملی مدیریت در سطح بخش
        string GetManagernationalCodeDistrict(int county, int department);
        #endregion

        #region ManageRoles
        //پاک کردن نقش
        bool DeleteRoleWithroleId(int roleId);
        //فعال کردن مجدد نقش حذف شده
        bool ActivationDeletedRoleWithroleId(int roleId);
        //اضافه کردن نقش جدید
        string AddNewRoleWithRroleTitle(string roleTitle);
        #endregion

        #region GetInformationRoleWithId
        //استخراج اطلاعات نقش از پایگاه داده نقش ها
        Role GetInformationRoleWithId(int roleId);
        //استخراج اطلاعات نقش پاک شده از پایگاه داده نقش ها
        Role GetInformationDeletedRoleWithRoleId(int roleId);
        #endregion

        #region UploadDocumentPC
        //بارگذاری مدارک در سرور سطح استان
        bool UploadProvincePC(IFormFile file, UploadViewModel document);
        //بارگذاری مدارک در سرور سطح شهرستان
        bool UploadCountyPC(IFormFile file, UploadViewModel document);
        //بارگذاری مدارک در سرور سطح بخش
        bool UploadDistrictPC(IFormFile file, UploadViewModel document);
        #endregion

        #region UploadDocumentDB
        //بارگذاری مدارک در پایگاه داده سطح استان
        bool UploadProvinceDB(IFormFile document, UploadViewModel content, List<int> SelectedRoles);
        //بارگذاری مدارک در پایگاه داده سطح شهرستان
        bool UploadCountyDB(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> SelectedRoles);
        //بارگذاری مدارک در پایگاه داده سطح بخش
        bool UploadDistrictDB(IFormFile document, UploadViewModel content, List<int> CountyDestination, List<int> DistrictDestination, List<int> SelectedRoles);
        #endregion

        #region ShowDescription
        //نمایش توضیحات در صفحه تبادلات 
        TransferDocumentsBetweenLevels getTransferDocumentWithId(int id);
        //نمایش توضیحات تبادل حذف شده در صفحه تبادلات 
        TransferDocumentsBetweenLevels getDeletedTransferDocumentWithId(int id);

        #endregion

        #region IsExistSpecifiedRole
        //وجودیت نقش خاص در سطح استان
        bool IsExistSpecifiedRoleProvince(List<int> RoleId);
        //وجودیت نقش خاص در سطح شهرستان
        bool IsExistSpecifiedRoleCounty(List<int> RoleId, List<int> CountyId);
        //وجودیت نقش خاص در سطح بخش
        bool IsExistSpecifiedRoleDistrict(List<int> RoleId, List<int> CountyId, List<int> DistrictId);
        #endregion

        #region IsExistMember
        //وجودیت عضو در سطح شهرستان
        bool IsExistMemberCounty(List<int> CountyId);
        //وجودیت عضو در سطح بخش
        bool IsExistMemberDistrict(List<int> CountyId, List<int> DistrictId);
        #endregion

        #region Chart
        //محاسبه تعداد کارمندان فعال و غیر فعال
        List<int> CountEmployeesPerDepartment(string area, int county, int department);
        //محاسبه تعداد پرسنل مشغول در هر معاونت
        List<ChartValues> RoleCount(string area, int county, int department);
        //محاسبه تعداد کارمندان از نظر تفکیک جنسیت
        List<ChartValues> GenderCount(string area, int county, int department);
        //محاسبه تعداد کارمندان از نظر تفکیک وضعیت استخدامی
        List<ChartValues> EmploymentCount(string area, int county, int department);
        //محاسبه تعداد کارمندان بر اساس مدرک نحصیلی اخذ شده
        List<ChartValues> EducationDegreeCount(string area, int county, int department);
        #endregion

        #region GetIdWithNationalCodeForForeignKey
        //دریافت شناسه از طریق کد ملی سطح استان
        int GetIdWithNationalCodeForForeignKeyProvince(string nationalCode);
        //دریافت شناسه از طریق کد ملی سطح شهرستان
        int GetIdWithNationalCodeForForeignKeyCounty(string nationalCode);
        //دریافت شناسه از طریق کد ملی سطح بخش
        int GetIdWithNationalCodeForForeignKeyDistrict(string nationalCode);
        #endregion
    }
}
