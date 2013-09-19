using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Calc.aboutTime;
using System.Windows.Forms;
using Calc.aboutNode;
using Calc.columFiled;
using System.ComponentModel;

namespace Calc
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        #region ///variable
        configNode configer = null;
        Dictionary<String, String> dt = null;
        HashSet<KeyValuePair<string, string>> lt = null;
        HashSet<string> hs = null;
        string fileDir = null;
        string filename = null;
        string tableName = null;
        string vPath = null;
        string vTableName = null;
        private BackgroundWorker backgroundWorker;
#endregion
        public Page1()
        {
            InitializeComponent();
            ComboBoxIni();
            progressBar1.Visibility = Visibility.Hidden;
            backgroundWorker = ((BackgroundWorker)this.FindResource("backgroundWorker"));
        }
        private void ComboBoxIni()
        {
            hs = new HashSet<string>();
            configer = new configNode();
            dt = new Dictionary<string, string>();

            configer.startConfig(out lt);
            foreach (KeyValuePair<string, string> kv in lt)
            {

                hs.Add(kv.Key);
            }
            foreach (String str in hs)
            {
                ComboBoxItem cbitem = new ComboBoxItem();
                cbitem.Content = str;
                comboBox1.Items.Add(cbitem);
            }
        }
        #region ///redirect
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Page2 expenseReportPage = new Page2();
            this.NavigationService.Navigate(expenseReportPage);
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Page3 expenseReportPage = new Page3();
            this.NavigationService.Navigate(expenseReportPage);
        }

        #endregion
        private void button_Start_Click(object sender, RoutedEventArgs e)
        {
            if (textBox2.Text.ToString().Equals(""))
            {
                System.Windows.Forms.MessageBox.Show("请选择路径");
            }
            else
            {
                progressBar1.Visibility = Visibility.Visible;
                columnFiledMaker coMaker = new columnFiledMaker();
                backgroundWorker.RunWorkerAsync(coMaker);
            }
        }
        #region ///button
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                openFileDialog.DefaultExt = ".bdf";
                openFileDialog.Filter = "所有文件|*.*";

                filename = openFileDialog.FileName;
                textBox2.Text = filename;

                int i = filename.LastIndexOf(".dbf");
                if (i == -1)
                {
                    i = filename.LastIndexOf(".DBF");
                }
                fileDir = filename.Remove(i);
                int j = fileDir.LastIndexOf("\\");

                vPath = fileDir.Substring(0, fileDir.LastIndexOf("\\"));
                tableName = filename.Substring(j + 1, fileDir.Length - j - 1);
                vTableName = tableName;
            }
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            App.Current.Shutdown();
        }
        #endregion
        #region  backgroundWork

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            columnFiledMaker coMaker = (columnFiledMaker)e.Argument;
            coMaker.makeColumList();
            backgroundWorker.ReportProgress(50);
            coMaker.makeResult(ref vPath, ref vTableName);
            backgroundWorker.ReportProgress(100);
            foreach (columField coField in coMaker.list)
            {
                Console.WriteLine(coField.Department + "#" + coField.Kind + "#" + coField.MaxValue + "#" + coField.MinValue + "#" + coField.OverValue + "#" + coField.AverageValue);
            }
        }
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = System.Windows.Input.Cursors.Arrow;
            progressBar1.Visibility = Visibility.Hidden;
            backgroundWorker.CancelAsync();
            GC.Collect();
        }
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }
        #endregion


    }
}
