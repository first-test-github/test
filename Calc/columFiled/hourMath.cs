using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
///<
///

namespace Calc.common
{
    class hourMath
    {

        #region  ///定义了上下班时间
        public static DateTime startMorning = new DateTime(2000, 1, 1, 8, 30, 0);
        public static DateTime endMorning = new DateTime(2000, 1, 1, 12, 0, 0);
        public static DateTime startAfternon = new DateTime(2000, 1, 1, 13, 0, 0);
        public static DateTime endAfternon = new DateTime(2000, 1, 1, 17, 30, 0);
        #endregion

        /// <summary>
        /// 判断nowDay是不是在假期的集合里面
        /// </summary>
        /// <param name="nowDay"></param>
        /// <param name="hsVac"></param>
        /// <returns>如果放假返回真否则返回假</returns>
        /// <remarks>
        /// 2013 -7-10 fls
        /// </remarks>
        public static Boolean judgeVac(DateTime nowDay, HashSet<DateTime> hsVac)
        {
            if (hsVac.Contains(nowDay.Date))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 计算一整天的有效工作时间
        /// </summary>
        /// <returns>double类型的小时</returns>
        /// <remarks>
        /// 2013-7-12 zsy
        /// </remarks>
        public static double oneDaytoHours()
        {
            return (endMorning - startMorning).TotalHours + (endAfternon - startAfternon).TotalHours;
        }
       
        /// <summary>
        /// 调整输入的时间若不在工作期内，则调整到工作期内
        /// </summary>
        /// <returns>返回调整结果</returns>
        ///  /// <remarks>
        /// 2013-7-12 zsy
        /// </remarks>
        public static DateTime TimeAdjust(DateTime dt)
        {
            DateTime rs = dt;
            if (dt - dt.Date < startMorning - startMorning.Date)//若时间小于上午上班时间，那么将时间调整为上午开始
            {
                rs = dt.Date.AddHours((startMorning - startMorning.Date).TotalHours);
            }
            if (dt - dt.Date > (endMorning - endMorning.Date) && dt - dt.Date < (startAfternon - startAfternon.Date))//若时间处于中午休息时间，那么将时间调整为上午结束时间
            {
                rs = dt.Date.AddHours((endMorning - endMorning.Date).TotalHours);
            }
            if (dt - dt.Date > (endAfternon - endAfternon.Date))//若时间大于下午结束时间，那么将时间调整为下午结束时间
            {
                rs = dt.Date.AddHours((endAfternon - endAfternon.Date).TotalHours);
            }

            return rs;
        }

        ///<summary>判断是否包含中午休息时间</summary>
        ///<value>输入当天的开始时间和结束时间</value>
        ///<returns>包含返回真，不包含返回假</returns>
        /// /// <remarks>
        /// 2013-7-12 zsy
        /// </remarks>
        public static bool IncludeRestTime(DateTime start, DateTime end)
        {
            if (start - start.Date <= endMorning - endMorning.Date && end - end.Date >= startAfternon - startAfternon.Date)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 计算提交任务当天花费的时间
        /// </summary>
        /// <param name="dtEnd">当天提交的时间</param>
        /// <param name="hsVac">假期集合</param>
        /// <returns>返回当天花费的时间</returns>
        ///  /// <remarks>
        /// 2013-7-12 zsy
        /// </remarks>
        public static double GetSendTime(DateTime dtEnd, HashSet<DateTime> hsVac)
        {
            double rs = 0;
            if (judgeVac(dtEnd, hsVac))//首先判断当天是否放假
            {
                rs = 0;
            }
            else//不放假时，计算提交当天花费的小时数.为当天提交时间减去当天开始时间，扣除中午休息时间（是否扣除根据条件进行判断）
            {
                rs = (dtEnd - dtEnd.Date).TotalHours - (startMorning - startMorning.Date).TotalHours;  //(dtEnd - startMorning).TotalHours;
                if (IncludeRestTime(startMorning, dtEnd))//包括中午休息时间时
                {
                    rs = rs - 1;//减去中午休息时间
                }
            }
            return rs;

        }

        /// <summary>
        /// 计算收到任务当天花费的时间
        /// </summary>
        /// <param name="dtstart">接受任务的时间</param>
        /// <param name="hsVac">假期集合</param>
        /// <returns>收到任务当天花费的时间</returns>
        public static double getRecieveTime(DateTime dtstart, HashSet<DateTime> hsVac)//计算收到任务当天的时间
        {
            double rs = 0;
            if (judgeVac(dtstart, hsVac))//首先判断当天是否放假
            {
                rs = 0;
            }
            else//不放假时，计算提交当天花费的小时数.为当天下午结束时间减去收到任务时的时间，扣除午休时间
            {
                rs = (endAfternon - endAfternon.Date).TotalHours - (dtstart - dtstart.Date).TotalHours;
                if (IncludeRestTime(dtstart, endAfternon))//包括中午休息时间时
                {
                    rs = rs - 1;//减去中午休息时间
                }
            }
            return rs;
        }
       
        /// <summary>
        /// 计算接到任务到提交任务之间经过的天数
        /// </summary>
        /// <param name="dtStart">接到任务的时间</param>
        /// <param name="dtEnd">提交任务的时间</param>
        /// <param name="hsVac">假期集合</param>
        /// <returns>天数</returns>
        public static int GetNumWorkDay(DateTime dtStart, DateTime dtEnd, HashSet<DateTime> hsVac)
        {
            int num = 0;
            DateTime tempTime = dtStart.Date.AddDays(1);
            while (tempTime.Date < dtEnd.Date)//计算相隔的工作日，若不是假期，则计数
            {
                if (!judgeVac(tempTime, hsVac))
                {
                    num++;
                }
                tempTime = tempTime.AddDays(1);
            }
            return num;

        }
       
        /// <summary>
        /// 这个函数用来计算有效的工作时间，它去除了时间段内的放假和下班的时间。
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="Atime">上午上班时间</param>
        /// <param name="Btime">上午下班时间</param>
        /// <param name="Ctime">下午上班时间</param>
        /// <param name="Dtime">下午下班时间</param>
        /// <parm name="hsVac">放假的集合</parm>
        /// <returns>有效的工作时间</returns>
        public static double CalculateWorkingDays(DateTime dtStart, DateTime dtEnd, HashSet<DateTime> hsVac)
        {
            double rs = 0;

            dtStart = TimeAdjust(dtStart);
            dtEnd = TimeAdjust(dtEnd);

            switch ((int)(dtEnd.Date - dtStart.Date).TotalDays)
            {
                case 0: //同一天时
                    if (judgeVac(dtEnd, hsVac))
                    {
                        rs = 0;
                    }
                    else
                    {
                        rs = (dtEnd - dtStart).TotalHours;
                        if (IncludeRestTime(dtStart, dtEnd))
                        {
                            rs = rs - 1;
                        }
                    }
                    break;

                case 1://相邻的两天时
                    rs = GetSendTime(dtEnd, hsVac) + getRecieveTime(dtStart, hsVac);
                    break;

                default://相隔一天以上时
                    rs = GetSendTime(dtEnd, hsVac) + getRecieveTime(dtStart, hsVac) + GetNumWorkDay(dtStart, dtEnd, hsVac) * oneDaytoHours();
                    break;
            }
            return rs;
        }


    }
}