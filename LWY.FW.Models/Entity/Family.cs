using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWY.FW.Models.Entity
{
   // Code First默认约定将命名为Id或“类名+Id”的属性视为表的键。
    public class Family
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
