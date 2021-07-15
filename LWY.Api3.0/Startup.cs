using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using LWY.Service;
using Autofac.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace LWY.Api3._0
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //获取配置文件数据  初始化
            new Api.ConfigHelper().Init(configuration);
        }

        public IConfiguration Configuration { get; }

        //  // This method gets called by the runtime. Use this method to add services to the container.
        //public IServiceProvider ConfigureServices(IServiceCollection services)
        //{
        //    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddControllersAsServices();

        //    // 添加 Autofac
        //    var containerBuilder = new ContainerBuilder();

        //    containerBuilder.Populate(services);

        //   containerBuilder.RegisterModule<DefaultModule>(); 
        //    var container = containerBuilder.Build();

        //    return new AutofacServiceProvider(container);
        //}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //#region 内置IOC   构造函数注入
            ////todo  从程序及中将所有的都找出来；自动注册
            //// 
            //// 注入生命周期为单例的服务
            ////Singleton生命能够周期服务在第一被请求时创建，在后续的每个请求都会使用同一个实例。如果你的应用需要单例服务，推荐的做法是交给服务容器来负责单例的创建和生命周期管理，而不是自己来走这些事情。
            //services.AddSingleton<IExampleService, ExampleServiceImpl>();
            //////注入生命周期为Scoped 的服务
            //////Scoped生命周期的服务是每次web请求被创建,局部单例对象, 在某个局部内是同一个对象(作用域单例,本质是容器单例);一次请求内是一个单例对象，多次请求则多个不同的单例对象.
            ////services.AddScoped<IExampleService, ExampleServiceImpl>();
            //////注入生命周期为瞬时的服务 
            //////瞬时生命周期, Transient服务在每次被请求时都会被创建一个新的对象。这种生命周期比较适用于轻量级的无状态服务。
            ////services.AddTransient<IExampleService, ExampleServiceImpl>();

            //#endregion

            #region  Swagger
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                // API  Group
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "This is My API",
                    TermsOfService = new Uri("https://localhost:44300/weatherforecast"),//系统地址
                    Contact = new OpenApiContact
                    {
                        Name = "名称",
                        Email = string.Empty,
                        Url = new Uri("https://localhost:44300/weatherforecast")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "许可证接口",
                        Url = new Uri("https://localhost:44300/weatherforecast")
                    }


                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "My API 2 ",
                    Version = "v2",
                    Description = "This is My API 2 ",
                    TermsOfService = new Uri("https://localhost:44300/weatherforecast"),//系统地址
                    Contact = new OpenApiContact
                    {
                        Name = "名称",
                        Email = string.Empty,
                        Url = new Uri("https://localhost:44300/weatherforecast")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "许可证接口",
                        Url = new Uri("https://localhost:44300/weatherforecast")
                    }


                });

                #region 这里还可以设置授权  授权暂不处理
                ////添加授权
                //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Description = "请输入带有Bearer开头的Token",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey
                //});
                //options.OperationFilter<AddResponseHeadersFilter>();
                //options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //options.OperationFilter<SecurityRequirementsOperationFilter>();
                //////认证方式，此方式为全局添加
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{


                //});
                ////options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                //// {
                ////                     { "Bearer", Enumerable.Empty<string>() }
                ////                 });
                #endregion
                // 获取xml文件名
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // 获取xml文件路径
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //var xmlmodelPath = Path.Combine(AppContext.BaseDirectory, "WebUserAPIMdoel.xml");//todo  添加model注释

                // 添加控制器层注释，true表示显示控制器注释
                //options.IncludeXmlComments(xmlmodelPath);
                options.IncludeXmlComments(xmlPath, true);
            });

            //最坑的就是这个，忘记加了，导致一直报错
            services.AddSwaggerGen(sw =>
            {
                sw.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            #endregion

            #region  全局异常过滤器
            services.AddMvc(opt =>
            {
                opt.Filters.Add<AuthorizationFilter>();

                opt.Filters.Add<TimingFilter>();
                opt.Filters.Add<ExceptionFilter>();
            });

            #endregion


            #region 版本控制

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                //个人最喜欢的用头部标识版本号的
                o.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
            #endregion

            #region  接口加缓存 缓存配置 但未生效，仍需配置
            services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("default", new CacheProfile
                {
                    Duration = 60
                });

                options.CacheProfiles.Add("test", new CacheProfile
                {
                    Duration = 30,
                    Location = ResponseCacheLocation.Client
                });
            });

            #endregion 
        }

        /// <summary>
        /// 单独注册  Autofac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CustomAutofacModule());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 添加Swagger有关中间件
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //api group 
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Examples API Demo  v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Examples API Demo  v2");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });


            app.UseHttpsRedirection();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
