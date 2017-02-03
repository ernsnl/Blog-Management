using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Framework.Extensions.StringExtensions
{
    public static class StringExtension
    {
        public static string ReplaceTurkishCharactes(this string input)
        {
            input = input.Replace("İ", "I");
            input = input.Replace("Ü", "U");
            input = input.Replace("Ö", "O");
            input = input.Replace("Ş", "S");
            input = input.Replace("Ç", "c");
            input = input.Replace("Ğ", "G");
            

            input = input.Replace("ç", "c");
            input = input.Replace("ğ", "g");
            input = input.Replace("ı", "i");
            input = input.Replace("ö", "o");
            input = input.Replace("ş", "s");
            input = input.Replace("ü", "u");

            return input;
        }

        public static string RemoveSpace(this string input)
        {
            return input.Replace(" ", "");
        }

    }
}
