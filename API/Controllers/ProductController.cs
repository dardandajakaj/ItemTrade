using API.Data;
using API.Dto;
using API.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("ListProcducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> listProducts(){
            //return await _context.Products.ToListAsync();
            return await (from product in _context.Set<Product>()
                            join user in _context.Set<User>()
                            on product.InsertedBy equals user.Id
                            join category in _context.Set<Category>()
                            on product.CategoryId equals category.CategoryId
                            select new ProductDto{
                                Name = product.Name,
                                Description = product.Description,
                                Owner = user.Fullname,
                                PublishedOn = product.InsertedOn,
                                Category = category.Name,
                                Price = product.Price,
                                OnSale = product.IsSale
                            }
            ).ToListAsync();
        }

        [HttpGet("FindProductByName")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getProductByName(String searchTerm){
            //return await _context.Products.Where(x => x.Name.ToLower() == searchTerm.ToLower()).ToListAsync();
            return await (from product in _context.Set<Product>().Where(item => item.Name.ToLower() == searchTerm.ToLower())
                            join user in _context.Set<User>()
                            on product.InsertedBy equals user.Id
                            join category in _context.Set<Category>()
                            on product.CategoryId equals category.CategoryId
                            select new ProductDto{
                                Name = product.Name,
                                Description = product.Description,
                                Owner = user.Fullname,
                                PublishedOn = product.InsertedOn,
                                Category = category.Name,
                                Price = product.Price,
                                OnSale = product.IsSale
                            }            
            ).ToListAsync();            
        }

        [HttpGet("FindProductByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getProductByCategory(int categoryId){
            //return await _context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
            return await (from product in _context.Set<Product>().Where( item => item.CategoryId == categoryId)
                          join user in _context.Set<User>()
                          on product.InsertedBy equals user.Id
                          join category in _context.Set<Category>()
                          on product.CategoryId equals category.CategoryId
                          select new ProductDto{
                            Name = product.Name,
                            Description = product.Description,
                            Owner = user.Fullname,
                            PublishedOn = product.InsertedOn,
                            Category = category.Name,
                            Price = product.Price,
                            OnSale = product.IsSale
                          }
            ).ToListAsync();
        }

        [HttpGet("FindProductFavorites")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getFavoriteProducts(int userId){
            return await (from item in _context.Set<UserFavorites>().Where(i => i.UserId == userId)
                           join product in _context.Set<Product>()
                           on item.ProductId equals product.ProductId
                           join user in _context.Set<User>()
                           on item.UserId equals user.Id
                           join category in _context.Set<Category>()
                           on product.CategoryId equals category.CategoryId
                           select new ProductDto{
                            Name = product.Name,
                            Description = product.Description,
                            Owner = user.Fullname,
                            PublishedOn = product.InsertedOn,
                            Category = category.Name,
                            Price = product.Price,
                            OnSale = product.IsSale
                           }
            ).ToListAsync();
        }
        
    }
}