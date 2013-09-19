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
using Calc.aboutNode;
using System.Collections;
namespace Calc
{
    /// <summary>
    /// Interaction logic for Page3.xaml
    /// </summary>
    public partial class Page3 : Page
    {
        configNode configer = null;
        Dictionary<String, String> dt = null;
        HashSet<KeyValuePair<string, string>> lt = null;
        HashSet<string> hs = null;
        public Page3()
        {
            InitializeComponent();
            hs = new HashSet<string>();
            configer = new configNode();
            dt = new Dictionary<string, string>();
            configer.startConfig(out lt);
            startComobox();
        }
        private void startComobox()
        {
            foreach (KeyValuePair<string, string> kv in lt)
            {
                hs.Add(kv.Key);
            }
            ComboBoxItem itemAll = new ComboBoxItem();
            itemAll.Content = "所有";
            comboBox1.Items.Add(itemAll);
            comboBox1.SelectedItem = itemAll;
            foreach (String s in hs)
            {
                ComboBoxItem comItem = new ComboBoxItem();
                comItem.Content = s;
                comboBox1.Items.Add(s);
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            IEnumerator enumerator = listView1.SelectedItems.GetEnumerator();
            List<object> l = new List<object>();
            while (enumerator.MoveNext())
            {
                l.Add(enumerator.Current);
            }
            foreach (Object o in l)
            {
                try
                {
                    string s = o.ToString().Trim();
                    s = s.Replace('[', ' '); s = s.Replace(']', ' ');
                    string[] spStr = s.Trim().Split(',');
                    KeyValuePair<string, string> kp = new KeyValuePair<string, string>(spStr[0].Trim(), spStr[1].Trim());
                    if (lt.Contains(kp))
                    {
                        lt.Remove(kp);
                        freshUI();
                        configer.updateConfig(ref lt);
                    }
                    else
                    {
                        MessageBox.Show("没有信息删除");
                    }
                }
                catch (IndexOutOfRangeException ex)
                {
                    MessageBox.Show("输入错误");
                    Console.WriteLine(ex);
                }
            }

            listView1.SelectedIndex = listView1.Items.Count - 1;
            listView1.ScrollIntoView(listView1.SelectedItem);

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                textBox1.Text = textBox1.Text.Trim();
                string[] spStr = textBox1.Text.ToString().Split(',');
                KeyValuePair<string, string> kp = new KeyValuePair<string, string>(spStr[0], spStr[1]);
                if (!hs.Contains(spStr[0]))
                {
                    hs.Add(spStr[0]);
                    comboBox1.Items.Add(spStr[0]);
                }
                if (lt.Contains(kp))
                {
                    MessageBox.Show("重复信息");
                }
                else
                {
                    lt.Add(kp);
                    configer.updateConfig(ref lt);
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("输入错误");
                Console.WriteLine(ex);
            }
            freshUI();
            listView1.SelectedIndex = listView1.Items.Count - 1;
            listView1.ScrollIntoView(listView1.SelectedItem);

        }
        public void freshUI()
        {
            string str = comboBox1.SelectedItem.ToString();
            List<KeyValuePair<string, string>> newlt = new List<KeyValuePair<string, string>>();
            if (str.Equals("System.Windows.Controls.ComboBoxItem: 所有"))
            {
                foreach (KeyValuePair<string, string> kv in lt)
                {
                    newlt.Add(kv);
                }
                listView1.ItemsSource = newlt;
            }
            else
            {
                foreach (KeyValuePair<string, string> kv in lt)
                {
                    if (kv.Key.ToString() == comboBox1.SelectedItem.ToString())
                    {
                        newlt.Add(kv);
                    }
                }
                listView1.ItemsSource = newlt;
            }
        }
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            freshUI();
        }
    }

}