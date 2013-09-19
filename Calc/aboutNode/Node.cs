using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Calc.aboutNode
{
     class PropertyNodeItem : TreeViewItem
    {
        public int Level { get; set; }
        public PropertyNodeItem(string name, string DisplayName, int level)
        {
            this.Level = level;
            this.Header = DisplayName;
            this.Name = name;
            // 在此点下面插入创建对象所需的代码。
        }
       
    }
}
