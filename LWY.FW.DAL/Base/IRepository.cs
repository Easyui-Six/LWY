using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Core.Objects;

namespace LWY.FW.DAL
{
    /// <summary>
    /// 实体相关的仓储接口
    /// </summary>
    /// <typeparam name="TEntity">操作的实体类型</typeparam>
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// 根据条件查询所有符合条件的实体
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>查询到的结果</returns>
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 根据条件查询单个实体(更新用)
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns>查询到的结果</returns>
        TEntity GetForUpdate(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///  根据条件  查找单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 创建空实体
        /// </summary>
        /// <returns>创建实体</returns>
        TEntity Create();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">待新增实体</param>
        /// <returns>新增后实体</returns>
        TEntity AddNotCommite(TEntity entity);

        /// <summary>
        /// 插入(立即提交到数据库)
        /// </summary>
        /// <param name="entity">待插入实体</param>
        /// <returns>插入后实体</returns>
        TEntity Add(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        void DeleteNotCommite(int id);

        /// <summary>
        /// 删除(立即提交到数据库)
        /// </summary>
        /// <param name="id">id</param>
        void Delete(int id);
        /// <summary>
        /// 删除(立即提交到数据库)
        /// </summary>
        /// <param name="id">id</param>
        void Delete(long id);

        /// <summary>
        /// 删除(立即提交到数据库)
        /// </summary>
        /// <param name="predicate"></param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 删除(立即提交到数据库)
        /// </summary>
        /// <param name="predicate"></param>
        void DeleteNotCommite(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">待删除实体</param>
        void DeleteNotCommite(TEntity entity);

        /// <summary>
        /// 删除(立即提交到数据库)
        /// </summary>
        /// <param name="entity">待删除实体</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除多个(立即提交到数据库)
        /// </summary>
        /// <param name="entities">待删除的实体集合</param>
        void DeleteRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="entities">待删除的实体集合</param>
        void DeleteRangeNotCommite(IEnumerable<TEntity> entities);

        /// <summary>
        /// 更新
        /// 用法1: 通过get获取，修改后提交到该方法保存
        /// 用法2: new 一个带id的对象， 对所有的字段重新赋值
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新实体</returns>
        TEntity UpdateNotCommite(TEntity entity);

        /// <summary>
        /// 更新
        /// 用法1: 通过get获取，修改后提交到该方法保存
        /// 用法2: new 一个带id的对象， 对所有的字段重新赋值
        /// </summary>
        /// <param name="entity">待更新实体</param>
        /// <returns>更新实体</returns>
        TEntity Update(TEntity entity);

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
        //void Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression); 

        /// <summary>
        /// 保存(批量提交用)
        /// </summary>
        void Save();

        /// <summary>
        /// 异步执行原生数据库脚本
        /// </summary>
        /// <param name="commandText"></param>
        void ExecuteStoreCommandAsync(string commandText);

        /// <summary>
        /// 执行原生数据库脚本
        /// </summary>
        /// <param name="commandText"></param>
        int ExecuteStoreCommand(string commandText, params object[] args);

        /// <summary>
        /// 执行原生数据库脚本
        /// </summary>
        /// <param name="commandText"></param>
        ObjectResult<TEntity> ExecuteStoreQuery<TEntity>(string commandText);
    }
}
