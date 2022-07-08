using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenservice;
           private readonly IMapper _mapper;
        public AccountController(DataContext context, ITokenService tokenservice,     
        IMapper mapper)
        {
            _mapper = mapper;
            _tokenservice = tokenservice;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("User Name is Exist!!!");
            
var user = _mapper.Map<AppUser>(registerDto);
            
            using var hmac = new HMACSHA512();
                user.userName = registerDto.Username.ToLower();
                 user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
                 user.passwordSalt = hmac.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto
            {
                UserName = user.userName,
                Token = _tokenservice.CreateToken(user),
                knownAs=user.knownAs
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
            .Include(p=>p.photos)            
            .SingleOrDefaultAsync(x => x.userName == loginDto.UserName);
            if (user == null) return Unauthorized("Invalid User Name");

            using var hmac = new HMACSHA512(user.passwordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.passwordHash[i])
                    return Unauthorized("Invalid Password");
            }
            return new UserDto
            {
                UserName = user.userName,
                Token = _tokenservice.CreateToken(user),
                PhotoUrl = user.photos.FirstOrDefault(x => x.isMain)?.url,
                knownAs=user.knownAs
            };
        }



        public async Task<bool> UserExist(string username)
        {
            return await _context.Users.AnyAsync(x => x.userName == username.ToLower());
        }
    }
}