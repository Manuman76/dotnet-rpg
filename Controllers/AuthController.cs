using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.User;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            this.authRepo = authRepo;

        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await authRepo.Register(
                new User { Username = request.Username }, request.Password
            );

            if (!response.Success) { return BadRequest(response); };
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserRegisterDto request)
        {
            var response = await authRepo.Login(request.Username, request.Password);

            if (!response.Success) { return BadRequest(response); };
            return Ok(response);
        }
    }
}