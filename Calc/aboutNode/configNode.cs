using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;


namespace Calc.aboutNode
{
     class configNode
    {
 #region  ///引入DLL Kernel32 使用方法WritePrivateProfileString  GetPrivateProfileString
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);//写配置文件
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileStringA(string section, string key, string def, StringBuilder retVal, int size, string filePath);//读配置文件
       
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
        public  void writeIni(string section, string key, string value, string filePath)//写配置文件
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
            int i = GetPrivateProfileStringA(section, key, "", tempBuilder, 1024, filePath);
            return tempBuilder.ToString();
        }
        /// <summary>
        /// 启动软件时候的配置
        /// </summary>
        /// <param name="dt">放假的日子组成的集合</param>
        /// /// <remarks>
        /// 2013 -7-15 fls 15:00:00
        /// </remarks>
        public  void startConfig(out HashSet<KeyValuePair<string,string>> lt)
        {
           string tems = null;
           int index = 0;
           lt = new  HashSet<KeyValuePair<string,string>>();
           do
           {
               index++;
               tems = readIni("Node", index.ToString(), System.Environment.CurrentDirectory + @"/.configN.ini");
               if (!tems.Equals(""))
               {
                   string[] spStr = tems.Split(',');
                   try
                   {
                       KeyValuePair<string, string> kp = new KeyValuePair<string, string>(spStr[0], spStr[1]);
                       lt.Add(kp);
                   }
                   catch (System.Exception ex)
                   {
                       MessageBox.Show("配置错误，修改configN.ini");
                       Console.WriteLine(ex);
                   }
                 

               }
             
           } while (!tems.Equals(""));
        }
        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="dt">放假的日子组成的集合</param>
        /// <remarks>
        /// 2013 -7-15 fls 15:00:00
        /// </remarks>
        public void updateConfig(ref HashSet<KeyValuePair<string, string>> lt)
        {
            
            if (File.Exists(System.Environment.CurrentDirectory + @"/.configN.ini"))
            {
                File.Delete(System.Environment.CurrentDirectory + @"/.configN.ini");
            }
          
            int i = 0;
            
            foreach (KeyValuePair<string, string> kv in lt)
            {
                i++;
                writeIni("Node", i.ToString(), kv.Key+","+kv.Value, System.Environment.CurrentDirectory + @"/.configN.ini");
            }
           
        }


    }
}
