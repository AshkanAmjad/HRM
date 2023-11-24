using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERP.Convertors
{
    public static class NumberGenarator
    {
        //ایجاد اعدد تصادفی
        public static string RandomNumber()
        {
            var generator = new Random();
            var result = generator.Next(0, 100000).ToString("D5");
            return result;

        }
    }
}
