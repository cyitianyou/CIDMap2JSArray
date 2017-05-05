using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIDMap2JSArray
{
    public static class Helper
    {
        public static Int32 HexStringToInt(this string hexString){
            return Convert.ToInt32(hexString, 16);
        }

        public static string IntToHexString(this Int32 value)
        {
            var result= String.Format("{0:X}", value);
            result = result.PadLeft(4, '0');
            return result;
        }

        public static void AppendExt(this StringBuilder sb,string unicode,Int32 cid)
        {
            sb.Append(string.Format("\"{0}\":{1},", unicode.ToUpper(), cid.ToString()));
        }
    }
}
