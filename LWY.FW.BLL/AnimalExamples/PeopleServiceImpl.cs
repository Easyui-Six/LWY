using LWY.FW.Models;

namespace LWY.FW.BLL
{
    public class PeopleServiceImpl : AnimalBaseRealization, IAnimalService
    {
        public PeopleServiceImpl()
        {
            this.Animals = new People();
        }
        /// <summary>
        /// 做一个自我介绍
        /// </summary>
        /// <returns></returns>
        public string SelfIntroduction()
        {
            return new People().GetSelf();
        }

    }
}
