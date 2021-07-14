

namespace LWY.FW.Common
{
    /// <summary>
    ///  不带分页  响应结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class BaseWebApiResponse<TResult>
    {
        /// <summary>
        /// ErrorCode.MAX为严重错误，需要客户端跳出自动模式，进行人工干预
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 是否成功的状态
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
        public TResult Result { get; set; }

        public BaseWebApiResponse()
        {
            this.Code = ErrorCode.SUCCESS;
            this.Success = true;
        }

        public BaseWebApiResponse(TResult result)
        {
            this.Code = ErrorCode.SUCCESS;
            this.Success = true;
            this.Result = result;
        }
    }

    /// <summary>
    /// 分页  响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasePageableWebApiResponse<T> : BaseWebApiResponse<T>
    {
        /// <summary>
        /// 分页信息
        /// </summary>
        public PageModel Page { get; set; }

        ///// <summary>
        ///// 过滤\排序
        ///// </summary>
        //public QueryKeysSet KeysSet { get; set; }
    }

    public class PageModel
    {
        /// <summary>
        /// 从1开始的页码
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// 页码容量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        public static PageModel Default()
        {
            return new PageModel()
            {
                PageNo = 1,
                PageSize = 10
            };
        }
        /*
 
        private int _recordCount = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount
        {
            get { return _recordCount; }
            set
            {
                this._recordCount = value;
                this.PageSize = this.PageSize <= 0 ? 10 : this.PageSize;
                this.PageCount = this.RecordCount % this.PageSize == 0 ? this.RecordCount / this.PageSize : (this.RecordCount / this.PageSize + 1);
                this.PageNo = this.PageNo < 1 ? 1 : this.PageNo;
            }
        }

        /// <summary>
        /// Skip
        /// </summary>
        [JsonIgnore]
        public int Skip
        {
            get
            {
                return (this.PageNo - 1) * this.PageSize;
            }
        }
        [JsonIgnore]
        public bool CurrentPageIsEmpty
        {
            get { return RecordCount <= Skip; }
        }

        [JsonIgnore]
        public bool CurrentPageIsFull
        {
            get { return (RecordCount - Skip) >= PageSize; }
        }
        #region 扩展属性
        /// <summary>
        /// 扩展属性
        /// </summary>
        public string PageExtend { get; set; }
        #endregion
        */
    }

}
