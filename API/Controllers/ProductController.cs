using API.Data;
using API.Dto;
using API.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductController(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        //List all Items in Products
        //Later, Pagination should be added
        [HttpGet("items")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> listProducts(){
            var products = await _context.Products
                                .Include(u => u.User)
                                .Include(c => c.Category)
                                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            // return await (from product in _context.Set<Product>()
            //                 join user in _context.Set<User>()
            //                 on product.InsertedBy equals user.Id
            //                 join category in _context.Set<Category>()
            //                 on product.CategoryId equals category.CategoryId
            //                 select product                       
            // ).ToListAsync();
        }

        //List all items which have a similar name as the search term
        //Later, Pagination to be added
        [HttpGet("item/{searchTerm}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getProductByName(String searchTerm){
            //need autoMapper to Dto
            var productList = await _context.Products
                                .Include( u => u.User)            
                                .Include(c => c.Category)
                                .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(productList));
            // return await (from product in _context.Set<Product>().Where(item => item.Name.ToLower().Contains(searchTerm.ToLower()))
            //                 join user in _context.Set<User>()
            //                 on product.InsertedBy equals user.Id
            //                 join category in _context.Set<Category>()
            //                 on product.CategoryId equals category.CategoryId
            //                 select new ProductDto{
            //                     Name = product.Name,
            //                     Description = product.Description,
            //                     Owner = user.Id,
            //                     PublishedOn = product.InsertedOn,
            //                     Category = category.CategoryId,
            //                     Price = product.Price,
            //                     OnSale = product.IsSale
            //                 }            
            // ).ToListAsync();            
        }

        //List all items of a certain category
        //Later, PAgination should be added
        [HttpGet("items/category/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getProductByCategory(int categoryId){
            var products = await _context.Products
                                .Include(u => u.User)
                                .Include(c => c.Category)
                                .Where(p => p.CategoryId == categoryId).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            // return await (from product in _context.Set<Product>().Where( item => item.CategoryId == categoryId)
            //               join user in _context.Set<User>()
            //               on product.InsertedBy equals user.Id
            //               join category in _context.Set<Category>()
            //               on product.CategoryId equals category.CategoryId
            //               select new ProductDto{
            //                 Name = product.Name,
            //                 Description = product.Description,
            //                 Owner = user.Id,
            //                 PublishedOn = product.InsertedOn,
            //                 Category = category.CategoryId,
            //                 Price = product.Price,
            //                 OnSale = product.IsSale
            //               }
            // ).ToListAsync();
        }

        //List all items marked as favorite by the user
        //Pagination should be added if neccessary
        [HttpGet("items/favorite/{id}")] //could be http post method later
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getFavoriteProducts(int userId){
         
            // var products = await (from item in _context.Set<UserFavorites>().Where(i => i.UserId == userId)
            //                join product in _context.Set<Product>()
            //                on item.ProductId equals product.ProductId
            //                join user in _context.Set<User>()
            //                on item.UserId equals user.Id
            //                join category in _context.Set<Category>()
            //                on product.CategoryId equals category.CategoryId
            //                select new ProductDto{
            //                 Name = product.Name,
            //                 Description = product.Description,
            //                 Owner = user.Id,
            //                 PublishedOn = product.InsertedOn,
            //                 Category = category.CategoryId,
            //                 Price = product.Price,
            //                 OnSale = product.IsSale
            //                }
            // ).ToListAsync();                 
            var products = await _context.UserFavorites
                                        .Include(p => p.Product)
                                        .Include(u => u.User)
                                        .Include(c => c.Product.Category)
                                        .Where(x => x.UserId == userId)
                                        .ToListAsync();                    

            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        //Add item to products
        [HttpPost("item/add")]
        [Authorize]
        public async Task<ActionResult> addProduct(RegisterProductDto registerProductDto){
            if(!await _context.Users.AnyAsync(x => x.Id == registerProductDto.InsertedBy)){
                return BadRequest("User Not found");
            }
            var product = new Product{
                Name = registerProductDto.Name,
                Description = registerProductDto.Description,
                InsertedBy = registerProductDto.InsertedBy,
                InsertedOn = registerProductDto.InsertedOn,
                CategoryId = registerProductDto.CategoryId,
                Price = registerProductDto.Price,
                IsSale = registerProductDto.IsSale
            };
            _context.Products.Add(product);
            if(await _context.SaveChangesAsync()> 0){
                return Ok("Successfully added");
            }
            else{
                return BadRequest("Error while adding");
            }
        }

        [HttpGet("DeleteItem")]
        [Authorize]
        public async Task<ActionResult> deleteProduct(int productId, int userId){
            if(!_context.Users.Where(x => x.Role == 2 && x.Id == userId).Any() 
                    || !_context.Products.Where(x => x.ProductId == productId && x.InsertedBy == userId).Any()){
                return BadRequest("Not permitted to do that!");
            }

            var product = _context.Products.Where(x => x.ProductId == productId).FirstOrDefault(); 
            if(product != null){
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Ok("Product removed successfully!");
            }else{
                return BadRequest("Product could not be found!");
            }
        }

        [HttpGet("AddToFavoriteItem"), Authorize]
        public async Task<ActionResult> addToFavorites(int userId, int productId){
            
            if(!await _context.Users.AnyAsync(x => x.Id == userId)){
                return BadRequest("User not found");
            }

            if(!await _context.Products.AnyAsync(x => x.ProductId == productId)){
                return BadRequest("Product not found!");
            }

            var userFavorite = new UserFavorites{
                UserId = userId,
                ProductId = productId
            };

            _context.UserFavorites.Add(userFavorite);
            await _context.SaveChangesAsync();

            return Ok("Succeeded");
        }

        [HttpGet("RemoveFavorite")]
        [Authorize]
        public async Task<ActionResult> RemoveFavorite(int productId, int userId){
            
            var favoriteItem = await _context.UserFavorites.Where(x=> x.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync();
            if(favoriteItem != null){
                _context.UserFavorites.Remove(favoriteItem);
                await _context.SaveChangesAsync();
                return Ok("Item removed from Favorites");
            }

            return BadRequest("Error while removing");
        }

        [HttpGet("GetItemOfUser "),AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> listUserItems(int userId){
            var products = await _context.Products
                                .Include(u => u.User)
                                .Include(c => c.Category)
                                .Where(p => p.InsertedBy == userId).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            // return await (from product in _context.Set<Product>()
            //                 join user in _context.Set<User>()
            //                 on product.InsertedBy equals user.Id
            //                 select new ProductDto{
            //                     Name = product.Name,
            //                     Description = product.Description,
            //                     Owner = product.InsertedBy,
            //                     PublishedOn = DateTime.Today.Date,
            //                     Category = product.CategoryId,
            //                     Price = product.Price,
            //                     OnSale = product.IsSale
            //                 }).ToListAsync();
        }
    }

    
}