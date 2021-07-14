using System;
using System.Collections.Generic;

namespace LWY.Redis
{
    public class RedisDemo
    {
        public void Test()
        {
            HashSet<string> Hashset = new HashSet<string>();
            Hashset.Add("key");
            Hashset.Add("value");

            List<string> newList = new List<string>();
            newList.Add("123");
            newList.Add("456");

            HashSet<Model> obj = new HashSet<Model>();
            Model model = new Model
            {
                id = "123",
                name = "456"
            };
            obj.Add(model);

            //RedisCacheHelper.Add("wuchen1", "jiwuchen", DateTime.Now.AddDays(1));   ////存字符串
            //RedisCacheHelper.Add<List<string>>("wuchen", newList, DateTime.Now.AddDays(1));//存字符串数组
            //RedisCacheHelper.Add<HashSet<string>>("caoyu", Hashset, DateTime.Now.AddDays(1));//
            //RedisCacheHelper.Add<HashSet<Model>>("caoyu123", obj, DateTime.Now.AddDays(1));//存字符串

            //RedisCacheHelper.Add2("cy", "123", "456");///存哈希数据
            TimeSpan timeSpan = new TimeSpan(1);

            //  RedisCacheHelper.Add("cy123", "123", timeSpan);

            var s = RedisCacheHelper.Get<string>("cy123");
            //var s2 = RedisCacheHelper.Get<string>("caoyu");
            var s3 = RedisCacheHelper.GetAll();
            //bool bl = RedisCacheHelper.Exists("456", "cy");
            //RedisCacheHelper.Remove("456", "cy");
        }
    }

    public class Model
    {
        public string id { get; set; }

        public string name { get; set; }
    }
}
