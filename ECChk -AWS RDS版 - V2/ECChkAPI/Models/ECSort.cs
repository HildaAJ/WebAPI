using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECChkAPI.Models
{
    /// <summary>
    ///0：收貨人姓名
    ///1：廠商名稱
    ///2：進貨日期
    ///3：狀態
    ///4：末三碼
    /// </summary>
    public enum ECSort
    {
        Name,
        CompanyName,
        InDate,
        State,
        //NewNumber,
        EndThreeYard
    }
}
