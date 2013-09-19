using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calc.columFiled;
using Calc.aboutNode;
using CalCycleWPF.common;
using System.Data;
using Calc.aboutTime;
using Calc.common;
namespace  Calc.columFiled
{

    class columnFiledMaker
    {
        public List<columField> list = null;
        HashSet<KeyValuePair<string, string>> hs = null;
        double TempValue = 0.0;
        double MaxValue = Double.MinValue;
        double MinValue = Double.MaxValue;
      
        string maxId = "无最大值";
        string minId = "无最小值";
        HashSet<DateTime> vacationSet = null;
        public void makeColumList()
        {
            list = new List<columField>();
            hs = new HashSet<KeyValuePair<string, string>>();
            configNode config = new configNode();
            config.startConfig(out hs);
            foreach (KeyValuePair<String, String> kv in hs)
            {
                columField coField = new columField();
                coField.Kind = kv.Key;
                string[] coFieldstring = kv.Value.Split(';', '=', '-');
                coField.Department = coFieldstring[0];
                coField.Result = coFieldstring[1];
                coField.FirstColum = coFieldstring[2];
                coField.SecondColum = coFieldstring[3];
                coField.Weight = coFieldstring[4];
                coField.Plus = coFieldstring[5];
                
                list.Add(coField);
            }
        }
        public void makeResult(ref String Path,ref String TableName)
        {
            vfAccess vfaccess = new vfAccess();
            vfaccess.Path = Path;
            vfaccess.TableName = TableName;
            DataSet ds = vfaccess.getDS();
            vacationSet = new HashSet<DateTime>();
            configTime configer = new configTime();
            configer.startConfig(ref vacationSet);
            foreach (columField coField in list)
            {
                int count = 0;
                double sum = 0.0;
                int overcount = 0;
                foreach (DataTable tbl in ds.Tables)
                {
                 
                    foreach (DataRow row in tbl.Rows)
                    {
                        Object[] o = row.ItemArray;
                        string IDstr = o[tbl.Columns.IndexOf("编号")].ToString();
                        if (o[tbl.Columns.IndexOf(coField.FirstColum)].ToString().Equals("") || o[tbl.Columns.IndexOf(coField.SecondColum)].ToString().Equals(""))
                        {

                        }
                        else
                        {
                            count++;
                            TempValue = hourMath.CalculateWorkingDays((DateTime)o[tbl.Columns.IndexOf(coField.SecondColum)], (DateTime)o[tbl.Columns.IndexOf(coField.FirstColum)], vacationSet);
                            maxValue(ref TempValue, ref MaxValue, ref maxId, ref IDstr);
                            minValue(ref TempValue, ref MinValue, ref minId, ref IDstr);
                            double weight =  Convert.ToDouble(coField.Weight);
                       //     Console.WriteLine(weight);
                            if (TempValue >weight)
                            {
                                overcount++;
                            }
                            sum += TempValue;
                        }
                    }
                }
                coField.MaxValue = MaxValue.ToString();
                coField.MinValue = MinValue.ToString();
                coField.MaxValueID = maxId.ToString();
                coField.MinValueID = minId.ToString();
                coField.OverValue = ((Double)((Double)overcount / (Double)count)).ToString();
                coField.AverageValue = (sum / count).ToString();  
            }
        }
        double maxValue(ref double effectValue, ref double max, ref string maxId, ref string sObjectID)
        {
            if (effectValue > max)
            {
                max = effectValue;
                maxId = sObjectID.Trim();

            }
            return max;
        }
        double minValue(ref double effectValue, ref double min, ref string minId, ref string sObjectID)
        {
            if (effectValue <= min)
            {
                min = effectValue;
                minId = sObjectID.Trim();

            }
            return min;
        }
    
    }
}
