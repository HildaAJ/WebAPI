using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECChkAPI.Models;
using Newtonsoft.Json;
using ECChkAPI.Interface;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Amazon;
using Amazon.Util;

namespace ECChkAPI.Repository
{
  
    public class AWSRedisDb:IDbStorage
    {
       
        //private readonly string PrimaryEndPoint = "ecchkredis.a4wfph.ng.0001.apne1.cache.amazonaws.com:6379";
        //private readonly string ReaderEndPoint = "ecchkredis-ro.a4wfph.ng.0001.apne1.cache.amazonaws.com:6379";
        private  IConnectionMultiplexer _muxer;
        //private readonly string ConnectUrl = "localhost:6379";
        public AWSRedisDb(string conn)
        {
            // _muxer = ConnectionMultiplexer.Connect($"{PrimaryEndPoint},{ReaderEndPoint}");
            _muxer = ConnectionMultiplexer.Connect(conn);
           // AWSSDKHandler.RegisterXRayForAllServices();
            //AWSSDKHandler.RegisterXRay<IConnectionMultiplexer>();
        }
        /// <summary>
        /// 從redis中取得對應店號的EC資料
        /// </summary>
        /// <typeparam name="T">IFECCUTF</typeparam>
        /// <param name="storeno">店號</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetCacheByStoreno<T>(string storeno)where T:class,new()
        {
           
            Serilog.Log.Information("Start access redis to get data");
            string result =await _muxer.GetDatabase().StringGetAsync(storeno);
           
            if (string.IsNullOrEmpty(result))
            {
                Serilog.Log.Information("Get nothing from redis");
                return null;
            }
            var deserializedObj = JsonConvert.DeserializeObject<IEnumerable<T>>(result);
            Serilog.Log.Information("End access redis to get data");
            return deserializedObj;

        }

        /// <summary>
        /// 將EC資料寫入redis,key為店號,value為json格式再序列化後的EC資料
        /// </summary>
        /// <param name="storeKey">店號</param>
        /// <param name="value">EC資料</param>
        /// <returns></returns>
        public async Task<bool> SetCacheByStoreNo(string storeKey,string value)
        {
            Serilog.Log.Information("Start access redis to set data");
            var result = await _muxer.GetDatabase().StringSetAsync(storeKey, value,TimeSpan.FromDays(10),When.Always);
            
            Serilog.Log.Information("End access redis to set data");
            return result;
        }


        /// <summary>
        /// 刪除指定key
        /// </summary>
        /// <param name="storeKey"></param>
        /// <returns></returns>
        public async Task<bool> DeleteKey(string storeKey)
        {
            bool result = await _muxer.GetDatabase().KeyDeleteAsync(storeKey);
            return result;
        }
    }
}
