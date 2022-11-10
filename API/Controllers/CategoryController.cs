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

        [HttpPost("AddCategory")]
        public async Task<ActionResult> AddCategory(CategoryDto categoryDto){
            if(await _context.Categories.AnyAsync(x=> x.Name == categoryDto.Name)){
                return BadRequest("Username is taken");
            }

            var category = new Category{
                Name = categoryDto.Name,
                VAT = categoryDto.VAT,
                AddedBy = categoryDto.AddedBy,
                AddedOn = categoryDto.AddedOn
            };
            
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            if(await _context.Categories.AnyAsync(x => x.Name == categoryDto.Name)){
                return Accepted("Succeeded");
            }else{
                return BadRequest("Ups Something went south!");
            }
        }

        [HttpGet("ListCategories")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> ListCategories(){
            return await (from category in _context.Set<Category>()
                            select new CategoryDto{
                                Name = category.Name,
                                VAT = category.VAT,
                                AddedBy = category.AddedBy,
                                AddedOn = category.AddedOn
                            }).ToListAsync();
        }
    }
}