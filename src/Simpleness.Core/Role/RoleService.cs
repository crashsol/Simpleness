using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Simpleness.Utility.CommonDto;
using Simpleness.Core.Role.Dtos;
using Simpleness.DataEntityFramework;
using Simpleness.DataEntityFramework.Entity;
using Simpleness.Infrastructure.AspNetCore.UserException;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Simpleness.Core.Role
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleService(SimplenessDbContext dbContext,
            ILogger<RoleService> logger,
            IMapper mapper,
            RoleManager<AppRole> roleManager,
            IHttpContextAccessor httpContextAccessor) : base(dbContext, logger, mapper, httpContextAccessor)
        {
            _roleManager = roleManager;
        }

        public async Task<Guid> CreateAsync(RoleCDto dto)
        {
            var entity = _mapper.Map<AppRole>(dto);
            var result = await _roleManager.CreateAsync(entity);
            if (!result.Succeeded)
            {
                var errormsg = result.Errors.Select(b => b.Description).Aggregate((i, next) => $"{i},{next}");
                _logger.LogError($"创建角色出错:{ errormsg}");
                throw new UserOperationException(errormsg);
            }
            return entity.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContent.Roles.FindAsync(id);
            if (entity == null)
            {
                _logger.LogError($"删除角色出错: { id } 不存在 ");
                throw new UserOperationException("角色不存在");
            }
            _dbContent.Roles.Remove(entity);
            await _dbContent.SaveChangesAsync();
        }

        public async Task<RoleUDto> GetRoleByIdAsync(Guid id)
        {
            var entity = await _dbContent.Roles.FindAsync(id);
            if (entity == null)
            {
                _logger.LogError($"获取角色出错: { id } 不存在 ");
                throw new UserOperationException("角色不存在");
            }
            return _mapper.Map<RoleUDto>(entity);

        }


        public async Task<TransferDto<Guid>> GetRoleUsersByRoleIdAsync(Guid roleId)
        {
            var allUser = await _dbContent.Users.Select(b => new TransferItem<Guid>
            {
                Key = b.Id,
                Label = b.UserName,
            }).ToListAsync();
            var selectUsersId = await _dbContent.UserRoles.Where(b => b.RoleId == roleId).Select(b => b.UserId).ToListAsync();
            return new TransferDto<Guid> { Items = allUser, SelectItems = selectUsersId };

        }

        public async Task<PageResultDto<RoleRDto>> RoleListsAsync(PageQueryDto pageQuery)
        {
            var total = await _dbContent.Roles.CountAsync();
            var roles = await _dbContent.Roles.AsNoTracking()
                .OrderBy(b=>b.Name)
                .Skip((pageQuery.CurrentPage - 1 )* (pageQuery.PageSize)).Take(pageQuery.PageSize)
                .ToListAsync();
            var items = _mapper.Map<List<RoleRDto>>(roles ?? new List<AppRole>());
            return new PageResultDto<RoleRDto>(total, items);
        }

        public async Task UpdateRoleUsersAsync(RoleUsersDto dto)
        {
            //移除元角色下的用户
            var oddUsers = await _dbContent.UserRoles.Where(b => b.RoleId == dto.Id).ToListAsync();
            _dbContent.UserRoles.RemoveRange(oddUsers);
            await _dbContent.SaveChangesAsync();
            //给角色添加新的用户
            var newUsers = dto.UserIds.Select(b => new IdentityUserRole<Guid> { RoleId = dto.Id, UserId = b });
            await _dbContent.AddRangeAsync(newUsers);
            await _dbContent.SaveChangesAsync();
        }

        public async Task UpdateAsync(RoleUDto dto)
        {
            var role = await _roleManager.FindByIdAsync(dto.Id.ToString());
            role.Name = dto.Name;
            role.Description = dto.Description;
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(b => b.Description).Aggregate((i, next) => $"{i},{next}");
                _logger.LogError($"更新角色信息出错:{dto.Id}, {errors}");
                throw new UserOperationException($"{errors}");
            }
        }

        public async Task<TreeDto<string>> GetRolePermissionAsync(Guid roleid)
        {
            var role = await _dbContent.Roles.FindAsync(roleid);
            if (role == null)
                throw new UserOperationException("角色不存在");
            var treeItem = _httpContextAccessor.HttpContext.RequestServices.GetService<TreeItem<string>>();
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var selectPermmissonClaims = roleClaims.Where(b => b.Type == "permission").Select(b => b.Value).ToList();
            return new TreeDto<string>
            {
                Tree = treeItem,
                SelectKeys = selectPermmissonClaims ?? new List<string>()
            };
        }

        public async Task UpdateRolePermissionAsync(RolePermissionDto dto)
        {
            var role = await _dbContent.Roles.FindAsync(dto.Id);
            if (role == null)
                throw new UserOperationException("角色不存在");
            //移除角色旧的权限
            var oldClaims = await _dbContent.RoleClaims.Where(b => b.RoleId == dto.Id).ToListAsync();
            _dbContent.RoleClaims.RemoveRange(oldClaims);
            await _dbContent.SaveChangesAsync();
            //给角色添加新的用户
            var newClaims = dto.Permissions.Select(b => new IdentityRoleClaim<Guid> { RoleId = dto.Id, ClaimType = "permission", ClaimValue = b }).ToList();
            await _dbContent.RoleClaims.AddRangeAsync(newClaims);
            await _dbContent.SaveChangesAsync();

        }
    }
}
