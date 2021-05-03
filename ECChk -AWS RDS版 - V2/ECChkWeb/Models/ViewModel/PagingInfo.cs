using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ECChkWeb.Models.ViewModel
{
    public class PagingInfo
    {
        //public IPagedList<IFECCUTF> ECPageList{get;set;}

        public int TotalItems { get; set; }

        //每頁顯示幾個
        public int ItemsPerPage { get; set; }
        //目前頁次
        public int CurrentPage { get; set; }
        //總頁數
        public int TotalPage => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);

        //public string UrlParam { get; set; }
        //排序方式
        public ECSort eCSort { get; set; }

    }
}
