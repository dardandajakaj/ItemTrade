using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Dto;
using API.Entity;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{ 
    public class AccountController : BaseApiController
    {
       
        private readonly DataContext _context;
        private readonly ITokenServiceProvider _tokenService;

        public AccountController(DataContext context, ITokenServiceProvider tokenservice){
            _context = context;
            _tokenService = tokenservice;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){

            if(await UserExits(registerDto.Username)) return BadRequest("Username is taken");

            using (var hmac = new HMACSHA512()){
                var user = new AppUser(){
                    UserName = registerDto.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key
                }; 
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return new UserDto{
                    Username = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){

            var user = await _context.Users.SingleOrDefaultAsync<AppUser>(x => x.UserName.ToLower() == loginDto.Username.ToLower());

            if(user == null) return BadRequest("No User found!");

            using( var hmac = new HMACSHA512(user.PasswordSalt)){
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

                int i = 0;
                while(i < computedHash.Length){
                    if(computedHash[i] != user.PasswordHash[i]) return BadRequest("Invalid Password");
                    i++;
                }

                return new UserDto{
                    Username = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };;
            }
        }

        private async Task<bool> UserExits(string username){
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
    }
}