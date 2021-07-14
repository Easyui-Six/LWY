
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LWY.FW.MongoDB
{


    /// <summary>
    /// 单纯的连接、增加、查找和删除
    /// </summary>
    public class MongoDBDEmo
    {

        // 定义接口
        protected static IMongoDatabase _database;
        // 定义客户端
        protected static IMongoClient _client;

        /// <summary>
        /// 连接字符串 mongodb://127.0.0.1:27017/Demo    数据库名称：Demo
        /// </summary>
        /// <param name="mongodbConn"></param>
        public void SetMongoDB(string mongodbConn)
        {
            var mongoUrl = new MongoUrlBuilder(mongodbConn);

            // 获取数据库名称
            string databaseName = mongoUrl.DatabaseName;
            // 创建并实例化客户端
            _client = new MongoClient(mongoUrl.ToMongoUrl());

            _client.ListDatabases().ToList().Select(db => db.GetValue("name").AsString);
            //  根据数据库名称实例化数据库 这里可以多个数据库的
            _database = _client.GetDatabase(databaseName);

        }


        /// <summary>
        /// 开启认证
        /// </summary>
        /// <param name="mongodbConn"></param>
        /// <param name="userNmae">账户</param>
        /// <param name="pwd">密码</param>
        public void SetMongoDBAuth(string mongodbConn, string userNmae, string pwd)
        {
            var mongoUrl = new MongoUrlBuilder(mongodbConn);

            mongoUrl.Username = userNmae;
            mongoUrl.Password = pwd;
            // 获取数据库名称
            string databaseName = mongoUrl.DatabaseName;
            // 创建并实例化客户端
            _client = new MongoClient(mongoUrl.ToMongoUrl());

            _client.ListDatabases().ToList().Select(db => db.GetValue("name").AsString);
           // 根据数据库名称实例化数据库 这里可以多个数据库的
            _database = _client.GetDatabase(databaseName);

        }

        

        /// <summary>
        /// 插入  可以多条插入
        /// </summary>
        /// <param name="collectionName">集合名称，相当于数据表</param>
        /// <param name="content">内容   MogonDB的文档 相当于数据行</param>
        public void Add(string collectionName, Dictionary<string, string> content)
        {
            // 根据集合名称获取集合  集合即关系型数据中的 表名
            var collection = _database.GetCollection<BsonDocument>(collectionName);

            #region 单数据插入 
            //foreach (var item in content)
            //{
            //    //方式一
            //    BsonElement bsonElement = new BsonElement(item.Key, item.Value);
            //    BsonDocument bsonElements = new BsonDocument(bsonElement);
            //    //方拾贰
            //    BsonDocument bsonElementBs = new BsonDocument(item.Key, item.Value);
            //    //方式叁
            //    Dictionary<string, object> dic = new Dictionary<string, object>();
            //    dic.Add(item.Key, item.Value);
            //    BsonDocument bsonElementCs = new BsonDocument(dic);

            //    collection.InsertOneAsync(bsonElements);
            //    collection.InsertOneAsync(bsonElementBs);
            //    collection.InsertOneAsync(bsonElementCs);
            //}
            #endregion


            #region 多数据插入

            //1.2 多个数据插入
            List<BsonDocument> bsonElements1 = new List<BsonDocument>();
            //这里是个list集合即可
            foreach (var item in content)
            {
                bsonElements1.Add(new BsonDocument(item.Key, item.Value));
            }
            collection.InsertManyAsync(bsonElements1);


            #endregion 

        }

        /// <summary>
        /// 查询数据，查所有的和查点那个的
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="content"></param>
        public object QueryDocument(string collectionName, Dictionary<string, string> content)
        {
            // 根据集合名称获取集合  集合即关系型数据中的 表名
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            var filter = new BsonDocument();
            // 查询集合中的文档   即 关系型数据库中的数据
            var list = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;
            //示例
            var oneFilter = new BsonDocument("Cat", "miaomiao");

            //Expression<Func<TDoc, bool>> filter
            var oneCat = Task.Run(async () => await collection.Find(oneFilter).ToListAsync()).Result;
            return list;
        }

        public object QueryLinq(string collectionName,string collectionNameB)
        {
            var collection = _database.GetCollection<DemoTestB>(collectionName);
            var collectionB = _database.GetCollection<DemoTestC>(collectionNameB);

            //拉姆达表达式和linq语法，一摸一样
            var resultA = collection.AsQueryable().Where(a => a.Cat == "123").FirstOrDefault();
            //连接查询 去结果
            var result1 = from a  in collection.AsQueryable()
                          join b in collectionB.AsQueryable()
                            on a.Cat equals b.LiulLian
                          select new { stuno = b.LiulLian, a.Cat };

            ////创建索引  带扩展  添加多个索引，删除索引等，在indexs 返回类中查看
            IndexKeysDocument doc = new IndexKeysDocument();//新建索引
            BsonValue value = BsonValue.Create("2d");//创建2d索引
            doc.Add("Cat", value);//lo为数据库中2d索引的对象名称
            collection.Indexes.CreateOne(doc);
            return "";
        }
        /// <summary>
        /// 删除，只支持单个删除
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="content"></param>
        public void DeleteDocument(string collectionName, Dictionary<string, string> content)
        {
            // 根据集合名称获取集合  集合即关系型数据中的 表名
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            //示例
            var oneFilter = new BsonDocument("Cat", "miaomiao");
            //Expression<Func<TDoc, bool>> filter
            collection.DeleteOne(oneFilter);
        }


        public void Query(string mongodbConn, string collectionName, string key)
        {

            var mongoUrl = new MongoUrlBuilder(mongodbConn);

            // 获取数据库名称
            string databaseName = mongoUrl.DatabaseName;
            // 创建并实例化客户端
            _client = new MongoClient(mongoUrl.ToMongoUrl());

            _client.ListDatabases().ToList().Select(db => db.GetValue("name").AsString);
            //  根据数据库名称实例化数据库
            _database = _client.GetDatabase(databaseName);
            // 根据集合名称获取集合  集合即关系型数据中的 表名
            var collection = _database.GetCollection<BsonDocument>(collectionName);

            //1. 插入数据
            //1.1 单个数据
            BsonElement bsonElement = new BsonElement("Cat", "miaomiao");
            BsonDocument bsonElements = new BsonDocument(bsonElement);
            BsonDocument bsonElementBs = new BsonDocument("Dog", "Wangwang");
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("liulian", "my Name is LiuLian");
            BsonDocument bsonElementCs = new BsonDocument(dic);

            collection.InsertOneAsync(bsonElements);
            collection.InsertOneAsync(bsonElementBs);
            collection.InsertOneAsync(bsonElementCs);
            //1.2 多个数据插入
            List<BsonDocument> bsonElements1 = new List<BsonDocument>();
            //这里是个list集合即可
            bsonElements1.Add(new BsonDocument("XiaopangSpeak", "miaowo,miaowo"));
            bsonElements1.Add(new BsonDocument("Xiaopang", "my Name is XiaoPang"));
            collection.InsertManyAsync(bsonElements1);





            var filter = new BsonDocument();
            // 查询集合中的文档   即 关系型数据库中的数据
            var list = Task.Run(async () => await collection.Find(filter).ToListAsync()).Result;

            var oneFilter = new BsonDocument("Cat", "miaomiao");
            //Expression<Func<TDoc, bool>> filter
            var oneCat = Task.Run(async () => await collection.Find(oneFilter).ToListAsync()).Result;


            collection.DeleteOne(oneFilter);

        }

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
    /// <summary>
    /// 封装过的  数据库类
    /// </summary>
    public class MongoHelper
    {




        /// <summary>
        /// 是否存在该数据库
        /// </summary>
        /// <param name="mongoClient"></param>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public bool ExitDBName(MongoClient mongoClient, string dbName)
        {
            string entityDbName = mongoClient.ListDatabaseNames().ToList().Where(x => x.Contains(dbName)).Select(a => a).ToString();
            if (!string.IsNullOrWhiteSpace(entityDbName) && entityDbName.Equals(dbName))
            {
                return true;
            }
            return false;
        }



    }
}
