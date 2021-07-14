using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWY.FW.Common
{
    /// <summary>
    /// 通用请求信息
    /// </summary>
    public class CommonRequest
    {
        /// <summary>
        /// 客户端的基本（通用）信息
        /// </summary>
        public ServiceContext ClientCommonInfo { get; set; }
    }
    /// <summary>
    /// 请求基类
    /// </summary>
    /// <typeparam name="T">请求参数类型</typeparam>
    public class BaseWebApiRequest<T> : CommonRequest
    {
        public T Param { get; set; }
        /// <summary>
        /// 客户端增量同步标识，对于支持客户端缓存的接口，
        /// 用于标识客户端本地上次成功同步的版本Id,请求上带上
        /// </summary>
        public DateTime? SyncId { get; set; }
    }

    /// <summary>
    /// 分步操作，实时保存的请求模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseStepSaveWebApiRequest<T> : BaseWebApiRequest<T>
    {
        /// <summary>
        /// 是否是新增操作，用于标识客户端是在新增的界面
        /// </summary>
        public bool IsAddNew { get; set; }
        /// <summary>
        /// 是否完成新增操作，标识用户在客户端已确认提交新增的用户信息
        /// </summary>
        public bool IsFinalStep { get; set; }
    }

    /// <summary>
    /// 带分页的请求基类
    /// </summary>
    /// <typeparam name="T">请求参数类型</typeparam>
    public class BasePageableWebApiRequest<T> : BaseWebApiRequest<T>
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageModel Page { get; set; }
        ///// <summary>
        ///// 过滤\排序
        ///// </summary>
        //public FilterSortMap FilterSortMap { get; set; }
    }

    //public class Sort
    //{
    //    /// <summary>
    //    /// 排序字段名
    //    /// </summary>
    //    public string ColumnName { get; set; }
    //    /// <summary>
    //    /// 排序类型，参见：EnumSortType 
    //    /// </summary>
    //    public string SortType { get; set; }
    //}
}
