
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redis;



namespace LWY.Redis
{
    public class RedisCacheHelper
    {
        protected RedisClient Redis = new RedisClient("127.0.0.1", 6379, "lwywoaini");

        private static readonly PooledRedisClientManager pool = null;
        private static readonly string[] redisHosts = null;
        public static int RedisMaxReadPool = 3;
        public static int RedisMaxWritePool = 2;

        static RedisCacheHelper()
        {
            var redisHostStr = "127.0.0.1:6379";

            if (!string.IsNullOrEmpty(redisHostStr))
            {
                redisHosts = redisHostStr.Split(',');
                ////组装密码
                redisHosts[0] = "lwywoaini@" + redisHosts[0];

                if (redisHosts.Length > 0)
                {
                    pool = new PooledRedisClientManager(redisHosts, redisHosts,
                        new RedisClientManagerConfig()
                        {
                            DefaultDb=4,////使用第几个位置的数据库0-15
                            MaxWritePoolSize = RedisMaxWritePool,
                            MaxReadPoolSize = RedisMaxReadPool,
                            AutoStart = true
                        });
                }
            }
        }

        #region Add

        /// <summary>
        /// 添加和修改hash数据  
        /// redisKey key 相同时    会覆盖value  即修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="redisKey"></param>
        public static void Add2(string redisKey, string key, string value)
        {
            if (value == null)
            {
                return;
            }
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.SetEntryInHash(redisKey, key, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
            }

        }

        /// <summary>
        /// redis 存字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry">大于当前时间  则会执行  否则删除</param>
        public static void Add<T>(string key, T value, DateTime expiry)
        {
            if (value == null)
            {
                return;
            }

            if (expiry <= DateTime.Now)
            {

                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, expiry - DateTime.Now);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
            }

        }


        public static void Add2<T>(string key, T value, TimeSpan slidingExpiration)
        {
            if (value == null)
            {
                return;
            }

            if (slidingExpiration.TotalSeconds <= 0)
            {
                Remove(key);

                return;
            }

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Set(key, value, slidingExpiration);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "存储", key);
            }
        }

        public static T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            T obj = default(T);

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            obj = r.Get<T>(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
            }


            return obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get2(string rediskey, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }

            string obj = "";

            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            obj = r.GetValueFromHash(rediskey, key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", key);
            }


            return obj;
        }

        /// <summary>
        /// 得到所有的数据
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static List<string> GetAll()
        {
            List<string> list = new List<string>();
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            list = r.GetAllKeys();
                            list = r.GetAll<string>(list);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", "");
            }


            return list;
        }
        //public static List<string> GetAll(List<string> keys)
        //{
        //    List<string> list = new List<string>();
        //    try
        //    {
        //        if (pool != null)
        //        {
        //            using (var r = pool.GetClient())
        //            {
        //                if (r != null)
        //                {
        //                    r.SendTimeout = 1000;
        //                    list = r.GetAll<List<string>>(keys);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "获取", keys);
        //    }


        //    return list;
        //}
        #endregion
        /// <summary>
        /// 删除的字符串
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.Remove(key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "删除", key);
            }

        }
        /// <summary>
        /// 删除哈希数值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="key"></param>
        public static void Remove2(string redisKey, string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            r.RemoveEntryFromHash(redisKey, key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "删除", key);
            }

        }

        public static bool Exists(string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.ContainsKey(key);
                            /////哈希查找
                            ///r.HashContainsEntry(redisKey,key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
            }

            return false;
        }


        public static bool Exists2(string redisKey, string key)
        {
            try
            {
                if (pool != null)
                {
                    using (var r = pool.GetClient())
                    {
                        if (r != null)
                        {
                            r.SendTimeout = 1000;
                            return r.HashContainsEntry(redisKey, key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0}:{1}发生异常!{2}", "cache", "是否存在", key);
            }

            return false;
        }

    }
}


