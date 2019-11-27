using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YfCloud.Models;
using YfCloud.Extentions;
using YfCloud.Framework;

namespace YfCloud.App.Module.FileUpload
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMvc(options =>
            {
                //options.Filters.Add<YFAuthorizationFilterAttribute>();
            });

            //配置Swagger参数信息
            services.AddSwaggerGen(sg =>
            {
                sg.SwaggerDoc("FileUploadModule", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "文件上传模块API",
                    Version = "2.0.0.1"
                });
                //当前项目的xml注释文件
                var currProjectXmlFile = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                sg.IncludeXmlComments(currProjectXmlFile);

                //YfCloud.Models
                var modelXmlFile = Path.Combine(AppContext.BaseDirectory, "YfCloud.Models.xml");
                sg.IncludeXmlComments(modelXmlFile);
            });

            //权限过滤器 读取当前模块的密钥
            TokenManager.SecurityKey = Configuration.GetSection("SecurityKey").Value.ToString();

            //配置系统参数信息
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));
                        
            //注册当前程序集所有标记了UseDI的服务
            services.AutoRegisterServicesFromAssembly(Assembly.GetExecutingAssembly().GetName().Name);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger().UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/FileUploadModule/swagger.json", "FileUploadModule 2.0.0.1");
                s.RoutePrefix = string.Empty;

            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
