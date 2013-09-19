using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Calc.aboutTime
{
    class vacation
    {
        /// <summary>
        /// 获取本年度nowday 之前的周末。
        /// </summary>
        /// <returns> 使用DateTime 作为节点的一个List </returns>
        ///<marks>
        /// 2013-7-11 fls 17:18:00s
        /// </marks> 
        public HashSet<DateTime> getVacationsList(ref HashSet<DateTime> listManul, DateTime nowDay)
        {
            DateTime start = new DateTime(nowDay.Year, 1, 1);
            DateTime end = new DateTime(nowDay.Year, 12, 31);
            while (start <= end)
            {
                if(start.DayOfWeek==DayOfWeek.Saturday||start.DayOfWeek==DayOfWeek.Sunday)
                {
                    listManul.Add(start);
                }
                start = start.AddDays(1.0);
            }
            return listManul;
        }
     
    }
}
