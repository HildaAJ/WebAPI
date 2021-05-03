using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using ECChkAPI.Data;
using ECChkAPI.Repository;
using ECChkAPI.Models.Mapper;
using ECChkAPI.Interface;
using Serilog;
using Serilog.Events;

namespace ECChkAPI
{
    public class Startup

    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //新增資料庫的連線服務
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            //加入所有在ECMappings下的mapping 
            services.AddAutoMapper(typeof(ECMappings));

            ////Redis連線
            services.AddSingleton<IDbStorage, AWSRedisDb>
                (options => new AWSRedisDb(Configuration.GetConnectionString("RedisConnection")));

            services.AddScoped<ECChkRepository>();

            //加入versioning的設定
            services.AddApiVersioning(options =>
            {
                //若是沒有指定 會載入預設的版本
                options.AssumeDefaultVersionWhenUnspecified = true;

                //執行後會顯示在Response headers中的api-supported-versions
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");//VVV為版本名稱

            //加入swagger的設定
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //新增swagger的設定,會增加參數 IApiVersionDescriptionProvider provider
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //app.UseExceptionHandler("/Error");
            //app.UseXRay("SECchk");
            //app.UseStaticFiles();

            //加入swagger的設定
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json",
                        desc.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = "";
            });

            app.UseRouting();

            app.UseAuthorization();

            //// 設定 Serilog 紀錄 Request 的相關資訊
            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "Handled {RequestPath}";
                options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug;
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.Run(context =>
            //{
            //    return context.Response.WriteAsync("Hello from ASP.NET Core!");
            //});

        }
    }
}
