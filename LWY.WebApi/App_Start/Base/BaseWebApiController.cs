
using System.Web.Http;

namespace LWY.WebApi
{
    public class BaseWebApiController : ApiController
    {
        /// <summary>
        /// 获取基地址, 如: 源地址为http://localhost:8080/product/index, 
        ///                 基地址为http://localhost:8080
        /// </summary>
        /// <returns></returns>
        protected string GetUrlBase()
        {
            return this.Request.RequestUri.AbsoluteUri.Replace(this.Request.RequestUri.AbsolutePath, "");
        }

        /// <summary>
        /// 一定不要删除，跨域必须
        /// </summary>
        /// <returns></returns>
        public string Options()
        {
            //CrossDomainHelper.SupportApiCross();
            return null;
        }
    }
}