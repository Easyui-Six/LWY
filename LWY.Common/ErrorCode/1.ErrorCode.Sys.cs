using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LWY.Common
{
    /// <summary>
    /// 系统设置模块  从0001000 开始 每个模块预留 玖佰玖拾玖个
    /// 以后其他模块  从0002000 开始 i+1;
    /// </summary>
    public partial class ErrorCode
    {
        /// <summary>
        /// 系统设置 状态不能为空
        /// </summary>
        [Description("状态不能为空")]
        public const int SYSSTATUSNUMM = 0001000;

    }
}
