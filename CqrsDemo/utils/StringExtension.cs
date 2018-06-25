using System;
using System.Collections.Generic;
using System.Text;

namespace CqrsDemo.utils
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
