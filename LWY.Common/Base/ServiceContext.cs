
using System.Linq;
using System.Net.Http;

namespace LWY.Common
{
    /// <summary>
    /// 保存放在http header中的可能会在Service层用到的业务逻辑信息，
    /// 该信息放入 BaseWebApiRequest中传入Service层供业务逻辑使用。
    /// </summary>
    public class ServiceContext
    {

        private const string clientInfoKey = "";
            //= "exiuHttpItemClientInfo";

        /// <summary>
        /// 当前登陆用户的ID，如果客户端用户未登陆，则该属性为空或0
        /// </summary>
        public int CurrentUserId { get; set; }
        /// <summary>
        /// 用户授权的Token, 该Token与用户相关，如果客户端用户未登陆，则该token为空
        /// </summary>
        public string UserToken { get; set; }
        /// <summary>
        /// App授权的Token,
        /// </summary>
        public string AppToken { get; set; }
        /// <summary>
        /// 终端的来源，如安卓端，IOS端 PC端等, 类似AppId
        /// </summary>
        public TerminalSource TerminalSource { get; set; }
        /// <summary>
        /// 终端的来源的版本号
        /// </summary>
        public string TerminalSourceVersion { get; set; }
        /// <summary>
        /// 终端描述
        /// 安卓:[SDK],[Brand],[Mode]  例如: SDK 5.0,华为,荣耀
        /// IOS :[IOS Version],[Model] 例如: IOS 7, IPhone5
        /// </summary>
        public string TerminalSourceDesc { get; set; }
        /// <summary>
        /// 移动设备的唯一编号
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// 区域码
        /// </summary>
        public string AreaCode { get; set; }

        //internal void WriteToContext(HttpRequestMessage request = null)
        //{
        //    if (request != null && !request.Properties.ContainsKey(clientInfoKey))
        //        request.Properties.Add(clientInfoKey, this);
        //    if (HttpContext.Current != null && HttpContext.Current.Items != null && !HttpContext.Current.Items.Contains(clientInfoKey))
        //        HttpContext.Current.Items.Add(clientInfoKey, this);
        //}
        //internal static ServiceContext GetFromContext(HttpRequestMessage request = null)
        //{
        //    object result = null;
        //    if (request != null && request.Properties.ContainsKey(clientInfoKey))
        //        request.Properties.TryGetValue(clientInfoKey, out result);
        //    if (request == null &&
        //        HttpContext.Current != null && HttpContext.Current.Items != null && HttpContext.Current.Items.Contains(clientInfoKey))
        //        result = HttpContext.Current.Items[clientInfoKey];
        //    return result as ServiceContext;
        //}

        /// <summary>
        /// 维度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }


        /// <summary>
        /// 统计类型,如果参与统计请设定该字段
        /// 对应的枚举参照EnumStatisticsType 
        /// </summary>
        public string StatisticsType { get; set; }
        public string Board { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Product { get; set; }
        public string Sdk { get; set; }
        internal bool TraceEnabled { get; set; }
    }
}
