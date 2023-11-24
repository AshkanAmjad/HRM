using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services
{
    public interface ISMSService
    {
        #region GetTelUser
        //استخراج شماره تماس  در سطح استان
        string GetTelUserProvince(string nationalCode);
        //استخراج شماره تماس  در سطح شهرستان
        string GetTelUserCounty(string nationalCode);
        //استخراج شماره تماس  در سطح بخش
        string GetTelUserDistrict(string nationalCode);
        #endregion

        #region SendLookupSMS
        //ارسال پیامک احراز هویت
        Task SendLookupSMS(string phoneNumber, string templateName, string token1, string token2, string? token3 = "");
        #endregion

    }
}
