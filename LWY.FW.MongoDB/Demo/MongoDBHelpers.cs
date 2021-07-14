using LWY.FW.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static LWY.FW.Common.LambdaExpressionBuilder;

namespace LWY.FW.MongoDB
{
    /// <summary>
    /// other People
    /// </summary>
    public class MongoDbCsharpHelper
    {
        private readonly string connectionString = null;
        private readonly string databaseName = null;
        private IMongoDatabase database = null;
        private readonly bool autoCreateDb = false;
        private readonly bool autoCreateCollection = false;

        static MongoDbCsharpHelper()
        {
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        }

        public MongoDbCsharpHelper(
            string mongoConnStr,
            string dbName,
            bool autoCreateDb = false,
            bool autoCreateCollection = false)
        {
            this.connectionString = mongoConnStr;
            this.databaseName = dbName;
            this.autoCreateDb = autoCreateDb;
            this.autoCreateCollection = autoCreateCollection;
        }

        #region 私有方法

        private MongoClient CreateMongoClient()
        {
            return new MongoClient(connectionString);
        }


        private IMongoDatabase GetMongoDatabase()
        {
            if (database == null)
            {
                var client = CreateMongoClient();
                if (!DatabaseExists(client, databaseName) && !autoCreateDb)
                {
                    throw new KeyNotFoundException("此MongoDB名称不存在：" + databaseName);
                }

                database = CreateMongoClient().GetDatabase(databaseName);
            }

            return database;
        }

        private bool DatabaseExists(MongoClient client, string dbName)
        {
            try
            {
                var dbNamess = client.ListDatabases().ToList();
                var dbNames = client.ListDatabases().ToList().Select(db => db.GetValue("name").AsString);
                return dbNames.Contains(dbName);
            }
            catch //如果连接的账号不能枚举出所有DB会报错，则默认为true
            {
                return true;
            }

        }

        private bool CollectionExists(IMongoDatabase database, string collectionName)
        {
            var options = new ListCollectionsOptions
            {
                Filter = Builders<BsonDocument>.Filter.Eq("name", collectionName)
            };

            return database.ListCollections(options).ToEnumerable().Any();
        }


        private IMongoCollection<TDoc> GetMongoCollection<TDoc>(string name, MongoCollectionSettings settings = null)
        {
            var mongoDatabase = GetMongoDatabase();

            if (!CollectionExists(mongoDatabase, name) && !autoCreateCollection)
            {
                throw new KeyNotFoundException("此Collection名称不存在：" + name);
            }

            return mongoDatabase.GetCollection<TDoc>(name, settings);
        }

