using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Web.Http.Versioning;

namespace LWY.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //自定义路由规则
            config.Routes.MapHttpRoute(
              name: "CustomApi",
              routeTemplate: "api/{controller}/{action}/{id}",
              defaults: new { id = RouteParameter.Optional }
          );
            // 版本控制
            config.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
            //   config.Routes.MapHttpRoute(
            //    name: "MyUrl", // 路由名称
            //    routeTemplate: "{controller}/{id}-{action}", // 带有参数的 URL
            //    defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional }, // 参数默认值
            //    constraints: new string[] { "MvcDemo.Controllers" }//命名空间
            //);

            //Url：http://localhost:0000/Custom/1-Detials

            //全局注册过滤器
            config.Filters.Add(new AuthorizationAttribute());//权限
            config.Filters.Add(new ExceptionAttribute());//异常
            config.Filters.Add(new TimingFilterAttribute());//耗时统计

            //解决跨域
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
        }
    }
}
