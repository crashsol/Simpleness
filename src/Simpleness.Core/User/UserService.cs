using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Simpleness.Core.User.Dtos;
using Simpleness.DataEntityFramework;
using Simpleness.DataEntityFramework.Entity;
using Simpleness.Infrastructure.AspNetCore.UserException;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Simpleness.Core.User
{
    public class UserService : BaseService, IUserService
    {

        private readonly UserManager<AppUser> _userManager;

        public UserService(SimplenessDbContext dbContext, ILogger<UserService> logger,
              IHttpContextAccessor httpContextAccessor,
              UserManager<AppUser> userManager,
              IMapper mapper
              ) : base(dbContext, logger,mapper, httpContextAccessor)
        {
            _userManager = userManager;
        }
        public async Task CreateUserAsync(UserCDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                NickName = dto.NickName,
            };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(b => b.Description));
                _logger.LogWarning($"创建用户失败 { dto.Email},错误{errors}");
                throw new UserOperationException(errors);
            }

            var addroleResult = await _userManager.AddToRolesAsync(user, dto.Roles);
            if (!addroleResult.Succeeded)
            {
                var errors = string.Join(",", addroleResult.Errors.Select(b => b.Description));
                _logger.LogWarning($"为用户添加角色失败 { dto.Email},错误{errors}");
                throw new UserOperationException(errors);
            }
        }

        public async Task<List<UserListDto>> GetAllUsersAsync()
        {
            return await _userManager.Users.Select(b => new UserListDto
            {
                Avatar = b.Avatar,
                Email = b.Email,
                Id = b.Id,
                LockoutEnd = b.LockoutEnd,
                NickName = b.NickName,
                Phone = b.PhoneNumber,
                UserName = b.UserName
            }).ToListAsync();
        }
    }
}
