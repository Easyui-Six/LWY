WebAPi
Do:
1.路由规则，已实现，需要扩展测试
2.api 统一出入参处理，已实现，待测试
3.过滤器实现和全局配置；待测试；自定义aop带扩展实现；
4. Autofac  属性注入为主，内置索引注入 （不暴露接口），构造函数注入（一般不用）；unity参照weikang
5.Swagger 包含注解和历史版本
6.版本控制
7.linqTree;
8.接口缓存  CacheOutput 引用 WebApi.OutputCache.V2（todo  postman 测试不出来，待其他验证） 
9. 设计模式 详见 DesignPatternDemo
10.反射实现

todo：
11.反射，线程，委托传方法  ,重写其他方法，比如linq的ordeyby  thenby

ORM：EF    sql MySql

Redis MQ MongoDB





Swagger:专题
1.添加NuGet包     Swagger.Net.UI；Swashbuckle；然后安装它Microsoft.AspNet.WebApi.HelpPage  再安装它WebApiTestClient     
（注意：要按顺序添加不然会报错 GetDocumentation 未被继承之类的，如果报错（在nuget命令行输入  install-package Microsoft.AspNet.WebApi.HelpPage  ）

2.在 SwaggerConfig 文件中添加类   
    private static string GetXmlCommentsPath()
        {
            return string.Format("{0}/bin/***.****.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }

之后再Register中 EnableSwagger 里添加  c.IncludeXmlComments(GetXmlCommentsPath());
3.在该项目，右键属性，生成 时  需要输出  xml文档文件
4.api地址后加 /Swagger 即可展示

api的版本控制（MVC和Core一样的）：1.引用 Microsoft.AspNet.WebApi.Versioning  
2. 在webapi.config中添加配置   config.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
3.再请请求的 heards 中添加  api-Version  2.0(版本号) 即可 





