using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using ECChkAPI.Data;
using ECChkAPI.Models;
using ECChkAPI.Models.Parameter;
using ECChkAPI.Repository;
using ECChkAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECChkAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class SECChkController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ECChkRepository _ecRepo;
        private readonly ApplicationDbContext _db;
      
        public SECChkController(IMapper mapper, ECChkRepository ecRepo, ApplicationDbContext db)
        {
            _mapper = mapper;
            _ecRepo = ecRepo;
            _db = db;
            
        }


        [HttpPost]
        [ProducesResponseType(200, Type = typeof(List<IFECCUTFDto>))]//回傳type
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType] //其他沒包括在上述設定ProducesResponseType的狀態會使用此屬性
        //public IActionResult GetAll(string StoreNo, [FromBody] ECSort sort = ECSort.InDate)
        public IActionResult GetAll([FromBody] StoreAndSort param)
        {
            try
            {
                Serilog.Log.Information("Start GetAll");
                if (param == null)
                {
                    Serilog.Log.Warning("End GetAll:No content");
                    return NoContent();
                }

                //從Db根據店號取得EC資料
                var objList = _ecRepo.GetECDataAsync(param.StoreNo).Result;
                if (objList == null || objList.Count()==0)
                    return NotFound();

                //將資料mapping到IFECCUTFDto
                var objDto = new List<IFECCUTFDto>();
                foreach (var item in objList)
                {
                    objDto.Add(_mapper.Map<IFECCUTFDto>(item));
                }

                //取得排序後的EC資料
                ECService service = new ECService();
                var objShow = service.SortECData(objDto, param.Sort);
                Serilog.Log.Information("End GetAll");
                return Ok(objShow);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"End GetAll:{ex.Message}");
                return StatusCode(500, ModelState);
            }

        }


        /// <summary>
        /// 使用末三碼查詢EC資料,以進貨日期排序顯示
        /// </summary>
        /// <param name="param">店號,末三碼</param>
        /// <returns></returns>
        [HttpPost(Name = "GetFromEndThreeYard")]
        [ProducesResponseType(200, Type = typeof(List<IFECCUTFDto>))]//回傳type
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType] //其他沒包括在上述設定ProducesResponseType的狀態會使用此屬性

        //public IActionResult GetFromEndThreeYard(string StoreNo, [FromBody] string EndThreeYard)
        public IActionResult GetFromEndThreeYard([FromBody] StoreAndThree param)
        {
            // 從Db根據店號及末三碼取得EC資料
            //var objList = _ecRepo.GetECDataAsync(StoreNo,EndThreeYard).ToList();
            try
            {
                var objList = _ecRepo.GetECDataAsync(param.StoreNo, param.EndThreeYard).ToList();
                if (objList == null |objList.Count() == 0)
                    return NotFound();

                //將資料mapping到IFECCUTFDto
                var objDto = new List<IFECCUTFDto>();
                foreach (var item in objList)
                {
                    objDto.Add(_mapper.Map<IFECCUTFDto>(item));
                }

                //取得指定頁次的EC資料
                ECService service = new ECService();
                var objShow = service.SortECData(objDto, ECSort.InDate);

                return Ok(objShow);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error($"End GetFromEndThreeYard:{ex.Message}");
                return StatusCode(500, ModelState);

            }
            
        }

        ///// <summary>
        ///// 測試用資料輸入:新增門市EC資料到Db
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult CreateECData()
        //{
        //    try
        //    {
        //        //寫死路徑
        //        string FilePath = @"C:\Users\Hilda\Documents\雲端POS\雲端POS\IFECCUTF\";
        //        string FileName = "IFECCUTF-3.xml";
        //        List<IFECCUTF> iFECCUTFs = new List<IFECCUTF>();
        //        XmlDocument xmlDoc = new XmlDocument();
        //        //string XmlTitle = "<?xml version=\"1.0\" encoding=\"UTF-16\"?>";
        //        xmlDoc.Load(FilePath + FileName);

        //        //擷取節點
        //        XmlNodeList xmlNodeList = xmlDoc.SelectNodes("/Root/ECData");

        //        foreach (XmlNode node in xmlNodeList)
        //        {
        //            IFECCUTF obj = new IFECCUTF();
        //            obj.StoreNo = "197308"; //自己寫店號
        //            obj.EcCode1 = node.ChildNodes[0].InnerText;
        //            obj.EcCode2 = node.ChildNodes[1].InnerText;
        //            // obj.EcLayer = node.ChildNodes[2].InnerText;
        //            //obj.Name = node.ChildNodes[3].InnerText;
        //            //obj.CompanyName = node.ChildNodes[4].InnerText;
        //            //obj.InDate = node.ChildNodes[5].InnerText.Substring(0,10);
        //            //obj.Price = node.ChildNodes[6].InnerText;
        //            //obj.State = node.ChildNodes[7].InnerText;
        //            //obj.DeliType = node.ChildNodes[8].InnerText;
        //            //obj.NewNumber = node.ChildNodes[9].InnerText;
        //            //obj.EndThreeYard = node.ChildNodes[10].InnerText;

        //            obj.Name = node.ChildNodes[2].InnerText;
        //            obj.CompanyName = node.ChildNodes[3].InnerText;
        //            obj.InDate = node.ChildNodes[4].InnerText.Substring(0, 10);
        //            obj.Price = node.ChildNodes[5].InnerText;
        //            obj.State = node.ChildNodes[6].InnerText;
        //            obj.EndThreeYard = node.ChildNodes[7].InnerText;

        //            _db.Add(obj);
        //            //  _db.SaveChanges();
        //        }
        //        _db.SaveChanges();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ModelState);
        //    }

        //}

        ///// <summary>
        ///// 取得門市EC資料,更新後刪除redis資料
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> CleanRedisWhenUpdateEC(string StoreNo)
        //{
        //    try
        //    {
        //        var data = await _db.IFECCUTFs.Where(a => a.StoreNo == StoreNo).Take(3).ToListAsync();


        //        //更新EC DB資料
        //        foreach (var item in data)
        //        {
        //            await _ecRepo.UpdateEC(item);
        //        }

        //        //清空Redis的值           
        //        await _ecRepo.DeleteECRedis(StoreNo);

        //        var after= await _db.IFECCUTFs.Where(a => a.StoreNo == StoreNo).Take(3).ToListAsync(); ;

        //        return Ok(after);
        //    }
        //    catch (Exception ex)
        //    {
        //        Serilog.Log.Error($"CleanRedisWhenUpdateEC: {ex.Message}");
        //        return StatusCode(500, ex.Message);
        //    }
        //}


    }
}
