using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LWY.FW.Common
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
        public void Query(string collectionName, string key)
        {
            configHelper configHelper = new configHelper();

            var mongoUrl = new MongoUrlBuilder(configHelper.MongodbConn);
            
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
            string entityDbName=mongoClient.ListDatabaseNames().ToList().Where(x => x.Contains(dbName)).Select(a=>a).ToString();
            if (!string.IsNullOrWhiteSpace(entityDbName) && entityDbName.Equals(dbName))
            {
                return true;
            }
            return false;
        }



    }
}
