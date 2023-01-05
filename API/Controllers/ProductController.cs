using System.Security.Claims;
using API.Data;
using API.Dto;
using API.Entity;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        public async Task<ActionResult<IEnumerable<ProductDto>>> listProducts([FromQuery] PageParams productParams)
        {
            var products = _context.Products
                                .Include(u => u.User)
                                .Include(c => c.Category)
                                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                                .AsNoTracking();

            var result = await PagedList<ProductDto>.CreateAsync((products), productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
            //return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            // return await (from product in _context.Set<Product>()
            //                 join user in _context.Set<User>()
            //                 on product.InsertedBy equals user.Id
            //                 join category in _context.Set<Category>()
            //                 on product.CategoryId equals category.CategoryId
            //                 select product                       
            // ).ToListAsync();
        }

        [HttpGet("myitems/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getUserProducts(int userId, [FromQuery] PageParams productParams)
        {
            var products = _context.Products
                            .Include(u => u.User)
                            .Include(c => c.Category)
                            .Where(x => x.User.Id == userId)
                            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                            .AsNoTracking();

            var result = await PagedList<ProductDto>.CreateAsync(products, productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
        }

        [HttpGet("item/{productId}")]
        public async Task<ActionResult<ProductDto>> getProductById(int productId)
        {
            var product = await _context.Products.Include(u => u.User).Include(c => c.Category).Where(x => x.ProductId == productId).FirstOrDefaultAsync();
            return Ok(_mapper.Map<ProductDto>(product));
        }

        //List all items which have a similar name as the search term
        //Later, Pagination to be added
        [HttpGet("item/search/{searchTerm}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getProductByName(String searchTerm, [FromQuery] PageParams productParams)
        {
            //need autoMapper to Dto
            var productList = _context.Products
                                .Include(u => u.User)
                                .Include(c => c.Category)
                                .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()))
                                .ProjectTo<ProductDto>(_mapper.ConfigurationProvider).AsNoTracking();
            var result = await PagedList<ProductDto>.CreateAsync(productList, productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
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

        [HttpGet("items/category/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getProductByCategory(int categoryId, [FromQuery] PageParams productParams)
        {
            var products = _context.Products
                                .Include(u => u.User)
                                .Include(c => c.Category)
                                .Where(p => p.CategoryId == categoryId).ProjectTo<ProductDto>(_mapper.ConfigurationProvider).AsNoTracking();
            var result = await PagedList<ProductDto>.CreateAsync(products, productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
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
            // ).ToListAsync(); versioni i pare 
        }

        [HttpGet("items/favorite/{id}")] //could be http post method later
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDto>>> getFavoriteProducts(int userId, [FromQuery] PageParams productParams)
        {

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
            var products = _context.UserFavorites
                                        .Include(p => p.Product)
                                        .Include(u => u.User)
                                        .Include(c => c.Product.Category)
                                        .Where(x => x.UserId == userId)
                                        .ProjectTo<ProductDto>(_mapper.ConfigurationProvider).AsNoTracking();

            var result = await PagedList<ProductDto>.CreateAsync(products, productParams.PageNumber, productParams.ItemsPerPage);
            Response.AddPaginationHeader(result.TotalItems, result.ItemsPerPage, result.TotalPages, result.CurrentPage);
            return Ok(result);
        }

        [HttpPost("item/add")]
        [Authorize]
        public async Task<ActionResult> addProduct(RegisterProductDto registerProductDto)
        {
            if (!await _context.Users.AnyAsync(x => x.Id == registerProductDto.InsertedBy))
            {
                return BadRequest("User Not found");
            }
            var product = new Product
            {
                Name = registerProductDto.Name,
                Description = registerProductDto.Description,
                InsertedBy = registerProductDto.InsertedBy,
                InsertedOn = registerProductDto.InsertedOn,
                CategoryId = registerProductDto.CategoryId,
                Price = registerProductDto.Price,
                IsSale = registerProductDto.IsSale
            };
            _context.Products.Add(product);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Error while adding");
            }
        }

        [HttpDelete("delete/{productId}")]
        [Authorize]
        public async Task<ActionResult> deleteProduct(int productId)
        {

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("Not permitted");
            }

            var product = await _context.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
            if (product == null)
            {
                return BadRequest("No product found!");
            }
            if (user.Role != 2 || user.Id != product.InsertedBy)
            {
                return BadRequest("Not permitted to do that!");
            }


            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();

        }

        [HttpGet("AddToFavoriteItem"), Authorize]
        public async Task<ActionResult> addToFavorites(int userId, int productId)
        {

            if (!await _context.Users.AnyAsync(x => x.Id == userId))
            {
                return BadRequest("User not found");
            }

            if (!await _context.Products.AnyAsync(x => x.ProductId == productId))
            {
                return BadRequest("Product not found!");
            }

            var userFavorite = new UserFavorites
            {
                UserId = userId,
                ProductId = productId
            };

            _context.UserFavorites.Add(userFavorite);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("RemoveFavorite")]
        [Authorize]
        public async Task<ActionResult> RemoveFavorite(int productId, int userId)
        {

            var favoriteItem = await _context.UserFavorites.Where(x => x.ProductId == productId && x.UserId == userId).FirstOrDefaultAsync();
            if (favoriteItem != null)
            {
                _context.UserFavorites.Remove(favoriteItem);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Error while removing");
        }

        [HttpPut("edit/{id}")]
        [Authorize]
        public async Task<ActionResult> editProduct(int id, UpdateProductDto productToUpdate)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();

            var product = await _context.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();

            if (product.InsertedBy != user.Id)
            {
                return BadRequest("Something went wrong!");
            }

            _mapper.Map<UpdateProductDto, Product>(productToUpdate, product);
            _context.Products.Update(product);
            if (await _context.SaveChangesAsync() > 0)
                return Ok();
            return BadRequest("Not Mapped!");
        }
    }
}