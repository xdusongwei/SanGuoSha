using System;

namespace BeaverMarkupLanguage
{
    /// <summary>
    /// Beaver解码器
    /// </summary>
    public class BeaverDecoder
    {
        /// <summary>
        /// 从字符串中解码.得到Beaver对象
        /// </summary>
        /// <param name="aCode"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Beaver Decoding(string aCode)
        {
            if (aCode.IndexOf('[') != 0)
            {
                throw new ArgumentException("beaver解析时发现\'>\'不能找到匹配");
            }
            if (aCode.LastIndexOf(']') != aCode.Length - 1)
            {
                throw new ArgumentException("beaver解析时发现\'<\'不能找到匹配");
            }
            return DecodingFunc([], aCode.Substring(1 , aCode.Length -2 ));
        }

        private static Beaver DecodingFunc(Beaver aRoot, string aCode)
        {
            int start = 0;
            int curr = 0;
            int gt = 0;
            string name = string.Empty;
            for (curr = 0; curr < aCode.Length; curr++)
            {
                switch (aCode[curr])
                {
                    case '[':
                        if ((gt = aCode.LastIndexOf(']')) == -1)
                        {
                            throw new ArgumentException("beaver解析时发现\'[\'不能找到匹配");
                        }
                        aRoot = aRoot.AddNoEsc(name, DecodingFunc([], aCode.Substring(curr + 1, gt - curr - 1)));
                        start = curr = gt + 2;
                        name = string.Empty;
                        break;
                    case ']':
                        throw new ArgumentException("beaver解析时发现\']\'不能找到匹配");
                    case '|':
                        name = aCode.Substring(start, curr - start).Replace("~cm;", ",").Replace( "~vt;","|").Replace( "~lt;","[").Replace( "~gt;","]");
                        start = curr + 1;
                        break;
                    case ',':
                        if (start == curr)
                        {
                            aRoot = aRoot.AddNoEsc(name, string.Empty);
                        }
                        else
                        {
                            aRoot = aRoot.AddNoEsc(name, aCode.Substring(start, curr - start).Replace("~cm;", ",").Replace("~vt;", "|").Replace("~lt;", "[").Replace("~gt;", "]"));
                        }
                        start = curr + 1;
                        name = string.Empty;
                        break;
                    default:

                        break;
                }
            }
            if(aCode.Length > start)
                aRoot = aRoot.AddNoEsc(name, aCode.Substring(start, curr - start));
            else if (name != string.Empty)
                aRoot = aRoot.AddNoEsc(name, string.Empty);
            return aRoot;
        }
    }
}
