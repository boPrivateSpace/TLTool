using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FengXuTLTool
{
    class HttpPost
    {
        public class Types
        {
            public static string JSON { get { return "json"; } }

            public static string X_WWW_FORM_URLENCODED { get { return "x-www-form-urlencoded"; } }
        }

        public static string Request(string url, string data, string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = Types.X_WWW_FORM_URLENCODED;
            }

            UTF8Encoding encoding = new UTF8Encoding();

            byte[] bytesToPost = encoding.GetBytes(data); //转换为bytes数据

            string responseResult = String.Empty;
            HttpWebRequest req = null;
            HttpWebResponse cnblogsRespone = null;
            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/" + type + ";charset=utf-8";
                req.ContentLength = bytesToPost.Length;

                // 解决通过网关请求微服务接口无法获取返回数据的问题
                req.ServicePoint.Expect100Continue = false;
                req.ProtocolVersion = HttpVersion.Version11;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bytesToPost, 0, bytesToPost.Length);
                }
                cnblogsRespone = (HttpWebResponse)req.GetResponse();
                if (cnblogsRespone != null && cnblogsRespone.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader sr;
                    using (sr = new StreamReader(cnblogsRespone.GetResponseStream()))
                    {
                        responseResult = sr.ReadToEnd();
                    }
                    sr.Close();
                }
                return responseResult;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cnblogsRespone != null)
                {
                    cnblogsRespone.Close();
                }

            }
        }





        public static string RequestWithToken(string url, string data, string type, String token)
        {

            if (string.IsNullOrEmpty(type))
            {
                type = Types.X_WWW_FORM_URLENCODED;
            }
            UTF8Encoding encoding = new UTF8Encoding();

            byte[] bytesToPost = encoding.GetBytes(data); //转换为bytes数据

            string responseResult = String.Empty;
            HttpWebRequest req = null;
            HttpWebResponse cnblogsRespone = null;
            try
            {
                req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/" + type;
                req.ContentLength = bytesToPost.Length;
                req.Headers.Add("Authorization", token);

                // 解决通过网关请求微服务接口无法获取返回数据的问题
                req.ServicePoint.Expect100Continue = false;
                req.ProtocolVersion = HttpVersion.Version11;

                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(bytesToPost, 0, bytesToPost.Length);
                }
                cnblogsRespone = (HttpWebResponse)req.GetResponse();
                if (cnblogsRespone != null && cnblogsRespone.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader sr;
                    using (sr = new StreamReader(cnblogsRespone.GetResponseStream()))
                    {
                        responseResult = sr.ReadToEnd();
                    }
                    sr.Close();

                }
                return responseResult;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (cnblogsRespone != null)
                {
                    cnblogsRespone.Close();
                }

            }
        }
    }
}
