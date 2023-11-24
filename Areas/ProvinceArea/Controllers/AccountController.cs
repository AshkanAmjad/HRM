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

namespace ERP.Areas.ProvinceArea.Controllers
{
    [Area("ProvinceArea")]
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
        [Route("PLogin")]
        public IActionResult PLogin(bool IsSuccessResetPass = false)
        {
            ViewBag.IsSuccessResetPass = IsSuccessResetPass;
            return View();
        }

        [HttpPost]
        [Route("PLogin")]
        [ValidateAntiForgeryToken]
        public IActionResult PLogin(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _ManagementService.PLoginUser(login);
            var employee = _ManagementService.ELoginUser(login, "استان");
            var userDeletedProvince = _ManagementService.PLoginUserDeleted(login);

            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.nationalCode.ToString()),
                        new Claim("nationalCode",user.nationalCode.ToString()),
                        new Claim("fName",user.fName.ToString()),
                        new Claim("lName",user.lName.ToString()),
                        new Claim("department",user.department.ToString()),
                        new Claim("role",user.role.ToString()),
                        new Claim("area",employee.area.ToString())
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                HttpContext.SignInAsync("ProvinceArea", principal);
                ViewBag.IsSuccess = true;
                return View(login);
            }
            if (userDeletedProvince != null )
            {
                ModelState.AddModelError("nationalCode", "حساب کاربری غیر فعال است.");
            }
            else 
            {
                ModelState.AddModelError("nationalCode", "کاربری با مشخصات وارد شده یافت نشد.");

            }
            return View(login);
        }

        #endregion

        #region ForgotPassword
        //فراموشی رمز عبور و احراز هویت کاربر در سطح استان
        [Route("ForgotPasswordProvince")]
        public IActionResult ForgotPasswordProvince(bool SendVerificationCodeByEmail = false)
        {
            ViewBag.SendVerificationCodeByEmail = SendVerificationCodeByEmail;
            return View();
        }

        [HttpPost]
        [Route("ForgotPasswordProvince")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPasswordProvince(ForgotPasswordViewModel forgot)
        { 
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }
            var user = _ManagementService.GetUserForgotPassProvince(forgot);
            var userDeletedProvince = _ManagementService.GetUserForgotPassDeletedProvince(forgot);

            if (user != null)
            {
                var nationalCode = forgot.nationalCode;
                return RedirectToAction("ChooseTheMethod", new { id = nationalCode });
            }
            if (userDeletedProvince != null )
            {
                ModelState.AddModelError("nationalCode", "حساب کاربری غیر فعال است.");
            }
            else
            {
                ModelState.AddModelError("nationalCode", "کاربری با مشخصات وارد شده یافت نشد.");

            }
            return View(forgot);
        }

        //انتخاب شیوه ارسال کد تایید در سطح استان
        [Route("ChooseTheMethodProvince/{id?}")]
        public IActionResult ChooseTheMethod(string id)
        {
            ViewBag.nationalCode = id;
            return View();
        }

        //ارسال کد تایید از طریق ایمیل در سطح استان
        [Route("SendVerificationCodeByEmailProvince/{id?}")]
        public IActionResult SendVerificationCodeByEmailProvince(string id)
        {
            ViewBag.nationalCode = id;
            return View();
        }

        [HttpPost]
        [Route("SendVerificationCodeByEmailProvince/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult SendVerificationCodeByEmailProvince(ResetPasswordByEmail emailforgotpass)
        {
            if (!ModelState.IsValid)
                return View(emailforgotpass);
            string fixedEmail = FixedText.FixEmail(emailforgotpass.email);
            ProvinceLevel user = _ManagementService.GetUserBynationalCodeProvinceLevel(emailforgotpass.nationalCode);
            string bodyEmail = _viewRender.RenderToStringAsync("_ResetPasswordByEmailProvince", user);
            SendEmail.Send(fixedEmail, "بازیابی رمز عبور حساب کاربری", bodyEmail);
            return Redirect("/ForgotPasswordProvince?SendVerificationCodeByEmail=true");
        }

        //ارسال کد تایید از طریق پیامک در سطح استان
        [HttpGet]
        [Route("VerificationCodeProvince/{id?}")]
        public async Task<IActionResult> VerificationCodeProvince(string id)
        {
            var nationalCode = id;
            var tel = _smsService.GetTelUserProvince(nationalCode);
            var randomNumber = NumberGenarator.RandomNumber();
            ViewBag.nationalCode = nationalCode;
            await _smsService.SendLookupSMS(tel, "ForgotPassword",
                nationalCode, randomNumber);
            ViewBag.SendVerificationByPhone = true;
            ViewBag.nationalCode = nationalCode;
            ViewBag.randomNumber = randomNumber.ToString();
            return View("VerificationCodeProvince");
        }

        //تایید کد احراز هویت در سطح استان
        [HttpPost]
        [Route("VerificationCodeProvince/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult VerificationCodeProvince(VerificationCode pVertify)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.randomNumber = pVertify.randomNumber;
                ViewBag.nationalCode = pVertify.nationalCode;
                return View();
            }
            if (pVertify.randomNumber == pVertify.verificationCode)
            {
                return RedirectToAction("ResetPasswordProvince", new { id = pVertify.nationalCode });
            }
            else
            {
                ViewBag.falseverificationCode = true;
                ViewBag.randomNumber = pVertify.randomNumber;
                ViewBag.nationalCode = pVertify.nationalCode;
                return View();
            }

            return NotFound();
        }
        #endregion

        #region ResetPassword
        //تغییر رمز عبور در سطح استان
        [Route("ResetPasswordProvince/{id?}")]
        public IActionResult ResetPasswordProvince(string id)
        {
            ViewBag.nationalCode = id;
            return View();
        }

        [HttpPost]
        [Route("ResetPasswordProvince/{id?}")]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPasswordProvince(ResetPassword reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }
            bool result = _ManagementService.ResetPassswordProvince(reset);
            if (result == true)
            {
                ViewBag.IsSuccessResetPass = true;
                return Redirect("/PLogin?IsSuccessResetPass=true");
            }
            return NotFound();
        }
        #endregion

        #region Logout
        //تابع خروج
        [Route("PLogout")]
        public IActionResult PLogout()
        {
            HttpContext.SignOutAsync("ProvinceArea");
            return Redirect("/PLogin");
        }
        #endregion
    }
}
