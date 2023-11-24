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

namespace ERP.Areas.CountyArea.Controllers
{
    [Area("CountyArea")]
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

        //تابع ورود
        [Route("CLogin")]
        public IActionResult CLogin(bool IsSuccessResetPass = false)
        {
            ViewBag.IsSuccessResetPass = IsSuccessResetPass;
            return View();
        }

        [HttpPost]
        [Route("CLogin")]
        [ValidateAntiForgeryToken]
        public IActionResult CLogin(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _ManagementService.CLoginUser(login);
            var employee = _ManagementService.ELoginUser(login, "شهرستان");
            var userDeletedCounty = _ManagementService.CLoginUserDeleted(login);
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
                HttpContext.SignInAsync("CountyArea", principal);
                ViewBag.IsSuccess = true;
                return View(login);
            }
            if (userDeletedCounty != null )
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
        //فراموشی رمز عبور و احراز هویت کاربر در سطح شهرستان
        [Route("ForgotPasswordCounty")]
        public IActionResult ForgotPasswordCounty(bool SendVerificationCodeByEmail = false)
        {
            ViewBag.SendVerificationCodeByEmail = SendVerificationCodeByEmail;
            return View();
        }

        [HttpPost]
        [Route("ForgotPasswordCounty")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> ForgotPasswordCounty(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }
            var user = _ManagementService.GetUserForgotPassCounty(forgot);
            var userDeletedCounty = _ManagementService.GetUserForgotPassDeletedCounty(forgot);
            if (user != null)
            {
                var nationalCode = forgot.nationalCode;
                return RedirectToAction("ChooseTheMethod", new { id = nationalCode });
            }
            if (userDeletedCounty != null)
            {
                ModelState.AddModelError("nationalCode", "حساب کاربری غیر فعال است.");
            }
            else
            {
                ModelState.AddModelError("nationalCode", "کاربری با مشخصات وارد شده یافت نشد.");

            }
            return View(forgot);
        }

        //انتخاب شیوه ارسال کد تایید در سطح شهرستان
        [Route("ChooseTheMethodCounty/{id?}")]
        public IActionResult ChooseTheMethod(string id)
        {
            ViewBag.nationalCode = id;
            return View();
        }

        //ارسال کد تایید از طریق ایمیل در سطح شهرستان
        [Route("SendVerificationCodeByEmailCounty/{id?}")]
        public IActionResult SendVerificationCodeByEmailCounty(string id)
        {
            ViewBag.nationalCode = id;
            return View();
        }

        [HttpPost]
        [Route("SendVerificationCodeByEmailCounty/{id?}")]
        [ValidateAntiForgeryToken]

        public IActionResult SendVerificationCodeByEmailCounty(ResetPasswordByEmail emailforgotpass)
        {
            if (!ModelState.IsValid)
                return View(emailforgotpass);
            string fixedEmail = FixedText.FixEmail(emailforgotpass.email);
            CountyLevel user = _ManagementService.GetUserBynationalCodeCountyLevel(emailforgotpass.nationalCode);
            string bodyEmail = _viewRender.RenderToStringAsync("_ResetPasswordByEmailCounty", user);
            SendEmail.Send(fixedEmail, "بازیابی رمز عبور حساب کاربری", bodyEmail);
            return Redirect("/ForgotPasswordCounty?SendVerificationCodeByEmail=true");
        }

        //ارسال کد تایید از طریق پیامک در سطح شهرستان
        [HttpGet]
        [Route("VerificationCodeCounty/{id?}")]
        public async Task<IActionResult> VerificationCodeCounty(string id)
        {
            var nationalCode = id;
            var tel = _smsService.GetTelUserCounty(nationalCode);
            var randomNumber = NumberGenarator.RandomNumber();
            ViewBag.nationalCode = nationalCode;
            await _smsService.SendLookupSMS(tel, "ForgotPassword",
                nationalCode, randomNumber);
            ViewBag.SendVerificationByPhone = true;
            ViewBag.nationalCode = nationalCode;
            ViewBag.randomNumber = randomNumber.ToString();
            return View("VerificationCodeCounty");
        }

        //تایید کد احراز هویت در سطح شهرستان
        [HttpPost]
        [Route("VerificationCodeCounty/{id?}")]
        [ValidateAntiForgeryToken]

        public IActionResult VerificationCodeCounty(VerificationCode cVertify)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.randomNumber = cVertify.randomNumber;
                ViewBag.nationalCode = cVertify.nationalCode;
                return View();
            }
            if (cVertify.randomNumber == cVertify.verificationCode)
            {
                return RedirectToAction("ResetPasswordCounty", new { id = cVertify.nationalCode });
            }
            else
            {
                ViewBag.falseverificationCode = true;
                ViewBag.randomNumber = cVertify.randomNumber;
                ViewBag.nationalCode = cVertify.nationalCode;
                return View();
            }

            return NotFound();
        }
        #endregion

        #region ResetPassword
        //تغییر رمز عبور در سطح شهرستان
        [Route("ResetPasswordCounty/{id?}")]
        public IActionResult ResetPasswordCounty(string id)
        {
            ViewBag.nationalCode = id;
            return View();
        }

        [HttpPost]
        [Route("ResetPasswordCounty/{id?}")]
        [ValidateAntiForgeryToken]

        public IActionResult ResetPasswordCounty(ResetPassword reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }
            bool result = _ManagementService.ResetPassswordCounty(reset);
            if (result == true)
            {
                ViewBag.IsSuccessResetPass = true;
                return Redirect("/CLogin?IsSuccessResetPass=true");
            }
            return NotFound();
        }
        #endregion

        #region Logout
        //تابع خروج
        [Route("CLogout")]
        public IActionResult CLogout()
        {
            HttpContext.SignOutAsync("CountyArea");
            return Redirect("/CLogin");
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
