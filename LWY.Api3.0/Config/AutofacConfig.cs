using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

 namespace LWY.Api3._0
{

    //public class AutofacConfig : Autofac.Module
    //{       //重写Autofac管道中的Load方法，在这里注入注册的内容
    //    protected override void Load(ContainerBuilder builder)
    //    {
    //        #region  示例
    //        //注册当前程序集中以“Ser”结尾的类,暴漏类实现的所有接口，生命周期为PerLifetimeScope
    //        builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Server")).AsImplementedInterfaces().InstancePerLifetimeScope();
    //        //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
    //        //注册所有"MyApp.Repository"程序集中的类
    //        //builder.RegisterAssemblyTypes(GetAssembly("MyApp.Repository")).AsImplementedInterfaces();
    //        #endregion 
    //    }

    //    public static Assembly GetAssembly(string assemblyName)
    //    {
    //        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(AppContext.BaseDirectory + $"{assemblyName}.dll");
    //        return assembly;
    //    }
    //}


    //注册服务独立出来，新增一个类，继承 Autofac.Module
    //注册服务独立出来，新增一个类，继承 Autofac.Module


    public class CustomAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册服务
            // builder.RegisterType(typeof(DemoServer)).As<DemoAIServer>().SingleInstance();//单例
            //builder.RegisterType(typeof(Service2)).As<Interface2>().InstancePerLifetimeScope();//线程独立
            //builder.RegisterType(typeof(Service3)).As<Interface3>().InstancePerDependency();//瞬时，为每个依赖或者调用(Resolve())都创建一个新的对象
            // Server 是 实现类 的后缀规则
            //var assembly = builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Server"));
            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Server"))
            //    .AsImplementedInterfaces()
            //    .InstancePerLifetimeScope()
            //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);


            //注册其他仓库  这里被坑过，core这样取所有的程序集
            IEnumerable<Assembly> allAssemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load);

            //Assembly[] assemblies = allAssemblies.Where(m =>
            //          m.FullName.Contains(".ApplicationService") ||
            //          m.FullName.Contains(".Infrastructure.Repository"))
            //    .ToArray();

            // var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(e => e.FullName.Contains("LWY")).ToList();
            //var s = builder.RegisterAssemblyTypes(assemblys.ToArray())
            //    .Where(t => t.Name.EndsWith("Service"));
            builder.RegisterAssemblyTypes(allAssemblies.ToArray())
                .Where(t => t.Name.EndsWith("ServiceImpl"))//实现类的后缀名
                //.Where(t => t.Name.EndsWith("ServiceImpl"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);




        }
    }

}
