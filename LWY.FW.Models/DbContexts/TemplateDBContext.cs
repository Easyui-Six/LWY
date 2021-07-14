using System.Data.Entity;

namespace LWY.FW.Models
{
    public class TemplateDBContext : DbContext
    {
        //static TemplateDBContext()
        //{
        //    //初始化数据库
        //    //Database.SetInitializer<TemplateDBContext>(null);
        //    // 启用自动迁移（应用启动时自动执行迁移脚本）
        //    //Database.SetInitializer<eXiuMySqlEntities>(new MigrateDatabaseToLatestVersion<eXiuMySqlEntities, Configuration>());
        //}

        public TemplateDBContext()
            : base("Name=templateConnectStr")
        {
        }

        public TemplateDBContext(string name)
           : base("Name=" + name)
        {
        }


        public virtual DbSet<Entity.User> User { get; set; }


        public virtual DbSet<Entity.Family> Family { get; set; }
        
    }
}
