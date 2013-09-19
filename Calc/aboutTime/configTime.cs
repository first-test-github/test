using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;


namespace Calc.aboutTime
{
     class configTime
    {
 #region  ///引入DLL Kernel32 使用方法WritePrivateProfileString  GetPrivateProfileString
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);//写配置文件
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);//读配置文件
        
#endregion
        
        /// <summary>
        /// 往配置文件里面写内容
        /// </summary>
        /// <param name="section">写入的区域</param>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <param name="filePath">路径</param>
        /// /// <remarks>
        /// 2013 -7-15 fls 15:00:00
        /// </remarks>
        public static void writeIni(string section, string key, string value, string filePath)//写配置文件
        {

            WritePrivateProfileString(section, key, value, filePath);
        }
        /// <summary>
        ///读取配置文件的内容
        /// </summary>
        /// <param name="section">读取的区域</param>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <param name="filePath">路径</param>
        /// /// <remarks>
        /// 2013 -7-15 fls 15:00:00
        /// </remarks>
        public  string readIni(string section, string key, string filePath)//读配置文件
        {
            StringBuilder tempBuilder = new StringBuilder(1024);
            int i = GetPrivateProfileString(section, key, "", tempBuilder, 1024, filePath);
            return tempBuilder.ToString();
        }
        /// <summary>
        /// 启动软件时候的配置
        /// </summary>
        /// <param name="dt">放假的日子组成的集合</param>
        /// /// <remarks>
        /// 2013 -7-15 fls 15:00:00
        /// </remarks>
        public  void startConfig(ref HashSet<DateTime> dt)
        {
         
            int i = 0;
            for (i = 0; i < 100; i++)
            {
                string tems = readIni("compress", i.ToString(), System.Environment.CurrentDirectory + @"/.configT.ini");
              //  File.SetAttributes(@"/.config.ini", FileAttributes.Hidden); 
                if (tems.ToString().Equals(""))
                {

                }
                else
                {
                    dt.Add(Convert.ToDateTime(tems));
                }

            }

        }
        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="dt">放假的日子组成的集合</param>
        /// <remarks>
        /// 2013 -7-15 fls 15:00:00
        /// </remarks>
        public  void updateConfig(ref HashSet<DateTime> dt)
        {
            
            if (File.Exists(System.Environment.CurrentDirectory + @"/.configT.ini"))
            {
                File.Delete(System.Environment.CurrentDirectory + @"/.configT.ini");
            }
          
            int i = 0;
            foreach (DateTime d in dt)
            {
                i++;
                writeIni("compress", i.ToString(), d.ToString(), System.Environment.CurrentDirectory + @"/.configT.ini");
            }
           
        }


    }
}
