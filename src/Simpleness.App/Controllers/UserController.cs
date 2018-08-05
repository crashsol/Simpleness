using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Simpleness.Core.User;
using Simpleness.Core.User.Dtos;
using Simpleness.Infrastructure.AspNetCore.Authorize;

namespace Simpleness.App.Controllers
{   
    
    public class UserController : BaseController
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(List<UserListDto>),200)]
        public async Task<IActionResult> IndexAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }
    }
}