using System;
using System.Reflection;
using System.Linq;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

using LWY.FW.Models;
using LWY.FW.DAL;
using LWY.FW.Common;
using LWY.FW.BLL;

namespace LWY.WebApi.App_Start
{

    /// <summary>
    /// 1.Autofac 属性注入
    /// 2.内置索引注入
    /// </summary>
    public class DependencyConfig
    {
        //属性注入
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            // 获取api的控制器
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(LWY.WebApi.BaseWebApiController)))
                .PropertiesAutowired();
            //这里是处理mvc的控制器的  
            //builder.RegisterControllers(Assembly.GetExecutingAssembly())
            //    .PropertiesAutowired();
            //注册上下文
            builder.Register(f =>
            {
                var efDB = new TemplateDBContext();
                //efDB.Database.Log = s => log4net.LogManager.GetLogger("EFSql").Debug(s);
                return efDB;
            }).As<TemplateDBContext>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            builder.Register(f =>
            {
                var readDb = new ReadOnlyEntities();
                //readDb.Database.Log = s => log4net.LogManager.GetLogger("EFSql").Debug(s);
                return readDb;
            }).As<ReadOnlyEntities>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
            //注册其他仓库
            var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(e => e.FullName.Contains("LWY.FW")).ToList();
            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("ServiceImpl"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                //.EnableInterfaceInterceptors()
                //.EnableClassInterceptors()
                //.InterceptedBy(typeof(CacheInterceptor))
                //.InterceptedBy(typeof(LogInterceptor))
                ;
            //builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("JobImpl"))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
            //    //.EnableInterfaceInterceptors()
            //    //.EnableClassInterceptors()
            //    //.InterceptedBy(typeof(CacheInterceptor))
            //    ;
            //builder.Register(c => new CacheInterceptor())
            //    .PropertiesAutowired();
            //builder.Register(e => new LogInterceptor());
            //builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("RepositoryImpl"))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope()
            //    .PropertiesAutowired()
            //    //.EnableInterfaceInterceptors()
            //    //.EnableClassInterceptors()
            //    //.InterceptedBy(typeof(CacheInterceptor))
            //    //.InterceptedBy(typeof(LogInterceptor))
            //    ;
            builder.RegisterGeneric(typeof(BaseEFRepositoryImpl<>)).As(typeof(IRepository<>))
                .InstancePerLifetimeScope()
                .PropertiesAutowired()
                //.EnableInterfaceInterceptors()
                //.EnableClassInterceptors()
                //.InterceptedBy(typeof(CacheInterceptor))
                //.InterceptedBy(typeof(LogInterceptor))
                ;
            builder.RegisterGeneric(typeof(BaseEFReadOnlyRepositoryImpl<>)).As(typeof(IReadOnlyRepository<>))
                .InstancePerLifetimeScope()
                .PropertiesAutowired()
                //.EnableInterfaceInterceptors()
                //.EnableClassInterceptors()
                //.InterceptedBy(typeof(CacheInterceptor))
                //.InterceptedBy(typeof(LogInterceptor))
                ;

            //以上是属性注入，以下是根据关键字注入
            //索引
            builder.RegisterType<PeopleServiceImpl>()
.Keyed<IAnimalService>(AnimalEnum.People+"")
.InstancePerLifetimeScope()
.PropertiesAutowired();

            builder.RegisterType<DogServiceImpl>()
        .Keyed<IAnimalService>(AnimalEnum.Dog+"")
        .InstancePerLifetimeScope()
        .PropertiesAutowired();
            builder.RegisterType<CatServiceImpl>()
   .Keyed<IAnimalService>(AnimalEnum.Cat+"")
   .InstancePerLifetimeScope()
   .PropertiesAutowired();
            //Named 名字
//            builder.RegisterType<PeopleServiceImpl>()
//.Named<IAnimalActionService>(AnimalEnum.People + "")
//.InstancePerLifetimeScope()
//.PropertiesAutowired();
//            builder.RegisterType<DogServiceImpl>()
//     .Named<IAnimalActionService>(AnimalEnum.Dog + "")
//     .InstancePerLifetimeScope()
//     .PropertiesAutowired();
//            builder.RegisterType<CatServiceImpl>()
//.Named<IAnimalActionService>(AnimalEnum.Cat + "")
//.InstancePerLifetimeScope()
//.PropertiesAutowired();
            //



            //将注册好的东西加到condfig中
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));
            //// HangFire
            //Hangfire.GlobalConfiguration.Configuration.UseAutofacActivator(container);
        }

    }
}