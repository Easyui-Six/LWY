using System;
using System.Collections.Generic;
using System.Linq;
using LWY.FW.Models;
using LWY.FW.Common;
using System.Data.Entity;
using System.Linq.Expressions;

namespace LWY.FW.DAL
{

    /// <summary>
    /// 只读数据仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseEFReadOnlyRepositoryImpl<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class, new()
    {
        #region 属性注入只读DbContext

        /// <summary>
        /// 只读上下文
        /// </summary>
        public ReadOnlyEntities DbContext { private get; set; }
        /// <summary>
        /// 模板上下文,可用来更新的
        /// </summary>
        public TemplateDBContext DbContextForTrans { private get; set; }

        public DbContext getDbContext()
        {
            return DbContext;
        }
        public DbContext getDbContextForUpdate()
        {
            return DbContextForTrans;
        }

        #endregion

        #region 实现接口
        ///// <summary>
        ///// 查找单个实体
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>查询到的结果</returns>
        //public virtual TEntity Get(int id)
        //{
        //    try
        //    {
        //        var entity = getDbContext().Set<TEntity>().Find(id);
        //        if (entity != null)
        //            DbContxtHelper.DetachEntityFromContext(getDbContext(), entity);
        //        return entity;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        ///// <summary>
        ///// 查找单个实体
        ///// </summary>
        ///// <param name="id">id</param>
        ///// <returns>查询到的结果</returns>
        //public virtual TEntity Get(string id)
        //{
        //    try
        //    {
        //        var entity = getDbContext().Set<TEntity>().Find(id);
        //        if (entity != null)
        //            DbContxtHelper.DetachEntityFromContext(getDbContext(), entity);
        //        return entity;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        /// 根据条件查询单个实体
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>查询到的结果</returns>
        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return getDbContext().Set<TEntity>().AsNoTracking().Where(predicate).FirstOrDefault();
        }
        public virtual TEntity GetLag(Expression<Func<TEntity, bool>> predicate)
        {
            var result = getDbContext().Set<TEntity>().AsNoTracking().Where(predicate).FirstOrDefault();
            if (result == null)
                result = getDbContextForUpdate().Set<TEntity>().AsNoTracking().Where(predicate).FirstOrDefault();
            return result;
        }


        /// <summary>
        /// 根据条件查询单个实体(更新用)
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>查询到的结果</returns>
        public TEntity GetForUpdate(Expression<Func<TEntity, bool>> predicate)
        {
            return getDbContextForUpdate().Set<TEntity>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// 查询记录条数
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>查询到的条数</returns>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate, bool tryMainStoreIfZero = false)
        {
            var result = getDbContext().Set<TEntity>().AsNoTracking().Count(predicate);
            if (result == 0)
            {
                result = getDbContextForUpdate().Set<TEntity>().AsNoTracking().Count(predicate);
            }
            return result;
        }

        /// <summary>
        /// 查询所有符合条件的实体
        /// </summary>
        /// <returns>所有符合条件的实体</returns>
        public virtual IQueryable<TEntity> Query()
        {
            return getDbContext().Set<TEntity>().AsNoTracking().AsQueryable();
        }

        /// <summary>
        /// 查询所有实体并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query<TKey>(Dictionary<Func<TEntity, TKey>, SortType> orderBy)
        {
            var query = getDbContext().Set<TEntity>().AsNoTracking().AsQueryable();
            //排序
            query = query.SortBy(orderBy).AsQueryable();
            return query;
        }

        /// <summary>
        ///  查询所有实体并分页
        /// </summary>
        /// <param name="page">分页信息</param>
        /// <param name="orderBy">排序</param>
        /// <returns>查询到的结果</returns>
        public virtual IQueryable<TEntity> Query<TKey>(Dictionary<Func<TEntity, TKey>, SortType> orderBy, PageModel page)
        {
            if (page == null)
                page = Common.PageModel.Default();
            var query = getDbContext().Set<TEntity>().AsNoTracking().AsQueryable();
            page.RecordCount = query.Count();
            //排序
            query = query.SortBy(orderBy).AsQueryable();
            //获取分页数据
            int skip = (page.PageNo - 1) * page.PageSize;
            query = query.Skip(skip).Take(page.PageSize);
            return query;
        }

