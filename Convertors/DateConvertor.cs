using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ERP.Convertors
{
    public static class DateConvertor
    {
        //مبدل تاریخ میلادی به شمسی با قالب تاریخ - زمان
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc=new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") +"\r\n"+ pc.GetHour(value).ToString("00")+":"+pc.GetMinute(value).ToString("00")+":"+pc.GetSecond(value).ToString("00");
        }

        //مبدل تاریخ میلادی به شمسی با قالب تاریخ زمان برای نام فایل بارگذاری شده 
        public static string ToShamsiFileUpload(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value)+ pc.GetMonth(value).ToString("00")+
                   pc.GetDayOfMonth(value).ToString("00")+ pc.GetHour(value).ToString("00") + pc.GetMinute(value).ToString("00") + pc.GetSecond(value).ToString("00");
        }

        //مبدل تاریخ میلادی به شمسی با قالب زمان | تاریخ
        public static string ToShamsiMyDocuments(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetHour(value).ToString("00") + ":" + pc.GetMinute(value).ToString("00") + ":" + pc.GetSecond(value).ToString("00")
                + " | " +
                pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" + pc.GetDayOfMonth(value).ToString("00");
        }
    }
}
