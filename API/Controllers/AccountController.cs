using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Dto;
using API.Entity;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
       
        private readonly DataContext _context;
        private readonly ITokenServiceProvider _tokenService;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenServiceProvider tokenservice, IMapper mapper){
            _mapper = mapper;
            _context = context;
            _tokenService = tokenservice;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){

            if(await UserExits(registerDto.Username)) return BadRequest("Username is taken");

            using (var hmac = new HMACSHA512()){
                var user = new User(){
                    Fullname = registerDto.Fullname,
                    Email = registerDto.Email,
                    UserName = registerDto.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key,
                    Street = registerDto.Street,
                    City = registerDto.City,
                    State = registerDto.State,
                    Phone = registerDto.Phone,
                    DateOfBirth = registerDto.DateOfBirth,
                    Registered = DateTime.Now,
                    Role = 1,
                    IsActive = true
                }; 
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Token = _tokenService.CreateToken(user);
                return userDto;
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){

            var user = await _context.Users.SingleOrDefaultAsync<User>(x => x.UserName.ToLower() == loginDto.Username.ToLower());

            if(user == null) return BadRequest("No User found!");

            using( var hmac = new HMACSHA512(user.PasswordSalt)){
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

                int i = 0;
                while(i < computedHash.Length){
                    if(computedHash[i] != user.PasswordHash[i]) return BadRequest("Invalid Password");
                    i++;
                }

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Token = _tokenService.CreateToken(user);
                return userDto; 
            }
        }

        private async Task<bool> UserExits(string username){
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
    }
}