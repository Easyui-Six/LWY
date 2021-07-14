using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWY.FW.Models
{
    public class Animal
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool Sex { get; set; }
        public int Age { get; set; }

        public double Life { get; set; }

        public string Speak { get; set; }

        
    }

    public class People : Animal
    {
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCard { get; set; }

        public string GetSelf()
        {
            return "大家好，我是PeoPle类，haha,我属于所有动物中最聪明的物种";
        }
    }

    public class Cat : Animal
    {
        public string OwnerCode { get; set; }
        public string GetSelf()
        {
            return "大家好，我是Cat类，miaomiao,我属于所有动物中最可爱的物种";
        }
    }

    public class Dog : Animal
    {
        public string OwnerCode { get; set; }
        public string GetSelf()
        {
            return "大家好，我是DOg类，WangWang,我属于所有动物中最忠诚的物种";
        }
    }

}
