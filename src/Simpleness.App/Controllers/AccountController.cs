using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Simpleness.App.Models;
using Simpleness.DataEntityFramework;
using Simpleness.DataEntityFramework.Entity;
using Simpleness.Infrastructure.AspNetCore.Models;


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


        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger, 
            IOptionsSnapshot<JwtSettings> options,
            SimplenessDbContext dbContext
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
            _jwtSetting = options.Value ?? throw new ArgumentNullException("JWT配置参数错误");
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]      
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginViewModel model)
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

                //mock admin permission
                if (user.UserName =="admin@qq.com")
                {
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Users)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Users_Create)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Users_Delete)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Users_Locked)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Roles)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Roles_Create)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Roles_Delete)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Roles_Edit)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Roles_Memeber)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Roles_Permission)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Departments)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Departments_Create)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Departments_Delete)));
                    claims.Add(new Claim("permission", nameof(PermissionSettings.Departments_Edit)));
                }
            

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
            //账号被锁定
            if (result.IsLockedOut)
            {
                _logger.LogWarning($"账户被锁定: {model.UserName} ");
                return BadRequest("账号被锁定,无法登录,请联系管理员!");
            }
            return BadRequest("用户名密码不匹配");
        }        
    }
}