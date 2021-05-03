using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECChkWeb.Models.ViewModel
{
    public class ECListViewModel
    {
        public List<IFECCUTF> ECList { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public ECListViewModel()
        {
            ECList = new List<IFECCUTF>();
        }
    }
}
