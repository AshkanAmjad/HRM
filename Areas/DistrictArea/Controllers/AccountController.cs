using ERP.Convertors;
using ERP.Data;
using ERP.Models;
using ERP.Senders;
using ERP.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ERP.Areas.DistrictArea.Controllers
{
    [Area("DistrictArea")]
    [AutoValidateAntiforgeryToken]

    public class AccountController : Controller
    {
        #region DataBase

        //ارتباط با پایگاه داده

        private ERPContext _context;
        private IManagementService _ManagementService;
        private readonly ISMSService _smsService;
        private IViewRenderService _viewRender;


        public AccountController(ERPContext context, IManagementService managementService, ISMSService smsService, IViewRenderService viewRender)
        {
            _context = context;
            _ManagementService = managementService;
            _smsService = smsService;
            _viewRender = viewRender;

        }
        #endregion

        #region Login

        //نابع ورود

        [Route("DLogin")]
        public IActionResult DLogin(bool IsSuccessResetPass = false)
        {
            ViewBag.IsSuccessResetPass = IsSuccessResetPass;
            return View();
        }


        [HttpPost]
        [Route("DLogin")]
        [ValidateAntiForgeryToken]

        public IActionResult DLogin(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _ManagementService.DLoginUser(login);
            var employee = _ManagementService.ELoginUser(login, "بخش");
            var userDeletedDistrict = _ManagementService.DLoginUserDeleted(login);
            if (user != null)
            {
                var claims = new List<Claim>
                    {
                       new Claim(ClaimTypes.NameIdentifier,user.nationalCode.ToString()),
                        new Claim("nationalCode",user.nationalCode.ToString()),
                        new Claim("fName",user.fName.ToString()),
                        new Claim("lName",user.lName.ToString()),
                        new Claim("county",user.county.ToString()),
                        new Claim("department",user.department.ToString()),
                        new Claim("role",user.role.ToString()),
                        new Claim("area",employee.area.ToString())

                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync("DistrictArea", principal);
                ViewBag.IsSuccess = true;
                return View(login);
            }
            if (userDeletedDistrict != null)
            {
                ModelState.AddModelError("nationalCode", "حساب کاربری غیر فعال است.");
            }
            ModelState.AddModelError("nationalCode", "کاربری با مشخصات وارد شده یافت نشد.");
            return View(login);
        }
        #endregion

        #region ForgotPassword
        //فراموشی رمز عبور و احراز هویت کاربر در سطح بخش
        [Route("ForgotPasswordDistrict")]
        public IActionResult ForgotPasswordDistrict(bool SendVerificationCodeByEmail = false)
        {
            ViewBag.SendVerificationCodeByEmail = SendVerificationCodeByEmail;
            return View();
        }

        [HttpPost]
        [Route("ForgotPasswordDistrict")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ForgotPasswordDistrict(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }
            var user = _ManagementService.GetUserForgotPassDistrict(forgot);
            var userDeletedDistrict = _ManagementService.GetUserForgotPassDeletedDistrict(forgot);
            if (user != null)
            {
                var nationalCode = forgot.nationalCode;
                return RedirectToAction("ChooseTheMethod", new { id = nationalCode });
            }
            if (userDeletedDistrict != null)
            {
                ModelState.AddModelError("nationalCode", "حساب کاربری غیر فعال است.");
            }
            ModelState.AddModelError("nationalCode", "کاربری با مشخصات وارد شده یافت نشد.");
            return View(forgot);
        }

        //انتخاب شیوه ارسال کد تایید در سطح بخش
        [Route("ChooseTheMethodDistrict/{id?}")]
        public IActionResult ChooseTheMethod(string id)
        {
            ViewBag.nationalCode = id;
            return View();
        }

        //ارسال کد تایید از طریق ایمیل در سطح بخش
        [Route("SendVerificationCodeByEmailDistrict/{id?}")]
        public IActionResult SendVerificationCodeByEmailDistrict(string id)
        {
            ViewBag.nationalCode = id;
            return View();
        }

        [HttpPost]
        [Route("SendVerificationCodeByEmailDistrict/{id?}")]
        [ValidateAntiForgeryToken]

        public IActionResult SendVerificationCodeByEmailDistrict(ResetPasswordByEmail emailforgotpass)
        {
            if (!ModelState.IsValid)
                return View(emailforgotpass);
            string fixedEmail = FixedText.FixEmail(emailforgotpass.email);
            DistrictLevel user = _ManagementService.GetUserBynationalCodeDistrictLevel(emailforgotpass.nationalCode);
            string bodyEmail = _viewRender.RenderToStringAsync("_ResetPasswordByEmailDistrict", user);
            SendEmail.Send(fixedEmail, "بازیابی رمز عبور حساب کاربری", bodyEmail);
            return Redirect("/ForgotPasswordDistrict?SendVerificationCodeByEmail=true");
        }

        //ارسال کد تایید از طریق پیامک در سطح بخش
        [HttpGet]
        [Route("VerificationCodeDistrict/{id?}")]
        public async Task<IActionResult> VerificationCodeDistrict(string id)
        {
            var nationalCode = id;
            var tel = _smsService.GetTelUserDistrict(nationalCode);
            var randomNumber = NumberGenarator.RandomNumber();
            ViewBag.nationalCode = nationalCode;
            await _smsService.SendLookupSMS(tel, "ForgotPassword",
                nationalCode, randomNumber);
            ViewBag.SendVerificationByPhone = true;
            ViewBag.nationalCode = nationalCode;
            ViewBag.randomNumber = randomNumber.ToString();
            return View("VerificationCodeDistrict");
        }

        //تایید کد احراز هویت در سطح بخش
        [HttpPost]
        [Route("VerificationCodeDistrict/{id?}")]
        [ValidateAntiForgeryToken]

        public IActionResult VerificationCodeDistrict(VerificationCode dVertify)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.randomNumber = dVertify.randomNumber;
                ViewBag.nationalCode = dVertify.nationalCode;
                return View();
            }
            if (dVertify.randomNumber == dVertify.verificationCode)
            {
                return RedirectToAction("ResetPasswordDistrict", new { id = dVertify.nationalCode });
            }
            else
            {
                ViewBag.falseverificationCode = true;
                ViewBag.randomNumber = dVertify.randomNumber;
                ViewBag.nationalCode = dVertify.nationalCode;
                return View();
            }

            return NotFound();
        }
        #endregion

        #region ResetPassword
        //تغییر رمز عبور در سطح بخش
        [Route("ResetPasswordDistrict/{id?}")]
        public IActionResult ResetPasswordDistrict(string id)
        {
            ViewBag.nationalCode = id;
            return View();
        }

        [HttpPost]
        [Route("ResetPasswordDistrict/{id?}")]
        [ValidateAntiForgeryToken]

        public IActionResult ResetPasswordDistrict(ResetPassword reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }
            bool result = _ManagementService.ResetPassswordDistrict(reset);
            if (result == true)
            {
                ViewBag.IsSuccessResetPass = true;
                return Redirect("/DLogin?IsSuccessResetPass=true");
            }
            return NotFound();
        }
        #endregion

        #region Logout
        //تابع خروج
        [Route("DLogout")]
        public IActionResult DLogout()
        {
            HttpContext.SignOutAsync("DistricyArea");
            return Redirect("/DLogin");
        }
        #endregion

        #region Index
        //تابع انتفال به صفحه اصلی
        public IActionResult Index()
        {
            return View();
        }
        #endregion


    }
}