        private List<UpdateDefinition<TDoc>> BuildUpdateDefinition<TDoc>(object doc, string parent)
        {
            var updateList = new List<UpdateDefinition<TDoc>>();
            foreach (var property in typeof(TDoc).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var key = parent == null ? property.Name : string.Format("{0}.{1}", parent, property.Name);
                //非空的复杂类型
                if ((property.PropertyType.IsClass || property.PropertyType.IsInterface) && property.PropertyType != typeof(string) && property.GetValue(doc) != null)
                {
                    if (typeof(IList).IsAssignableFrom(property.PropertyType))
                    {
                        #region 集合类型
                        int i = 0;
                        var subObj = property.GetValue(doc);
                        foreach (var item in subObj as IList)
                        {
                            if (item.GetType().IsClass || item.GetType().IsInterface)
                            {
                                updateList.AddRange(BuildUpdateDefinition<TDoc>(doc, string.Format("{0}.{1}", key, i)));
                            }
                            else
                            {
                                updateList.Add(Builders<TDoc>.Update.Set(string.Format("{0}.{1}", key, i), item));
                            }
                            i++;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 实体类型
                        //复杂类型，导航属性，类对象和集合对象
                        var subObj = property.GetValue(doc);
                        foreach (var sub in property.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        {
                            updateList.Add(Builders<TDoc>.Update.Set(string.Format("{0}.{1}", key, sub.Name), sub.GetValue(subObj)));
                        }
                        #endregion
                    }
                }
                else //简单类型
                {
                    updateList.Add(Builders<TDoc>.Update.Set(key, property.GetValue(doc)));
                }
            }

            return updateList;
        }


        private void CreateIndex<TDoc>(IMongoCollection<TDoc> col, string[] indexFields, CreateIndexOptions options = null)
        {
            if (indexFields == null)
            {
                return;
            }
            var indexKeys = Builders<TDoc>.IndexKeys;
            IndexKeysDefinition<TDoc> keys = null;
            if (indexFields.Length > 0)
            {
                keys = indexKeys.Descending(indexFields[0]);
            }
            for (var i = 1; i < indexFields.Length; i++)
            {
                var strIndex = indexFields[i];
                keys = keys.Descending(strIndex);
            }

            if (keys != null)
            {
                col.Indexes.CreateOne(keys, options);
            }

        }

        #endregion

        public void CreateCollectionIndex<TDoc>(string collectionName, string[] indexFields, CreateIndexOptions options = null)
        {
            CreateIndex(GetMongoCollection<TDoc>(collectionName), indexFields, options);
        }

        public void CreateCollection<TDoc>(string[] indexFields = null, CreateIndexOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            CreateCollection<TDoc>(collectionName, indexFields, options);
        }

        public void CreateCollection<TDoc>(string collectionName, string[] indexFields = null, CreateIndexOptions options = null)
        {
            var mongoDatabase = GetMongoDatabase();
            mongoDatabase.CreateCollection(collectionName);
            CreateIndex(GetMongoCollection<TDoc>(collectionName), indexFields, options);
        }


        public List<TDoc> Find<TDoc>(Expression<Func<TDoc, bool>> filter, FindOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            return Find<TDoc>(collectionName, filter, options);
        }

        public List<TDoc> Find<TDoc>(string collectionName, Expression<Func<TDoc, bool>> filter, FindOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            return colleciton.Find(filter, options).ToList();
        }


        public List<TDoc> FindByPage<TDoc, TResult>(Expression<Func<TDoc, bool>> filter, Expression<Func<TDoc, TResult>> keySelector, int pageIndex, int pageSize, out int rsCount)
        {
            string collectionName = typeof(TDoc).Name;
            return FindByPage<TDoc, TResult>(collectionName, filter, keySelector, pageIndex, pageSize, out rsCount);
        }

        public List<TDoc> FindByPage<TDoc, TResult>(string collectionName, Expression<Func<TDoc, bool>> filter, Expression<Func<TDoc, TResult>> keySelector, int pageIndex, int pageSize, out int rsCount)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            rsCount = colleciton.AsQueryable().Where(filter).Count();

            int pageCount = rsCount / pageSize + ((rsCount % pageSize) > 0 ? 1 : 0);
            if (pageIndex > pageCount) pageIndex = pageCount;
            if (pageIndex <= 0) pageIndex = 1;

            return colleciton.AsQueryable(new AggregateOptions { AllowDiskUse = true }).Where(filter).OrderByDescending(keySelector).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public void Insert<TDoc>(TDoc doc, InsertOneOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            Insert<TDoc>(collectionName, doc, options);
        }

        public void Insert<TDoc>(string collectionName, TDoc doc, InsertOneOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.InsertOne(doc, options);
        }


        public void InsertMany<TDoc>(IEnumerable<TDoc> docs, InsertManyOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            InsertMany<TDoc>(collectionName, docs, options);
        }

        public void InsertMany<TDoc>(string collectionName, IEnumerable<TDoc> docs, InsertManyOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.InsertMany(docs, options);
        }

        public void Update<TDoc>(TDoc doc, Expression<Func<TDoc, bool>> filter, UpdateOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            List<UpdateDefinition<TDoc>> updateList = BuildUpdateDefinition<TDoc>(doc, null);
            colleciton.UpdateOne(filter, Builders<TDoc>.Update.Combine(updateList), options);
        }

        public void Update<TDoc>(string collectionName, TDoc doc, Expression<Func<TDoc, bool>> filter, UpdateOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            List<UpdateDefinition<TDoc>> updateList = BuildUpdateDefinition<TDoc>(doc, null);
            colleciton.UpdateOne(filter, Builders<TDoc>.Update.Combine(updateList), options);
        }


        public void Update<TDoc>(TDoc doc, Expression<Func<TDoc, bool>> filter, UpdateDefinition<TDoc> updateFields, UpdateOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            Update<TDoc>(collectionName, doc, filter, updateFields, options);
        }

        public void Update<TDoc>(string collectionName, TDoc doc, Expression<Func<TDoc, bool>> filter, UpdateDefinition<TDoc> updateFields, UpdateOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.UpdateOne(filter, updateFields, options);
        }


        public void UpdateMany<TDoc>(TDoc doc, Expression<Func<TDoc, bool>> filter, UpdateOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            UpdateMany<TDoc>(collectionName, doc, filter, options);
        }


        public void UpdateMany<TDoc>(string collectionName, TDoc doc, Expression<Func<TDoc, bool>> filter, UpdateOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            List<UpdateDefinition<TDoc>> updateList = BuildUpdateDefinition<TDoc>(doc, null);
            colleciton.UpdateMany(filter, Builders<TDoc>.Update.Combine(updateList), options);
        }


        public void Delete<TDoc>(Expression<Func<TDoc, bool>> filter, DeleteOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            Delete<TDoc>(collectionName, filter, options);
        }

        public void Delete<TDoc>(string collectionName, Expression<Func<TDoc, bool>> filter, DeleteOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.DeleteOne(filter, options);
        }


        public void DeleteMany<TDoc>(Expression<Func<TDoc, bool>> filter, DeleteOptions options = null)
        {
            string collectionName = typeof(TDoc).Name;
            DeleteMany<TDoc>(collectionName, filter, options);
        }


        public void DeleteMany<TDoc>(string collectionName, Expression<Func<TDoc, bool>> filter, DeleteOptions options = null)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            colleciton.DeleteMany(filter, options);
        }

        public void ClearCollection<TDoc>(string collectionName)
        {
            var colleciton = GetMongoCollection<TDoc>(collectionName);
            var inddexs = colleciton.Indexes.List();
            List<IEnumerable<BsonDocument>> docIndexs = new List<IEnumerable<BsonDocument>>();
            while (inddexs.MoveNext())
            {
                docIndexs.Add(inddexs.Current);
            }
            var mongoDatabase = GetMongoDatabase();
            mongoDatabase.DropCollection(collectionName);

            if (!CollectionExists(mongoDatabase, collectionName))
            {
                CreateCollection<TDoc>(collectionName);
            }

            if (docIndexs.Count > 0)
            {
                colleciton = mongoDatabase.GetCollection<TDoc>(collectionName);
                foreach (var index in docIndexs)
                {
                    foreach (IndexKeysDefinition<TDoc> indexItem in index)
                    {
                        try
                        {
                            colleciton.Indexes.CreateOne(indexItem);
                        }
                        catch
                        { }
                    }
                }
            }

        }
    }


