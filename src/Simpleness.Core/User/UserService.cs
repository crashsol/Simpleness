using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Simpleness.Core.User.Dtos;
using Simpleness.DataEntityFramework;
using Simpleness.DataEntityFramework.Entity;

namespace Simpleness.Core.User
{
    public class UserService :BaseService, IUserService
    {

        private readonly UserManager<AppUser> _userManager;

        public UserService(SimplenessDbContext dbContext, ILogger<UserService> logger,
              IHttpContextAccessor httpContextAccessor,
              UserManager<AppUser> userManager
              ):base(dbContext,logger,httpContextAccessor)
        {
            _userManager = userManager;
        }
        public Task CreateUser(UserCDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserListDto>> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
