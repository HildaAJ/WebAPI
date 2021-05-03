using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECChkWeb.Models
{
    public class IFECCUTF
    {
        public int Id { get; set; }

        public string StoreNo { get; set; }

        [Required]
        [Display(Name = "溫層")]
        public string EcLayer { get; set; }

        [Required]
        [Display(Name = "取貨人姓名")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "廠商名稱")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "進貨日期")]
        public string InDate { get; set; }

        [Required]
        [Display(Name = "金額")]
        public string Price { get; set; }

        [Required]
        [Display(Name = "狀態")]
        public string State { get; set; }

        [Required]
        [Display(Name = "配送編號")]
        public string NewNumber { get; set; }

        [Required]
        [Display(Name = "末三碼")]
        public string EndThreeYard { get; set; }
    }
}
