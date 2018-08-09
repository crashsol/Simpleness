using AutoMapper;
using Simpleness.Core.Department.Dto;
using Simpleness.Core.Role.Dtos;
using Simpleness.DataEntityFramework.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Core
{
    /// <summary>
    /// 服务层实体与Dto映射配置
    /// </summary>
    public class ServiceAutoMapperProfile : Profile
    {
        public ServiceAutoMapperProfile()
        {
            CreateMap<AppRole, RoleRDto>();
            CreateMap<RoleCDto, AppRole>();
            CreateMap<RoleUDto, AppRole>().ReverseMap();

            CreateMap<DepartmentCDto, DataEntityFramework.Entity.Department>();
            CreateMap<DepartmentUDto, DataEntityFramework.Entity.Department>().ReverseMap();
            CreateMap<DataEntityFramework.Entity.Department, DepartmentRDto>();
        }
    }
}
