using System.Reflection;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
using System.IO;
using Simpleness.Core.User;
using Simpleness.Core.Role;
using AutoMapper;
using Simpleness.Core.Department;
using Simpleness.Utility.CommonDto;
using System.Linq;
using Simpleness.App.Controllers;
using Simpleness.Infrastructure.AspNetCore.Authorize;
using Microsoft.AspNetCore.Authorization;
using Simpleness.Core;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using AspNetCore.WeixinOAuth;
using Simpleness.Infrastructure.AspNetCore.Middlewares;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

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
                //option.UseSqlite("Data Source = Simpleness.db");
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            //identity setting
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<SimplenessDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(option =>
            {
                option.SignIn.RequireConfirmedEmail = true;
                option.Password.RequireUppercase = false;

            });

            #region JWT配置

            //获取JWT配置
            var jwtSetting = new JwtSettings();
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSetting"));
            Configuration.GetSection("JwtSetting").Bind(jwtSetting);

            //清除默认的JwtToken默认的绑定
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            //添加Jwt验证
            services.AddAuthentication(option =>
            {
                option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
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


            //services.AddAuthentication().AddWeixinOAuth(option =>
            //{

            //    option.AppId = Configuration["WeixinAuth:AppId"];
            //    option.AppSecret = Configuration["WeixinAuth:AppSecret"];
            //    option.SaveTokens = true;
            //});

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
                //添加Requirement,必须添加
                option.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer",new string[]{ } }
                });
                option.OperationFilter<FileOperation>();

            });
            #endregion          

            #region AutoMapper
            services.AddAutoMapper();
            #endregion

            //应用层服务注入
            services.AddServiceLayer();

            //注入PolicyProvider
            services.AddPermissions(Assembly.GetExecutingAssembly(), typeof(BaseController));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                //设置Sap访问的根目录，与ClientApp bulid文件输出路径一致
                configuration.RootPath = "wwwroot/dist";
            });

            var mailOption = new MailKitOptions();
            Configuration.GetSection("MailKitOptions").Bind(mailOption);

            //Add MailKit
            services.AddMailKit(optionbuilder =>
            {
                optionbuilder.UseMailKit(mailOption);
            });

            //添加CSRF配置 
            services.AddTransient<AntiforgeryMiddlerware>();
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");


            services.AddMvc(option =>
            {
                //禁止合并AuthorizeFilter
                option.AllowCombiningAuthorizeFilters = false;
                //Exception Filter
                option.Filters.Add(typeof(GlobalExceptionFilter));

                //CSRF Token
                option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                //使用ApiController标记了控制器，会默认验证模型，如果验证不通过会BadRequest(ModelState);
                //option.Filters.Add(typeof(ValidateModelFilter));

            }).AddJsonOptions(option =>
                {
                //对数据进行转化出错时，抛出异常
                option.AllowInputFormatterExceptionMessages = true;
                    option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //关闭默认的ApiBehavior
            services.Configure<ApiBehaviorOptions>(option =>
            {
                //使用自己的模型验证返回数据          
                option.InvalidModelStateResponseFactory = (action =>
                {
                    return new BadRequestObjectResult(action.ModelState.Values.SelectMany(v => v.Errors).Select(g => g.ErrorMessage).Aggregate((i, next) => $"{i},{next}"));
                });
            });




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simpleness Api");
                });
                app.UseCors(option =>
                {
                    option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials();
                });
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            //认证
            app.UseAuthentication();

            //CSRF添加Anti_token
            app.UseAntiforgery();

            //静态文件
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            //mvc管道
            app.UseMvc();
            //单页面
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }


        /// <summary>
        /// 给Swagger 添加文件上传对话框
        /// </summary>
        public class FileOperation : IOperationFilter
        {
            public void Apply(Operation operation, OperationFilterContext context)
            {
                if (operation.OperationId.ToLower() == "apiuploadpost")
                {
                    operation.Parameters.Clear();//Clearing parameters
                    operation.Parameters.Add(new NonBodyParameter
                    {
                        Name = "File",
                        In = "formData",
                        Description = "Upload Image",
                        Required = true,
                        Type = "file",
                        MultipleOf = 10
                    });
                    operation.Consumes.Add("multipart/form-data");
                }
            }
        }

    }
}
