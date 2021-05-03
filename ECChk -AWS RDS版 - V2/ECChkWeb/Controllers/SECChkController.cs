using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECChkWeb.Models;
using ECChkWeb.Models.Parameter;
using ECChkWeb.Models.ViewModel;
using ECChkWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace ECChkWeb.Controllers
{
    public class SECChkController : Controller
    {
        private readonly ECChkRepository _ecRepo;
        private int PageSize = 10;
        private static ECSort eCSort;

        //之後可用其他方式存
        private static IEnumerable<IFECCUTF> data { get; set; }


        public SECChkController(ECChkRepository ecRepo)
        {
            _ecRepo = ecRepo;
          
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 末三碼查詢EC
        /// </summary>
        /// <param name="storeNo">店號</param>
        /// <param name="EndThreeYard">末三碼</param>
        /// <param name="page">頁次</param>
        /// <returns></returns>
        public async Task<IActionResult> GetECByEndThreeYard(string storeNo, string EndThreeYard, int page = 1)
        {
            ECListViewModel vm = new ECListViewModel();

            StoreAndThree param = new StoreAndThree
            {
                StoreNo = storeNo,
                EndThreeYard = EndThreeYard
            };

            //data = await _ecRepo.GetECByEndThreeYardAsync(SD.ECChkAPIPath + "GetFromEndThreeYard?storeno=" + storeNo, EndThreeYard);
            data = await _ecRepo.GetECByEndThreeYardAsync(SD.ECChkAPIPath + "GetFromEndThreeYard", param);
            if (data.Count() > 0)
            {
                //放入EC資料至ViewModel
                vm.ECList = data.ToList();
                var count = vm.ECList.Count();

                //頁數資料
                vm.PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = count,
                    eCSort = ECSort.InDate
                };

                //當頁顯示EC資料
                if (count > 10)
                {
                    vm.ECList = vm.ECList.Skip((vm.PagingInfo.CurrentPage - 1) * PageSize).Take(PageSize).ToList();
                }

                return View(vm);
            }
            else { return null; }     
        }

        /// <summary>
        /// 根據店號、頁數、排序取得EC資料並顯示在畫面上
        /// </summary>
        /// <param name="storeNo">店號</param>
        /// <param name="page">目前頁次</param>
        /// <param name="sort">排序方式</param>
        /// <returns></returns>
        public async Task<IActionResult> GetAll(string storeNo, int page = 1, ECSort sort = ECSort.InDate)
        {
            ECListViewModel vm = new ECListViewModel();
            StoreAndSort param = new StoreAndSort
            {
                StoreNo = storeNo,
                Sort = sort
            };

            //if(data==null || data.First().StoreNo != storeNo||eCSort!=sort)
            if (data==null || data.First().StoreNo != param.StoreNo || eCSort!= param.Sort)
            {
                // eCSort = sort;
                eCSort = param.Sort;
                //data = await _ecRepo.GetECAsync(SD.ECChkAPIPath + "GetAll?storeno=" + storeNo, sort);
                data = await _ecRepo.GetECAsync(SD.ECChkAPIPath + "GetAll", param);
            }

            //放入EC資料至ViewModel
            vm.ECList = data.ToList();
            var count = vm.ECList.Count();
            
            //頁數資料
            vm.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = count,
                //eCSort = sort
                eCSort = param.Sort
            };
            //當頁顯示EC資料
            vm.ECList=vm.ECList.Skip((vm.PagingInfo.CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            return View(vm);
        }
    }
}
