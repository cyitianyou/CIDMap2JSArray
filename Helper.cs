﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIDMap2JSArray
{
    public static class Helper
    {
        public static Int32 HexStringToInt(this string hexString)
        {
            return Convert.ToInt32(hexString, 16);
        }

        public static string IntToHexString(this Int32 value, bool IsPadLeft = true)
        {
            var result = String.Format("{0:X}", value);
            if (IsPadLeft)
                result = result.PadLeft(4, '0');
            return result;
        }

        public static void AppendExt(this StringBuilder sb, int unicode, Int32 cid)
        {
            if (unicode < 256 || (unicode > 19967 && unicode < 40870)) //ASCII 和中文
                sb.Append(string.Format("{0}{1},", unicode.IntToHexString(), cid.IntToHexString(false)));
        }
    }
}
