using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECChkWeb
{
    /// <summary>
    /// 定義路由
    /// </summary>
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:44322";
        
        //public static string APIBaseUrl = "http://ecchkapi-dev.ap-northeast-1.elasticbeanstalk.com";
        public static string ECChkAPIPath = APIBaseUrl + "/api/SECChk/";
    }
}