        /// <summary>
        /// 根据条件查询所有符合条件的实体
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>查询到的结果</returns>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return getDbContext().Set<TEntity>().AsNoTracking().Where(predicate);
        }

        /// <summary>
        /// 根据条件查询所有符合条件的实体(更新用)
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>查询到的结果</returns>
        public IQueryable<TEntity> QueryForUpdate(Expression<Func<TEntity, bool>> predicate)
        {
            return getDbContextForUpdate().Set<TEntity>().Where(predicate);
        }

        /// <summary>
        /// 根据条件查询并排序
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> predicate, Dictionary<Func<TEntity, TKey>, SortType> orderBy)
        {
            var query = getDbContext().Set<TEntity>().AsNoTracking().Where(predicate);
            query = query.SortBy(orderBy).AsQueryable();
            return query;
        }

        public virtual IQueryable<TEntity> Query<TKey>(Expression<Func<TEntity, bool>> predicate, Func<TEntity, TKey> orderBy, SortType sortType, PageModel page)
        {
            var orderByP = new Dictionary<Func<TEntity, TKey>, SortType>() { { orderBy, sortType } };
            return Query<TKey>(predicate, orderByP, page);
        }

        /// <summary>
        /// 根据条件查询并分页
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="page">分页信息</param>
        /// <returns>查询到的结果</returns>
        public virtual IQueryable<TEntity> Query<TValue>(Expression<Func<TEntity, bool>> predicate, Dictionary<Func<TEntity, TValue>, SortType> orderBy, PageModel page)
        {
            if (page == null)
                page = PageModel.Default();
            var query = getDbContext().Set<TEntity>().AsNoTracking().Where(predicate);
            page.RecordCount = query.Count();
            //排序
            query = query.SortBy(orderBy).AsQueryable();
            //获取分页数据
            int skip = (page.PageNo - 1) * page.PageSize;
            query = query.Skip(skip).Take(page.PageSize);
            return query;
        }
        #endregion
    }

    /// <summary>
    /// 不依赖于具体实体的EF仓储类
    /// </summary>
    public class EFCommonRepositoryImpl : ICommonRepository
    {
        #region 属性区域

        /// <summary>
        /// 只读上下文
        /// </summary>
        public ReadOnlyEntities DbContext { private get; set; }
        /// <summary>
        /// 模板上下文,可用来更新的
        /// </summary>
        public TemplateDBContext DbContextForTrans { private get; set; }

        public DbContext getDbContext()
        {
            return DbContextForTrans;
        }
        #endregion

        #region 方法区域

        /// <summary>
        /// 获取服务器当前时间
        /// </summary>
        /// <returns>获取当前时间</returns>
        public DateTime GetServerNow()
        {
            return getDbContext().Database.SqlQuery<DateTime>("select current_timestamp()").First();
        }

        /// <summary>
        /// SQL语句查询
        /// </summary>
        /// <param name="query">查询语句</param>
        /// <param name="parameters">查询参数</param>
        /// <returns>查询到的结果</returns>
        public virtual IQueryable<TEntity> SelectQuery<TEntity>(string query, params object[] parameters)
        {
            return getDbContext().Database.SqlQuery<TEntity>(query, parameters).AsQueryable();
        }


        /// <summary>
        /// SQL执行
        /// </summary>
        /// <param name="sqlstr">执行语句</param>
        /// <param name="parameters">执行参数</param>
        /// <returns>执行行数</returns>
        public virtual int ExecuteSqlCmd(string sqlstr, params object[] parameters)
        {
            return DbContextForTrans.Database.ExecuteSqlCommand(sqlstr, parameters);
        }
        #endregion
    }
}
