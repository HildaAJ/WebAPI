using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECChkAPI.Models
{
    public class IFECCUTFDto
    {
        
        public int Id { get; set; }

        [Required]
        public string StoreNo { get; set; }

        //[Required]
        ////溫層
        //public string EcLayer { get; set; } 

        [Required]
        //取貨人姓名
        public string Name { get; set; }

        [Required]
        //廠商名稱
        public string CompanyName { get; set; }

        [Required]
        //進貨日期
        public string InDate { get; set; }

        [Required]
        //金額
        public string Price { get; set; }

        [Required]
        //狀態
        public string State { get; set; }

        //[Required]
        ////配送編號
        //public string NewNumber { get; set; }

        [Required]
        //末三碼
        public string EndThreeYard { get; set; }
    }
}
