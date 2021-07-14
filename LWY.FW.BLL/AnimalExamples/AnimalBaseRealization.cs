using LWY.FW.Models;

namespace LWY.FW.BLL
{
    public abstract class AnimalBaseRealization
    {
        protected Animal Animals { get; set; }

        public string GetAnimalName()
        {
            return this.Animals.Name;
        }


        public string Speak()
        {
            return this.Animals.Speak;
        }

        public string GetAnimalLife()
        {
            return this.Animals.Life.ToString();
        }
      

    }
}
