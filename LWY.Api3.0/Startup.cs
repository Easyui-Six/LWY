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

            //��ȡ�����ļ�����  ��ʼ��
            new Api.ConfigHelper().Init(configuration);
        }

        public IConfiguration Configuration { get; }

        //  // This method gets called by the runtime. Use this method to add services to the container.
        //public IServiceProvider ConfigureServices(IServiceCollection services)
        //{
        //    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddControllersAsServices();

        //    // ��� Autofac
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

            //#region ����IOC   ���캯��ע��
            ////todo  �ӳ����н����еĶ��ҳ������Զ�ע��
            //// 
            //// ע����������Ϊ�����ķ���
            ////Singleton�����ܹ����ڷ����ڵ�һ������ʱ�������ں�����ÿ�����󶼻�ʹ��ͬһ��ʵ����������Ӧ����Ҫ���������Ƽ��������ǽ��������������������Ĵ������������ڹ����������Լ�������Щ���顣
            //services.AddSingleton<IExampleService, ExampleServiceImpl>();
            //////ע����������ΪScoped �ķ���
            //////Scoped�������ڵķ�����ÿ��web���󱻴���,�ֲ���������, ��ĳ���ֲ�����ͬһ������(��������,��������������);һ����������һ���������󣬶������������ͬ�ĵ�������.
            ////services.AddScoped<IExampleService, ExampleServiceImpl>();
            //////ע����������Ϊ˲ʱ�ķ��� 
            //////˲ʱ��������, Transient������ÿ�α�����ʱ���ᱻ����һ���µĶ��������������ڱȽ�����������������״̬����
            ////services.AddTransient<IExampleService, ExampleServiceImpl>();

            //#endregion

            #region  Swagger
            //ע��Swagger������������һ���Ͷ��Swagger �ĵ�
            services.AddSwaggerGen(options =>
            {
                // API  Group
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "This is My API",
                    TermsOfService = new Uri("https://localhost:44300/weatherforecast"),//ϵͳ��ַ
                    Contact = new OpenApiContact
                    {
                        Name = "����",
                        Email = string.Empty,
                        Url = new Uri("https://localhost:44300/weatherforecast")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "���֤�ӿ�",
                        Url = new Uri("https://localhost:44300/weatherforecast")
                    }


                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "My API 2 ",
                    Version = "v2",
                    Description = "This is My API 2 ",
                    TermsOfService = new Uri("https://localhost:44300/weatherforecast"),//ϵͳ��ַ
                    Contact = new OpenApiContact
                    {
                        Name = "����",
                        Email = string.Empty,
                        Url = new Uri("https://localhost:44300/weatherforecast")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "���֤�ӿ�",
                        Url = new Uri("https://localhost:44300/weatherforecast")
                    }


                });

                #region ���ﻹ����������Ȩ  ��Ȩ�ݲ�����
                ////�����Ȩ
                //options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Description = "���������Bearer��ͷ��Token",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey
                //});
                //options.OperationFilter<AddResponseHeadersFilter>();
                //options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                //options.OperationFilter<SecurityRequirementsOperationFilter>();
                //////��֤��ʽ���˷�ʽΪȫ�����
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                //{


                //});
                ////options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                //// {
                ////                     { "Bearer", Enumerable.Empty<string>() }
                ////                 });
                #endregion
                // ��ȡxml�ļ���
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // ��ȡxml�ļ�·��
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //var xmlmodelPath = Path.Combine(AppContext.BaseDirectory, "WebUserAPIMdoel.xml");//todo  ���modelע��

                // ��ӿ�������ע�ͣ�true��ʾ��ʾ������ע��
                //options.IncludeXmlComments(xmlmodelPath);
                options.IncludeXmlComments(xmlPath, true);
            });

            //��ӵľ�����������Ǽ��ˣ�����һֱ����
            services.AddSwaggerGen(sw =>
            {
                sw.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            #endregion

            #region  ȫ���쳣������
            services.AddMvc(opt =>
            {
                opt.Filters.Add<AuthorizationFilter>();

                opt.Filters.Add<TimingFilter>();
                opt.Filters.Add<ExceptionFilter>();
            });

            #endregion


            #region �汾����

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                //������ϲ������ͷ����ʶ�汾�ŵ�
                o.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
            #endregion

            #region  �ӿڼӻ��� �������� ��δ��Ч����������
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
        /// ����ע��  Autofac
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

            // ���Swagger�й��м��
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
