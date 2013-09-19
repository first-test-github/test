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
namespace Calc
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        private configTime configer = null;
        public static HashSet<DateTime> vacationSet = null;
        public Page2()
        {
            InitializeComponent();
            vacationSet = new HashSet<DateTime>();
            configer = new configTime();
            configer.startConfig(ref vacationSet);
            foreach (var dt in vacationSet)
            {
                listBox1.Items.Add(dt);
            }
        }
        #region ///calendar
        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            vacationSet.Add(new DateTime(calendar.SelectedDate.Value.Year, calendar.SelectedDate.Value.Month, calendar.SelectedDate.Value.Day));
            listBox1.Items.Clear();
            foreach (var dt in vacationSet)
            {
                listBox1.Items.Add(dt);
            }
            listBox1.ScrollIntoView(this.listBox1.Items[this.listBox1.Items.Count - 1]);

        }
        private void button_ChooseAll_Click(object sender, RoutedEventArgs e)
        {
            vacation vac = new vacation();
            vac.getVacationsList(ref vacationSet, DateTime.Today);
            configer.updateConfig(ref vacationSet);
            listBox1.Items.Clear();
            foreach (var dt in vacationSet)
            {
                listBox1.Items.Add(dt);
            }

            listBox1.ScrollIntoView(this.listBox1.Items[this.listBox1.Items.Count - 1]);
        }
        private void button_Clear_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.Clear();
            vacationSet.Clear();
            configer.updateConfig(ref vacationSet);
        }
        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            vacationSet.Remove((DateTime)listBox1.SelectedItem);
            int index = this.listBox1.SelectedIndex;
            listBox1.Items.RemoveAt(index);

        }
        #endregion

    }
}
