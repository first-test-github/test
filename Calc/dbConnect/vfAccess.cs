using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data;
using System.Windows;
using System.Windows.Forms;

namespace Calc.common
{
    class vfAccess
    {

        public string Path { get; set; }
        public string TableName { get; set; }

        /// <summary>
        /// 从数据库中取出了一个表6月图书完成数据  ,将它的数据放入DateSet类，  返回一个DateSet类
        /// </summary>
        /// <returns>DataSet</returns>
        ///<marks>
        ///2013-4-11 fsl 16:13:00
        /// </marks> 
        public DataSet getDS()
        {
            //if (Path.Length==2)
            //{
            //    Path += "\\";
            //}
            //string CS = @"Provider=vfpoledb; Data Source=" + Path +";Collating Sequence=machine"; 
            string CS = @"SourceType=DBF;SourceDB=" + this.Path + ";Driver={Microsoft Visual FoxPro Driver};Exclusive=No;";
            DataSet ds = new DataSet();
            try
            {
                using (OdbcConnection con = new OdbcConnection(CS))
                {
                    con.Open();
                    OdbcCommand cmd = new OdbcCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "select * from " + TableName;
                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                System.Windows.Forms.MessageBox.Show("请选择dbf文件 ");
            }

            return ds;
        }


        public DataSet getDS(string plusSql)
        {
            if (Path.Length == 2)
            {
                Path += "\\";
            }
            //string CS = @"Provider=vfpoledb; Data Source=" + Path +";Collating Sequence=machine"; 
            string CS = @"SourceType=DBF;SourceDB=" + this.Path + ";Driver={Microsoft Visual FoxPro Driver};Exclusive=No;";
            DataSet ds = new DataSet();
            try
            {
                using (OdbcConnection con = new OdbcConnection(CS))
                {
                    con.Open();
                    OdbcCommand cmd = new OdbcCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "select * from " + TableName + plusSql;
                    OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                System.Windows.Forms.MessageBox.Show("请选择dbf文件 ");
            }

            return ds;
        }
    }
}
