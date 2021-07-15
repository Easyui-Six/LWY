using LWY.FW.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Zzsey2018.Common
{
    public class XmlHelper
    {
        /// <summary>
        /// 根据字典获取xml格式的字符串
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public string SetXmlStr(Dictionary<string, string> dic)
        {
            StringBuilder strbuilder = new StringBuilder();
            strbuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            strbuilder.Append("<xml>");
            foreach (var key in dic.Keys)
            {
                strbuilder.Append("<" + key + ">");
                strbuilder.Append(dic[key]);
                strbuilder.Append("</" + key + ">");
            }
            //strbuilder.Append("<TranCode>b0002</TranCode>");
            //strbuilder.Append("<ORGCODE>1</ORGCODE>");
            strbuilder.Append("</xml>");

            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(tmpstr);
            return strbuilder.ToString();
        }

        /// <summary>
        /// 解析xml字符串为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public T AnalyXmlStr<T>(string str) where T : class
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            T result = default(T);
            //解析  根据key解析value  可以用反射实现
            List<string> members = ReflectHelper.GetReflextHelper().GetClassMembers<T>();
            foreach (var item in members)
            {
                string tmpcontent = doc.DocumentElement.SelectSingleNode(item)?.FirstChild?.Value;
                ReflectHelper.GetReflextHelper().SetPropertyValue(result, item, (object)tmpcontent);
            }

            return result;
        }
    }
}
