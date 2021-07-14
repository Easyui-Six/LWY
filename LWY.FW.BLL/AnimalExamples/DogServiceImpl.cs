using LWY.FW.Models;

namespace LWY.FW.BLL
{
    public  class DogServiceImpl : AnimalBaseRealization, IAnimalService
    {
        public DogServiceImpl()
        {
            this.Animals = new Dog();
        }
        /// <summary>
        /// 做一个自我介绍
        /// </summary>
        /// <returns></returns>
        public string SelfIntroduction()
        {
            return new Dog().GetSelf();
        }
    }
}
