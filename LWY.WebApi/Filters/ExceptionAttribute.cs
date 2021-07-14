using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace LWY.WebApi
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class ExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            string message = actionExecutedContext.Exception.Message;

            var ex = actionExecutedContext.Exception.InnerException ?? actionExecutedContext.Exception;
            return Task.Run(() =>
            {
                //异常记录日志到本地 
                //Common.LogHelpter.AddLog(ex.ToString());

                //异常后，给httpResponse 设置值
                string msg = ex.Message;
                // Result result = new Result(200, msg);
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    code = -1,
                    message = "异常"
                });

                HttpResponseMessage httpResponse = new HttpResponseMessage();
                HttpContent httpContent = new StringContent(json);
                httpResponse.Content = httpContent;
                actionExecutedContext.Response = httpResponse;
            });
        }
    }
}