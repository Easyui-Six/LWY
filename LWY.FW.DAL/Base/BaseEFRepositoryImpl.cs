using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using LWY.FW.Models;

namespace LWY.FW.DAL
{
    /// <summary>
    /// 仓储基础
    /// </summary>
    /// <typeparam name="TEntity">表实体类</typeparam>
    public class BaseEFRepositoryImpl<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        #region 属性注入DbContext
        /// <summary>
        /// db上下文
        /// </summary>
        public TemplateDBContext DbContext { protected get; set; }
        #endregion

        #region IRepository<T> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().AsNoTracking().Where(predicate);
        }

        /// <summary>
        /// 根据条件查询单个实体(更新用)
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>查询到的结果</returns>
        public TEntity GetForUpdate(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// 查询所有符合条件的实体
        /// </summary>
        /// <returns>所有符合条件的实体</returns>
        public virtual IQueryable<TEntity> Query()
        {
            return DbContext.Set<TEntity>().AsNoTracking().AsQueryable();
        }


        /// <summary>
        ///  根据条件  查找单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().AsNoTracking().Where(predicate).FirstOrDefault();
        }
        /// <summary>
        /// 创建空实体
        /// </summary>
        /// <returns>创建实体</returns>
        public virtual TEntity Create()
        {
            return DbContext.Set<TEntity>().Create();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">待新增实体</param>
        /// <returns>新增后实体</returns>
        public virtual TEntity AddNotCommite(TEntity entity)
        {
            var result = DbContext.Set<TEntity>().Add(entity);
            return result;
        }

        /// <summary>
        /// 插入(立即提交到数据库)
        /// </summary>
        /// <param name="entity">待插入实体</param>
        /// <returns>插入后实体</returns>
        public virtual TEntity Add(TEntity entity)
        {
            //TemplateDBContext templateDBContext = new TemplateDBContext();
            //var result=templateDBContext.Set<TEntity>().Add(entity);
            var result = DbContext.Set<TEntity>().Add(entity);
            DbContext.SaveChanges();
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        public virtual void DeleteNotCommite(int id)
        {
            var dbEneity = DbContext.Set<TEntity>().Find(id);
            DbContext.Set<TEntity>().Remove(dbEneity);
        }

        /// <summary>
        /// 删除(立即提交到数据库)
        /// </summary>
        /// <param name="id">id</param>
        public virtual void Delete(int id)
        {
            var dbEneity = DbContext.Set<TEntity>().Find(id);
            DbContext.Set<TEntity>().Remove(dbEneity);
            DbContext.SaveChanges();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(long id)
        {
            var dbEneity = DbContext.Set<TEntity>().Find(id);
            DbContext.Set<TEntity>().Remove(dbEneity);
            DbContext.SaveChanges();
        }
        /// <summary>
        /// 删除(立即提交到数据库)
        /// </summary>
        /// <param name="predicate"></param>
        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var dbEneity = DbContext.Set<TEntity>().Where(predicate);
            DbContext.Set<TEntity>().RemoveRange(dbEneity);
            DbContext.SaveChanges();
        }

        /// <summary>
        /// 删除(立即提交到数据库)
        /// </summary>
        /// <param name="predicate"></param>
        public void DeleteNotCommite(Expression<Func<TEntity, bool>> predicate)
        {
            var dbEneity = DbContext.Set<TEntity>().Where(predicate);
            DbContext.Set<TEntity>().RemoveRange(dbEneity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">待删除实体</param>
        public virtual void DeleteNotCommite(TEntity entity)
        {
            //DbContxtHelper.DetachEntityFromContext(DbContext, entity);
            DbContext.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// 删除(立即提交到数据库)
        /// </summary>
        /// <param name="entity">待删除实体</param>
        public virtual void Delete(TEntity entity)
        {
            //DbContxtHelper.DetachEntityFromContext(DbContext, entity);
            DbContext.Set<TEntity>().Remove(entity);
            DbContext.SaveChanges();
        }

        /// <summary>
        /// 删除多个(立即提交到数据库)
        /// </summary>
        /// <param name="entities">待删除的实体集合</param>
        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            //DbContxtHelper.DetachEntitiesFromContext(DbContext, entities);
            DbContext.Set<TEntity>().RemoveRange(entities);
            DbContext.SaveChanges();
        }

        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="entities">待删除的实体集合</param>
        public virtual void DeleteRangeNotCommite(IEnumerable<TEntity> entities)
        {
            //DbContxtHelper.DetachEntitiesFromContext(DbContext, entities);
            DbContext.Set<TEntity>().RemoveRange(entities);
        }

        /// <summary>
        /// 更新
        /// 用法1: 通过get获取，修改后提交到该方法保存
        /// 用法2: new 一个带id的对象， 对所有的字段重新赋值
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新实体</returns>
        [Obsolete]
        public virtual TEntity UpdateNotCommite(TEntity entity)
        {
            //DbContxtHelper.DetachEntityFromContext(DbContext, entity);
            //DbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            return entity;
        }

        /// <summary>
        /// 更新
        /// 用法1: 通过get获取，修改后提交到该方法保存
        /// 用法2: new 一个带id的对象， 对所有的字段重新赋值
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新实体</returns>
        [Obsolete]
        public virtual TEntity Update(TEntity entity)
        {
            //DbContxtHelper.DetachEntityFromContext(DbContext, entity);
            //DbContext.Entry<TEntity>(entity).State = EntityState.Modified;
            DbContext.SaveChanges();
            return entity;
        }

        ///// <summary>
        ///// 部分更新 
        ///// </summary>
        ///// <param name="predicate">更新条件</param>
        /////  <example>
        /////     写法举例:
        /////     
        /////     // 错误的写法
        /////     var newData = new DataTypistDbModel() { RealName = "侯瑞德1" };
        /////     update<DataTypistDbModel>(e => e.DataTypistId == 1, e => newData);
        /////
        /////     // 正确的写法1
        /////     update<DataTypistDbModel>(e => e.DataTypistId == 1, e => new DataTypistDbModel() { RealName = "侯瑞德" });
        /////
        /////     // 正确的写法2
        /////     Expression<Func<DataTypistDbModel, bool>> predicateExpr = e => e.DataTypistId == 1;
        /////     Expression<Func<DataTypistDbModel, DataTypistDbModel>> updateExpr = e => new DataTypistDbModel() { RealName = "侯瑞德1" };
        /////     update<DataTypistDbModel>(predicateExpr, updateExpr);
        ///// </example>
        ///// <param name="updateExpression">
        /////     更新内容部分， 不能包含逐渐字段
        ///// </param>
        //public void Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        //{
        //    DbContext.Set<TEntity>().Where(predicate).AsQueryable<TEntity>().Update(updateExpression);
        //}

        /// <summary>
        /// 保存(批量提交用)
        /// </summary>
        public virtual void Save()
        {
            DbContext.SaveChanges();
        }
        /// <summary>
        /// 执行原生数据库脚本
        /// 存储过程
        /// </summary>
        /// <param name="commandText"></param>
        public ObjectResult<TEntity> ExecuteStoreQuery<TEntity>(string commandText)
        {
            var obj = ((IObjectContextAdapter)DbContext).ObjectContext.ExecuteStoreQuery<TEntity>(commandText);
            return obj;
        }
        /// <summary>
        /// 异步执行原生数据库脚本
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteStoreCommandAsync(string commandText)
        {
            ((IObjectContextAdapter)DbContext).ObjectContext.ExecuteStoreCommandAsync(commandText);
        }

        /// <summary>
        /// 异步执行原生数据库脚本
        /// </summary>
        /// <param name="commandText"></param>
        public int ExecuteStoreCommand(string commandText, params object[] args)
        {
            return ((IObjectContextAdapter)DbContext).ObjectContext.ExecuteStoreCommand(commandText, args);
        }
        #endregion
    }
}
