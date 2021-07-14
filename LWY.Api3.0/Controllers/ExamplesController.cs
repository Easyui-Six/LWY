
using Microsoft.AspNetCore.Mvc;
using LWY.Common;
using LWY.Service;
using AspNetCore.CacheOutput;

namespace LWY.Api3._0.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamplesController : ControllerBase
    {
        #region 构件 DI  ProP

        public IExampleService ExampleService { get; set; }


        /// <summary>
        /// 内置ioc的构造函数注入方式
        /// </summary>
        /// <param name="_exampleService"></param>
        public ExamplesController(IExampleService _exampleService)
        {
            ExampleService = _exampleService;
        }

        #endregion 

        /// <summary>
        /// 测试接口A
        /// </summary>
        /// <param name="request">入参</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ApiExplorerSettings(GroupName = "v1")]
        //[CacheOutput(ClientTimeSpan=20)]
        [ResponseCache(Duration = 600)]
        //[ResponseCache(CacheProfileName = "default")] 从配置文件取 缓存配置
        [ProducesDefaultResponseType(typeof(BaseWebApiResponse<string>))]
        public IActionResult DemoPostA([FromBody] BaseWebApiRequest<string> request)
        {
            //Response.HttpContext.Request.Headers.Add("Cache-Control", "public, max-age=120");
            //反射demo
            string s = ReflectHelper.GetReflextHelper().GetResultByDllName<string>("LWY.Service", "ExampleServiceImpl", "GetStr", null);
            return Ok(new BaseWebApiResponse<string>(ExampleService.GetStr()));
        }

        /// <summary>
        /// 测试接口B
        /// </summary>
        /// <param name="request">入参</param>
        /// <returns></returns>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "v2")]
        [ProducesDefaultResponseType(typeof(BaseWebApiResponse<object>))]
        public IActionResult DemoPostB([FromBody] BaseWebApiRequest<string> request)
        {
            return Ok(new BaseWebApiResponse<object>());
        }
    }
}
