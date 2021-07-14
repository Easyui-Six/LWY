using System.ComponentModel;

namespace LWY.Common
{


    /// <summary>
    /// 终端来源
    /// </summary>
    [Description("终端来源")]
    public enum TerminalSource
    {
        /// <summary>
        /// 安卓商家端
        /// </summary>
        [Description("安卓端")]
        Android = 1,

        /// <summary>
        /// IOS端
        /// </summary>
        [Description("IOS端")]
        IOS = 2,
        /// <summary>
        /// pc车主端
        /// </summary>
        [Description("pc端")]
        Pc = 3,


        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeChat = 4,
        /// <summary>
        /// 微信小程序
        /// </summary>
        [Description("微信小程序")]
        WeChatSmallProgram = 5,

        [Description("后台excel导入")]
        ImportFormExcel = 6,

        [Description("测试客户端")]
        Test_Client = 7,

        /// <summary>
        /// 后台管理
        /// </summary>
        [Description("后台管理")]
        BackendManager = 8,


    }


    /// <summary>
    /// Api身份验证终端编号
    /// </summary>
    [Description("Api身份验证终端编号")]
    public enum ApiClient
    {
        [Description("安卓")]
        Android = 1, //安卓
        [Description("IOS")]
        IOS = 2, //IOS
        [Description("Web")]
        Web = 3 //Web
    }


    /// <summary>
    /// 升序or降序
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// 升序
        /// </summary>
        [Description("升序")]
        Asc = 0,
        /// <summary>
        /// 降序
        /// </summary>
        [Description("降序")]
        Desc = 1
    }
}
