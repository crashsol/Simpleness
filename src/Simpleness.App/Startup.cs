using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Simpleness.DataEntityFramework;
using Simpleness.DataEntityFramework.Entity;
using Simpleness.Infrastructure.AspNetCore.Filters;
using Simpleness.Infrastructure.AspNetCore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace Simpleness.App
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

            services.AddDbContext<SimplenessDbContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //identity setting
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<SimplenessDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequireUppercase = false;
            });



            #region JWT配置

            //获取JWT配置
            var jwtSetting = new JwtSettings();
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSetting"));
            Configuration.GetSection("JwtSetting").Bind(jwtSetting);

            //添加Jwt验证
            services.AddAuthentication(option =>
            {
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),   //认证秘钥
                    ValidateIssuer = true,
                    ValidIssuer = jwtSetting.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSetting.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(10),
                };
            });


            #endregion


            services.AddMvc(option =>
            {
                //禁止合并AuthorizeFilter
                option.AllowCombiningAuthorizeFilters = false;
                //Exception Filter
                option.Filters.Add(typeof(GlobalExceptionFilter));
                //validate Filter
                option.Filters.Add(typeof(ValidateModelFilter));

            }).AddJsonOptions(option =>
            {
                //对数据进行转化出错时，抛出异常
                option.AllowInputFormatterExceptionMessages = true;
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Swashbuckle Api文档配置
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Simpleness",
                    Description = "学习使用AspNetCore,Vue"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPash = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPash);

                //添加Token
                option.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"


                });

            });
            #endregion


            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

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
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseCors(option =>
            {
                option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
            });

            //swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simpleness Api");
            });
            //认证
            app.UseAuthentication();


            //静态文件
            app.UseStaticFiles(); 
            app.UseSpaStaticFiles();
            //mvc管道
            app.UseMvc();
            //单页面
            app.UseSpa(spa =>
            {       
                spa.Options.SourcePath = "ClientApp";        
                if(env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:9528");
                }
            });
        }
    }
}
