using System.Web.Http;
using System.Web.Http.Description;
using LWY.FW.Common;
using LWY.FW.BLL;
using LWY.FW.Models.Entity;
using Microsoft.Web.Http;
using Autofac.Features.Indexed;

namespace LWY.WebApi.Controllers.V2
{
    [ApiVersion("2.0")]
    public class ExamplesController : BaseWebApiController
    {
        #region  prop DI 构造

        public ISysManagerService SysManagerService { get; set; }

        /// <summary>
        /// 内置索引注入
        /// </summary>
        public IIndex<string, IAnimalService> AnimalService { get; set; }
        #endregion  

        [ResponseType(typeof(BaseWebApiResponse<int>))]
        public IHttpActionResult DemoPostA([FromBody] BaseWebApiRequest<string> request)
        {
            //先不处理ORM
            //User user = new User()
            //{
            //    Code = "GC-2021",
            //    PassWord = "PawwWord",
            //    Name = "Name",
            //    Sex = true,
            //    Age = 1
            //};
            ////Family family = new Family() { Name="UI"};
            ////SysManagerService.AddFamily(family);
            //return Ok(SysManagerService.AddUser(user));

            return Ok(new BaseWebApiResponse<object>());
        }


        [ResponseType(typeof(BaseWebApiResponse<object>))]
        public IHttpActionResult DemoPostB([FromBody] BaseWebApiRequest<string> request)
        {
            return Ok(new BaseWebApiResponse<object>());
        }


        [ResponseType(typeof(BaseWebApiResponse<string>))]
        public IHttpActionResult DemoPostC([FromBody] BaseWebApiRequest<int> request)
        {
            string result = AnimalService[AnimalEnum.People + ""].SelfIntroduction();
            return Ok(new BaseWebApiResponse<string>(result+"2.0"));
        }

        //[ResponseType(typeof(BaseWebApiResponse<object>))]
        //public IHttpActionResult DemoPostC([FromBody] string name)
        //{
        //    return Ok(new BaseWebApiResponse<object>());
        //}

    }
}
