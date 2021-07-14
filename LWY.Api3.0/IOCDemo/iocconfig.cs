using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
//using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;

// 属性注入ioc region
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using ClassLibrary2;
using WebApplication1.Controllers;

//构造器注入




namespace WebApplication1.LWY.Api3._0
{
    public class iocconfig
    {
        public static void Region()
        {

            #region   常见ioc异常信息
            // 将 WebApiConfig.Register(GlobalConfiguration.Configuration);换成GlobalConfiguration.Configure(WebApiConfig.Register);
            #endregion


            #region AutoFac依赖注入 服务注册
            // 创建一个容器
            var builder = new ContainerBuilder();

            //注册自定义服务   class1 实现类  Interface1接口类
            builder.RegisterType<TestImpl>().As<TestInterfaceImpl>().InstancePerDependency();
            //builder.RegisterType<MyTestBusiness>().As<MyTestBusiness>().InstancePerDependency();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            #endregion
        }

        /// <summary>
        ///  构造器注入
        /// </summary>
        public static void Region2()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(WebApiApplication).Assembly)
                .PropertiesAutowired();//注册mvc容器的实现
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly).PropertiesAutowired();//注册api mvc容器的实现

            //如果有web类型，请使用如下获取Assenbly方法(获取所有需要用到的程序集，放到list中)

            //用GetReferencedAssemblies方法获取当前应用程序下所有的程序集
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            //
            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("Impl"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                ;

            #region   策略模式
            builder.RegisterType<ClassLibrary2.people>()
          .Keyed<ClassLibrary2.AnimalInterface>(ClassLibrary2.Class2.people)
          .InstancePerLifetimeScope()
          .PropertiesAutowired();

            builder.RegisterType<ClassLibrary2.Dog>()
        .Keyed<ClassLibrary2.AnimalInterface>(ClassLibrary2.Class2.Dog)
        .InstancePerLifetimeScope()
        .PropertiesAutowired();

            #endregion 




            var container = builder.Build(); //Build()方法是表示：创建一个容器
            //config.DependencyResolver = new AutofacDependencyResolver(container);//注册api容器需要使用HttpConfiguration对象
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); // 注册依赖关系

            GlobalConfiguration.Configuration.DependencyResolver =
new AutofacWebApiDependencyResolver(container); //  如果不加此句，会报错  err:  没有参数的 构造函数

        }

        public static void Region3(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(ApiController)))
        .PropertiesAutowired();
            builder.RegisterControllers(Assembly.GetExecutingAssembly())
                .PropertiesAutowired();

            builder.RegisterControllers(System.Reflection.Assembly.GetExecutingAssembly());//注册mvc容器的实现


            //如果有web类型，请使用如下获取Assenbly方法(获取所有需要用到的程序集，放到list中)

            //用GetReferencedAssemblies方法获取当前应用程序下所有的程序集
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            //
            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("Impl"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                //.EnableInterfaceInterceptors()
                //.EnableClassInterceptors()
                //.InterceptedBy(typeof(CacheInterceptor))
                //.InterceptedBy(typeof(LogInterceptor))
                ;
            //var container = builder.Build(); //Build()方法是表示：创建一个容器
            ////config.DependencyResolver = new AutofacDependencyResolver(container);//注册api容器需要使用HttpConfiguration对象
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));

        }

        /// <summary>
        /// 属性注入
        /// </summary>
        public static void Region4()
        {
            // 创建一个容器
            var builder = new ContainerBuilder();
            //用GetReferencedAssemblies方法获取当前应用程序下所有的程序集
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();
            builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Name.EndsWith("Impl"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                   ;
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly, Assembly.GetAssembly(typeof(ApiController)))
            .PropertiesAutowired();//注册api mvc容器的实现
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces().PropertiesAutowired();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver =
new AutofacWebApiDependencyResolver(container);
        }
    }


}



