using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LWY.Common;
using LWY.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LWY.Api3._0.Controllers.V2
{
    [ApiVersion("2.0")]
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
        [ProducesDefaultResponseType(typeof(BaseWebApiResponse<string>))]
        public IActionResult DemoPostA([FromBody] BaseWebApiRequest<string> request)
        {
            return Ok(new BaseWebApiResponse<string>("version-2.0"));
        }
    }
}