    /// <summary>
    /// MySelf  Mongodb 帮助类
    /// </summary>
    public class MongoDBHelper
    {
        // 定义接口
        protected static IMongoDatabase _database;
        // 定义客户端
        protected static IMongoClient _client;

        /// <summary>
        /// 连接字符串 示例 mongodb://127.0.0.1:27017/admin    默认对应的数据库是  admin
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
            //  根据数据库名称实例化数据库 这里可以多个数据库的
            _database = _client.GetDatabase(databaseName);

        }

        /// <summary>
        /// 设置数据库名称？？这里可以跳转到 当前用户下所有能操作的数据下。
        /// </summary>
        /// <param name="dataBaseName"></param>
        public void SetDataBase(string dataBaseName)
        {
            _client.ListDatabases().ToList().Select(db => db.GetValue("name").AsString);
            _database = _client.GetDatabase(dataBaseName);
        }
        /// <summary>
        /// 查询当前数据库的表集合
        /// </summary>
        /// <returns></returns>
        public List<string> QueryCollection()
        {
            return _database.ListCollectionNamesAsync().Result.ToList();
        }

        /// <summary>
        /// 创建表
        /// true  创建成功  flase  创建失败 表已存在
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public bool CreateCollection(string collectionName)
        {
            if (ExitCollection(collectionName))
            {
                return false;
            }

            _database.CreateCollection(collectionName);
            return true;

        }

