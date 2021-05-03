using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECChkAPI.Models
{
    public class IFECCUTF
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StoreNo { get; set; }

        [Required]
        public string EcCode1 { get; set; }

        [Required]
        public string EcCode2 { get; set; }

        //[Required]
        //public string EcLayer { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string InDate { get; set; }

        [Required]
        public string Price { get; set; }

        [Required]
        public string State { get; set; }

        //[Required]
        //public string DeliType { get; set; }

        //[Required]
        //public string NewNumber { get; set; }

        [Required]
        public string EndThreeYard { get; set; }

        //陳列編號，一定為4碼
        //public string ECCabinetBarcode { get; set; }

        //陳列貨架名稱 英文、數字、中文(最多5個全形中文) 共 10 byte
        //public string ECCabinetName { get; set; }
    }
}
