using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Convertors
{
    public static class WorkingStatus
    {
        //محاسبه سابقه ثبت نام در سامانه به ساعت
        public static string HourlyWorkHistor(this DateTime value)
        {
            DateTime startTime = value;
            DateTime endTime = DateTime.Now;
            float hourlyWorkHistory = ((float)(endTime - startTime).TotalMinutes)/60;
            double hourlyWorkHistoryRounded = Math.Round(hourlyWorkHistory, 1);
            return hourlyWorkHistoryRounded.ToString();
        }
        //محاسبه سابقه ثبت نام در سامانه به روز
        public static string DailyWorkHistory(this DateTime value)
        {
            DateTime startTime = value;
            DateTime endTime = DateTime.Now;
            float dailyWorkHistory = ((float)(endTime - startTime).TotalHours)/24;
            double dailyWorkHistoryRounded = Math.Round(dailyWorkHistory, 2);
            return dailyWorkHistoryRounded.ToString();
        }


    }
}
