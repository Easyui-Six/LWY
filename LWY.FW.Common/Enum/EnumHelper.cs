using System;
using System.ComponentModel;

namespace LWY.FW.Common
{
    public class EnumHelper
    {
        /// <summary>
        /// 获取 枚举的Description
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetsDescription<T>(int value)
        {
            var attr = "";
            foreach (int key in Enum.GetValues(typeof(T)))
            {
                if (key != value) continue;
                Type type = typeof(T);
                System.Reflection.MemberInfo[] memInfo = type.GetMember(Enum.GetName(type, key));
                if (memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attrs.Length > 0)
                        attr = ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return attr;
        }

        //取枚举字符串 AnimalEnum.People+""

        //取枚举索引      (int)AnimalEnum.People
    }

    /// <summary>
    /// 禁止客户端读取属性
    /// </summary>
    public class DisabledClientAttribute : Attribute
    {

    }


}
