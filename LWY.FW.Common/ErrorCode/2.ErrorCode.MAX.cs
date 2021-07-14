

using System.ComponentModel;

namespace LWY.FW.Common
{
    /// <summary>
    /// 严重出错Code，此时需要人工干预或其他重点处理
    /// </summary>
    public partial class ErrorCode
    {
        /// <summary>
        /// 系统设置 状态不能为空
        /// </summary>
        [Description("出错的最大Code")]
        public const int MaxCode = 1000000;

    }
}
