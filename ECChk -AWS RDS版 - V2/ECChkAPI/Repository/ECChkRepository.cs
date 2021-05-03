using ECChkAPI.Data;
using ECChkAPI.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ECChkAPI.Interface;

namespace ECChkAPI.Repository
{
    public class ECChkRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IDbStorage _cache;

        //之後可用其他方式存
        //private static IEnumerable<IFECCUTF> _data { get; set; }

        private IEnumerable<IFECCUTF> result { get; set; }
        public ECChkRepository(ApplicationDbContext db, IDbStorage cache)
        {
            _db = db;
            _cache = cache;
        }

        /// <summary>
        /// 根據店號取得EC資料
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IFECCUTF>> GetECDataAsync(string StoreNo)
        {
            try
            {
                Serilog.Log.Information("Start GetECDataAsync");
                result = await _cache.GetCacheByStoreno<IFECCUTF>(StoreNo);

                if (result == null)
                {

                    var data = await _db.IFECCUTFs.Where(a => a.StoreNo == StoreNo).ToListAsync();
                    if (data != null)
                    {
                        var value = JsonConvert.SerializeObject(data);
                        bool setStatus = await _cache.SetCacheByStoreNo(StoreNo, value);
                        if (setStatus == false)
                        {
                            result = await _db.IFECCUTFs.Where(a => a.StoreNo == StoreNo).ToListAsync();
                            Serilog.Log.Information("End GetECDataAsync:fail save data in redis");
                            return result;
                        }
                        else
                        {
                            Serilog.Log.Information("End GetECDataAsync:success save data in redis");
                            result = await _cache.GetCacheByStoreno<IFECCUTF>(StoreNo);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Serilog.Log.Error(ex, $"GetECDataAsync In catch, time: {DateTime.Now}");
            }

            return result;
            //if (_data != null && _data.ToList()[0].StoreNo == StoreNo)
            //    return _data;
            //else
            //{
            //    _data = await _db.IFECCUTFs.Where(a => a.StoreNo == StoreNo).ToListAsync();
            //    return _data;
            //}

        }

        /// <summary>
        /// 根據店號及末三碼取得EC資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IFECCUTF> GetECDataAsync(string StoreNo, string EndThreeYard)
        {
            List<IFECCUTF> lstEndThreeData = new List<IFECCUTF>();
            List<IFECCUTF> lstNameSerach = new List<IFECCUTF>();
            List<IFECCUTF> data = new List<IFECCUTF>();
            Serilog.Log.Information("Start GetECDataAsync via EndThreeYard");
            //_data = GetECDataAsync(StoreNo).Result;
            result = GetECDataAsync(StoreNo).Result.ToList();

            //找尋末三碼EC資料
            lstEndThreeData = result.Where(a => a.StoreNo == StoreNo && a.EndThreeYard == EndThreeYard).ToList();
            data.AddRange(lstEndThreeData);

            //末三碼不同 姓名相同 也一併顯示 並以進貨日期排序
            lstNameSerach = (from x in lstEndThreeData
                             join y in result
                             on x.Name equals y.Name
                             where y.EndThreeYard != EndThreeYard
                             select x
                     ).OrderBy(x => x.InDate).ToList();
            data.AddRange(lstNameSerach);
            Serilog.Log.Information("End GetECDataAsync via EndThreeYard");
            return data;
        }


        /// <summary>
        /// 更新門市EC資料,將redis的該門市value清空
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task UpdateEC(IFECCUTF data)
        {
            try
            {
                string StoreNo = data.StoreNo;

                data.State = "2";
                await UpdateDb(data);
               
            }
            catch (Exception ex)
            {           
                Serilog.Log.Error($"DeleteECRedis:{ex.Message}");
                
            }

        }

        private async Task UpdateDb(IFECCUTF iFECCUTF)
        {
            try
            {
                 _db.Update(iFECCUTF);
                await Save();
            }
            catch (Exception ex)
            {

                Serilog.Log.Error($"UpdateDb: {ex.Message}");
            }
            
            
        }

        private async Task Save()
        {
            
            try
            {
                await _db.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"Save: {ex.Message}");
                
            }


        }

        /// <summary>
        /// 清空Redis門市資料
        /// </summary>
        /// <param name="StoreNo">店號</param>
        /// <returns></returns>
        public async Task DeleteECRedis(string StoreNo)
        {
            await _cache.DeleteKey(StoreNo);
        }
    }
}
