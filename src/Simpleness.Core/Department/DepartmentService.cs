using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Simpleness.Utility.CommonDto;
using Simpleness.Core.Department.Dto;
using Simpleness.DataEntityFramework;
using Simpleness.Infrastructure.AspNetCore.UserException;
using DepartmentEntity = Simpleness.DataEntityFramework.Entity.Department;
using Simpleness.DataEntityFramework.Entity;

namespace Simpleness.Core.Department
{

    public class DepartmentService : BaseService, IDepartmentService
    {
        public DepartmentService(SimplenessDbContext dbContext,
            ILogger<BaseService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(dbContext, logger, mapper, httpContextAccessor)
        {
        }

        public async Task<Guid> CreateAsync(DepartmentCDto dto)
        {
            var parient = await _dbContent.Departments.FindAsync(dto.Pid);
            if (parient == null)
            {
                _logger.LogError($"创建部门出错:上级部门 { dto.Pid} 不存在 ");
                throw new UserOperationException("上级部门不存在");
            }
            var entity = _mapper.Map<DepartmentEntity>(dto);
            entity.FullPath = parient.FullPath + "/" + entity.Name;
            try
            {
                await _dbContent.AddAsync(entity);
                await _dbContent.SaveChangesAsync();
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建部门失败");
                throw new UserOperationException("已存在相同名称的部门");

            }

        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContent.Departments.FindAsync(id);
            if (entity == null)
                throw new UserOperationException("部门不存在");
            if (entity.Pid == default(Guid))
            {
                throw new UserOperationException("根节点不能删除");
            }
            var allEntity = await _dbContent.Departments.
                                        Where(b => b.FullPath.StartsWith(entity.FullPath)).ToListAsync();
            _dbContent.RemoveRange(allEntity);
            await _dbContent.SaveChangesAsync();
        }

        public async Task<List<DepartmentRDto>> DepartmentListAsync()
        {
            return _mapper.Map<List<DepartmentRDto>>(await _dbContent.Departments.AsNoTracking().OrderBy(b => b.Order).OrderBy(b => b.FullPath).ToListAsync());
        }

        public async Task<DepartmentUDto> GetDepartmentByIdAsync(Guid id)
        {
            var entity = await _dbContent.Departments.FindAsync(id);
            if (entity == null)
            {
                _logger.LogError($"部门 {id }不存在");
                throw new UserOperationException("部门不存在");
            }
            return _mapper.Map<DepartmentUDto>(entity);
        }

        public async Task UpdateAsync(DepartmentUDto dto)
        {          
            var parent = await _dbContent.Departments.FindAsync(dto.Pid);
            if (parent == null && dto.Pid != default(Guid))
                throw new UserOperationException("上级部门不存在");
            var entity = await _dbContent.Departments.FindAsync(dto.Id);
            if (entity == null)
                throw new UserOperationException("数据不存在");
            var oldEntityFullPath = entity.FullPath; //A_B         
            entity.Pid = dto.Pid;
            entity.Name = dto.Name;
            entity.Order = dto.Order;
            entity.Description = dto.Description;
            entity.FullPath = parent.FullPath + "/" + dto.Name;

            //检查当前节点的子节点
            var children = await _dbContent.Departments
                                        .Where(b => b.FullPath.StartsWith(oldEntityFullPath) && b.Id != entity.Id).ToListAsync();
            foreach (var item in children)
            {
                item.FullPath = item.FullPath.Replace(oldEntityFullPath, entity.FullPath);
            }
            _dbContent.Departments.UpdateRange(entity);
            _dbContent.Departments.UpdateRange(children);
            await _dbContent.SaveChangesAsync();
        }

        public async Task<TransferDto<Guid>> GetDepartmentUsersAsync(Guid id)
        {
            var allUsers = await _dbContent.Users.AsNoTracking().Select(b => new TransferItem<Guid>
            {
                Key = b.Id,
                Label = b.UserName,
            }).ToListAsync();
            var selectUserIds = await _dbContent.UserDepartments.AsTracking().Where(b => b.DepartmentId == id).Select(b => b.AppUserId).ToListAsync();
            return new TransferDto<Guid>
            {
                Items = allUsers ?? new List<TransferItem<Guid>>(),
                SelectItems = selectUserIds ?? new List<Guid>()
            };
        }

        public async Task UpdateDepartmentUsersAsync(DepartmentUsersDto dto)
        {
            //移除旧的关系
            var odds = await _dbContent.UserDepartments.Where(b => b.DepartmentId == dto.Id).ToListAsync();
            _dbContent.RemoveRange(odds);
            await _dbContent.SaveChangesAsync();

            //添加新的
            var news = dto.UserIds.Select(b => new UserDepartments { AppUserId = b, DepartmentId = dto.Id }).ToList();
            await _dbContent.UserDepartments.AddRangeAsync(news);
            await _dbContent.SaveChangesAsync();
        }

        public async Task<DepartmentTreeItem> GetDepartmentTreeAsync()
        {
            var allDepartments = await _dbContent.Departments.AsNoTracking().ToListAsync();
            var temp = allDepartments.SingleOrDefault(b => b.Pid == default(Guid));
            var root = new DepartmentTreeItem
            {
                Id = temp.Id,
                Label = temp.Name,
                Description =temp.Description,
                Order =temp.Order
            };
            CreateTree(allDepartments, root);
            return root;
        }

        private void CreateTree(List<DepartmentEntity> departments, DepartmentTreeItem root)
        {
            var children = departments.Where(b => b.Pid == root.Id).OrderBy(b => b.Order).ToList();
            for (int i = 0; i < children.Count(); i++)
            {
                root.Children.Add(new DepartmentTreeItem
                {
                    Id = children[i].Id,
                    Label = children[i].Name,
                    Order =children[i].Order,
                    Description =children[i].Description
                    
                });
                CreateTree(departments, root.Children[i]);
            }

        }
    }
}
