using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace YfCloud.Utilities.NetOperater
{
    /// <summary>
    /// RestfulApiClient 提供RestfulAPI的访问
    /// </summary>
    public class RestfulApiClient 
    {
        public static string UplaodUrl { get; set; }

        /// <summary>
        /// 缓存地址
        /// </summary>
        public static string CacheUrl { get; set; }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="route"></param>
        /// <param name="heads"></param>
        /// <param name="tosend"></param>
        public static  T Post<T>(string baseUrl,string route,Dictionary<string,string>heads,Object tosend)  where T : new()
        {
            try
            {
                var client = new RestClient(baseUrl);

                var request = new RestRequest(route);
                if (heads != null)
                {
                    foreach (string key in heads.Keys)
                    {
                        request.AddHeader(key, heads[key]);
                    }
                }
                if(tosend!=null)
                {
                    request.AddJsonBody(tosend);
                }
               
                return client.PostAsync<T>(request).Result;
                
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        public static Object UploadFile(string baseUrl, string route, Dictionary<string, string> heads, Stream stream,string name)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(route);

            Action<Stream> writer = (s) => {
                stream.CopyTo(s);
            };

            if (heads != null)
            {
                foreach (string key in heads.Keys)
                {
                    request.AddHeader(key, heads[key]);
                }
            }

            request.AddFile(name, writer, name, stream.Length);

            var s1 = client.PostAsync<Object>(request).Result;

            return s1;
        }
    }
}
