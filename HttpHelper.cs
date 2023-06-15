using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHelper
{
    public static class HttpHelper
    {
        #region 同步
        public static JObject HttpGet(string url, Dictionary<string, string> param = null)
        {
            string fullUrl = url;
            if (param != null && param.Count > 0)
            {
                fullUrl += $"?{param.FirstOrDefault().Key}={param.FirstOrDefault().Value}";
                param.Remove(param.FirstOrDefault().Key);
                foreach (var item in param)
                {
                    fullUrl += $"&{item.Key}={item.Value}";
                }
            }
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, fullUrl);
            using var client = new HttpClient().Send(httpRequestMessage);
            var responseBody = client.Content.ReadAsStringAsync().Result;
            return JObject.Parse(responseBody);
        }

        public static JObject HttPost(string url, JObject pairs)
        {
            using StringContent jsonContent = new StringContent(pairs.ToString(), Encoding.UTF8, "application/json");
            string responseBody = @"{}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequestMessage.Content = jsonContent;

            using var client = new HttpClient().Send(httpRequestMessage);
            responseBody = client.Content.ReadAsStringAsync().Result;
            return JObject.Parse(responseBody);
        }
        #endregion

        #region 异步

        public static async Task<HttpResponseMessage> HttpGetAsync(string url, Dictionary<string, string> param = null)
        {
            string fullUrl = url;
            if (param != null && param.Count > 0)
            {
                fullUrl += $"?{param.FirstOrDefault().Key}={param.FirstOrDefault().Value}";
                param.Remove(param.FirstOrDefault().Key);
                foreach (var item in param)
                {
                    fullUrl += $"&{item.Key}={item.Value}";
                }
            }
            using var repose = await new HttpClient().GetAsync(fullUrl);
            repose.EnsureSuccessStatusCode();
            var responseBody = await repose.Content.ReadAsStringAsync();

            return repose;
        }
        public static async Task<HttpResponseMessage> HttPostAsync(string url, JObject pairs)
        {
            using StringContent jsonContent = new StringContent(pairs.ToString(), Encoding.UTF8, "application/json");
            string responseBody = @"{}";

            using var repose = await new HttpClient().PostAsync(url, jsonContent);
            repose.EnsureSuccessStatusCode();
            responseBody = await repose.Content.ReadAsStringAsync();

            return repose;
        }

        #endregion


    }
}
