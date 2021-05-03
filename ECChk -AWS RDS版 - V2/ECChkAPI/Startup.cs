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
            //�s�W��Ʈw���s�u�A��
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            //�[�J�Ҧ��bECMappings�U��mapping 
            services.AddAutoMapper(typeof(ECMappings));

            ////Redis�s�u
            services.AddSingleton<IDbStorage, AWSRedisDb>
                (options => new AWSRedisDb(Configuration.GetConnectionString("RedisConnection")));

            services.AddScoped<ECChkRepository>();

            //�[�Jversioning���]�w
            services.AddApiVersioning(options =>
            {
                //�Y�O�S�����w �|���J�w�]������
                options.AssumeDefaultVersionWhenUnspecified = true;

                //�����|��ܦbResponse headers����api-supported-versions
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");//VVV�������W��

            //�[�Jswagger���]�w
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //�s�Wswagger���]�w,�|�W�[�Ѽ� IApiVersionDescriptionProvider provider
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

            //�[�Jswagger���]�w
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

            //// �]�w Serilog ���� Request ��������T
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
