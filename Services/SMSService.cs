using ERP.Data;
using ERP.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Services
{
    //تابع ارسال پیامک برای بازیابی فراموشی رمز عبور
    public class SMSService : ISMSService
    {

        #region DataBase
        private KavenegarInfoViewModel _kavenegarInfo;
        private ERPContext _context;
        public SMSService(IOptions<KavenegarInfoViewModel> kavenegarInfo, ERPContext context)
        {
            _context = context;
            _kavenegarInfo = kavenegarInfo.Value;
        }
        #endregion

        #region GetTelUser
        public string GetTelUserProvince(string nationalCode)
        {
            string userName = nationalCode;
            string tel = "0" + _context.ProvinceLevel.SingleOrDefault(u => u.nationalCode == userName).tel.ToString();
            return tel;
        }
        public string GetTelUserCounty(string nationalCode)
        {
            string userName = nationalCode;
            string tel = "0" + _context.CountyLevels.SingleOrDefault(u => u.nationalCode == userName).tel.ToString();
            return tel;
        }
        public string GetTelUserDistrict(string nationalCode)
        {
            string userName = nationalCode;
            string tel = "0" + _context.DistrictLevels.SingleOrDefault(u => u.nationalCode == userName).tel.ToString();
            return tel;
        }

        #endregion

        #region SendLookupSMS
        public async Task SendLookupSMS(string phoneNumber, string templateName, string token1, string? token2 = "", string? token3 = "")

        {
                try
                {
                    var api = new Kavenegar.KavenegarApi(_kavenegarInfo.ApiKey);

                    var result = await api.VerifyLookup(phoneNumber, token1, token2, token3, templateName);
                }
                catch (Kavenegar.Core.Exceptions.ApiException ex)
                {
                    throw new Exception(ex.Message);
                }
                catch (Kavenegar.Core.Exceptions.HttpException ex)
                {
                    throw new Exception(ex.Message);
                }
        }
        #endregion
    }
}
