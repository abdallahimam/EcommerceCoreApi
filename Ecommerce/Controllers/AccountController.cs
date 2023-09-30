using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Extensions;
using Core.Identity;
using Core.IServices;
using Ecommerce.Dtos;
using Ecommerce.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto) {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponseError(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponseError(401));

            var userDto = new UserDto {
                Email = loginDto.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(userDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto) {

            var user = new AppUser {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName

            };

            if (user == null) return Unauthorized(new ApiResponseError(401));

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponseError(400));

            var userDto = new UserDto {
                Email = registerDto.Email,
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user)
            };

            return Ok(userDto);
        }

        [Authorize]
        [HttpGet("getcurrentuser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Email).Value;

            if (email == null) return BadRequest(new ApiResponseError(404));

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return BadRequest(new ApiResponseError(404, "User not found"));

            return Ok(new UserDto { DisplayName = user.DisplayName, Email = email, Token = _tokenService.CreateToken(user) });
        }

        [HttpGet("emailexist")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            var user = await _userManager.GetUserByEmailByClaimsPrincipal(User);
            if (user == null) return BadRequest(new ApiResponseError(404, "This email not exists"));
            return Ok(true);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<IActionResult> GetUserAddress()
        {


            var user = await _userManager.GetUserByEmailByClaimsPrincipalWithAddress(User);

            if (user == null) return BadRequest(new ApiResponseError(404, "User not found"));

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }

        [Authorize]
        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUserAddress(AddressDto addressDto)
        {
            var user = await _userManager.GetUserByEmailByClaimsPrincipalWithAddress(User);
            user.Address = _mapper.Map<Address>(addressDto);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));
            return BadRequest(new ApiResponseError(400, "Error: some problems occure while updating the address"));
        }

        [HttpGet("test")]
        [Authorize]
        public  IActionResult Test() {
            return Ok("Tested");
        }
    }
}