using System;
using System.Collections;
using System.Collections.Generic;

namespace BeaverMarkupLanguage
{
    /// <summary>
    /// Beavers标记对象
    /// </summary>
    public class Beaver : IEnumerable<string>
    {
        internal static Beaver _default = new Beaver();

        /// <summary>
        /// 一个没有内容的Beaver对象
        /// </summary>
        public Beaver()
        {

        }

        /// <summary>
        /// 获得当前数据项中的字符串
        /// </summary>
        public string GetString
        {
            get
            {
                if (lst.Count > 0 && lst[0].ChildValue == _default)
                {
                    return lst[0].StringValue;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 该数据项是否包含字符串值
        /// </summary>
        public bool HasStringValue
        {
            get
            {
                return lst.Count > 0 && lst[0].ChildValue != _default ? true : false;
            }
        }

        /// <summary>
        /// 获取当前数据项中的子项
        /// </summary>
        public Beaver GetChild
        {
            get
            {
                if (lst.Count > 0)
                {
                    return lst[0].ChildValue;
                }
                return _default;
            }
        }

        /// <summary>
        /// 获取当前数据项的描述符
        /// </summary>
        public string GetElementName
        {
            get
            {
                if (lst.Count > 0)
                    return lst[0].ElementName;
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// 用于在Beaver数据项中设置第一个元素的标记名称
        /// </summary>
        /// <param name="aName">标记的名称</param>
        public void SetHeaderElementName(string aName)
        {
            if (aName == null) aName = string.Empty;
            if (lst.Count != 0) //没有数据项就不加这个标记，这是为了避免解码之后认为标记后面是一个空值引起的不便
            {
                Node i = lst[0];
                i.ElementName = aName;
                lst[0] = i;
            }
        }


        /// <summary>
        /// 通过偏移查找数据项
        /// </summary>
        /// <param name="i">偏移量,非负</param>
        /// <returns>返回查询到的Beaver对象</returns>
        /// <exception cref="ArgumentException"></exception>
        public Beaver this[int i]
        {
            get
            {
                if (i < lst.Count && lst.Count > 0)
                {
                    return new Beaver { lst = lst.GetRange(i, lst.Count - i) };
                }
                else if (i < 0)
                {
                    throw new ArgumentException("Beaver偏移查询取值不能小于0");
                }
                else
                {
                    return _default;
                }
            }
        }

        /// <summary>
        /// 通过描述符查找数据项
        /// </summary>
        /// <param name="ElementName">描述符</param>
        /// <returns>返回查询到的Beaver对象</returns>
        public Beaver this[string ElementName]
        {
            get
            {
                if (string.IsNullOrEmpty(ElementName)) return _default;
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].ElementName == ElementName)
                    {
                        return new Beaver { lst = lst.GetRange(i, lst.Count - i) };
                    }
                }
                return _default;
            }
        }

        /// <summary>
        /// 创建新的Beaver对象
        /// </summary>
        /// <param name="aElementName">第一个数据项的描述符</param>
        /// <param name="aParmas">数据项</param>
        public Beaver(string aElementName, params object[] aParmas)
        {
            Add2List(aElementName, aParmas , true);
        }

        private void Add2List(string aElementName, object[] aObjs ,bool aEsc)
        {
            if (aElementName == null) aElementName = string.Empty;
            if(aEsc)
                aElementName = aElementName.Replace(",", "~cm;").Replace("|", "~vt;").Replace("[", "~lt;").Replace("]", "~gt;");
            bool first = true;
            foreach (object i in aObjs)
            {
                if (i is string)
                {
                    string s = string.Empty;
                    if (aEsc)
                        s = (i as string).Replace(",", "~cm;").Replace("|", "~vt;").Replace("[", "~lt;").Replace("]", "~gt;");
                    else
                        s = i as string;
                    if (first)
                    {
                        lst.Add(new Node { ElementName = aElementName, StringValue = s, ChildValue = _default });
                    }
                    else
                    {
                        lst.Add(new Node { ElementName = string.Empty, StringValue = s, ChildValue = _default });
                    }
                }
                else if (i is Beaver)
                {
                    if (first)
                    {
                        lst.Add(new Node { ElementName = aElementName, StringValue = string.Empty, ChildValue = i as Beaver });
                    }
                    else
                    {
                        lst.Add(new Node { ElementName = string.Empty, StringValue = string.Empty, ChildValue = i as Beaver });
                    }
                }
                else if (i is int)
                {
                    if (first)
                    {
                        lst.Add(new Node { ElementName = aElementName, StringValue = ((int)i).ToString(), ChildValue = _default });
                    }
                    else
                    {
                        lst.Add(new Node { ElementName = string.Empty, StringValue = ((int)i).ToString(), ChildValue = _default });
                    }
                }
                first = false;
            }
        }

        internal Beaver AddNoEsc(string aElementName, params object[] aTexts)
        {
            Add2List(aElementName, aTexts, false);
            return this;
        }

        /// <summary>
        /// 向Beaver对象附加更多的数据项
        /// </summary>
        /// <param name="aElementName">新添加的数据项第一个描述符</param>
        /// <param name="aTexts">数据项</param>
        /// <returns>原有的Beaver对象</returns>
        public Beaver Add(string aElementName, params object[] aTexts)
        {
            Add2List(aElementName, aTexts , true);
            return this;
        }

        /// <summary>
        /// 将Beaver对象编码
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return BeaverEncoder.Encoding(this);
        }

        internal List<Node> lst = new List<Node>();

        internal struct Node
        {
            public string ElementName;
            public string StringValue;
            public Beaver ChildValue;
        }

        /// <summary>
        /// 获得当前数据项的字符串迭代器
        /// </summary>
        /// <remarks>此方法将迭代从此偏移开始之后的任何有字符串信息的数据项,包含子Beaver对象的数据项不会被迭代</remarks>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            foreach (Node i in lst) 
            {
                if (i.ChildValue == _default)
                    yield return i.StringValue;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (Node i in lst)
            {
                if (i.ChildValue == _default)
                    yield return i.StringValue;
            }
        }
    }
}
