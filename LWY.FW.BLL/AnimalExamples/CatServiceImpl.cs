using LWY.FW.Models;

namespace LWY.FW.BLL
{
    /// <summary>
    /// 公用的方法放在基类里
    /// 特有的方法放在接口实现类里，
    /// 
    /// </summary>
    public class CatServiceImpl : AnimalBaseRealization, IAnimalService
    {
        public CatServiceImpl()
        {
            this.Animals = new Cat();
        }
        /// <summary>
        /// 做一个自我介绍
        /// </summary>
        /// <returns></returns>
        public string SelfIntroduction()
        {
            return new Cat().GetSelf();
        }
    }
}
