using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using API.Dto;
using API.Interfaces;
using AutoMapper.QueryableExtensions;
using API.Helpers;

namespace API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private ITokenServiceProvider _tokenService { get; }
        public UserController(DataContext context, IMapper mapper, ITokenServiceProvider tokenService)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery]PageParams userParams){
            // var users = await _context.Users.ToListAsync();
            // var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            // for(int i = 0; i < users.Count; i++){
            //     userDtos.ElementAt(i).Token = _tokenService.CreateToken(users[i]); 
            // }
            // return Ok(userDtos);
            var users = _context.Users.ProjectTo<UserDto>(_mapper.ConfigurationProvider).AsNoTracking();
            return await PagedList<UserDto>.CreateAsync(users, userParams.PageNumber, userParams.ItemsPerPage);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUser(int id){
            var user = await _context.Users.FindAsync(id);
            var userDto = user != null ? _mapper.Map<UserDto>(user) : null;
            userDto.Token= _tokenService.CreateToken(user);
            return userDto;
        }
    }
}