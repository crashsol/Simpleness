using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using Simpleness.App.Models;
using Simpleness.DataEntityFramework;
using Simpleness.DataEntityFramework.Entity;
using Simpleness.Infrastructure.AspNetCore.Models;
using System.Net.Http;
using System.Web;
using Simpleness.App.Filters;

namespace Simpleness.App.Controllers
{

    [AllowAnonymous]
    public class AccountController : BaseController
    {

        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SimplenessDbContext _dbContext;
        private readonly JwtSettings _jwtSetting;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger,
            IOptionsSnapshot<JwtSettings> options,
            SimplenessDbContext dbContext,
            IEmailService emailService,
            IConfiguration configuration
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
            _jwtSetting = options.Value ?? throw new ArgumentNullException("JWT配置参数错误");
            _emailService = emailService;
            _configuration = configuration;
        }

        [DisableAudit]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {

            var user = await _dbContext.Users.FirstOrDefaultAsync(b => b.UserName == model.UserName || b.Email == model.UserName);
            if (user == null)
            {
                _logger.LogInformation($"未注册用户： {model.UserName},尝试登录!");
                return BadRequest("用户不存在");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
            if (result.Succeeded)
            {

                //获取用户所有的claims
                var userclaims = await _userManager.GetClaimsAsync(user);            
                var roleClaims = await _dbContext.UserRoles.Join(_dbContext.Roles, a => a.RoleId, b => b.Id, (a, b) => new { a, b })
                                                    .Join(_dbContext.RoleClaims, x => x.b.Id, c => c.RoleId, (x, c) => new { x, c })
                                                    .AsNoTracking().Where(b => b.x.a.UserId == user.Id)
                                                    .Select(d => new Claim(d.c.ClaimType, d.c.ClaimValue)).ToListAsync();
                var claims = roleClaims.Union(userclaims).Distinct().ToList();
                claims.Add(new Claim("sub", user.Id.ToString()));
                claims.Add(new Claim("name", user.UserName));
                claims.Add(new Claim("avatar", user.Avatar ?? ""));

                //获得 加密后的key
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecretKey));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);     //使用加密后的key 创建 登录证书
                //生产加密token
                var token = new JwtSecurityToken(
                    _jwtSetting.Issuer,
                    _jwtSetting.Audience,
                    claims,
                    DateTime.Now,
                    DateTime.Now.AddHours(4),
                    cred
                    );
                //返回token
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });

            }
            if (result.IsNotAllowed)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = $"{_configuration["SiteUri"]}/api/account/ConfirmEmail?userId={user.Id}&code={HttpUtility.UrlEncode(code)}";
                await _emailService.SendAsync(user.Email, "邮箱验证", $"请点击链接,已验证你的邮箱<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>验证邮箱</a>.", true);
                return BadRequest("邮箱未验证，请登录邮箱验证！");
            }
            //账号被锁定
            if (result.IsLockedOut)
            {
                _logger.LogWarning($"账户被锁定: {model.UserName} ");
                return BadRequest("账号被锁定,无法登录,请联系管理员!");
            }
            return BadRequest("用户名密码不匹配");
        }


        /// <summary>
        /// 新用户注册
        /// </summary>
        /// <param name="Input"><see cref="RegisterModel"/>注册模型</param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel Input)
        {
            var user = new AppUser { UserName = Input.Email, Email = Input.Email };
            var result = await _userManager.CreateAsync(user, Input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);


                var callbackUrl = Url.Action(
                    "ConfirmEmailAsync",
                    "Account",
                    new { code = HttpUtility.UrlEncode(code),userId = user.Id },
                    protocol: Request.Scheme);

             //   var callbackUrl = $"{HttpContext.Request.PathBase}/api/account/ConfirmEmail?userId={user.Id}&code={HttpUtility.UrlEncode(code)}";
                await _emailService.SendAsync(user.Email, "新用户注册,邮箱验证", $"请点击链接,已验证你的邮箱<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>验证邮箱</a>.", true);
                return Ok("注册成功，请登录邮箱验证！");
            }
            return BadRequest(result.Errors.Select(b => b.Description).Aggregate((i, next) => $"{i},{next}"));
        }


        /// <summary>
        /// 验证邮箱地址
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="code">验证CODE</param>
        /// <returns></returns>
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest($"无法找到ID为 '{userId}'的用户.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                return Redirect("/login?status=fail");
                //return BadRequest($" '{userId}' 用户验证邮箱失败:");
            }
            return Redirect("/login?status=success");
        }


        /// <summary>
        /// 通过邮箱地址找回密码
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <returns></returns>      
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("forgotpwd/{email}")]
        public async Task<IActionResult> ForgotPasswordAsync([EmailAddress(ErrorMessage = "必须输入邮箱地址")]string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return BadRequest("该邮箱未注册!");
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action(
                   "ResetPasswordAsync",
                   "Account",
                   new { code = HttpUtility.UrlEncode(code) },
                   protocol: Request.Scheme);


           // var callbackUrl = $"{_configuration["SiteUri"]}/resetpwd?code={HttpUtility.UrlEncode(code)}";
            await _emailService.SendAsync(user.Email, "邮箱验证", $"请点击链接,重置你的密码<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>重置密码</a>.", true);
            return Ok("请重置密码邮件已发送至你的邮箱,请查收!");
        }

        /// <summary>
        /// 通过邮件Code重置密码
        /// </summary>
        /// <param name="dto"><see cref="ResetPasswordWithCode"/>重置密码</param>
        /// <returns></returns>

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [HttpPost("resetpwd")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordWithCode dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest("该邮箱未注册!");
            }
            var result = await _userManager.ResetPasswordAsync(user, dto.Code, dto.Password);
            if (result.Succeeded)
            {
                return Ok("重置密码成功");
            }
            return BadRequest(result.Errors.Select(b => b.Description).Aggregate((i, next) => $"{i},{next}"));

        }




    }
}