using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LWY.Api3._0
{
    /// <summary>
    /// 接口耗时过滤器 使用的mvc的过滤器
    /// </summary>
    public class TimingFilter : ActionFilterAttribute
    {
        public const string timeStart = "_Time_Start";

        /// <summary>
        /// 功能：执行方法之前
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //方法调用之前在表头添加时间戳 记录接口耗时

            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string timeStartStr = Convert.ToInt64(ts.TotalSeconds).ToString();

            context.HttpContext.Request.Headers.Add(timeStart, timeStartStr);
        }

        /// <summary>
        /// 功能：执行方法之后
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            string time = context.HttpContext.Request.Headers[timeStart];

        }


        /// <summary>
        /// 功能 返回Result结果之后
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            //context.Result
            string s = context.HttpContext.Request.Method;
        }
        /// <summary>
        /// 功能：返回Result结果之前
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            string s = context.HttpContext.Request.Method;


        }
    }


    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            return Task.Run(() =>
            {
                string message = context.Exception.Message;

                var ex = context.Exception.InnerException ?? context.Exception;
                string msg = ex.Message;
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    code = -1,
                    message = "异常测试"
                });
                ContentResult result = new ContentResult
                {
                    StatusCode = 999,
                    ContentType = "application/json; charset=utf-8"
                };

                result.Content = json;
                context.Result = result;
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            });
        }
    }


    /// <summary>
    /// 功能：身份验证
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Token"];
            //if (string.IsNullOrWhiteSpace(token))
            //{

            //    string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            //    {
            //        code = -1,
            //        message = "权限认证测试测试"
            //    });
            //    ContentResult result = new ContentResult
            //    {
            //        StatusCode = 999,
            //        ContentType = "application/json; charset=utf-8"
            //    };

            //    result.Content = json;
            //    context.Result = result;
            //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //}
            //else
            //{
            //    //Yes
            //}
        }
    }

}
