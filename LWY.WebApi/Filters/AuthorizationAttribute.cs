using System.Net.Http;
using System.Web.Http.Filters;
using System.Threading.Tasks;
using System.Net;

namespace LWY.WebApi
{
    /// <summary>
    /// 权限身份验证
    /// </summary>
    public class AuthorizationAttribute : IAuthenticationFilter
    {
        public bool AllowMultiple => true;

        public Task AuthenticateAsync(HttpAuthenticationContext context, System.Threading.CancellationToken token)
        {
            string tokenStr = context.Request.Headers.GetValues("Token").ToString();
            if (string.IsNullOrWhiteSpace(tokenStr))
            {

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    code = -1,
                    message = "权限认证测试测试"
                });
                HttpResponseMessage result = new HttpResponseMessage();
                HttpContent httpContent = new StringContent(json);
                result.Content = httpContent;
                context.ActionContext.Response = result;
            }
            else
            {
                //Yes
            }
            return Task.Run(() =>
            {

            });
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, System.Threading.CancellationToken token)
        {
            string tokenStr = context.Request.Headers.GetValues("Token").ToString();
            if (string.IsNullOrWhiteSpace(tokenStr))
            {

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    code = -1,
                    message = "权限认证测试测试"
                });
                HttpResponseMessage result = new HttpResponseMessage();
                HttpContent httpContent = new StringContent(json);
                result.Content = httpContent;
                context.ActionContext.Response = result;
            }
            else
            {
                //Yes
            }
            return Task.Run(() =>
            {

            });
        }

        //public bool AllowMultiple()
        //{
        //    return true;
        //}

    }
}