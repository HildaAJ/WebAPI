using ECChkAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace ECChkAPI.Service
{
    public class ECService
    {
         readonly string State1 = "在店";
         readonly string State2 = "已取";
         readonly string State3 = "挑退";
         readonly string State4 = "驗短";
         readonly string State5 = "轉日";
         readonly string State6 = "1配在途";
         readonly string State7 = "2配在途";
         readonly string Layer1 = "常溫";
         readonly string Layer2 = "冷藏";
         readonly string Layer3 = "冷凍";

        /// <summary>
        /// 指定排序取得欲顯示的EC資料
        /// </summary>
        /// <param name="dtos">顯示於畫面的EC資料</param>
        /// <param name="sort">指定排序值</param>
        /// <returns>排序後的資料</returns>
        public ICollection<IFECCUTFDto> SortECData(List<IFECCUTFDto> dtos, ECSort sort)
        {
            List<IFECCUTFDto> result = new List<IFECCUTFDto>();
            //zh-TW 預設用筆劃排序 0x00000404、替代排序為注音 0x00030404
            CultureInfo taiwanCulture = new CultureInfo("zh-TW");
            CultureStringComparer comparerTaiwan = new CultureStringComparer(taiwanCulture, CompareOptions.None);
            
            switch (sort)
            {
                case ECSort.Name:
                    result = dtos.OrderBy(a => a.Name,comparerTaiwan).ToList();
                    break;

                case ECSort.CompanyName:
                    result = dtos.OrderBy(a => a.CompanyName,comparerTaiwan).ToList();
                    break;

                case ECSort.InDate:
                    result = dtos.OrderByDescending(a => a.InDate).ThenBy(a=>a.Name).ToList();
                    break;

                case ECSort.State:
                    result = dtos.OrderBy(a => a.State).ToList();
                    break;

                //case ECSort.NewNumber:
                //    result = dtos.OrderBy(a => a.NewNumber).ToList();
                //    break;

                case ECSort.EndThreeYard:
                    result = dtos.OrderBy(a => a.EndThreeYard).ToList();
                    break;

                default://預設以進貨日期排序
                    result = dtos.OrderBy(a => a.InDate).ToList();
                    break;
            }

            //將頁面資料的溫層及狀態用中文顯示
            var data = ChangeName(result);

            return data;
        }

        /// <summary>
        /// 將溫層及狀態換成中文字顯示
        /// </summary>
        /// <param name="SortData">目前頁面的EC資料</param>
        /// <returns></returns>
        private ICollection<IFECCUTFDto> ChangeName(List<IFECCUTFDto> SortData)
        {
            List<IFECCUTFDto> result = new List<IFECCUTFDto>();

            foreach (var item in SortData)
            {
               // var layer = item.EcLayer;
                var state = item.State;
                //switch (layer)
                //{
                //    case "1": //常溫
                //        item.EcLayer = Layer1;
                //        break;
                //    case "2"://冷藏
                //        item.EcLayer = Layer2;                       
                //        break;
                //    case "3": //冷凍
                //        item.EcLayer = Layer3;                       
                //        break;
                //}

                switch (state)
                {
                    case "1": //在店
                        item.State = State1;
                        break;
                    case "2"://已取
                        item.State = State2;
                        break;
                    case "3": //挑退
                        item.State = State3;
                        break;
                    case "4": //驗短
                        item.State = State4;
                        break;
                    case "5"://轉日   
                        item.State = State5;
                        break;
                    case "6": //1配在途
                        item.State = State6;
                        break;
                    case "7": //2配在途
                        item.State = State7;
                        break;
                }

                result.Add(item);

            }

            return result;
           
        }



        ///// <summary>
        ///// 排序後顯示於畫面的EC資料
        ///// </summary>
        ///// <param name="dtos">經過dto後的所有EC資料</param>
        ///// <param name="PageNo">顯示頁數</param>
        ///// <param name="sort">排序方式</param>
        ///// <returns></returns>
        //public ICollection<IFECCUTFDto> ShowECData(List<IFECCUTFDto> dtos, string PageNo, ECSort sort)
        //{
        //    int page = Convert.ToInt32(PageNo);

        //    if (dtos.Count() <= PageSize)
        //        page = 1;

        //    //取得第PageNo頁的資料
        //    var OnePagedata = dtos.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        //    //進行排序
        //    var SortData = SortECData(OnePagedata, sort).ToList();

        //    //將頁面資料的溫層及狀態用中文顯示
        //    var result = ChangeName(SortData);

        //    return result;
        //}
   
    
    }
}
