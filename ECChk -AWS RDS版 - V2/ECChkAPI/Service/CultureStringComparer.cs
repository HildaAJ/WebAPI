using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ECChkAPI.Service
{
    public class CultureStringComparer:IComparer<string>
    {
         CultureInfo CurrentCultureInfo { get; set; }
         CompareOptions Options { get; set; }
        public CultureStringComparer() { }

        public CultureStringComparer(CultureInfo currentCultureInfo, CompareOptions options)
        {
            CurrentCultureInfo = currentCultureInfo;
            Options = options;
        }

        /// <summary>
        /// 比較
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(string x, string y)
        {
            // 呼叫 CultureInfo 內已經實作的 Compare
            return CurrentCultureInfo.CompareInfo.Compare(x, y, Options);
        }

    }
}
