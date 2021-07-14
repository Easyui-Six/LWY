using LWY.FW.MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LWY.FW.Common.LambdaExpressionBuilder;

namespace MongoDBDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // 本地测试
            ApiMongoDBTset();
            //MongoDBDEmo mongoDBDEmo = new MongoDBDEmo();
            //mongoDBDEmo.SetMongoDB("mongodb://127.0.0.1:27017/Demo");
            ////.Query("mongodb://127.0.0.1:27017/Demo", "demo", "");
            //object obj = mongoDBDEmo.QueryDocument("demo", new Dictionary<string, string>());

            // 别人封装好的代码研究
            //#region  
            ////初始化入参必须是数据库中已经存在的数据库
            //MongoDbCsharpHelper mongoDbHelper = new MongoDbCsharpHelper("mongodb://127.0.0.1:27017", "lwyDemo");

            ////创建数据库中的表
            //mongoDbHelper.CreateCollection<DemoTest>(new[] { "DemoTest" });
            ////往表里插入 一条数据
            //mongoDbHelper.Insert<DemoTest>("DemoTest", new DemoTest { d = "Info", Dog = "WangWang" });
            ////mongoDbHelper.Insert<DemoTestB>("DemoTest", new DemoTestB { Cat = "xiaopang", Speak = "miaomiao" });
            ////mongoDbHelper.Insert<DemoTestC>("DemoTest", new DemoTestC { LiulLian = "LiulLian", Speak = "miaomiao",LiulLianSpeak="wuwuwu" });
            ////往表里插入多条数据
            //List<DemoTestB> demoTestBs = new List<DemoTestB>();
            //demoTestBs.Add(new DemoTestB()
            //{
            //    Cat = "xiaopang",
            //    Speak = "miaomiao"
            //});
            //demoTestBs.Add(new DemoTestB()
            //{
            //    Cat = "LiuLian",
            //    Speak = "wuwuuwu"
            //});
            //mongoDbHelper.InsertMany<DemoTestB>("DemoTest", demoTestBs);

            ////查询跟某个字段的值相等的数据
            //List<DemoTest> s = mongoDbHelper.Find<DemoTest>("Demo", t => t.Dog == "WangWang");
            ////删除某个数据
            //mongoDbHelper.Delete<DemoTest>("Demo", t => t.Dog == "WangWang");
            ////清楚整个表内容
            //mongoDbHelper.ClearCollection<DemoTest>("Demo");
            ////todo  关联查询 创建索引

            ////    var mongoDbHelper = new MongoDbCsharpHelper("MongoDbConnectionString", "LogDB");

            ////mongoDbHelper.CreateCollection<SysLogInfo>("SysLog1", new[] { "LogDT" });

            ////mongoDbHelper.Find<SysLogInfo>("SysLog1", t => t.Level == "Info");

            ////int rsCount = 0;
            ////mongoDbHelper.FindByPage<SysLogInfo, SysLogInfo>("SysLog1", t => t.Level == "Info", t => t, 1, 20, out rsCount);

            ////mongoDbHelper.Insert<SysLogInfo>("SysLog1", new SysLogInfo { LogDT = DateTime.Now, Level = "Info", Msg = "测试消息" });

            ////mongoDbHelper.Update<SysLogInfo>("SysLog1", 
            ///new SysLogInfo { LogDT = DateTime.Now, Level = "Error", Msg = "测试消息2" },
            ///t => t.LogDT == new DateTime(1900, 1, 1));

            ////mongoDbHelper.Delete<SysLogInfo>(t => t.Level == "Info");

            ////mongoDbHelper.ClearCollection<SysLogInfo>("SysLog1");

            //#endregion 

            Console.WriteLine("123");
            Console.ReadLine();

        }

        /// <summary>
        /// MongoDb 的手写类  demo
        /// </summary>
        static void ApiMongoDBTset()
        {
            MongoDBHelper mongoDBHelper = new MongoDBHelper();
            //mongoDBHelper.SetMongoDB("mongodb://127.0.0.1:27017/admin");// NO Auth 直接连数据库
            //Auth  1
            //1.1. 用户信息是跟随数据库的。 数据库名称和对应的账户密码要对应
            //1.2. 认证通过之后，默认取当前数据库，可以切换到当前用户拥有权限的任意数据库下，使用SetDataBase
            mongoDBHelper.SetMongoDBAuth("mongodb://127.0.0.1:27017/admin", "lwy", "woaini");
            //切换数据库集合(表)
            mongoDBHelper.SetDataBase("lwyDemo");
            List<string> collections = mongoDBHelper.QueryCollection();
            // 2.也可以直接验证相应的数据库；
            mongoDBHelper.SetMongoDBAuth("mongodb://127.0.0.1:27017/lwyDemo", "caoyu", "123456");
            List<string> collectionss = mongoDBHelper.QueryCollection();
            //3.创建数据库？ 暂不处理；实现：创建集合(表)、更新集合名称、清楚集合数据；
            //3.1 Create
            mongoDBHelper.CreateCollection("SysLog");
            //3.2 ReName
            mongoDBHelper.ReNameCollection("Logs", "SysLog");
            //3.3 Delete
            mongoDBHelper.RemoveCollrction("Logs");

            //4 往集合中添加数据（批量） 更新数据（批量） 删除数据（批量)  查询数据  创建索引
            mongoDBHelper.CreateCollection("SysLog");
            //4.1 批量添加数据，可以添加不同类型的对象
            List<Dog> result = new List<Dog>() {
            new Dog(){ Name="DogA"+DateTime.Now.ToString("yyyyMMddHHmmss"),Age="10",DogFeat="WangWang"},
            new Dog(){  Name="DogA"+DateTime.Now.ToString("yyyyMMddHHmmss"),Age="12",DogFeat="WangWang"},
            new Dog(){  Name="DogA",Age="12",DogFeat="WangWang"},
            new Dog(){  Name="DogA",Age="13",DogFeat="CatMiaoMiao"},
            new Dog(){  Name="DogA",Age="14",DogFeat="CatMiaoMiao"}
            };
            mongoDBHelper.AddDocuments<Dog>("SysLog", result);
            List<People> peopleresult = new List<People>() {
            new People(){ Name="peopleA"+DateTime.Now.ToString("yyyyMMddHHmmss"),Age="10",PeopleFeat="haha"},
            new People(){  Name="peopleA"+DateTime.Now.ToString("yyyyMMddHHmmss"),Age="10",PeopleFeat="haha"}
            };
            mongoDBHelper.AddDocuments<People>("SysLog", peopleresult);
            //4.2 更新数据

            //筛选条件FilterCollection 不同数组之间是 and  相同数组之间是or
            FilterCollection updtaeFilters = new FilterCollection();
            IList<Filter> updatefilters1 = new List<Filter>();
            updatefilters1.Add(new Filter()
            {
                PropertyName = "Name",
                Operation = Op.Equals,
                Value = "doga"// 这里如果用 express帮助类要用小写，里面做了转换小写的处理
            }); updtaeFilters.Add(updatefilters1);
            //要修改的字段结合
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            //keyValuePairs.Add("Name", "xiaopang");
            keyValuePairs.Add("Age", "60Days");
            keyValuePairs.Add("DogFeat", "wawa");
            mongoDBHelper.EditDocument<Dog>("SysLog", updtaeFilters, keyValuePairs, true);

            //4.3批量删除数据
            mongoDBHelper.RemoveDocument<Dog>("SysLog", updtaeFilters, false);
            mongoDBHelper.RemoveDocument<Dog>("SysLog", updtaeFilters, true);

            //4.4创建索引 删除索引
            List<string> indexs = new List<string>() {
           "Name", //Dog.Name,
            "Age"//Dog.Age,
            };
            mongoDBHelper.SetCollectionIndex<Dog>("SysLog", indexs);
            mongoDBHelper.RemoveCollectionIndex<Dog>("SysLog", indexs);


            //4.5 查询


            //4.6  添加视图；多表连接查询


            //4.7 插入的数据，自定义主键








        }

        static void CoreMongoDBTset()
        {

        }
    }

    //[BsonIgnoreExtraElements]
    public class DemoTest
    {
        public ObjectId _id { get; set; }
        public string b { get; set; }
        public string d { get; set; }
        public string Dog { get; set; }
        public string LiulLian { get; set; }
        public string XiaopangSpeak { get; set; }
    }
    public class DemoTestB
    {
        public string Cat { get; set; }
        public string Speak { get; set; }
    }

    public class DemoTestC
    {
        public string LiulLian { get; set; }
        public string Speak { get; set; }

        public string LiulLianSpeak { get; set; }
    }

}