        /// <summary>
        /// 修改结合名称
        /// true  修改成功  flase 要修改的表名已存在，不能修改
        /// </summary>
        /// <param name="newName"></param>
        /// <param name="oldName"></param>
        /// <returns></returns>
        public bool ReNameCollection(string newName, string oldName)
        {
            if (ExitCollection(newName))
            {
                return false;
            }

            _database.RenameCollection(oldName, newName);
            return true;

        }

        /// <summary>
        /// 删除表
        /// ture 删除成功 flase 对应的集合名称不存在   如果数据过多会耗时
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public bool RemoveCollrction(string collectionName)
        {
            if (!ExitCollection(collectionName))
            {
                return false;
            }

            _database.DropCollection(collectionName);
            return true;

        }

        /// <summary>
        /// 库中是否存在该表 collectionName
        /// true  存在  false  不存在
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public bool ExitCollection(string collectionName)
        {
            var options = new ListCollectionsOptions
            {
                Filter = Builders<BsonDocument>.Filter.Eq("name", collectionName)
            };

            return _database.ListCollections(options).ToEnumerable().Any();

            //这种方式不行，这种方式mongodb集合如果不存在则 会返回一个新的
            //var collection = _database.GetCollection<BsonDocument>(collectionName);
            //return collection == null ? false : true;
        }




        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool AddDocument<T>(string collectionName, T list) where T : class
        {
            if (!ExitCollection(collectionName))
            {
                return false;
            }
            var collection = _database.GetCollection<T>(collectionName);
            collection.InsertOne(list, null);
            //collection.InsertOneAsync(list, null);

            return true;
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool AddDocuments<T>(string collectionName, List<T> list) where T : class
        {
            if (!ExitCollection(collectionName))
            {
                return false;
            }
            var collection = _database.GetCollection<T>(collectionName);
            collection.InsertMany(list, null);
            //collection.InsertManyAsync(list, null);
            return true;
        }


        /// <summary>
        /// 单个或批量修改，批量修改的字段是相同的，
        /// todo：批量修改不同的数据；
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">名字</param>
        /// <param name="filters">修改的字段依据</param>
        /// <param name="dic">要修改的键值对</param>
        /// <param name="IsMamy">是否批量操作</param>
        /// <returns></returns>
        public bool EditDocument<T>(string collectionName, FilterCollection filters, Dictionary<string, string> dic, bool IsMamy) where T : class
        {
            if (!ExitCollection(collectionName))
            {
                return false;
            }
            var collection = _database.GetCollection<T>(collectionName);

            var updatecompareExp = LambdaExpressionBuilder.GetExpression<T>(filters);

            var firstdic = dic.First();
            var update = Builders<T>.Update.Set(firstdic.Key, firstdic.Value);
            foreach (var item in dic)
            {
                //第一个已经处理过了，不需要在处理了
                if (item.Key.Equals(firstdic.Key))
                {
                    continue;
                }
                update = update.Set(item.Key, item.Value);
            }
            //是否批量操作
            if (IsMamy)
            {
                UpdateResult upresultMony = collection.UpdateMany<T>(updatecompareExp, update);
            }
            else
            {//修改一个，是找到的第一个元素
                UpdateResult upresult = collection.UpdateOne<T>(updatecompareExp, update);
            }



            #region  编辑的其他写法
            //var query = Query.And(Query.EQ("Name", "DogA"));
            //var tmp = Builders<T>.Update.Set("Age", "38");
            //UpdateResult upresult = collection.UpdateOne<T>(updatecompareExp, tmp);


            #endregion
            return true;
        }

        /// <summary>
        /// 单个或批量删除数据，单个删除时，默认删除找到的第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName">表名</param>
        /// <param name="filters">删除的字段依据</param>
        /// <param name="IsMany">是否 批量删除</param>
        /// <returns></returns>
        public bool RemoveDocument<T>(string collectionName, FilterCollection filters, bool IsMany)
        {
            if (!ExitCollection(collectionName))
            {
                return false;
            }
            var collection = _database.GetCollection<T>(collectionName);
            var removecompareExp = LambdaExpressionBuilder.GetExpression<T>(filters);
            //是否批量操作
            if (IsMany)
            {
                DeleteResult result = collection.DeleteMany<T>(removecompareExp);
            }
            else
            {
                DeleteResult result = collection.DeleteOne<T>(removecompareExp);
            }

            return true;
        }


        /// <summary>
        /// 创建索引
        /// todo：组合索引等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="indexs">索引对应的字段名称集合</param>
        /// <returns></returns>
        public bool SetCollectionIndex<T>(string collectionName, List<string> indexs) where T : class
        {

            if (!ExitCollection(collectionName))
            {
                return false;
            }
            var collection = _database.GetCollection<T>(collectionName);

            ////创建索引  带扩展  添加多个索引，删除索引等，在indexs 返回类中查看
            List<IndexKeysDocument> docs = new List<IndexKeysDocument>();//新建索引
            List<CreateIndexModel<string>> createIndexModels = new List<CreateIndexModel<string>>();

            foreach (var item in indexs)
            {
                BsonValue value = BsonValue.Create("2d");//创建2d索引
                //doc.Add(item, value);//lo为数据库中2d索引的对象名称
                docs.Add(new IndexKeysDocument(item, value));
                createIndexModels.Add(new CreateIndexModel<string>(item));
            }

            ///collection.Indexes.CreateMany<string>(createIndexModels);
            //单个索引
            //IndexKeysDocument onebsonElements = new IndexKeysDocument();
            //BsonValue value = BsonValue.Create("2d");//创建2d索引
            //onebsonElements.Add("Cat", value);//lo为数据库中2d索引的对象名称
            //collection.Indexes.CreateOne(onebsonElements);

            return true;
        }

        /// <summary>
        /// 查询单个表的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public List<T> QueryCollectionDocument<T>(string collectionName, FilterCollection filters)
        {
            if (!ExitCollection(collectionName))
            {
                return null;
            }
            var collection = _database.GetCollection<T>(collectionName);
            var querycompareExp = LambdaExpressionBuilder.GetExpression<T>(filters);
            return collection.AsQueryable<T>().Where(querycompareExp).ToList();
        }

        /// <summary>
        /// 多个表查询数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionAnimal"></param>
        /// <param name="collectionPeople"></param>
        /// <returns></returns>
        public List<T> QueryCollectionsDocument<T>(IMongoCollection<Animal> collectionAnimal,
            IMongoCollection<People> collectionPeople)
        {
            var animalIQuery = collectionAnimal.AsQueryable();
            var peopleIQuery = collectionPeople.AsQueryable();
            var result = (from a in animalIQuery
                              //join b in peopleIQuery on a.Name equals b.Name
                          join c in peopleIQuery on new { a.Name, a.Age } equals new { c.Name, c.Age }
                          into sb
                          from re in sb.DefaultIfEmpty()//加了这个 为左连接或右连接 不加 全连接
                          //group regroup by re.Age
                          select sb);
            var st = (from a in animalIQuery
                     group a by a.Name);//group by 

            return null;
        }


    }



    public class Animal
    {
        public string Name { get; set; }
        public string Age { get; set; }

        public string AnimalFeat { get; set; }
    }
    public class People
    {
        public string Name { get; set; }
        public string Age { get; set; }

        public string PeopleFeat { get; set; }
    }
    //声明默认主键，不然在查询时 会报错
    [BsonIgnoreExtraElements]
    public class Dog
    {
        public string Name { get; set; }
        public string Age { get; set; }

        public string DogFeat { get; set; }
    }
}