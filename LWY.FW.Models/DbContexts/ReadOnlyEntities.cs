using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWY.FW.Models
{
    /// <summary>
    /// 只读的对象
    /// </summary>
    public class ReadOnlyEntities : TemplateDBContext
    {
        public ReadOnlyEntities() : base("ReadOnlyEntities")
        {
            // 关闭Tracking Change
            //this.Configuration.AutoDetectChangesEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ValidateOnSaveEnabled = false;
        }
        public override int SaveChanges()
        {
            throw new Exception("不能在只读库上执行保存操作！");
        }
    }
}
