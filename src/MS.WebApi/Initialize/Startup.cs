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
        //���autofac��DI��������
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //ע��IBaseService��IRoleService�ӿڼ���Ӧ��ʵ����
            //builder.RegisterType<BaseService>().As<IBaseService>().InstancePerLifetimeScope();
            //builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();

            //ע��aop������ 
            //��ҵ���������ƴ��˽�ȥ����ҵ���ӿں�ʵ������ע�ᣬҲ��ҵ�������������˴���
            builder.AddAopService(ServiceExtensions.GetAssemblyName());
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //��Ӷ����Ա��ػ�֧��
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
                    factory.Create(typeof(SharedResource));//��ע����ӱ��ػ���Դ�ṩ��Localizerprovider
                })
                .AddJsonOptions(config =>
                {
                    //���趨���JsonResult���ı����������
                    config.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                    // ��ȡ������DateTime��ʽ
                    config.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                    // ��ȡ������DateTime?��ʽ
                    config.JsonSerializerOptions.Converters.Add(new DateTimeNullableConvert());
                });
            #region newtonsoft
            //.AddNewtonsoftJson(options =>
            //{
            //    //���л�ʱ����nullֵ�ֶ�
            //    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

            //    ////�޸��������Ƶ����л���ʽ������ĸСд
            //    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();

            //    //�޸�ʱ������л���ʽ
            //    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
            //});
            #endregion

            //ע��������
            services.AddCorsPolicy(Configuration);
            //ע��webcore������վ��Ҫ���ã�
            services.AddWebCoreService(Configuration);

            //ע�Ṥ����Ԫ��ͬʱע����DBContext��
            services.AddUnitOfWorkService<MSDbContext>(options => { options.UseMySql(Configuration.GetSection("ConectionStrings:MSDbContext").Value); });

            //ע��automapper����
            services.AddAutomapperService();

            //ע��jwt����
            services.AddJwtService(Configuration);

            //ע��swagger����
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MS.WebApi", Version = "v1" });
                //����Ȩ��С��
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                //��header�����token�����ݵ���̨
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���)ֱ���������������Bearer {token}(ע������֮����һ���ո�) \"",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
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
                    c.DocExpansion(DocExpansion.None); //->�޸Ľ����ʱ�Զ��۵�
                    c.DefaultModelsExpandDepth(-1); //->����Ϊ-1 �ɲ���ʾmodels
                });
                //app.DocExpansion(DocExpansion.None); //->�޸Ľ����ʱ�Զ��۵�
            }

            app.UseMultiLanguage(Configuration);//��Ӷ����Ա��ػ�֧��

            app.UseRouting();

            app.UseCors(WebCoreExtensions.MyAllowSpecificOrigins);//��ӿ���

            app.UseAuthentication();//�����֤�м��

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
