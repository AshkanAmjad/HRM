using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Convertors
{
    public class NameGenerator
    {
        //ایجاد برای نام فایل های آپلودی در پایگاه داده
        public static string GenerateUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
