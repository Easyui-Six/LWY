using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LWY.Common
{
    /// <summary>
    /// 基础码  从0000000  到0000999
    /// </summary>
    public partial class ErrorCode
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        [Description("成功")]
        public const int SUCCESS = 0000000;
        /// <summary>
        /// 操作失败
        /// </summary>
        [Description("失败")]
        public const int FALSE = 0000001;
        /// <summary>
        /// 操作异常
        /// </summary>
        [Description("错误")]
        public const int ERROR = 0000002;


        /// <summary>
        /// 参数不能为空
        /// </summary>
        [Description("参数不能为空")]
        public const int PARAMISNUMM = 0000003;

    }

}
