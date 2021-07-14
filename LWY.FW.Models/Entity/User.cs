using System;
using System.Collections.Generic;
using System.Text;

namespace LWY.FW.Models.Entity
{
    public class User
    {
        public int ID { get; set; }

        public string Code { get; set; }

        public string PassWord { get; set; }
        public string Name { get; set; }
        public bool Sex { get; set; }

        public int Age { get; set; }

        /// <summary>
        /// 图像
        /// </summary>
        public string Image { get; set; }
    }
}
