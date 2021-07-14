using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWY.FW.Models
{
    public class UserViewModel
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string PassWord { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }

        public string Age { get; set; }
    }
}
