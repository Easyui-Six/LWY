using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace LWY.FW.Common
{
    public class HttpHelper
    {
        // 懒 :多线程，垃圾内存
        public static HttpHelper httpHelper { get; set; }

        public HttpHelper()
        {
            
        }
        public static HttpHelper SetHttpHelper()
        {
            if (httpHelper == null)
            {
                httpHelper = new HttpHelper();
            }
            return httpHelper;
        }

        /// <summary>
        /// post请求 json入参的格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ApiName"></param>
        /// <param name="functionName"></param>
        /// <param name="obj"></param>
        /// <param name="serverAddr"></param>
        /// <returns></returns>
        public T PostDataWtihModel<T>(string url, object obj, string serverAddr)
        {
            try
            {
                string result = "";

                var settings = new JsonSerializerSettings() { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat };
                var content = JsonConvert.SerializeObject(obj, settings);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";

                #region 添加Post 参数
                byte[] data = Encoding.UTF8.GetBytes(content);
                request.ContentLength = data.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Close();
                }
                #endregion

                using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                {
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception("error");
                    }
                    else
                    {
                        Stream stream = resp.GetResponseStream();
                        //获取响应内容
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                        result = reader.ReadToEnd();
                    }

                }

                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// post  xml入参的格式
        /// errorList:
        /// 1.如果提示操作超时 添加代码 request.ServicePoint.Expect100Continue = false;request.ProtocolVersion = HttpVersion.Version11;
        /// </summary>
        /// <param name="targetURL"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string PostAndGetHTML(string targetURL, string param)
        {
            //formData用于保存提交的信息
            string formData = HttpUtility.UrlEncode(param);

            formData = "strXml=" + formData;
            //把提交的信息转码（post提交必须转码）
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(formData);



            //开始创建请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetURL);
            request.Method = "POST";    //提交方式：post
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.ServicePoint.Expect100Continue = false;
            request.ProtocolVersion = HttpVersion.Version11;


            Stream newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);//将请求的信息写入request
            newStream.Close();
            try

            {
                //向服务器发送请求
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.GetEncoding("utf-8"));
                string strResult = sr.ReadToEnd();
                return strResult;

            }
            catch (Exception ex)
            {
                return "";
            }

        }

    }
}
