using API.Data;
using API.Dto;
using API.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context){
            this._context = context;
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddCategory(Category category){
            if(await _context.Categories.AnyAsync(x=> x.Name == category.Name)){
                return BadRequest("Category already exists");
            }            
            
            _context.Categories.Add(category);
            if(await _context.SaveChangesAsync()>0){
                return Ok("Successfully added");
            }else{
                return BadRequest("Something went South!");
            }

            
        }

        [HttpGet("ListCategories")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> ListCategories(){
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("DeleteCategory")]
        [Authorize]
        public async Task<ActionResult> DeleteCategory(int categoryId, int userId){
            if(!await _context.Users.Where(x => x.Role == 2 && x.Id == userId).AnyAsync()){
                return BadRequest("You are not allowed to do that!");
            }
            var category = await _context.Categories.Where(x => x.CategoryId == categoryId).FirstOrDefaultAsync();
            if(category != null) {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return Ok("Category deleted successfully");
            }else{
                return BadRequest("Category not found");
            }
            
        }
    }
}