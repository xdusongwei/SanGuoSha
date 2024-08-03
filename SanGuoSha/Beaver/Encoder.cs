using System.Text;

namespace BeaverMarkupLanguage
{
    /// <summary>
    /// Beaver编码器
    /// </summary>
    public class BeaverEncoder
    {
        /// <summary>
        /// 将Beaver对象进行编码
        /// </summary>
        /// <param name="aBeaver"></param>
        /// <returns></returns>
        public static string Encoding(Beaver aBeaver)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Beaver.Node i in aBeaver.lst)
            {
                if (i.ElementName == string.Empty)
                {
                    if (i.ChildValue != Beaver._default)
                    {
                        sb.Append(Encoding(i.ChildValue));
                        sb.Append(",");
                    }
                    else
                    {
                        sb.Append(i.StringValue);
                        sb.Append(",");
                    }
                }
                else
                {
                    if (i.ChildValue != Beaver._default)
                    {
                        sb.Append(i.ElementName);
                        sb.Append("|");
                        sb.Append(Encoding(i.ChildValue));
                        sb.Append(",");
                    }
                    else
                    {
                        sb.Append(i.ElementName);
                        sb.Append("|");
                        sb.Append(i.StringValue);
                        sb.Append(",");
                    }
                }
            }
            if (sb.Length > 0)
            {
                sb = sb.Remove(sb.Length - 1, 1);
                return string.Format("[{0}]", sb.ToString());
            }
            else
            {
                return string.Format("[{0}]", sb.ToString());
            }
        }
    }
}
