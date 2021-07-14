using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LWY.Api
{
    /// <summary>
    /// 获取配置文件的数据
    /// </summary>
    public class ConfigHelper
    {
        //声明要使用的配置字段
        private static string mongodbConn;
        public static string MongodbConn { get { return mongodbConn; } }

        private static ConfigDemoModel model;
        public static ConfigDemoModel Model { get { return model; } }

        /// <summary>
        /// 从配置文件中将数据拿出来
        /// </summary>
        /// <param name="configuration"></param>
        public void Init(IConfiguration configuration)
        {
            //获取字符串
            mongodbConn = configuration["mongodbConn"];

            //获取一个对象
            model = new ConfigDemoModel();
            model.Name = configuration["ConfigDemo:Name"];
            model.Sex = int.Parse(configuration["ConfigDemo:Sex"]);

            Dog tmpDog = new Dog();
            tmpDog.Age = int.Parse(configuration["ConfigDemo:Dog:Age"]);
            tmpDog.Speak = configuration["ConfigDemo:Dog:Speak"];
            model.dog = tmpDog;
        }

        /// <summary>
        /// 取其他配置文件的内容
        /// Step：1.设置配置文件  属性设置为 始终复制
        /// 2.安装包   Microsoft.Extensions.Configuration
        /// 3.这里可以在任何地方使用
        /// </summary>
        /// <param name="fileName"></param>
        public List<Dog> InitByFileName(string fileName)
        {
            //fileName
            var builder = new ConfigurationBuilder().AddJsonFile("JsonFile/jsonDemo.json");
            var config = builder.Build();

            List<Dog> dogs = new List<Dog>();

            Dog DogA = new Dog(); Dog DogB = new Dog();
            DogA.Speak = config["Dogs:0:Speak"];
            DogA.Age = int.Parse(config["Dogs:0:Age"]);
            DogB.Speak = config["Dogs:1:Speak"];
            DogB.Age = int.Parse(config["Dogs:1:Age"]);
            dogs.Add(DogA); dogs.Add(DogB);
            return dogs;
        }
    }

    public class ConfigDemoModel
    {
        public string Name { get; set; }

        public int Sex { get; set; }

        public Dog dog { get; set; }
    }
    public class Dog
    {
        public string Speak { get; set; }

        public int Age { get; set; }
    }
}
