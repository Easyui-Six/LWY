using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace LWY.WebApi
{
    /// <summary>
    /// 记录接口耗时 
    /// </summary>
    public class TimingFilterAttribute : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// 执行方法之前 插入时间戳，计算接口耗时用的
        /// </summary>
        public const string _InitTime = "_InitTime_start";
        /// <summary>
        /// 功能：执行方法之后
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        {
            string time = filterContext.Request.Headers.GetValues(_InitTime).FirstOrDefault();
        }


        /// <summary>
        /// 功能：执行方法之后  执行任务
        /// Memo：OnActionExecutedAsync的优先级是高于OnActionExecuted
        ///        1.计算接口耗时  2.记录操作日志
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnActionExecutedAsync(HttpActionExecutedContext filterContext, CancellationToken cancellationToken)
        {
            //方法调用之前在表头添加时间戳 记录接口耗时

            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string timeStart = Convert.ToInt64(ts.TotalSeconds).ToString();

            string time = filterContext.Request.Headers.GetValues(_InitTime).FirstOrDefault();

            return Task.Run(() =>
            {
                //方法：1.可以执行 方法耗时统计  2. 记录方法的 操作日志根据根据Respon 判断是否执行成功
                //int i = int.Parse("bobo");
            });
        }



        /// <summary>
        /// 功能：执行方法之前
        /// Memo:1.插入 接口计时的开始时间戳，2.对参数进行验证。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //方法调用之前在表头添加时间戳 记录接口耗时

            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string timeStart = Convert.ToInt64(ts.TotalSeconds).ToString();

            filterContext.Request.Headers.Add(_InitTime, timeStart);
        }





    }
}