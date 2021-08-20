using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MS.Component.Aop;
using MS.Component.Jwt;
using MS.DbContexts;
using MS.Models.Automapper;
using MS.Services;
using MS.UnitOfWork;
using MS.WebApi.Filters;
using MS.WebCore;
using MS.WebCore.MultiLanguages;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using static MS.Common.Extensions.TimeExtension;

namespace MS.WebApi
{
    public class Startup
    {
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}
        public ILifetimeScope AutofacContainer { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            // In ASP.NET Core 3.0 `env` will be an IWebHostingEnvironment, not IHostingEnvironment.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        //添加autofac的DI配置容器
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //注册IBaseService和IRoleService接口及对应的实现类
            //builder.RegisterType<BaseService>().As<IBaseService>().InstancePerLifetimeScope();
            //builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();

            //注册aop拦截器 
            //将业务层程序集名称传了进去，给业务层接口和实现做了注册，也给业务层各方法开启了代理
            builder.AddAopService(ServiceExtensions.GetAssemblyName());
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //添加多语言本地化支持
            services.AddMultiLanguages();

            services
                .AddControllers(options =>
                {
                    options.Filters.Add<ApiResponseFilter>();
                    options.Filters.Add<ApiExceptionFilter>();
                })
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedResource));//给注解添加本地化资源提供器Localizerprovider
                })
                .AddJsonOptions(config =>
                {
                    //此设定解决JsonResult中文被编码的问题
                    config.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                    // 获取或设置DateTime格式
                    config.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    // 获取或设置DateTime?格式
                    config.JsonSerializerOptions.Converters.Add(new DateTimeNullableConvert());
                });
            #region newtonsoft
            //.AddNewtonsoftJson(options =>
            //{
            //    //序列化时忽略null值字段
            //    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            //    ////修改属性名称的序列化方式，首字母小写
            //    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();

            //    //修改时间的序列化方式
            //    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            //});
            #endregion

            //注册跨域策略
            services.AddCorsPolicy(Configuration);
            //注册webcore服务（网站主要配置）
            services.AddWebCoreService(Configuration);

            //注册工作单元（同时注册了DBContext）
            services.AddUnitOfWorkService<MSDbContext>(options => { options.UseMySql(Configuration.GetSection("ConectionStrings:MSDbContext").Value); });

            //注册automapper服务
            services.AddAutomapperService();

            //注册jwt服务
            services.AddJwtService(Configuration);

            //注册swagger服务
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MS.WebApi", Version = "v1" });
                //开启权限小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传递)直接在下面框中输入Bearer {token}(注意两者之间是一个空格) \"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MS.WebApi v1");
                    c.DocExpansion(DocExpansion.None); //->修改界面打开时自动折叠
                    c.DefaultModelsExpandDepth(-1); //->设置为-1 可不显示models
                });
                //app.DocExpansion(DocExpansion.None); //->修改界面打开时自动折叠
            }

            app.UseMultiLanguage(Configuration);//添加多语言本地化支持

            app.UseRouting();

            app.UseCors(WebCoreExtensions.MyAllowSpecificOrigins);//添加跨域

            app.UseAuthentication();//添加认证中间件

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute(
                //    name: "default",
                //    pattern: "/mp/v1_0/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
