using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Convertors
{
    public class FixedText
    {
        //تابع حذف فاصله های اضافی و کوچک نمودن حروف ایمیل
        public static string FixEmail(string email)
        {
            return email.Trim().ToLower();
        }
    }
}
