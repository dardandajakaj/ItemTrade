using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BookController : BaseApiController
    {
        private readonly DataContext _context;

        public BookController(DataContext context)
        {
            _context = context;
        }

        // [HttpPost("search")]
        // public async Task<IActionResult> searchByTitle(String title){

        //     var results = _context.Books.Where(b => b.Title.Contains(title));
        //     return BadRequest();
        // }
    }
}