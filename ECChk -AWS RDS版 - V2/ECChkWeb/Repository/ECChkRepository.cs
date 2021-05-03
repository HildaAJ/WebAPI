using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ECChkWeb.Models;
using ECChkWeb.Models.Parameter;
using Newtonsoft.Json;

namespace ECChkWeb.Repository
{
    public class ECChkRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public ECChkRepository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// 根據末三碼取得EC資料
        /// </summary>
        /// <param name="url"></param>
        /// <param name="EndThreeYard"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<IFECCUTF>> GetECByEndThreeYardAsync(string url,string EndThreeYard)
        public async Task<IEnumerable<IFECCUTF>> GetECByEndThreeYardAsync(string url, StoreAndThree param)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            //request.Content = new StringContent(
            //    JsonConvert.SerializeObject(EndThreeYard), Encoding.UTF8, "application/json");

            request.Content = new StringContent(
                JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            //送出請求
            HttpResponseMessage response = await client.SendAsync(request);

            //若回傳狀態201,代表成功
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //取得回覆內容,在反序列化回去string
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<IFECCUTF>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取得EC資料
        /// </summary>
        /// <param name="url"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<IFECCUTF>> GetECAsync(string url,ECSort sort=ECSort.InDate)
        public async Task<IEnumerable<IFECCUTF>> GetECAsync(string url, StoreAndSort param)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,url);
            IEnumerable<IFECCUTF> obj;
            //request.Content = new StringContent(
            //    JsonConvert.SerializeObject(sort), Encoding.UTF8, "application/json");
            request.Content = new StringContent(
                JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
            
            var client = _clientFactory.CreateClient();
            //送出請求
            HttpResponseMessage response = await client.SendAsync(request);

            //若回傳狀態201,代表成功
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //取得回覆內容,在反序列化回去string
                var jsonString = await response.Content.ReadAsStringAsync();
                obj= JsonConvert.DeserializeObject<IEnumerable<IFECCUTF>>(jsonString);
                return JsonConvert.DeserializeObject<IEnumerable<IFECCUTF>>(jsonString);
            }
            else
            {
                var msg = response.RequestMessage;
                return null;
            }
        }
    }
}
