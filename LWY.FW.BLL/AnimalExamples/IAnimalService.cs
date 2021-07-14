using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWY.FW.BLL
{
   public interface IAnimalService
    {
        /// <summary>
        /// 让动物做一个自我介绍哈
        /// </summary>
        /// <returns></returns>
        string SelfIntroduction();

        /// <summary>
        /// 获取动物的名字
        /// </summary>
        /// <returns></returns>
        string GetAnimalName();

        /// <summary>
        /// 获取动物说话的方式
        /// </summary>
        /// <returns></returns>
        string Speak();
        /// <summary>
        /// 获取动物的寿命
        /// </summary>
        /// <returns></returns>
        string GetAnimalLife();

    }
}
